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
        int maxrc, rowcount, colcount;
        double cellwidth, cellheight, maxcellheight, aspectratio;
        protected override Size MeasureOverride(Size availableSize)
        {
            // Determine the square that can contain this number of items.
            maxrc = (int)Math.Ceiling(Math.Sqrt(Children.Count));
            // Get an aspect ratio from availableSize, decides whether to trim row or column.
            aspectratio = availableSize.Width / availableSize.Height;

            // Now trim this square down to a rect, many times an entire row or column can be omitted.
            //if (aspectratio > 1)
            //{
            //    rowcount = maxrc;
            //    colcount = (maxrc > 2 && Children.Count < maxrc * (maxrc - 1)) ? maxrc - 1 : maxrc;
            //}
            //else
            //{
            //    rowcount = (maxrc > 2 && Children.Count < maxrc * (maxrc - 1)) ? maxrc - 1 : maxrc;
            //    colcount = maxrc;
            //}

            // Now that we have a column count, divide available horizontal, that's our cell width.
            cellwidth = (int)Math.Floor(availableSize.Width / MainPage.TotalColumns);
            // Next get a cell height, same logic of dividing available vertical by rowcount.
            //cellheight = double.IsInfinity(availableSize.Height) ? double.PositiveInfinity : availableSize.Height / MainPage.TotalRows;
            cellheight = (availableSize.Width * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
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
                input.Height = cellheight * MainPage.TotalRows;
                // cellheight = maxcellheight;
            }
            return input;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = 1;
            double x, y;
            int LastRowTotalItems = Children.Count - ((MainPage.TotalRows - 1) * MainPage.TotalColumns);
            int LastRowStartIndex;
            if (MainPage.TotalRows <= 1)
            {
                LastRowStartIndex = 0;
            }
            else
            {
                LastRowStartIndex = (MainPage.TotalRows - 1) * MainPage.TotalColumns;
            }
            for (int i = 0; i < LastRowStartIndex; i++)
            {
                UIElement child = Children[i];
                x = (count - 1) % MainPage.TotalColumns * child.DesiredSize.Width;
                y = (count - 1) / MainPage.TotalColumns * child.DesiredSize.Height;
                Point anchorPoint = new Point(x, y);
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                count++;
            }
            int CenterItemIndex = MainPage.TotalColumns / 2 + MainPage.TotalColumns % 2;
            for (int i = 1; i <= LastRowTotalItems; i++)
            {
                int indexFactor = LastRowStartIndex - 1;
                int index = indexFactor + i;
                UIElement child = Children[index];
                int RightMargin = 10;
                int centerX = (int)finalSize.Width / 2;
                y = (count - 1) / MainPage.TotalColumns * child.DesiredSize.Height;
                if (MainPage.TotalColumns == LastRowTotalItems)
                {
                    x = (count - 1) % MainPage.TotalColumns * child.DesiredSize.Width;
                }
                else if (i > CenterItemIndex)
                {
                    // x = (count - 1) % MainPage.TotalColumns * child.DesiredSize.Width;
                    x = centerX + ((int)((LastRowTotalItems - i) * child.DesiredSize.Width + RightMargin)) + ((int)child.DesiredSize.Width / 2) - RightMargin;//- ((int)AdaptiveGridViewControl.DesiredWidth / 2)
                }
                else
                {
                    int offset = (LastRowTotalItems < CenterItemIndex) ? LastRowTotalItems : CenterItemIndex;
                    x = centerX - ((int)((offset - i) * child.DesiredSize.Width + RightMargin)) - ((int)child.DesiredSize.Width / 2) + RightMargin;//- ((int)AdaptiveGridViewControl.DesiredWidth / 2)               
                }
                if (x < 0)
                    x = 0;
                Point anchorPoint = new Point(x, y);
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                count++;
            }
            return finalSize;
        }
        private double CalculateHorizontalCoordinate(int childIndex)
        {
            double x = 0;
            int LastRowTotalItems = Children.Count - ((MainPage.TotalRows - 1) * MainPage.TotalColumns);
            int LastRowStartIndex;
            if (MainPage.TotalRows <= 1)
            {
                LastRowStartIndex = 0;
            }
            else
            {
                LastRowStartIndex = (MainPage.TotalRows - 1) * MainPage.TotalColumns;
            }
            return x;
        }
    }
}
