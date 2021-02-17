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
            cellheight = double.IsInfinity(availableSize.Height) ? double.PositiveInfinity : availableSize.Height / MainPage.TotalRows;

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
                input.Height = maxcellheight * MainPage.TotalColumns;
                cellheight = maxcellheight;
            }
            return input;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = 1;
            double x, y;
            foreach (UIElement child in Children)
            {
                x = (count - 1) % MainPage.TotalColumns * child.DesiredSize.Width;
                y = (count - 1) / MainPage.TotalColumns * child.DesiredSize.Height;
                Point anchorPoint = new Point(x, y);
                child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                count++;
            }
            return finalSize;
        }
    }
}
