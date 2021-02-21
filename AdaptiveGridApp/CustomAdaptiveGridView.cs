using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AdaptiveGridApp
{
    public partial class CustomAdaptiveGridView : GridView
    {
        private bool _needContainerMarginForLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAdaptiveGridView"/> class.
        /// </summary>
        public CustomAdaptiveGridView()
        {
            IsTabStop = false;
            ItemClick += OnItemClick;
            // Prevent issues with higher DPIs and underlying panel. #1803
            UseLayoutRounding = false;
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="obj">The element that's used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject obj, object item)
        {
            base.PrepareContainerForItemOverride(obj, item);
            if (obj is FrameworkElement element)
            {
                var heightBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ItemHeight"),
                    Mode = BindingMode.TwoWay
                };

                var widthBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ItemWidth"),
                    Mode = BindingMode.TwoWay
                };

                element.SetBinding(HeightProperty, heightBinding);
                element.SetBinding(WidthProperty, widthBinding);
            }

            if (obj is ContentControl contentControl)
            {
                contentControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                contentControl.VerticalContentAlignment = VerticalAlignment.Stretch;
            }

            if (_needContainerMarginForLayout)
            {
                _needContainerMarginForLayout = false;
                RecalculateLayout(ActualWidth);
            }
        }

        /// <summary>
        /// Calculates the width of the grid items.
        /// </summary>
        /// <param name="containerWidth">The width of the container control.</param>
        /// <returns>The calculated item width.</returns>
        protected virtual double CalculateItemWidth(double containerWidth)
        {
            if (double.IsNaN(DesiredWidth))
            {
                return DesiredWidth;
            }

            var columns = CalculateColumns(containerWidth, DesiredWidth);

            // If there's less items than there's columns, reduce the column count (if requested);
            if (Items != null && Items.Count > 0 && Items.Count < columns && StretchContentForSingleRow)
            {
                columns = Items.Count;
            }

            // subtract the margin from the width so we place the correct width for placement
            var fallbackThickness = default(Thickness);
            var itemMargin = AdaptiveHeightValueConverter.GetItemMargin(this, fallbackThickness);
            if (itemMargin == fallbackThickness)
            {
                // No style explicitly defined, or no items or no container for the items
                // We need to get an actual margin for proper layout
                _needContainerMarginForLayout = true;
            }

            return (containerWidth / columns) - itemMargin.Left - itemMargin.Right;
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var cmd = ItemClickCommand;
            if (cmd != null)
            {
                if (cmd.CanExecute(e.ClickedItem))
                {
                    cmd.Execute(e.ClickedItem);
                }
            }
        }
        private void RecalculateLayout(double containerWidth)
        {
            var panelMargin = ItemsPanelRoot is Panel itemsPanel ?
                              itemsPanel.Margin.Left + itemsPanel.Margin.Right :
                              0;
            var padding = Padding.Left + Padding.Right;
            var border = BorderThickness.Left + BorderThickness.Right;

            // width should be the displayable width
            containerWidth = containerWidth - padding - panelMargin - border;
            if (containerWidth > 0)
            {
                var newWidth = CalculateItemWidth(containerWidth);
                ItemWidth = Math.Floor(newWidth);
            }
        }

    }
}

