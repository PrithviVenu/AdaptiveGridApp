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
    public class ScreensharePanel : Panel
    {
        double maxcellheight, LastRowcellwidth, LastRowcellheight, aspectratio;
        public int TotalColumns = 1;
        public int TotalColumnsWithinViewPort = 1;
        //public int TotalRowsFilledWithinViewPort = 1;
        public int MaxRowsWithinViewPort = 1;
        public int MaxColumnsWithinViewPort = 1;
        public int TotalRows = 1;
        double cellwidth = 250;
        double cellheight = 140;
        public static int MinimumWidth = 250;
        public static int MinimumHeight = 140;
        public static int ItemMargin = 15;
        public static readonly DependencyProperty ListingControlProperty = DependencyProperty.Register(
                                                                           "ListingControl",
                                                                           typeof(ListViewBase),
                                                                       typeof(ScreensharePanel),
                                                                   new PropertyMetadata(null)
                                                                            );

        public ListViewBase ListingControl
        {
            get { return (ListViewBase)GetValue(ListingControlProperty); }
            set { SetValue(ListingControlProperty, value); }
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            //if (ListingControl != null && ListingControl.Parent is Grid grid)
            //{
            //    double AvailableWidth = grid.ColumnDefinitions[0].ActualWidth;
            //    availableSize.Width = AvailableWidth;
            //    availableSize.Height = grid.ActualHeight;
            //    availableSize = new Size(ListingControl.ActualWidth, ListingControl.ActualHeight);
            //}
            if (ParticipantHomeControl.VideoPosition == VideoPosition.Right || ParticipantHomeControl.VideoPosition == VideoPosition.Left)
            {
                MeasureForVerticalMode(availableSize, ParticipantHomeControl.VideoPosition);
            }
            else
            {
                MeasureForHorizontalMode(availableSize, ParticipantHomeControl.VideoPosition);
            }

            return LimitUnboundedSize(availableSize);
        }


        private void MeasureForHorizontalMode(Size availableSize, VideoPosition videoPosition = VideoPosition.Botton)
        {
            ComputeAndSetDimension(availableSize, videoPosition);
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];
                child.Measure(new Size(cellwidth, cellheight));
            }
        }
        public void MeasureForVerticalMode(Size availableSize, VideoPosition videoPosition = VideoPosition.Right)
        {
            ComputeAndSetDimension(availableSize, videoPosition);
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];
                child.Measure(new Size(cellwidth, cellheight));
            }
        }

        Size LimitUnboundedSize(Size input)
        {
            if (double.IsInfinity(input.Height))
            {
                if (TotalRows == 0 || Children.Count == 0)
                    input.Height = 0;
                else
                {
                    input.Height = cellheight * TotalRows;
                }
            }
            if (double.IsInfinity(input.Width))
            {
                if (TotalColumns == 0 || Children.Count == 0)
                    input.Width = 0;
                else
                    input.Width = cellwidth * (TotalColumns);
            }
            return input;
        }

        private void ComputeAndSetDimension(Size availableSize, VideoPosition VideoPosition = VideoPosition.Right)
        {
            if (ListingControl != null && ListingControl.Parent is Grid grid)
            {
                if (VideoPosition == VideoPosition.Right || VideoPosition == VideoPosition.Left)
                {
                    //double availableWidth = ListingControl.ActualWidth;
                    //double ViewPortHeight = ListingControl.ActualHeight;
                    double availableWidth = availableSize.Width;
                    double ViewPortHeight = grid.ActualHeight;
                    TotalRows = 1;
                    TotalColumns = 1;
                    MaxColumnsWithinViewPort = (int)Math.Floor(availableWidth / MinimumWidth);
                    MaxRowsWithinViewPort = (int)Math.Floor(ViewPortHeight / MinimumHeight);
                    int rowsOccupiedWithinViewPort = (MaxRowsWithinViewPort < Children.Count) ? MaxRowsWithinViewPort : Children.Count;
                    //int cols = (int)Math.Floor(availableWidth / cellwidth);
                    if (rowsOccupiedWithinViewPort > 0)
                    {
                        int cols = (int)Math.Floor(Children.Count / (double)rowsOccupiedWithinViewPort);
                        if (cols > MaxColumnsWithinViewPort)
                        {
                            cols = MaxColumnsWithinViewPort;
                        }
                        if (cols > 0)
                            TotalColumns = cols;
                        else
                            TotalColumns = 1;
                    }
                    TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                    double itemWidth = availableWidth / TotalColumns;
                    if (itemWidth < MinimumWidth)
                    {
                        //itemWidth = MinimumWidth;
                        //itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                        //int MaxColumns = (int)(availableWidth / MinimumWidth);
                        TotalColumns = MaxColumnsWithinViewPort;
                        if (TotalColumns > 0)
                            itemWidth = availableWidth / TotalColumns;
                    }
                    double itemHeight = (itemWidth * ParticipantHomeControl.CurrentAspectHeightRatio) / ParticipantHomeControl.CurrentAspectWidthRatio;
                    cellwidth = itemWidth;
                    cellheight = itemHeight;
                }
                else
                {
                    TotalRows = 1;
                    TotalColumns = 1;
                }
            }
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            if (ParticipantHomeControl.VideoPosition == VideoPosition.Right || ParticipantHomeControl.VideoPosition == VideoPosition.Left)
            {
                ArrangeForVerticalMode(finalSize, ParticipantHomeControl.VideoPosition);
            }
            else
            {
                ArrangeForHorizontalMode(finalSize, ParticipantHomeControl.VideoPosition);
            }
            return finalSize;
        }



        public void ArrangeForHorizontalMode(Size finalSize, VideoPosition videoPosition = VideoPosition.Right)
        {

        }


        public void ArrangeForVerticalMode(Size finalSize, VideoPosition videoPosition = VideoPosition.Right)
        {
            if (Children.Count == 0)
                return;
            if (TotalRows <= MaxRowsWithinViewPort)
            {
                for (int index = 0; index < Children.Count; index++)
                {
                    UIElement child = Children[index];
                    double x = (index) % TotalColumns * child.DesiredSize.Width;
                    double y = (index) / TotalColumns * child.DesiredSize.Height;
                    Point anchorPoint = new Point(x, y);
                    child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                }

                for (int i = 1; i <= TotalColumns; i++)
                {
                    for (int j = 1; j <= TotalRows; j++)
                    {
                        double x = (i - 1) * cellwidth;
                        double y = (j - 1) * cellheight;
                        UIElement child = Children[i - 1];
                        Point anchorPoint = new Point(x, y);
                        child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                    }
                }
            }
            else
            {

            }
        }
    }
}
