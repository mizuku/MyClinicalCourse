namespace MyClinicalCourse.ClinicalCourse
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// 看護経過表の軸目盛り コントロールです。
    /// このコントロールを利用者が直接作成することはありません。
    /// </summary>
    internal class AxisTick : Control
    {
        static AxisTick()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AxisTick), new FrameworkPropertyMetadata(typeof(AxisTick)));
        }

        internal AxisTick()
        {
        }

        #region 依存関係プロパティ


        /// <summary>
        /// 軸の配置場所を表す値を を取得します。
        /// </summary>
        /// <remarks>このプロパティは読み取り専用です。</remarks>
        [Category("看護経過表")]
        [Description("軸の配置場所を表す値を を取得します。このプロパティは読み取り専用です。")]
        public AxisPosition Position
        {
            get { return (AxisPosition)GetValue(PositionProperty); }
            private set { SetValue(PositionPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey PositionPropertyKey =
            DependencyProperty.RegisterReadOnly("Position", typeof(AxisPosition), typeof(AxisTick),
                new FrameworkPropertyMetadata(AxisPosition.Left, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                    (oo, ee) =>
                    {
                        var sender = oo as AxisTick;
                        sender?.OnPositionPropertyChanged(sender.Position);
                    }, null, false, UpdateSourceTrigger.PropertyChanged));
        public static readonly DependencyProperty PositionProperty = PositionPropertyKey.DependencyProperty;

        /// <summary>
        /// 軸目盛り領域の幅 を取得します。
        /// </summary>
        /// <remarks>このプロパティは読み取り専用です。</remarks>
        [Category("看護経過表")]
        [Description("軸目盛り領域の幅 を取得します。このプロパティは読み取り専用です。")]
        public double TickAreaWidth
        {
            get { return (double)GetValue(TickAreaWidthProperty); }
            private set { SetValue(TickAreaWidthPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for TickAreaWidth.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey TickAreaWidthPropertyKey =
            DependencyProperty.RegisterReadOnly("TickAreaWidth", typeof(double), typeof(AxisTick),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    null, null, false, UpdateSourceTrigger.PropertyChanged));
        public static readonly DependencyProperty TickAreaWidthProperty = TickAreaWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// 軸目盛り領域の高さ を取得します。
        /// </summary>
        /// <remarks>このプロパティは読み取り専用です。</remarks>
        [Category("看護経過表")]
        [Description("軸目盛り領域の高さ を取得します。このプロパティは読み取り専用です。")]
        public double TickAreaHeight
        {
            get { return (double)GetValue(TickAreaHeightProperty); }
            private set { SetValue(TickAreaHeightPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for TickAreaHeight.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey TickAreaHeightPropertyKey =
            DependencyProperty.RegisterReadOnly("TickAreaHeight", typeof(double), typeof(AxisTick),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    null, null, false, UpdateSourceTrigger.PropertyChanged));
        public static readonly DependencyProperty TickAreaHeightProperty = TickAreaHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// 軸目盛りのコレクション を取得します。
        /// </summary>
        /// <remarks>このプロパティは読み取り専用です。</remarks>
        [Category("看護経過表")]
        [Description("軸目盛りのコレクション を取得します。このプロパティは読み取り専用です。")]
        public IEnumerable<AxisTickModel> TickItems
        {
            get { return (IEnumerable<AxisTickModel>)GetValue(TickItemsProperty); }
            private set { SetValue(TickItemsPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for TickItems.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey TickItemsPropertyKey =
            DependencyProperty.RegisterReadOnly("TickItems", typeof(IEnumerable<AxisTickModel>), typeof(AxisTick),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, null, null, false, UpdateSourceTrigger.PropertyChanged));
        public static readonly DependencyProperty TickItemsProperty = TickItemsPropertyKey.DependencyProperty;

        #endregion

        /// <summary>
        /// 軸目盛りの変更イベントハンドラー
        /// </summary>
        /// <param name="sender">イベントの発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        internal void OnAxisTickChanged(object sender, AxisTickChangedEventArgs e)
        {
            this.Position = e.Position;
            this.TickItems = e.Ticks.ToList();
            this.TickAreaWidth = e.Position.IsSideAxis() ? e.TickAreaSize : double.NaN;
            this.TickAreaHeight = e.Position.IsSideAxis() ? double.NaN : e.TickAreaSize;
            this.Visibility = e.IsShowTick ? Visibility.Visible : Visibility.Collapsed;

            VisualStateManager.GoToState(this, this.Position.ToString(), false);
        }

        /// <summary>
        /// 軸の配置変更時の処理
        /// </summary>
        /// <param name="position">軸の配置</param>
        private void OnPositionPropertyChanged(AxisPosition position)
        {
            VisualStateManager.GoToState(this, position.ToString(), false);
        }

    }
}
