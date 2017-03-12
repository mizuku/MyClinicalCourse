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
    /// プロットのマーカーコントロールです。
    /// </summary>
    public class Markers : ListBox
    {
        static Markers()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Markers), new FrameworkPropertyMetadata(typeof(Markers)));
        }

        /// <summary>
        /// マーカーの Template を取得または設定します。
        /// </summary>
        [Category("プロット")]
        [Description("マーカーの Template を取得または設定します。")]
        public DataTemplate MarkerTemplate
        {
            get { return (DataTemplate)GetValue(MarkerTemplateProperty); }
            set { SetValue(MarkerTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkerTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkerTemplateProperty =
            DependencyProperty.Register("MarkerTemplate", typeof(DataTemplate), typeof(Markers),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (oo, ee) =>
                    {
                    }, null, false, UpdateSourceTrigger.PropertyChanged));

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var container = ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
            if (null != container)
            {
                container.SizeChanged += Container_SizeChanged;
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            var container = ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
            if (null != container)
            {
                container.SizeChanged -= Container_SizeChanged;
            }
        }

        private void Container_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var container = sender as ListBoxItem;
            if (null != container)
            {
                // 常に座標に対してセンタリングするように変形
                container.RenderTransform = new TranslateTransform(-e.NewSize.Width / 2.0d, -e.NewSize.Height / 2.0d);
            }
        }
    }
}
