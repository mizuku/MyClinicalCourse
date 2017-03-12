namespace MyClinicalCourse.ClinicalCourse
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// 看護経過表のシリーズ コントロールです。
    /// このコントロールは ClinicalCourse コントロール、および Axis コントロールと合わせて使う機能として作成しています。
    /// </summary>
    public class Series : Control
    {
        static Series()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Series), new FrameworkPropertyMetadata(typeof(Series)));
        }

        #region 依存関係プロパティ

        /// <summary>
        /// データのフィールド名 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("データのフィールド名 を取得または設定します。")]
        public string DataField
        {
            get { return (string)GetValue(DataFieldProperty); }
            set { SetValue(DataFieldProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataField.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataFieldProperty =
            DependencyProperty.Register("DataField", typeof(string), typeof(Series),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// このシリーズに関連付けられる Axis コントロールの名前 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("このシリーズに関連付けられる Axis コントロールの名前 を取得または設定します。")]
        public string AxisName
        {
            get { return (string)GetValue(AxisNameProperty); }
            set { SetValue(AxisNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AxisName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AxisNameProperty =
            DependencyProperty.Register("AxisName", typeof(string), typeof(Series),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// グラフの線の描画に使用する Brush を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("グラフの線の描画に使用する Brush を取得または設定します。")]
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stroke.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(Series),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender,
                    (oo, ee) =>
                    {
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// グラフの線を描画する太さ を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("グラフの線を描画する太さ を取得または設定します。")]
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Series),
                new FrameworkPropertyMetadata(1.0d, FrameworkPropertyMetadataOptions.AffectsRender,
                    (oo, ee) =>
                    {
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// グラフと凡例に描画するマーカーの DataTemplate を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("グラフと凡例に描画するマーカーの DataTemplate を取得または設定します。")]
        public DataTemplate MarkerTemplate
        {
            get { return (DataTemplate)GetValue(MarkerTemplateProperty); }
            set { SetValue(MarkerTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkerTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkerTemplateProperty =
            DependencyProperty.Register("MarkerTemplate", typeof(DataTemplate), typeof(Series), new FrameworkPropertyMetadata(null));


        /// <summary>
        /// グラフ描画に使用する Point のコレクション を取得します。
        /// </summary>
        /// <remarks>このプロパティは読み取り専用です。</remarks>
        [Category("看護経過表")]
        [Description("グラフ描画に使用する Point のコレクション を取得します。このプロパティは読み取り専用です。")]
        public PointCollection PlotPoints
        {
            get { return (PointCollection)GetValue(PlotPointsProperty); }
            private set { SetValue(PlotPointsPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for PlotPoints.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey PlotPointsPropertyKey =
            DependencyProperty.RegisterReadOnly("PlotPoints", typeof(PointCollection), typeof(Series),
                new FrameworkPropertyMetadata(new PointCollection(), FrameworkPropertyMetadataOptions.AffectsArrange,
                    (oo, ee) =>
                    {
                    }, null, false, UpdateSourceTrigger.PropertyChanged));
        public static DependencyProperty PlotPointsProperty = PlotPointsPropertyKey.DependencyProperty;

        #endregion 依存関係プロパティ

        /// <summary>
        /// グラフ描画に使用する Point コレクションを設定します。
        /// </summary>
        /// <param name="plotPoints">設定する Point コレクション</param>
        internal void SetPlotPoints(IEnumerable<Point> plotPoints)
        {
            this.PlotPoints = new PointCollection(plotPoints);
        }
    }
}
