﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace AdaptiveGridApp
{
    public class CustomPanel : Panel
    {
        double cellwidth, cellheight, maxcellheight, LastRowcellwidth, LastRowcellheight, aspectratio;
        public int TotalColumns = 1;
        public int TotalRows = 1;
        public static int MinimumWidth = 250;
        public static int ItemMargin = 15;
        public static readonly DependencyProperty ListingControlProperty = DependencyProperty.Register(
                                                                           "ListingControl",
                                                                           typeof(ListViewBase),
                                                                       typeof(CustomPanel),
                                                                   new PropertyMetadata(null)
                                                                            );


        public ListViewBase ListingControl
        {
            get { return (ListViewBase)GetValue(ListingControlProperty); }
            set { SetValue(ListingControlProperty, value); }
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            if (MainPage.ScrollMode == ScrollMode.Vertical)
                MeasureForVerticalMode(availableSize);
            else
                MeasureForHorizontalMode(availableSize);
            return LimitUnboundedSize(availableSize);
        }

        private void MeasureForHorizontalMode(Size availableSize)
        {
            if (ListingControl != null && ListingControl.Parent is Grid grid)
            {
                double AvailableHeight = grid.RowDefinitions[1].ActualHeight;
                availableSize.Height = AvailableHeight;
                // Determine the square that can contain this number of items.
                ComputeAndSetDimension(availableSize, ScrollMode.Horizontal);
                // Get an aspect ratio from availableSize, decides whether to trim row or column.
                aspectratio = availableSize.Width / availableSize.Height;
                cellwidth = (int)Math.Floor(availableSize.Width / TotalColumns);
                // Next get a cell height, same logic of dividing available vertical by rowcount.
                cellheight = (cellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                LastRowcellwidth = cellwidth;
                LastRowcellheight = cellheight;
                int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
                int LastRowStartIndex;

                if (TotalRows <= 1)
                {
                    LastRowStartIndex = 0;
                }
                else
                {
                    LastRowStartIndex = (TotalRows - 1) * TotalColumns;
                }
                for (int i = 0; i < Children.Count; i++)
                {
                    UIElement child = Children[i];
                    if (MainPage.GridMode == GridMode.AspectFit || i < LastRowStartIndex)
                        child.Measure(new Size(cellwidth, cellheight));
                    else
                    {
                        LastRowcellwidth = availableSize.Width / LastRowTotalItems;
                        LastRowcellheight = (LastRowcellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                        child.Measure(new Size(LastRowcellwidth, LastRowcellheight));
                    }
                    maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
                }
            }
        }

        public void MeasureForVerticalMode(Size availableSize)
        {
            // Determine the square that can contain this number of items.
            ComputeAndSetDimension(availableSize);
            // Get an aspect ratio from availableSize, decides whether to trim row or column.
            aspectratio = availableSize.Width / availableSize.Height;
            cellwidth = (int)Math.Floor(availableSize.Width / TotalColumns);
            // Next get a cell height, same logic of dividing available vertical by rowcount.
            cellheight = (cellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
            LastRowcellwidth = cellwidth;
            LastRowcellheight = cellheight;
            int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
            int LastRowStartIndex;

            if (TotalRows <= 1)
            {
                LastRowStartIndex = 0;
            }
            else
            {
                LastRowStartIndex = (TotalRows - 1) * TotalColumns;
            }
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];
                if (MainPage.GridMode == GridMode.AspectFit || i < LastRowStartIndex)
                    child.Measure(new Size(cellwidth, cellheight));
                else
                {
                    LastRowcellwidth = availableSize.Width / LastRowTotalItems;
                    LastRowcellheight = (LastRowcellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                    child.Measure(new Size(LastRowcellwidth, LastRowcellheight));
                }
                maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
            }
        }
        Size LimitUnboundedSize(Size input)
        {
            if (double.IsInfinity(input.Height))
            {
                if (TotalRows == 0)
                    input.Height = 0;
                else
                    input.Height = cellheight * (TotalRows - 1) + LastRowcellheight;
            }
            return input;
        }

        private void ComputeAndSetDimension(Size availableSize, ScrollMode scrollMode = ScrollMode.Vertical)
        {
            if (scrollMode == ScrollMode.Vertical)
            {
                int cols = (int)Math.Ceiling(Math.Sqrt(Children.Count));
                if (cols > 0)
                    TotalColumns = cols;
                double itemWidth = availableSize.Width / TotalColumns;
                if (itemWidth < MinimumWidth)
                {
                    int MaxColumns = (int)(availableSize.Width / MinimumWidth);
                    TotalColumns = MaxColumns;
                }
                if (TotalColumns > 0)
                    TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
            }
            else
            {
                int cols = (int)Math.Ceiling(Math.Sqrt(Children.Count));
                if (cols > 0)
                    TotalColumns = cols;
                double itemWidth = availableSize.Width / TotalColumns;
                if (itemWidth < MinimumWidth)
                {
                    int MaxColumns = (int)(availableSize.Width / MinimumWidth);
                    TotalColumns = MaxColumns;
                }
                if (TotalColumns > 0)
                    TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
            }
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            if (MainPage.ScrollMode == ScrollMode.Vertical)
                ArrangeForVerticalMode(finalSize);
            else
                ArrangeForHorizontalMode(finalSize);
            return finalSize;
        }
        public void ArrangeForHorizontalMode(Size finalSize)
        {

        }

        public void ArrangeForVerticalMode(Size finalSize)
        {
            int count = 1;
            double x, y;
            if (Children.Count == 0)
                return;
            int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
            int LastRowStartIndex;
            if (TotalRows <= 1)
            {
                LastRowStartIndex = 0;
            }
            else
            {
                LastRowStartIndex = (TotalRows - 1) * TotalColumns;
            }
            for (int i = 0; i < LastRowStartIndex; i++)
            {
                UIElement child = Children[i];
                x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                y = (count - 1) / TotalColumns * child.DesiredSize.Height;
                Point anchorPoint = new Point(x, y);
                Rect rect = new Rect(anchorPoint, child.DesiredSize);
                if (child is GridViewItem item)
                {
                    item.CornerRadius = new CornerRadius(25);
                }
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                count++;
            }
            int CenterItemIndex = TotalColumns / 2 + TotalColumns % 2;
            for (int i = 1; i <= LastRowTotalItems; i++)
            {
                int indexFactor = LastRowStartIndex - 1;
                int index = indexFactor + i;
                UIElement child = Children[index];
                int centerX = (int)finalSize.Width / 2;
                y = (count - 1) / TotalColumns * cellheight;
                int offset = (LastRowTotalItems < CenterItemIndex) ? LastRowTotalItems : CenterItemIndex;
                int rightPanelOffset = LastRowTotalItems - offset;
                int leftPanelOffset = offset - 1;
                int horizontalOffset = leftPanelOffset - rightPanelOffset;
                double horizontalOffsetValue = 0;
                if (horizontalOffset > 0)
                {
                    horizontalOffsetValue = horizontalOffset * (child.DesiredSize.Width / 2);
                }
                if (TotalColumns == LastRowTotalItems)
                {
                    x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                }
                else if (i > CenterItemIndex)
                {
                    x = centerX + ((int)((i - CenterItemIndex - 1) * child.DesiredSize.Width)) + ((int)child.DesiredSize.Width / 2) + horizontalOffsetValue;
                }
                else
                {
                    x = centerX - ((int)((offset - i) * child.DesiredSize.Width)) - ((int)child.DesiredSize.Width / 2) + horizontalOffsetValue;
                }
                if (x < 0)
                    x = 0;
                if (child is GridViewItem item)
                {
                    item.CornerRadius = new CornerRadius(25);
                }
                Point anchorPoint = new Point(x, y);
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                count++;
            }
        }
    }
}
