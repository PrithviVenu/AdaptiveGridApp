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
    public class CustomPanel : Panel
    {
        double cellwidth, cellheight, maxcellheight, aspectratio;
        public int TotalColumns = 1;
        public int TotalRows = 1;
        public int MinimumWidth = 250;
        protected override Size MeasureOverride(Size availableSize)
        {
            // Determine the square that can contain this number of items.
            ComputeAndSetDimension(availableSize);
            // Get an aspect ratio from availableSize, decides whether to trim row or column.
            aspectratio = availableSize.Width / availableSize.Height;

            cellwidth = (int)Math.Floor(availableSize.Width / TotalColumns);
            // Next get a cell height, same logic of dividing available vertical by rowcount.
            cellheight = (cellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
            foreach (UIElement child in Children)
            {
                child.Measure(new Size(cellwidth, cellheight));
                maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
            }
            return LimitUnboundedSize(availableSize);
        }
        Size LimitUnboundedSize(Size input)
        {
            if (double.IsInfinity(input.Height))
            {
                input.Height = cellheight * TotalRows;
            }
            return input;
        }

        private void ComputeAndSetDimension(Size availableSize)
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

        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = 1;
            double x, y;
            if (Children.Count == 0)
                return finalSize;
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
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                count++;
            }
            int CenterItemIndex = TotalColumns / 2 + TotalColumns % 2;
            for (int i = 1; i <= LastRowTotalItems; i++)
            {
                int indexFactor = LastRowStartIndex - 1;
                int index = indexFactor + i;
                UIElement child = Children[index];
                int RightMargin = 10;
                int centerX = (int)finalSize.Width / 2;
                y = (count - 1) / TotalColumns * child.DesiredSize.Height;
                if (TotalColumns == LastRowTotalItems)
                {
                    x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                }
                else if (i > CenterItemIndex)
                {
                    x = centerX + ((int)((i - CenterItemIndex - 1) * child.DesiredSize.Width)) + ((int)child.DesiredSize.Width / 2);
                }
                else
                {
                    int offset = (LastRowTotalItems < CenterItemIndex) ? LastRowTotalItems : CenterItemIndex;
                    x = centerX - ((int)((offset - i) * child.DesiredSize.Width)) - ((int)child.DesiredSize.Width / 2);
                }
                if (x < 0)
                    x = 0;
                Point anchorPoint = new Point(x, y);
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                count++;
            }
            return finalSize;
        }
    }
}
