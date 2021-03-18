using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdaptiveGridApp
{
    public class ActiveSpeakerCustomPanel : Panel
    {
        double cellwidth, cellheight;
        public int TotalColumns = 0;
        public int TotalColumnsWithinViewPort = 1;
        public int MaxRowsWithinViewPort = 1;
        public int TotalRows = 0;
        public int MaxRightPortItems = 1;
        public int MaxBottomPortItems = 1;
        public static int MinimumWidth = 250;
        public static int MinimumHeight = 140;
        public static double ActiveSpeakerWidth = 100;
        public static double ActiveSpeakerHeight = 100;
        public static int TotalElementsOutsideViewPort = 0;
        public static int OtherSpeakersCount = 0;
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
            //if (MainPage.ScrollMode == ScrollMode.Vertical)
            MeasureForVerticalMode(availableSize);
            //else
            //    MeasureForHorizontalMode(availableSize);
            return LimitUnboundedSize(availableSize);
        }


        public void MeasureForVerticalMode(Size availableSize)
        {
            if (ListingControl != null && ListingControl.Parent is ActiveSpeakerControl activeSpeakerControl && activeSpeakerControl.Parent is Grid grid)
            {
                // Determine the square that can contain this number of items.
                //double AvailableHeight = grid.RowDefinitions[1].ActualHeight;
                double ViewPortHeight = activeSpeakerControl.ActualHeight;
                ComputeAndSetViewPortDimension(new Size(availableSize.Width, ViewPortHeight));
                if (Children.Count == 0)
                    return;
                UIElement ActiveSpeakerElement = Children[0];
                ActiveSpeakerElement.Measure(new Size(ActiveSpeakerWidth, ActiveSpeakerHeight));
                for (int i = 1; i < Children.Count; i++)
                {
                    UIElement child = Children[i];
                    child.Measure(new Size(cellwidth, cellheight));
                }
                // Get an aspect ratio from availableSize, decides whether to trim row or column.
                //aspectratio = availableSize.Width / availableSize.Height;
                //cellwidth = (int)Math.Floor(availableSize.Width / TotalColumns);
                // Next get a cell height, same logic of dividing available vertical by rowcount.
                //cellheight = (cellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                // LastRowcellwidth = cellwidth;
                //LastRowcellheight = cellheight;

            }
        }
        Size LimitUnboundedSize(Size input)
        {
            if (double.IsInfinity(input.Height))
            {
                if (ListingControl != null && ListingControl.Parent is ActiveSpeakerControl activeSpeakerControl)
                {
                    input.Height = activeSpeakerControl.ActualHeight;
                    if (OtherSpeakersCount >= MaxRightPortItems)
                    {
                        input.Height = MaxRightPortItems * cellheight + TotalRows * cellheight;
                    }
                }
                else
                {
                    input.Height = 0;
                }
            }
            if (double.IsInfinity(input.Width))
            {
                input.Width = cellwidth * (TotalColumns);
            }
            return input;
        }

        private void ComputeAndSetViewPortDimension(Size availableSize, ScrollMode scrollMode = ScrollMode.Vertical)
        {
            if (scrollMode == ScrollMode.Vertical)
            {
                double itemWidth = availableSize.Width / 4;
                if (itemWidth < MinimumWidth)
                {
                    itemWidth = MinimumWidth;
                }
                double itemHeight = itemWidth * MainPage.CurrentAspectHeightRatio / MainPage.CurrentAspectWidthRatio;
                ActiveSpeakerWidth = availableSize.Width;
                ActiveSpeakerHeight = availableSize.Height;
                OtherSpeakersCount = Children.Count - 1;
                if (Children.Count > 1)
                    ActiveSpeakerWidth = availableSize.Width - itemWidth;
                double OccupiedActualHeight = OtherSpeakersCount * (itemHeight);
                cellwidth = itemWidth;
                cellheight = itemHeight;
                ActiveSpeakerControl activeSpeakerControl = ListingControl?.Parent as ActiveSpeakerControl;
                MaxRightPortItems = (int)Math.Ceiling(activeSpeakerControl.ActualHeight / (cellheight));
                MaxBottomPortItems = (int)Math.Floor((availableSize.Width - cellwidth) / cellwidth);
                if (OccupiedActualHeight > (availableSize.Height))
                    ActiveSpeakerHeight = (MaxRightPortItems - 1) * itemHeight;
                //MaxRightPortItems = (int)Math.Ceiling(activeSpeakerControl.ActualHeight / (cellheight));
                //MaxBottomPortItems = (int)Math.Floor((availableSize.Width - cellwidth) / cellwidth);
                if (OtherSpeakersCount <= 0)
                {
                    TotalElementsOutsideViewPort = 0;
                    TotalRows = 0;
                    TotalColumns = 0;
                }
                else if (OtherSpeakersCount <= MaxRightPortItems)
                {
                    TotalElementsOutsideViewPort = 0;
                    TotalRows = 0;
                    TotalColumns = 0;
                }
                else if (OtherSpeakersCount <= (MaxBottomPortItems + MaxRightPortItems))
                {
                    TotalElementsOutsideViewPort = 0;
                    TotalRows = 0;
                    TotalColumns = 0;
                }
                else
                {
                    TotalElementsOutsideViewPort = OtherSpeakersCount - (MaxBottomPortItems + MaxRightPortItems);
                    TotalColumns = MaxBottomPortItems + 1;
                    TotalRows = (int)Math.Ceiling(TotalElementsOutsideViewPort / (double)TotalColumns);
                }
            }
            else
            {

            }
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            //if (MainPage.ScrollMode == ScrollMode.Vertical)
            ArrangeForVerticalMode(finalSize);
            //else
            //    ArrangeForHorizontalMode(finalSize);
            return finalSize;
        }

        public void ArrangeForVerticalMode(Size finalSize)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];
                double x = 0, y = 0;
                if (i == 0)
                {
                    x = 0;
                    y = 0;
                }
                else if (i <= MaxRightPortItems)
                {
                    double VerticalOffset = 0;
                    if (ListingControl != null && ListingControl.Parent is ActiveSpeakerControl activeSpeakerControl)
                    {
                        if (OtherSpeakersCount > 0 && OtherSpeakersCount <= MaxRightPortItems)
                            VerticalOffset = activeSpeakerControl.ActualHeight - (cellheight * (OtherSpeakersCount));
                    }
                    x = finalSize.Width - cellwidth;
                    y = (i - 1) * child.DesiredSize.Height;
                    if (VerticalOffset > 0)
                        y += (VerticalOffset / 2);
                    if (i == MaxRightPortItems)
                    {
                        int cols = MaxBottomPortItems + 1;
                        int totalFilled = cols;
                        if (TotalElementsOutsideViewPort == 0)
                        {
                            totalFilled = (Children.Count - (MaxRightPortItems)) % cols;
                        }
                        if (totalFilled == 0)
                            totalFilled = cols;
                        double HorizontalOffset = finalSize.Width - totalFilled * cellwidth;
                        if (HorizontalOffset > 0)
                        {
                            x -= HorizontalOffset / 2;
                        }
                    }
                }
                else if (i <= (MaxBottomPortItems + MaxRightPortItems))
                {
                    if (ListingControl != null && ListingControl.Parent is ActiveSpeakerControl activeSpeakerControl)
                    {
                        int cols = MaxBottomPortItems + 1;
                        int totalFilled = cols;
                        if (TotalElementsOutsideViewPort == 0)
                        {
                            totalFilled = (Children.Count - (MaxRightPortItems)) % cols;
                        }
                        if (totalFilled == 0)
                            totalFilled = cols;
                        double HorizontalOffset = finalSize.Width - totalFilled * cellwidth;
                        y = (child.DesiredSize.Height) * (MaxRightPortItems - 1);
                        x = finalSize.Width - (child.DesiredSize.Width) * (i - MaxRightPortItems + 1);
                        if (HorizontalOffset > 0)
                        {
                            x -= HorizontalOffset / 2;
                        }
                    }
                }
                else if (TotalColumns > 0 && TotalRows > 0 && TotalElementsOutsideViewPort > 0)
                {
                    double VerticalOffset = 0;
                    double HorizontalOffset = 0;
                    if (ListingControl != null && ListingControl.Parent is ActiveSpeakerControl activeSpeakerControl)
                    {
                        VerticalOffset = (child.DesiredSize.Height) * (MaxRightPortItems);
                        int totalFilled = TotalColumns;
                        if (Math.Ceiling((i - (MaxBottomPortItems + MaxRightPortItems)) / (double)TotalColumns) == TotalRows)
                        {
                            totalFilled = (Children.Count - (MaxBottomPortItems + MaxRightPortItems + 1)) % TotalColumns;
                            if (totalFilled == 0)
                                totalFilled = TotalColumns;
                        }

                        HorizontalOffset = finalSize.Width - totalFilled * cellwidth;
                    }
                    x = (i - (MaxBottomPortItems + MaxRightPortItems + 1)) % TotalColumns * child.DesiredSize.Width;
                    y = (i - (MaxBottomPortItems + MaxRightPortItems + 1)) / TotalColumns * child.DesiredSize.Height;
                    if (VerticalOffset > 0)
                        y += VerticalOffset;
                    if (HorizontalOffset > 0)
                    {
                        x += HorizontalOffset / 2;
                    }
                }
                Point anchorPoint = new Point(x, y);
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
            }
        }
    }
}
