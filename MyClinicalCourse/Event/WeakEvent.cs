using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MyClinicalCourse.Event
{
    // 汎用的なWeakEventパターンの実装
    // 参考: https://iwata0303.bitbucket.io/html/2014/06/30/implement_oreore_weakevent.html

    /// <summary>
    /// WeakEventのハンドラー
    /// </summary>
    /// <typeparam name="TEventArgs">イベント引数</typeparam>
    internal class WeakHandler<TEventArgs> : IEquatable<EventHandler<TEventArgs>>
        where TEventArgs : EventArgs
    {
        /// <summary>
        /// イベントハンドラーへの弱参照
        /// </summary>
        private readonly WeakReference targetRef;
        /// <summary>
        /// 
        /// </summary>
        private readonly MethodInfo method;
        /// <summary>
        /// 
        /// </summary>
        private readonly Action<object, object, TEventArgs> action;

        /// <summary>
        /// WeakEventハンドラーの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="handler">WeakEventパターンにするイベントハンドラー</param>
        public WeakHandler(EventHandler<TEventArgs> handler)
        {
            if (null == handler) throw new ArgumentNullException(nameof(handler));
            this.targetRef = null != handler.Target ? new WeakReference(handler.Target) : null;
            this.method = handler.Method;
            this.action = CreateOpenMethod(handler);
        }

        /// <summary>
        /// このイベントハンドラーの実行式をコンパイルし、キャッシュします。
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Action<object, object, TEventArgs> CreateOpenMethod(EventHandler<TEventArgs> handler)
        {
            var target = Expression.Parameter(typeof(object), "target");
            var sender = Expression.Parameter(typeof(object), "sender");
            var e = Expression.Parameter(typeof(TEventArgs), "e");

            var instance = null != handler.Target ? Expression.Convert(target, handler.Target.GetType()) : null;

            var expression =
                Expression.Lambda<Action<object, object, TEventArgs>>(
                    Expression.Call(instance, handler.Method, sender, e),
                    target, sender, e);

            return expression.Compile();
        }

        /// <summary>
        /// クラスインスタンスを取得できるかどうか を取得します。
        /// </summary>
        public bool IsStatic
        {
            get { return null == targetRef; }
        }

        /// <summary>
        /// イベントハンドラーが生存しているかどうか を取得します。
        /// </summary>
        public bool IsAlive
        {
            get { return this.IsStatic || targetRef.IsAlive; }
        }

        /// <summary>
        /// ハンドラーを実行します。
        /// </summary>
        /// <param name="sender">イベントの送信元</param>
        /// <param name="e">イベント引数</param>
        public void Invoke(object sender, TEventArgs e)
        {
            if (this.IsStatic)
            {
                action(null, sender, e);
            }
            else
            {
                var target = targetRef.Target;
                if (null != target)
                    action(target, sender, e);
            }
        }

        /// <summary>
        /// 指定されたインスタンスが等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">現在のオブジェクトと比較するオブジェクト</param>
        /// <returns></returns>
        public bool Equals(EventHandler<TEventArgs> other)
        {
            if (null == other.Target)
            {
                return null == this.targetRef && this.method == other.Method;
            }
            else
            {
                return null != this.targetRef && this.targetRef.Target == other.Target && this.method == other.Method;
            }
        }
    }

    /// <summary>
    /// 汎用WeakEventパターン
    /// </summary>
    /// <typeparam name="TEventArgs">イベント引数</typeparam>
    public class WeakEvent<TEventArgs> where TEventArgs : EventArgs
    {
        /// <summary>
        /// イベントハンドラーの管理コレクション
        /// </summary>
        private readonly List<WeakHandler<TEventArgs>> handlers = new List<WeakHandler<TEventArgs>>();

        /// <summary>
        /// イベントハンドラーを追加します。
        /// </summary>
        /// <param name="handler">追加するイベントハンドラー</param>
        public void AddHandler(EventHandler<TEventArgs> handler)
        {
            lock (handlers)
            {
                handlers.RemoveAll(w => !w.IsAlive);
                handlers.Add(new WeakHandler<TEventArgs>(handler));
            }
        }

        /// <summary>
        /// イベントハンドラーを削除します。
        /// </summary>
        /// <param name="handler">削除するイベントハンドラー</param>
        public void RemoveHandler(EventHandler<TEventArgs> handler)
        {
            lock (handlers)
            {
                handlers.RemoveAll(w => !w.IsAlive || w.Equals(handler));
            }
        }

        /// <summary>
        /// 登録済みのハンドラーへイベントを通知します。
        /// </summary>
        /// <param name="sender">イベントの送信元</param>
        /// <param name="e">イベント引数</param>
        public void RaiseEvent(object sender, TEventArgs e)
        {
            WeakHandler<TEventArgs>[] hs;
            lock (handlers)
            {
                handlers.RemoveAll(w => !w.IsAlive);
                hs = handlers.ToArray();
            }
            foreach (var h in hs)
            {
                h.Invoke(sender, e);
            }
        }
    }
}
