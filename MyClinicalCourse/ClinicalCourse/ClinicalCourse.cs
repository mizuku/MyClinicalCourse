namespace MyClinicalCourse.ClinicalCourse
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;
    using MyClinicalCourse;
    using MyClinicalCourse.Event;
    using System.Reflection;
    using System.ComponentModel;
    using System.Collections.Specialized;

    /// <summary>
    /// 看護経過表のコントロールです。
    /// このコントロールを使用するには Axis プロパティと Series プロパティを適切に設定し、 DataSource プロパティにデータを指定する必要があります。
    /// </summary>
    [TemplatePart(Name = "PART_PlotGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_TopAxesLabelGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_TopAxesTickGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_LeftAxesLabelGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_LeftAxesTickGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_RightAxesLabelGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_RightAxesTickGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_BottomAxesLabelGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_BottomAxesTickGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_HeaderItems", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "PART_TableHeaderCells", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_TableRows", Type = typeof(ListBox))]
    [ContentProperty("DataSource")]
    public class ClinicalCourse : Control
    {
        static ClinicalCourse()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClinicalCourse), new FrameworkPropertyMetadata(typeof(ClinicalCourse)));
        }

        /// <summary>
        /// plot領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> plotGrid = null;
        /// <summary>
        /// Plotの上軸ラベル領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> topAxesLabelGrid = null;
        /// <summary>
        /// Plotの上軸目盛り領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> topAxesTickGrid = null;
        /// <summary>
        /// Plotの左軸ラベル領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> leftAxesLabelGrid = null;
        /// <summary>
        /// Plotの左軸目盛り領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> leftAxesTickGrid = null;
        /// <summary>
        /// Plotの下軸ラベル領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> bottomAxesLabelGrid = null;
        /// <summary>
        /// Plotの下軸目盛り領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> bottomAxesTickGrid = null;
        /// <summary>
        /// Plotの右軸ラベル領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> rightAxesLabelGrid = null;
        /// <summary>
        /// Plotの右軸目盛り領域のグリッドインスタンス
        /// </summary>
        private WeakReference<Grid> rightAxesTickGrid = null;
        /// <summary>
        /// 見出しのコレクションインスタンス
        /// </summary>
        private WeakReference<ItemsControl> headerItems = null;
        /// <summary>
        /// 表見出しのコレクションインスタンス
        /// </summary>
        private WeakReference<ListBox> tableHeaderCells = null;
        /// <summary>
        /// 表の行コレクションインスタンス
        /// </summary>
        private WeakReference<ListBox> tableRows = null;
        /// <summary>
        /// ソート済みの上Axisコレクション
        /// </summary>
        private List<Axis> sortedTopAxes = null;
        /// <summary>
        /// ソート済みの左Axisコレクション
        /// </summary>
        private List<Axis> sortedLeftAxes = null;
        /// <summary>
        /// ソート済みの下Axisコレクション
        /// </summary>
        private List<Axis> sortedBottomAxes = null;
        /// <summary>
        /// ソート済みの右Axisコレクション
        /// </summary>
        private List<Axis> sortedRightAxes = null;

        private IEnumerable<PropertyInfo> props = null;

        /// <summary>
        /// 看護表コントロールの新しいインスタンスを初期化します。
        /// </summary>
        public ClinicalCourse() : base()
        {
            if (this.Axes.Any()) this.Axes = new Axes();
            if (this.Series.Any()) this.Series = new SeriesCollection();
        }

        #region 依存関係プロパティ

        /// <summary>
        /// 見出しラベルのフィールド名 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("見出しラベルのフィールド名 を取得または設定します。")]
        public string LabelField
        {
            get { return (string)GetValue(LabelFieldProperty); }
            set { SetValue(LabelFieldProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelField.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelFieldProperty =
            DependencyProperty.Register("LabelField", typeof(string), typeof(ClinicalCourse),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
                (oo, ee) =>
                {
                    (oo as ClinicalCourse)?.DataSourceChanged();
                }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 表データのフィールド名 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("表データのフィールド名 を取得または設定します。")]
        public string TableDictionaryField
        {
            get { return (string)GetValue(TableDictionaryFieldProperty); }
            set { SetValue(TableDictionaryFieldProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TableDictionaryField.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TableDictionaryFieldProperty =
            DependencyProperty.Register("TableDictionaryField", typeof(string), typeof(ClinicalCourse),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                        (oo as ClinicalCourse)?.DataSourceChanged();
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 1列の幅を表す数値 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("1列の幅を表す数値 を取得または設定します。")]
        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.Register("ColumnWidth", typeof(double), typeof(ClinicalCourse),
                new FrameworkPropertyMetadata(100.0d, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                        var cc = oo as ClinicalCourse;
                        if (null != cc)
                        {
                            // シリーズ再描画 (プロット幅が変更になるため)
                            foreach (var a in cc.Axes)
                            {
                                cc.UpdateSeries(a);
                            }
                        }
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 表の1行の高さを表す数値 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("表の1行の高さを表す数値 を取得または設定します。")]
        public double TableRowHeight
        {
            get { return (double)GetValue(TableRowHeightProperty); }
            set { SetValue(TableRowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TableRowHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TableRowHeightProperty =
            DependencyProperty.Register("TableRowHeight", typeof(double), typeof(ClinicalCourse),
                new FrameworkPropertyMetadata(50.0d, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// プロットするデータソース を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("プロットするデータソース を取得または設定します。")]
        public IEnumerable<object> DataSource
        {
            get { return (IEnumerable<object>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(IEnumerable<object>), typeof(ClinicalCourse),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                        var cc = oo as ClinicalCourse;
                        if (null != cc)
                        {
                            if (null != ee.OldValue && ee.OldValue is INotifyCollectionChanged)
                            {
                                var oldCollection = (INotifyCollectionChanged)ee.OldValue;
                                oldCollection.CollectionChanged -= cc.OnDataSourceCollectionChanged;
                            }
                            if (null != ee.NewValue && ee.NewValue is INotifyCollectionChanged)
                            { 
                                var newCollection = (INotifyCollectionChanged)ee.NewValue;
                                newCollection.CollectionChanged += cc.OnDataSourceCollectionChanged;
                            }
                            cc.DataSourceChanged();
                        }
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// 軸 を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("軸 を取得または設定します。Bindingできないプロパティです。")]
        public Axes Axes
        {
            get { return (Axes)GetValue(AxesProperty); }
            set { SetValue(AxesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Axes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AxesProperty =
            DependencyProperty.Register("Axes", typeof(Axes), typeof(ClinicalCourse),
                new FrameworkPropertyMetadata(new Axes(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.NotDataBindable,
                    (oo, ee) =>
                    {
                        (oo as ClinicalCourse)?.AxesChanged();
                    }, null, false, UpdateSourceTrigger.PropertyChanged));


        /// <summary>
        /// シリーズ を取得または設定します。
        /// </summary>
        [Category("看護経過表")]
        [Description("シリーズ を取得または設定します。Bindingできないプロパティです。")]
        public SeriesCollection Series
        {
            get { return (SeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Series.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(SeriesCollection), typeof(ClinicalCourse),
                new FrameworkPropertyMetadata(new SeriesCollection(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.NotDataBindable,
                    (oo, ee) =>
                    {
                        (oo as ClinicalCourse)?.AxesChanged();
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        #endregion 依存関係プロパティ

        /// <summary>
        /// テンプレートが適用されたときの処理を実行します。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.plotGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_PlotGrid") as Grid);
            this.topAxesLabelGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_TopAxesLabelGrid") as Grid);
            this.topAxesTickGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_TopAxesTickGrid") as Grid);
            this.leftAxesLabelGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_LeftAxesLabelGrid") as Grid);
            this.leftAxesTickGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_LeftAxesTickGrid") as Grid);
            this.bottomAxesLabelGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_BottomAxesLabelGrid") as Grid);
            this.bottomAxesTickGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_BottomAxesTickGrid") as Grid);
            this.rightAxesLabelGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_RightAxesLabelGrid") as Grid);
            this.rightAxesTickGrid = new WeakReference<Grid>(this.GetTemplateChild("PART_RightAxesTickGrid") as Grid);
            this.headerItems = new WeakReference<ItemsControl>(this.GetTemplateChild("PART_HeaderItems") as ItemsControl);
            this.tableHeaderCells = new WeakReference<ListBox>(this.GetTemplateChild("PART_TableHeaderCells") as ListBox);
            this.tableRows = new WeakReference<ListBox>(this.GetTemplateChild("PART_TableRows") as ListBox);

            this.AxesChanged();
            this.DataSourceChanged();
        }

        /// <summary>
        /// 軸の定義が変更されたときの処理を実行します。
        /// </summary>
        private void AxesChanged()
        {
            // 軸のindexをGridのindexに変換するローカル関数
            int i2Index(AxisPosition position, int i, int length)
            {
                switch (position)
                {
                    case AxisPosition.Bottom:
                    case AxisPosition.Right:
                        return i;
                    case AxisPosition.Top:
                    case AxisPosition.Left:
                    default:
                        return length - (i + 1);
                }
            }

            // ラベル軸をラベルグリッドに設定するローカル関数
            void setAxesToLabelGrid(AxisPosition position, ref List<Axis> sortedAxes, ref WeakReference<Grid> labelGrid)
            {
                if (null != labelGrid && labelGrid.TryGetTarget(out var lg))
                {
                    lg.Children.Clear();
                    foreach (var (axis, i) in sortedAxes.Select((a, i) => (a, i)))
                    {
                        if (position.IsSideAxis())
                        {
                            lg.ColumnDefinitions.Add(new ColumnDefinition()
                            {
                                Width = new GridLength(double.IsNaN(axis.Width) ? 30.0d : axis.Width, GridUnitType.Pixel)
                            });
                            axis.SetValue(Grid.ColumnProperty, i2Index(position, i, sortedAxes.Count));
                            lg.Children.Add(axis);
                        }
                        else
                        {
                            lg.RowDefinitions.Add(new RowDefinition()
                            {
                                Height = new GridLength(double.IsNaN(axis.Height) ? 30.0d : axis.Height, GridUnitType.Pixel)
                            });
                            axis.SetValue(Grid.RowProperty, i2Index(position, i, sortedAxes.Count));
                            lg.Children.Add(axis);
                        }
                    }
                }
            }

            // 目盛り軸を目盛りグリッドに設定するローカル関数
            void setAxesToTickGrid(AxisPosition position, ref List<Axis> sortedAxes, ref WeakReference<Grid> tickGrid)
            {
                if (sortedAxes.Any() && null != tickGrid && tickGrid.TryGetTarget(out var tg))
                {
                    tg.Children.Clear();
                    foreach (var axis in sortedAxes)
                    {
                        var axisTick = new AxisTick();
                        tg.Children.Add(axisTick);
                        // TODO removeHandler
                        axis.AxisTickChangedEvent.RemoveHandler(axisTick.OnAxisTickChanged);
                        axis.AxisTickChangedEvent.AddHandler(axisTick.OnAxisTickChanged);
                    }
                }
            }


            // 上軸
            {
                this.sortedTopAxes = Axes.Where(a => AxisPosition.Top == a.Position)
                                         .OrderBy(a => a.Index)
                                         .ToList();
                setAxesToLabelGrid(AxisPosition.Top, ref this.sortedTopAxes, ref this.topAxesLabelGrid);
                setAxesToTickGrid(AxisPosition.Top, ref this.sortedTopAxes, ref this.topAxesTickGrid);
            }
            // 左軸
            {
                this.sortedLeftAxes = Axes.Where(a => AxisPosition.Left == a.Position)
                                          .OrderBy(a => a.Index)
                                          .ToList();
                setAxesToLabelGrid(AxisPosition.Left, ref this.sortedLeftAxes, ref this.leftAxesLabelGrid);
                setAxesToTickGrid(AxisPosition.Left, ref this.sortedLeftAxes, ref this.leftAxesTickGrid);
            }
            // 下軸
            {
                this.sortedBottomAxes = Axes.Where(a => AxisPosition.Bottom == a.Position)
                                            .OrderBy(a => a.Index)
                                            .ToList();
                setAxesToLabelGrid(AxisPosition.Bottom, ref this.sortedBottomAxes, ref this.bottomAxesLabelGrid);
                setAxesToTickGrid(AxisPosition.Bottom, ref this.sortedBottomAxes, ref this.bottomAxesTickGrid);
            }
            // 右軸
            {
                this.sortedRightAxes = Axes.Where(a => AxisPosition.Right == a.Position)
                                           .OrderBy(a => a.Index)
                                           .ToList();
                setAxesToLabelGrid(AxisPosition.Right, ref this.sortedRightAxes, ref this.rightAxesLabelGrid);
                setAxesToTickGrid(AxisPosition.Right, ref this.sortedRightAxes, ref this.rightAxesTickGrid);
            }


            foreach (var axis in this.Axes)
            {
                axis.AxisChangedEvent.RemoveHandler(this.OnAxisChanged);
                axis.AxisChangedEvent.AddHandler(this.OnAxisChanged);
            }

            if (this.Series.Any() && null != this.plotGrid && this.plotGrid.TryGetTarget(out var pg))
            {
                pg.Children.Clear();
                foreach (var s in this.Series)
                {
                    pg.Children.Add(s);
                }
            }

        }

        /// <summary>
        /// データソースが変更されたときの処理を実行します。
        /// </summary>
        private void DataSourceChanged()
        {
            var ds = this.DataSource?.ToArray();
            if (null != ds && ds.Any())
            {
                // ヘッダーラベル
                if (null != headerItems && headerItems.TryGetTarget(out var ic))
                {
                    // TODO: 雑、っていうかたぶん遅い
                    this.props = ds.First().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Static);
                    if (!string.IsNullOrWhiteSpace(this.LabelField))
                    {
                        var labelProp = props.FirstOrDefault(p => p.Name == this.LabelField);
                        if (null != labelProp)
                        {
                            var labels = new List<HeaderModel>(ds.Length);
                            foreach (var item in ds)
                            {
                                // TODO: ボクシングやめたい
                                labels.Add(new HeaderModel(labelProp.GetValue(item).ToString(), this.ColumnWidth));
                            }
                            ic.ItemsSource = labels;
                        }
                    }
                }

                // 表
                if (null != this.tableHeaderCells && this.tableHeaderCells.TryGetTarget(out var th)
                    && null != this.tableRows && this.tableRows.TryGetTarget(out var tr))
                {
                    // TODO 直したい
                    var firstItem = ds.First();
                    this.props = firstItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Static);
                    if (!string.IsNullOrWhiteSpace(this.TableDictionaryField))
                    {
                        var tableProp = props.FirstOrDefault(p => p.Name == this.TableDictionaryField);
                        if (null != tableProp)
                        {
                            var keyValue = tableProp.GetValue(firstItem) as IDictionary;
                            if (null != keyValue)
                            {
                                var keys = new List<string>(keyValue.Keys.Count);
                                foreach (var key in keyValue.Keys)
                                {
                                    keys.Add(key.ToString());
                                }
                                th.ItemsSource = keys;

                                // 縱橫を逆転させる つらい
                                var valuesList = Enumerable.Range(0, keys.Count)
                                    .Select(i => new List<string>(ds.Length))
                                    .ToList();
                                for (int t = 0; t < ds.Length; ++t)
                                {
                                    var dict = tableProp.GetValue(ds[t]) as IDictionary;
                                    int i = 0;
                                    foreach (var v in dict.Values)
                                    {
                                        valuesList[i].Add(v.ToString());
                                        ++i;
                                    }
                                }
                                tr.ItemsSource = valuesList;
                            }
                        }
                    }
                }

                // シリーズも再描画
                foreach (var axis in this.Axes)
                {
                    UpdateSeries(axis);
                }
            }
        }

        private void OnDataSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.DataSourceChanged();
        }

        private void OnAxisChanged(object sender, AxisChangedEventArgs e)
        {
            var axis = sender as Axis;
            if (null != axis)
            {
                axis.Checked -= Axis_Checked;
                axis.Unchecked -= Axis_Checked;
                axis.Checked += Axis_Checked;
                axis.Unchecked += Axis_Checked;
                this.UpdateSeries(axis);
            }
        }

        private void Axis_Checked(object sender, RoutedEventArgs e)
        {
            var axis = sender as Axis;
            var series = this.Series.Where(s => !string.IsNullOrWhiteSpace(s.DataField));
            if (null != axis && series.Any())
            {
                var ser = series.FirstOrDefault(s => s.AxisName == axis.Name);
                if (null != ser)
                {
                    ser.Visibility = (axis.IsChecked.HasValue && axis.IsChecked.Value) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void UpdateSeries(Axis axis)
        {
            var series = this.Series.Where(s => !string.IsNullOrWhiteSpace(s.DataField));
            if (series.Any())
            {
                foreach (var s in series.Where(s => s.AxisName == axis.Name))
                {
                    // TODO: なにもかもダメ
                    var ps = this.DataSource?.ToArray();
                    if (null != ps && ps.Any())
                    {
                        this.props = ps.First().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Static);
                        var dataProp = props.FirstOrDefault(p => p.Name == s.DataField);
                        if (null != dataProp)
                        {
                            var points = new List<Point>(ps.Length);
                            int i = 0;
                            var oneSize = axis.GetOneSize();
                            s.Visibility = (axis.IsChecked.HasValue && axis.IsChecked.Value) ? Visibility.Visible : Visibility.Collapsed;
                            foreach (var item in ps)
                            {
                                if (double.TryParse(dataProp.GetValue(item).ToString(), out var d))
                                {
                                    points.Add(new Point((i + 0.5) * this.ColumnWidth, (d * oneSize) - (axis.Minimum * oneSize)));
                                }
                                ++i;
                            }
                            s.SetPlotPoints(points);
                        }
                    }
                }
            }
        }

    }
}
