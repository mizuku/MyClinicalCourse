namespace MyClinicalCourse
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 通知可能なオブジェクトの基底
    /// </summary>
    public class NotificationBase : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChangedイベントハンドラー
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 指定されたプロパティ名をパラメータに持つPropertyChangedイベントを発生します。
        /// </summary>
        /// <param name="propertyName">プロパティ名。省略時、呼び出し元のプロパティ名。</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null == propertyName ? string.Empty : propertyName));
        }

        /// <summary>
        /// value の値で storage を更新し、プロパティ更新時にPropertyChangedイベントを発生します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">更新するフィールド</param>
        /// <param name="value">更新する値</param>
        /// <param name="propertyName">プロパティ名。省略時、呼び出し元のプロパティ名。</param>
        /// <returns>プロパティを更新したとき true、更新しなかったとき false</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

    }
}
