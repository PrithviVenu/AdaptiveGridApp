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
            if (MainPage.VideoPosition == VideoPosition.Right || MainPage.VideoPosition == VideoPosition.Left)
            {
                MeasureForVerticalMode(availableSize, MainPage.VideoPosition);
            }
            else
            {
                MeasureForHorizontalMode(availableSize, MainPage.VideoPosition);
            }

            return LimitUnboundedSize(availableSize);
        }


        private void MeasureForHorizontalMode(Size availableSize, VideoPosition videoPosition = VideoPosition.Botton)
        {
            if (ListingControl != null && ListingControl.Parent is Grid grid)
            {
                ComputeAndSetDimension(availableSize, videoPosition);
            }
        }
        public void MeasureForVerticalMode(Size availableSize, VideoPosition videoPosition = VideoPosition.Right)
        {
            ComputeAndSetDimension(availableSize, videoPosition);
        }

        Size LimitUnboundedSize(Size input)
        {
            if (double.IsInfinity(input.Height))
            {
                if (TotalRows == 0)
                    input.Height = 0;
                else
                {
                    input.Height = cellheight * TotalRows;
                }
            }
            if (double.IsInfinity(input.Width))
            {
                input.Width = cellwidth * (TotalColumns);
            }
            return input;
        }

        private void ComputeAndSetDimension(Size availableSize, VideoPosition VideoPosition = VideoPosition.Right)
        {
            if (VideoPosition == VideoPosition.Right || VideoPosition == VideoPosition.Left)
            {
                double availableWidth = availableSize.Width;
                double ViewPortHeight = ListingControl.Height;
                MaxColumnsWithinViewPort = (int)Math.Floor(availableWidth / MinimumWidth);
                MaxRowsWithinViewPort = (int)Math.Floor(ViewPortHeight / MinimumHeight);
                int cols = (int)Math.Floor(availableWidth / cellwidth);
                if (cols > 0)
                    TotalColumns = cols;
                else
                    TotalColumns = 1;
                double itemWidth = availableWidth / TotalColumns;
                if (itemWidth < MinimumWidth)
                {
                    //itemWidth = MinimumWidth;
                    //itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                    //int MaxColumns = (int)(availableWidth / MinimumWidth);
                    TotalColumns = MaxColumnsWithinViewPort;
                    itemWidth = availableWidth / TotalColumns;
                }
            }
            else
            {

            }
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            if (MainPage.VideoPosition == VideoPosition.Right || MainPage.VideoPosition == VideoPosition.Left)
            {
                ArrangeForVerticalMode(finalSize, MainPage.VideoPosition);
            }
            else
            {
                ArrangeForHorizontalMode(finalSize, MainPage.VideoPosition);
            }
            return finalSize;
        }



        public void ArrangeForHorizontalMode(Size finalSize, VideoPosition videoPosition = VideoPosition.Botton)
        {

        }


        public void ArrangeForVerticalMode(Size finalSize, VideoPosition videoPosition = VideoPosition.Right)
        {


        }
    }
}
