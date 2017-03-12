namespace MyClinicalCourse.ClinicalCourse
{
    using MyClinicalCourse.Event;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// 看護経過表の軸 コントロールです。
    /// このコントロールは ClinicalCourse コントロールと合わせて使う機能として作成しています。
    /// </summary>
    public class Axis : System.Windows.Controls.Primitives.ToggleButton
    {
        static Axis()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Axis), new FrameworkPropertyMetadata(typeof(Axis)));
        }

        /// <summary>
        /// 軸コントロールの新しいインスタンスを初期化します。
        /// </summary>
        public Axis() : base()
        {
        }

        #region プロパティ

        /// <summary>
        /// 軸全体の大きさ
        /// </summary>
        private double TotalAxisSize { get; set; } = 0.0d;

        /// <summary>
        /// 軸の目盛りコレクション
        /// </summary>
        private List<AxisTickModel> Ticks { get; set; } = null;

        #endregion プロパティ

        #region 依存関係プロパティ

        /// <summary>
        /// 軸の最大値を表す数値 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("軸の最大値を表す数値 を取得または設定します。")]
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Axis),
                new FrameworkPropertyMetadata(100.0d, FrameworkPropertyMetadataOptions.AffectsArrange,
                    (oo, ee) =>
                    {
                        var sender = oo as Axis;
                        sender?.AxisChanged();
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 軸の最小値を表す数値 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("軸の最小値を表す数値 を取得または設定します。")]
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Axis),
                new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsArrange,
                    (oo, ee) =>
                    {
                        var sender = oo as Axis;
                        sender?.AxisChanged();
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 軸の配置場所を表す値を 取得または設定します。
        /// </summary>
        /// <remarks>Left以外の値は未対応です。指定するとコントロールの表示が正常でなくなる可能性があります。</remarks>
        [Category("看護経過表")]
        [Description("軸の配置場所を表す値を 取得または設定します。Left以外の値は未対応です。指定するとコントロールの表示が正常でなくなる可能性があります。")]
        public AxisPosition Position
        {
            get { return (AxisPosition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(AxisPosition), typeof(Axis),
                new FrameworkPropertyMetadata(AxisPosition.Left, FrameworkPropertyMetadataOptions.AffectsArrange,
                    (oo, ee) =>
                    {
                        var sender = oo as Axis;
                        sender?.PositionPropertyChanged(sender.Position);
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 配置が同じ軸内の配置順を表すインデックス を取得または設定します。<br>
        /// 値が小さいほど表の内側に配置されます。
        /// </summary>
        [Category("看護経過表")]
        [Description("配置が同じ軸内の配置順を表すインデックス を取得または設定します。<br>値が小さいほど表の内側に配置されます。")]
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(Axis),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange,
                    (oo, ee) =>
                    {
                        var sender = oo as Axis;
                        sender?.AxisChanged();
                    }, null, false, UpdateSourceTrigger.PropertyChanged));


        /// <summary>
        /// 目盛りの可視性 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("目盛りの可視性 を取得または設定します。")]
        public bool IsShowTick
        {
            get { return (bool)GetValue(IsShowTickProperty); }
            set { SetValue(IsShowTickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowTick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowTickProperty =
            DependencyProperty.Register("IsShowTick", typeof(bool), typeof(Axis),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                    (oo, ee) =>
                    {
                        (oo as Axis)?.IsShowTickPropertyChanged();
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 目盛り領域の大きさ を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("目盛り領域の大きさ を取得または設定します。")]
        public double TickAreaSize
        {
            get { return (double)GetValue(TickAreaSizeProperty); }
            set { SetValue(TickAreaSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickAreaSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickAreaSizeProperty =
            DependencyProperty.Register("TickAreaSize", typeof(double), typeof(Axis),
                new FrameworkPropertyMetadata(15.0d, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                        var sender = oo as Axis;
                        sender?.TickAreaSizePropertyChanged(sender.TickAreaSize);
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 長針の間隔を表す値 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("長針の間隔を表す値 を取得または設定します。")]
        public double MajorInterval
        {
            get { return (double)GetValue(MajorIntervalProperty); }
            set { SetValue(MajorIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MajorInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MajorIntervalProperty =
            DependencyProperty.Register("MajorInterval", typeof(double), typeof(Axis),
                new FrameworkPropertyMetadata(50.0d, FrameworkPropertyMetadataOptions.AffectsArrange,
                    (oo, ee) =>
                    {
                        (oo as Axis)?.TickIntervalPropertyChanged(TickType.Major);
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 短針の間隔を表す値 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("短針の間隔を表す値 を取得または設定します。")]
        public double MinorInterval
        {
            get { return (double)GetValue(MinorIntervalProperty); }
            set { SetValue(MinorIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinorInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinorIntervalProperty =
            DependencyProperty.Register("MinorInterval", typeof(double), typeof(Axis),
                new FrameworkPropertyMetadata(10.0d, FrameworkPropertyMetadataOptions.AffectsArrange,
                    (oo, ee) =>
                    {
                        (oo as Axis)?.TickIntervalPropertyChanged(TickType.Minor);
                    }, null, false, UpdateSourceTrigger.PropertyChanged));


        /// <summary>
        /// 長針の描画に使用する Brush を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("長針の描画に使用する Brush を取得または設定します。")]
        public Brush MajorTick
        {
            get { return (Brush)GetValue(MajorTickProperty); }
            set { SetValue(MajorTickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MajorTick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MajorTickProperty =
            DependencyProperty.Register("MajorTick", typeof(Brush), typeof(Axis),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender,
                    (oo, ee) =>
                    {
                        var sender = oo as Axis;
                        sender?.TickPropertyChanged(TickType.Major, sender.MajorTick);
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 短針の描画に使用する Brush を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("短針の描画に使用する Brush を取得または設定します。")]
        public Brush MinorTick
        {
            get { return (Brush)GetValue(MinorTickProperty); }
            set { SetValue(MinorTickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinorTick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinorTickProperty =
            DependencyProperty.Register("MinorTick", typeof(Brush), typeof(Axis),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender,
                    (oo, ee) =>
                    {
                        var sender = oo as Axis;
                        sender?.TickPropertyChanged(TickType.Minor, sender.MinorTick);
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 軸ラベルのコレクション を取得します。
        /// </summary>
        /// <remarks>このプロパティは読み取り専用です。</remarks>
        [Category("看護経過表")]
        [Description("軸ラベルのコレクション を取得します。このプロパティは読み取り専用です。")]
        public IEnumerable<AxisTickModel> LabelItems
        {
            get { return (IEnumerable<AxisTickModel>)GetValue(LabelItemsProperty); }
            private set { SetValue(LabelItemsPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for LabelItems.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey LabelItemsPropertyKey =
            DependencyProperty.RegisterReadOnly("LabelItems", typeof(IEnumerable<AxisTickModel>), typeof(Axis),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, null, null, false, UpdateSourceTrigger.PropertyChanged));
        public static readonly DependencyProperty LabelItemsProperty = LabelItemsPropertyKey.DependencyProperty;

        #endregion 依存関係プロパティ

        #region イベント

        /// <summary>
        /// この軸の情報変更イベント
        /// </summary>
        internal WeakEvent<AxisTickChangedEventArgs> AxisTickChangedEvent = new WeakEvent<AxisTickChangedEventArgs>();
        /// <summary>
        /// 軸全体に影響する変更イベント
        /// </summary>
        internal WeakEvent<AxisChangedEventArgs> AxisChangedEvent = new WeakEvent<AxisChangedEventArgs>();

        #endregion イベント

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.TotalAxisSize = this.Position.IsSideAxis() ? this.ActualHeight : this.ActualWidth;
            this.InitializeTicks();
            this.SizeChanged += OnTotalAxisSizeChanged;

            VisualStateManager.GoToState(this, this.Position.ToString(), false);
        }

        /// <summary>
        /// この軸の初期化処理
        /// </summary>
        private void InitializeTicks()
        {
            var oneSize = this.GetOneSize();
            var numRange = this.Maximum - this.Minimum;
            var majorItemNum = (int)Math.Floor(numRange / this.MajorInterval);
            var minorItemNum = (int)Math.Floor(numRange / this.MinorInterval);

            var list = new List<AxisTickModel>()
            {
                new AxisTickModel(TickType.Major, this.Minimum, 0.0, oneSize, this.MajorTick)
            };
            var ticks = list
                .Concat(Enumerable.Range(1, majorItemNum)
                    .Select(i => new AxisTickModel(TickType.Major, this.Minimum, i * this.MajorInterval, oneSize, this.MajorTick))
                )
                .Concat(Enumerable.Range(1, minorItemNum)
                    .Select(i => new AxisTickModel(TickType.Minor, this.Minimum, i * this.MinorInterval, oneSize, this.MinorTick))
                );
            this.Ticks = ticks.OrderBy(tm => (tm.Distance, tm.TickType)).Distinct().ToList();

            this.LabelItems = new ObservableCollection<AxisTickModel>(this.Ticks);
        }

        /// <summary>
        /// 軸全体の高さ変更イベントハンドラー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTotalAxisSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.TotalAxisSize = this.Position.IsSideAxis() ? e.NewSize.Height : e.NewSize.Width;
            this.InitializeTicks();

            // 軸変更のイベント発生
            this.AxisTickChangedEvent.RaiseEvent(this, new AxisTickChangedEventArgs(this.Ticks, this.Position, this.TotalAxisSize, this.TickAreaSize, this.IsShowTick));
            this.AxisChangedEvent.RaiseEvent(this, new AxisChangedEventArgs(this.Position, this.GetOneSize(), this.Minimum, this.TotalAxisSize));
        }

        /// <summary>
        /// 軸の変更で全体に及ぶ再構築が必要な処理
        /// </summary>
        private void AxisChanged()
        {
            this.InitializeTicks();

            // 軸変更のイベント発生
            this.AxisTickChangedEvent.RaiseEvent(this, new AxisTickChangedEventArgs(this.Ticks, this.Position, this.TotalAxisSize, this.TickAreaSize, this.IsShowTick));
            this.AxisChangedEvent.RaiseEvent(this, new AxisChangedEventArgs(this.Position, this.GetOneSize(), this.Minimum, this.TotalAxisSize));
        }

        /// <summary>
        /// 軸の目盛り可視性変更時の処理
        /// </summary>
        private void IsShowTickPropertyChanged()
        {
            // 軸変更のイベント発生
            this.AxisTickChangedEvent.RaiseEvent(this, new AxisTickChangedEventArgs(this.Ticks, this.Position, this.TotalAxisSize, this.TickAreaSize, this.IsShowTick));
        }

        /// <summary>
        /// 軸領域の大きさ変更時の処理
        /// </summary>
        /// <param name="tickAreaSize">変更後の軸領域の大きさ</param>
        private void TickAreaSizePropertyChanged(double tickAreaSize)
        {
            // 軸変更のイベント発生
            this.AxisTickChangedEvent.RaiseEvent(this, new AxisTickChangedEventArgs(this.Ticks, this.Position, this.TotalAxisSize, tickAreaSize, this.IsShowTick));
        }

        /// <summary>
        /// 軸の配置変更時の処理
        /// </summary>
        /// <param name="position">変更後の配置</param>
        private void PositionPropertyChanged(AxisPosition position)
        {
            VisualStateManager.GoToState(this, position.ToString(), false);
            // 軸変更のイベント発生
            this.AxisTickChangedEvent.RaiseEvent(this, new AxisTickChangedEventArgs(this.Ticks, position, this.TotalAxisSize, this.TickAreaSize, this.IsShowTick));
        }

        /// <summary>
        /// 軸の目盛りの種類変更時の処理
        /// </summary>
        /// <param name="tickType">軸の目盛りの種類</param>
        /// <param name="tick">軸の目盛りを描画する Brush</param>
        private void TickPropertyChanged(TickType tickType, Brush tick)
        {
            foreach (var l in this.LabelItems.Where(l => tickType == l.TickType))
            {
                l.Tick = tick;
            }
            // 軸変更のイベント発生
            this.AxisTickChangedEvent.RaiseEvent(this, new AxisTickChangedEventArgs(this.Ticks, this.Position, this.TotalAxisSize, this.TickAreaSize, this.IsShowTick));
        }

        /// <summary>
        /// 軸の目盛りの間隔変更時の処理
        /// </summary>
        /// <param name="tickType">軸の目盛りの種類</param>
        private void TickIntervalPropertyChanged(TickType tickType)
        {
            this.InitializeTicks();
            // 軸変更のイベント発生
            this.AxisTickChangedEvent.RaiseEvent(this, new AxisTickChangedEventArgs(this.Ticks, this.Position, this.TotalAxisSize, this.TickAreaSize, this.IsShowTick));
        }

        /// <summary>
        /// 値1あたりの描画上の大きさを取得します。
        /// </summary>
        /// <returns>値1あたりの描画上の大きさ</returns>
        internal double GetOneSize() => this.TotalAxisSize / (this.Maximum - this.Minimum);
    }

    /// <summary>
    /// 軸の目盛り変更イベント引数。ラベルと目盛りを別コントロールにしなければならなかったため、必要な情報を伝播します。
    /// </summary>
    internal class AxisTickChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 軸の目盛りコレクション
        /// </summary>
        internal IEnumerable<AxisTickModel> Ticks { get; set; } = null;

        /// <summary>
        /// 軸の位置
        /// </summary>
        internal AxisPosition Position { get; set; } = AxisPosition.Top;

        /// <summary>
        /// 軸全体の大きさ
        /// </summary>
        internal double TotalAxisSize { get; set; } = 0.0d;

        /// <summary>
        /// 目盛り領域の大きさ
        /// </summary>
        internal double TickAreaSize { get; set; } = 0.0d;

        /// <summary>
        /// 軸目盛りの可視性を表すbool値 を取得または設定します。
        /// </summary>
        internal bool IsShowTick { get; set; }

        /// <summary>
        /// このイベント引数の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="ticks"></param>
        internal AxisTickChangedEventArgs(IEnumerable<AxisTickModel> ticks, AxisPosition position, double totalAxisSize, double tickAreaSize, bool isShowTick) : base()
        {
            this.Ticks = ticks;
            this.Position = position;
            this.TotalAxisSize = totalAxisSize;
            this.TickAreaSize = tickAreaSize;
            this.IsShowTick = isShowTick;
        }
    }

    /// <summary>
    /// 軸の変更イベント引数。看護表の全体に及ぶ大きな変更のときに必要な情報を伝播します。
    /// </summary>
    internal class AxisChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 軸の位置 を取得または設定します。
        /// </summary>
        internal AxisPosition Position { get; set; } = AxisPosition.Left;

        /// <summary>
        /// 値1あたりの描画上の大きさを表す数値 を取得または設定します。
        /// </summary>
        internal double OneSize { get; set; } = 0.0d;

        /// <summary>
        /// 軸の最小値を表す数値 を取得または設定します。
        /// </summary>
        internal double Minimum { get; set; } = 0.0d;

        /// <summary>
        /// 軸全体の大きさ を取得または設定します。
        /// </summary>
        internal double TotalAxisSize { get; set; } = 0.0d;

        /// <summary>
        /// このイベント引数の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="ticks"></param>
        internal AxisChangedEventArgs(AxisPosition position, double oneSize, double minimum, double totalAxisSize) : base()
        {
            this.Position = position;
            this.OneSize = oneSize;
            this.Minimum = minimum;
            this.TotalAxisSize = totalAxisSize;
        }
    }

}
