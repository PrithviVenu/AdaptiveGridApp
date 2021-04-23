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
        double cellwidth, cellheight, maxcellheight, LastRowcellwidth, LastRowcellheight, aspectratio;
        public int TotalColumns = 1;
        public int TotalColumnsWithinViewPort = 1;
        //public int TotalRowsFilledWithinViewPort = 1;
        public int MaxRowsWithinViewPort = 1;
        public int MaxColumnsWithinViewPort = 1;
        public int TotalRows = 1;
        public static int MinimumWidth = 250;
        public static int MinimumHeight = 140;

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
            {
                if (MainPage.GridMode == GridMode.AspectFit)
                    MeasureForVerticalMode(availableSize);
                else if (MainPage.GridMode == GridMode.Fill)
                    MeasureForVerticalFillMode(availableSize);
            }
            else
            {
                if (MainPage.GridMode == GridMode.AspectFit)
                    MeasureForHorizontalMode(availableSize);
                else if (MainPage.GridMode == GridMode.Fill)
                    MeasureForHorizontalFillMode(availableSize);

            }
            return LimitUnboundedSize(availableSize);
        }

        private void MeasureForHorizontalMode(Size availableSize)
        {
            if (ListingControl != null && ListingControl.Parent is Grid grid)
            {
                double AvailableHeight = grid.RowDefinitions[2].ActualHeight;
                availableSize.Height = AvailableHeight;
                // Determine the square that can contain this number of items.
                ComputeAndSetDimension(new Size((grid.ActualWidth), availableSize.Height), ScrollMode.Horizontal);
                // Get an aspect ratio from availableSize, decides whether to trim row or column.
                aspectratio = availableSize.Width / availableSize.Height;
                //cellwidth = (int)Math.Floor(grid.ActualWidth / TotalColumnsWithinViewPort);
                // Next get a cell height, same logic of dividing available vertical by rowcount.
                // cellheight = (cellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                //TotalColumns = Children.Count / TotalRows;
                if (double.IsInfinity(availableSize.Width))
                {
                    availableSize.Width = cellwidth * (TotalColumns);
                }
                //double requiredWidth = TotalColumns * cellwidth;
                //TotalColumnsWithinViewPort = (int)Math.Floor(availableSize.Width / cellwidth);
                if (TotalColumns <= TotalColumnsWithinViewPort)
                {
                    LastRowcellwidth = cellwidth;
                    LastRowcellheight = cellheight;
                    int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumnsWithinViewPort);
                    int LastRowStartIndex;

                    if (TotalRows <= 1)
                    {
                        LastRowStartIndex = 0;
                    }
                    else
                    {
                        LastRowStartIndex = (TotalRows - 1) * TotalColumnsWithinViewPort;
                    }
                    for (int i = 0; i < Children.Count; i++)
                    {
                        UIElement child = Children[i];
                        if (MainPage.GridMode == GridMode.AspectFit || i < LastRowStartIndex)
                        {
                            child.Measure(new Size(cellwidth, cellheight));
                            LastRowcellheight = cellheight;
                        }
                        else
                        {
                            LastRowcellwidth = availableSize.Width / LastRowTotalItems;
                            LastRowcellheight = availableSize.Height - (TotalRows - 1) * cellheight;
                            if ((LastRowcellwidth / LastRowcellheight) > 3)
                            {
                                LastRowcellwidth = LastRowcellheight * 2;
                                // child.Measure(new Size(cellwidth, cellheight));
                            }
                            //else
                            //{
                            child.Measure(new Size(LastRowcellwidth, LastRowcellheight));
                            //}
                        }
                        maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
                    }
                }
                else
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        UIElement child = Children[i];
                        child.Measure(new Size(cellwidth, cellheight));
                        maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
                    }
                }
            }
        }
        private void MeasureForHorizontalFillMode(Size availableSize)
        {
            if (ListingControl != null && ListingControl.Parent is Grid grid)
            {
                double AvailableHeight = grid.RowDefinitions[2].ActualHeight;
                availableSize.Height = AvailableHeight;
                // Determine the square that can contain this number of items.
                ComputeAndSetFillDimension(new Size((grid.ActualWidth), availableSize.Height), ScrollMode.Horizontal);
                if (double.IsInfinity(availableSize.Width))
                {
                    availableSize.Width = cellwidth * (TotalColumns);
                }
                if (TotalColumns <= TotalColumnsWithinViewPort)
                {
                    LastRowcellwidth = cellwidth;
                    LastRowcellheight = cellheight;
                    int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumnsWithinViewPort);
                    int LastRowStartIndex;

                    if (TotalRows <= 1)
                    {
                        LastRowStartIndex = 0;
                    }
                    else
                    {
                        LastRowStartIndex = (TotalRows - 1) * TotalColumnsWithinViewPort;
                    }
                    for (int i = 0; i < Children.Count; i++)
                    {
                        UIElement child = Children[i];
                        if (LastRowTotalItems > Math.Ceiling(TotalColumns / 2.0)) //else if (i < LastRowStartIndex)
                        {
                            if (i < LastRowStartIndex)
                                child.Measure(new Size(cellwidth, cellheight));
                            else
                            {
                                LastRowcellwidth = availableSize.Width / LastRowTotalItems;
                                child.Measure(new Size(LastRowcellwidth, LastRowcellheight));
                            }
                        }
                        else
                        {
                            LastRowcellheight = 0;
                            if (TotalRows == 1 || i < LastRowStartIndex - LastRowTotalItems)
                                child.Measure(new Size(cellwidth, cellheight));
                            else
                            {
                                child.Measure(new Size(cellwidth, (cellheight / 2)));
                            }
                        }
                        //if (i < LastRowStartIndex)
                        //{
                        //    child.Measure(new Size(cellwidth, cellheight));
                        //    // LastRowcellheight = cellheight;
                        //}
                        //else
                        //{
                        //    LastRowcellwidth = availableSize.Width / LastRowTotalItems;
                        //    //LastRowcellheight = availableSize.Height - (TotalRows - 1) * cellheight;
                        //    //if ((LastRowcellwidth / LastRowcellheight) > 3)
                        //    //{
                        //    //    LastRowcellwidth = LastRowcellheight * 2;
                        //    //}
                        //    child.Measure(new Size(LastRowcellwidth, LastRowcellheight));
                        //}
                        maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
                    }
                }
                else
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        UIElement child = Children[i];
                        child.Measure(new Size(cellwidth, cellheight));
                        maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
                    }
                }
            }
        }

        public void MeasureForVerticalMode(Size availableSize)
        {
            // Determine the square that can contain this number of items.
            ComputeAndSetDimension(availableSize);

            // Get an aspect ratio from availableSize, decides whether to trim row or column.
            //aspectratio = availableSize.Width / availableSize.Height;
            //cellwidth = (int)Math.Floor(availableSize.Width / TotalColumns);
            // Next get a cell height, same logic of dividing available vertical by rowcount.
            //cellheight = (cellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
            LastRowcellwidth = cellwidth;
            LastRowcellheight = cellheight;
            //int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
            //int LastRowStartIndex;

            //if (TotalRows <= 1)
            //{
            //    LastRowStartIndex = 0;
            //}
            //else
            //{
            //    LastRowStartIndex = (TotalRows - 1) * TotalColumns;
            //}
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];
                if (MainPage.GridMode == GridMode.AspectFit)
                    child.Measure(new Size(cellwidth, cellheight));
                //else if (LastRowTotalItems > Math.Ceiling(TotalColumns / 2.0)) //else if (i < LastRowStartIndex)
                //{
                //    if (i < LastRowStartIndex)
                //        child.Measure(new Size(cellwidth, cellheight));
                //    else
                //    {
                //        LastRowcellwidth = availableSize.Width / LastRowTotalItems;
                //        //LastRowcellheight = ListingControl.ActualHeight - (TotalRows - 1) * cellheight;
                //        if (ListingControl != null)
                //        {
                //            double ViewPortHeight = ListingControl.ActualHeight;
                //            if ((TotalRows - 1) * cellheight < ViewPortHeight)
                //                LastRowcellheight = ViewPortHeight - (TotalRows - 1) * cellheight;
                //            //if (LastRowcellheight < MinimumHeight)
                //            //LastRowcellheight = cellheight;
                //        }
                //        //LastRowcellheight = (LastRowcellwidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                //        //if ((LastRowcellwidth / LastRowcellheight) > 3)
                //        //{
                //        //LastRowcellwidth = LastRowcellheight * 2;
                //        // child.Measure(new Size(cellwidth, cellheight));
                //        //}
                //        //else
                //        //{
                //        child.Measure(new Size(LastRowcellwidth, LastRowcellheight));
                //        //}
                //    }
                //}
                //else
                //{
                //    LastRowcellheight = 0;
                //    if (TotalRows == 1 || i < LastRowStartIndex - LastRowTotalItems)
                //        child.Measure(new Size(cellwidth, cellheight));
                //    else
                //    {
                //        child.Measure(new Size(cellwidth, (cellheight / 2)));
                //    }
                //}

                maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
            }
        }
        public void MeasureForVerticalFillMode(Size availableSize)
        {
            // Determine the square that can contain this number of items.
            ComputeAndSetFillDimension(availableSize);
            LastRowcellwidth = cellwidth;
            LastRowcellheight = cellheight;
            int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
            int LastRowStartIndex;
            bool isScrollEnabled = false;
            if (TotalRows <= 1)
            {
                LastRowStartIndex = 0;
            }
            else
            {
                LastRowStartIndex = (TotalRows - 1) * TotalColumns;
            }

            if ((TotalRows - MaxRowsWithinViewPort) > 1)
            {
                isScrollEnabled = true;
            }
            else if ((TotalRows - MaxRowsWithinViewPort) == 1 && LastRowTotalItems > Math.Ceiling(TotalColumns / 2.0))
            {
                isScrollEnabled = true;
            }

            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];
                if (LastRowTotalItems > Math.Ceiling(TotalColumns / 2.0) || isScrollEnabled) //else if (i < LastRowStartIndex)
                {
                    if (i < LastRowStartIndex || isScrollEnabled)
                        child.Measure(new Size(cellwidth, cellheight));
                    else
                    {
                        LastRowcellwidth = availableSize.Width / LastRowTotalItems;
                        child.Measure(new Size(LastRowcellwidth, LastRowcellheight));
                    }
                }
                else
                {
                    LastRowcellheight = 0;
                    if (TotalRows == 1 || i < LastRowStartIndex - LastRowTotalItems)
                        child.Measure(new Size(cellwidth, cellheight));
                    else
                    {
                        child.Measure(new Size(cellwidth, (cellheight / 2)));
                    }
                }

                maxcellheight = (child.DesiredSize.Height > maxcellheight) ? child.DesiredSize.Height : maxcellheight;
            }
        }

        Size LimitUnboundedSize(Size input)
        {
            if (double.IsInfinity(input.Height))
            {
                if (TotalRows == 0 || Children.Count==0)
                    input.Height = 0;
                else
                {
                    input.Height = cellheight * (TotalRows - 1) + LastRowcellheight;
                    if (LastRowcellheight == 0 && TotalRows == 1)
                        input.Height = cellheight;
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
        private void ComputeAndSetFillDimension(Size availableSize, ScrollMode scrollMode = ScrollMode.Vertical)
        {
            if (scrollMode == ScrollMode.Vertical)
            {
                double ViewPortHeight = 0;
                TotalRows = 1;
                TotalColumns = 1;
                TotalRows = 1;
                if (ListingControl != null && ListingControl.Parent is Grid grid1)
                {
                    Grid grid = grid1;
                    ViewPortHeight = grid.RowDefinitions[2].ActualHeight;
                }
                double availableWidth = availableSize.Width;
                MaxColumnsWithinViewPort = (int)Math.Floor(availableWidth / MinimumWidth);
                MaxRowsWithinViewPort = (int)Math.Floor(ViewPortHeight / MinimumHeight);
                int cols = (int)Math.Ceiling(Math.Sqrt(Children.Count));
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
                if (TotalColumns > 0 && Children.Count > 1)
                    TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                double itemHeight = ViewPortHeight / TotalRows;
                bool isLastRowMerged = false;
                if (itemHeight < MinimumHeight)
                {
                    itemHeight = MinimumHeight;
                    int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
                    if (((TotalRows - MaxRowsWithinViewPort) == 1) && LastRowTotalItems <= Math.Ceiling(TotalColumns / 2.0))
                    {
                        //TotalRows -= 1;
                        itemHeight = ViewPortHeight / (TotalRows - 1);// note itemheight cant go below minheight as itemheight increases
                        isLastRowMerged = true;
                    }
                }
                else
                {
                    int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
                    if (TotalRows > 1 && LastRowTotalItems <= Math.Ceiling(TotalColumns / 2.0))
                    {
                        //TotalRows -= 1;
                        itemHeight = ViewPortHeight / (TotalRows - 1);// note itemheight cant go below minheight as itemheight increases
                        isLastRowMerged = true;
                    }
                }
                //if (ViewPortHeight > 0)
                //{
                //    double RowsWithinViewPort = ViewPortHeight / itemHeight;
                //    int RowsCeil = (int)Math.Ceiling(RowsWithinViewPort); 
                //    int RowsFloor = (int)Math.Floor(RowsWithinViewPort);
                //    if (TotalRows >= RowsCeil && RowsCeil > RowsFloor) //&& (RowsWithinViewPort - RowsFloor) > 0.5//&& grid.ActualWidth<grid.ActualHeight
                //    {
                //        //TotalRows = RowsCeil;
                //        itemHeight = ViewPortHeight / RowsCeil;
                //        if (itemHeight < MinimumHeight)
                //            itemHeight = MinimumHeight;
                //        //itemWidth = (itemHeight * MainPage.CurrentAspectWidthRatio) / MainPage.CurrentAspectHeightRatio;
                //    }

                double occupiedViewPortSpace = TotalRows * itemHeight;
                if (isLastRowMerged)
                {
                    occupiedViewPortSpace -= itemHeight;
                }
                if (occupiedViewPortSpace > ViewPortHeight && TotalColumns < MaxColumnsWithinViewPort)//TotalRows > MaxRowsWithinViewPort 
                {
                    TotalColumns += 1;
                    TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                    itemWidth = availableWidth / TotalColumns;
                    //itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                    if (TotalRows * itemHeight > ViewPortHeight && TotalColumns < MaxColumnsWithinViewPort)//TotalRows > MaxRowsWithinViewPort 
                    {
                        TotalColumns += 1;
                        TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                        itemWidth = availableWidth / TotalColumns;
                        //itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                    }
                    itemHeight = ViewPortHeight / TotalRows;
                    if (itemHeight < MinimumHeight)
                    {
                        itemHeight = MinimumHeight;
                    }
                }
                //}
                cellwidth = itemWidth;
                cellheight = itemHeight;
            }
            else
            {
                double ViewPortHeight = 0;
                TotalRows = 1;
                TotalColumns = 1;
                if (ListingControl != null && ListingControl.Parent is Grid grid)
                {
                    ViewPortHeight = grid.RowDefinitions[2].ActualHeight;
                }
                int cols = (int)Math.Ceiling(Math.Sqrt(Children.Count));
                if (cols > 0)
                    TotalColumnsWithinViewPort = cols;
                else
                    TotalColumnsWithinViewPort = 1;

                MaxColumnsWithinViewPort = (int)Math.Floor(availableSize.Width / MinimumWidth);
                int MaxRowsinViewPort = (int)Math.Floor(ViewPortHeight / MinimumHeight);
                double itemWidth = availableSize.Width / TotalColumnsWithinViewPort;
                if (itemWidth < MinimumWidth)
                {
                    TotalColumnsWithinViewPort = MaxColumnsWithinViewPort;
                    itemWidth = availableSize.Width / TotalColumnsWithinViewPort;
                }
                TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumnsWithinViewPort);
                if (TotalRows > MaxRowsinViewPort)
                    TotalRows = MaxRowsinViewPort;
                if (TotalRows > 0)
                    TotalColumns = (int)Math.Ceiling(Children.Count / (double)TotalRows);
                double itemHeight = ViewPortHeight / TotalRows;
                if (TotalColumns <= TotalColumnsWithinViewPort)
                {
                    int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumnsWithinViewPort);
                    if (TotalRows > 1 && LastRowTotalItems <= Math.Ceiling(TotalColumns / 2.0))
                    {
                        itemHeight = ViewPortHeight / (TotalRows - 1);
                    }
                }
                cellwidth = itemWidth;
                cellheight = itemHeight;
                //int MaxRowsCeil = (int)Math.Ceiling(availableSize.Height / itemHeight);
                //int MaxRowsFloor = (int)Math.Floor(availableSize.Height / itemHeight);
                //if (TotalColumnsWithinViewPort > 0)
                //    MaxRowsWithinViewPort = (int)Math.Ceiling(Children.Count / (double)TotalColumnsWithinViewPort);
                //// MaxRowsWithinViewPort = MaxRowsCeil;
                //if (MaxRowsWithinViewPort > MaxRowsFloor)
                //{
                //    MaxRowsWithinViewPort = MaxRowsFloor;
                //    double CeilHeight = availableSize.Height / MaxRowsCeil;
                //    double CeilWidth = CeilHeight * MainPage.CurrentAspectWidthRatio / MainPage.CurrentAspectHeightRatio;
                //    if (CeilHeight >= MinimumHeight && CeilWidth >= MinimumWidth && Math.Floor(availableSize.Width / CeilWidth) == TotalColumnsWithinViewPort)
                //    {
                //        MaxRowsWithinViewPort = MaxRowsCeil;
                //        cellheight = CeilHeight;
                //        cellwidth = CeilWidth;
                //    }
                //}

                //TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumnsWithinViewPort);
                //if (TotalRows > MaxRowsWithinViewPort)
                //    TotalRows = MaxRowsWithinViewPort;
                //if (TotalRows > 0)
                //    TotalColumns = (int)Math.Ceiling(Children.Count / (double)TotalRows);
                MaxRowsWithinViewPort = MaxRowsinViewPort;
                if (TotalColumns * itemWidth > availableSize.Width && TotalColumnsWithinViewPort < MaxColumnsWithinViewPort)
                {
                    TotalColumnsWithinViewPort += 1;
                    cellwidth = availableSize.Width / TotalColumnsWithinViewPort;
                    //cellheight = cellwidth * MainPage.CurrentAspectHeightRatio / MainPage.CurrentAspectWidthRatio;
                    //TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumnsWithinViewPort);
                    //if (TotalRows > MaxRowsWithinViewPort)
                    //    TotalRows = MaxRowsWithinViewPort;
                    //if (TotalRows > 0)
                    //    TotalColumns = (int)Math.Ceiling(Children.Count / (double)TotalRows);
                }

                if (TotalColumns * itemWidth > availableSize.Width && (TotalRows < MaxRowsinViewPort))//TotalColumns > MaxColumnsWithinViewPort
                {
                    TotalRows += 1;
                    //TotalColumns = (int)Math.Ceiling(Children.Count / (double)TotalRows);
                    cellheight = availableSize.Height / TotalRows;
                    // cellwidth = (cellheight * MainPage.CurrentAspectWidthRatio) / MainPage.CurrentAspectHeightRatio;
                    //TotalColumnsWithinViewPort = (int)Math.Floor(availableSize.Width / cellwidth);
                }
            }
        }
        private void ComputeAndSetDimension(Size availableSize, ScrollMode scrollMode = ScrollMode.Vertical)
        {
            if (scrollMode == ScrollMode.Vertical)
            {
                double ViewPortHeight = 0;
                TotalRows = 1;
                TotalColumns = 1;
                if (ListingControl != null && ListingControl.Parent is Grid grid1)
                {
                    Grid grid = grid1;
                    ViewPortHeight = grid.RowDefinitions[2].ActualHeight;
                }
                double availableWidth = availableSize.Width;
                //if (grid != null)
                //    availableWidth = grid.ActualWidth;
                MaxColumnsWithinViewPort = (int)Math.Floor(availableWidth / MinimumWidth);
                MaxRowsWithinViewPort = (int)Math.Floor(ViewPortHeight / MinimumHeight);
                int cols = (int)Math.Ceiling(Math.Sqrt(Children.Count));
                if (cols > 0)
                    TotalColumns = cols;
                else
                    TotalColumns = 1;
                //if (availableSize.Width > ViewPortHeight)
                //{
                double itemWidth = availableWidth / TotalColumns;
                double itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                //}
                //else
                //{
                //    int RowsWithinViewPort = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                //    itemHeight = ViewPortHeight /;
                //    itemWidth = (itemHeight * MainPage.CurrentAspectWidthRatio) / MainPage.CurrentAspectHeightRatio;
                //}
                if (itemWidth < MinimumWidth)
                {
                    //itemWidth = MinimumWidth;
                    //itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                    //int MaxColumns = (int)(availableWidth / MinimumWidth);
                    TotalColumns = MaxColumnsWithinViewPort;
                    itemWidth = availableWidth / TotalColumns;
                    itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                }
                if (TotalColumns > 0)
                    TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                if (ViewPortHeight > 0)
                {
                    double RowsWithinViewPort = ViewPortHeight / itemHeight;
                    int RowsCeil = (int)Math.Ceiling(RowsWithinViewPort);
                    int RowsFloor = (int)Math.Floor(RowsWithinViewPort);
                    if (TotalRows >= RowsCeil && RowsCeil > RowsFloor) //&& (RowsWithinViewPort - RowsFloor) > 0.5//&& grid.ActualWidth<grid.ActualHeight
                    {
                        //TotalRows = RowsCeil;
                        itemHeight = ViewPortHeight / RowsCeil;
                        if (itemHeight < MinimumHeight)
                            itemHeight = MinimumHeight;
                        itemWidth = (itemHeight * MainPage.CurrentAspectWidthRatio) / MainPage.CurrentAspectHeightRatio;
                    }
                    if (TotalRows * itemHeight > ViewPortHeight && TotalColumns < MaxColumnsWithinViewPort)//TotalRows > MaxRowsWithinViewPort 
                    {
                        TotalColumns += 1;
                        TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                        itemWidth = availableWidth / TotalColumns;
                        itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                        if (TotalRows * itemHeight > ViewPortHeight && TotalColumns < MaxColumnsWithinViewPort)//TotalRows > MaxRowsWithinViewPort 
                        {
                            TotalColumns += 1;
                            TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumns);
                            itemWidth = availableWidth / TotalColumns;
                            itemHeight = (itemWidth * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio;
                        }
                    }
                }
                cellwidth = itemWidth;
                cellheight = itemHeight;
            }
            else
            {
                double ViewPortHeight = 0;
                TotalRows = 1;
                TotalColumns = 1;
                Grid grid = null;
                if (ListingControl != null && ListingControl.Parent is Grid grid1)
                {
                    grid = grid1;
                    ViewPortHeight = grid.RowDefinitions[2].ActualHeight;
                }
                int cols = (int)Math.Ceiling(Math.Sqrt(Children.Count));
                if (cols > 0)
                    TotalColumnsWithinViewPort = cols;
                else
                    TotalColumnsWithinViewPort = 1;
                double itemWidth = availableSize.Width / TotalColumnsWithinViewPort;
                double itemHeight = itemWidth * MainPage.CurrentAspectHeightRatio / MainPage.CurrentAspectWidthRatio;
                MaxColumnsWithinViewPort = (int)Math.Floor(availableSize.Width / MinimumWidth);
                int MaxRowsinViewPort = (int)Math.Floor(ViewPortHeight / MinimumHeight);
                if (itemWidth < MinimumWidth)
                {
                    int MaxColumns = (int)(availableSize.Width / MinimumWidth);
                    TotalColumnsWithinViewPort = MaxColumns;
                    itemWidth = availableSize.Width / TotalColumnsWithinViewPort;
                    itemHeight = itemWidth * MainPage.CurrentAspectHeightRatio / MainPage.CurrentAspectWidthRatio;
                }
                cellheight = itemHeight;
                cellwidth = itemWidth;

                int MaxRowsCeil = (int)Math.Ceiling(availableSize.Height / itemHeight);
                int MaxRowsFloor = (int)Math.Floor(availableSize.Height / itemHeight);
                if (TotalColumnsWithinViewPort > 0)
                    MaxRowsWithinViewPort = (int)Math.Ceiling(Children.Count / (double)TotalColumnsWithinViewPort);
                // MaxRowsWithinViewPort = MaxRowsCeil;
                if (MaxRowsWithinViewPort > MaxRowsFloor)
                {
                    MaxRowsWithinViewPort = MaxRowsFloor;
                    double CeilHeight = availableSize.Height / MaxRowsCeil;
                    double CeilWidth = CeilHeight * MainPage.CurrentAspectWidthRatio / MainPage.CurrentAspectHeightRatio;
                    if (CeilHeight >= MinimumHeight && CeilWidth >= MinimumWidth && Math.Floor(availableSize.Width / CeilWidth) == TotalColumnsWithinViewPort)
                    {
                        MaxRowsWithinViewPort = MaxRowsCeil;
                        cellheight = CeilHeight;
                        cellwidth = CeilWidth;
                    }
                }

                TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumnsWithinViewPort);
                if (TotalRows > MaxRowsWithinViewPort)
                    TotalRows = MaxRowsWithinViewPort;
                if (TotalRows > 0)
                    TotalColumns = (int)Math.Ceiling(Children.Count / (double)TotalRows);

                if (TotalColumns * itemWidth > availableSize.Width && TotalColumnsWithinViewPort < MaxColumnsWithinViewPort)
                {
                    TotalColumnsWithinViewPort += 1;
                    cellwidth = availableSize.Width / TotalColumnsWithinViewPort;
                    cellheight = cellwidth * MainPage.CurrentAspectHeightRatio / MainPage.CurrentAspectWidthRatio;
                    TotalRows = (int)Math.Ceiling(Children.Count / (double)TotalColumnsWithinViewPort);
                    if (TotalRows > MaxRowsWithinViewPort)
                        TotalRows = MaxRowsWithinViewPort;
                    if (TotalRows > 0)
                        TotalColumns = (int)Math.Ceiling(Children.Count / (double)TotalRows);
                }

                if (TotalColumns * itemWidth > availableSize.Width && (TotalRows < MaxRowsinViewPort))//TotalColumns > MaxColumnsWithinViewPort
                {
                    TotalRows += 1;
                    TotalColumns = (int)Math.Ceiling(Children.Count / (double)TotalRows);
                    cellheight = availableSize.Height / TotalRows;
                    cellwidth = (cellheight * MainPage.CurrentAspectWidthRatio) / MainPage.CurrentAspectHeightRatio;
                    TotalColumnsWithinViewPort = (int)Math.Floor(availableSize.Width / cellwidth);
                }
            }
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            if (MainPage.ScrollMode == ScrollMode.Vertical)
            {
                if (MainPage.GridMode == GridMode.AspectFit)
                    ArrangeForVerticalFitMode(finalSize);
                else
                    ArrangeForVerticalFillMode(finalSize);
            }
            else
            {
                if (MainPage.GridMode == GridMode.AspectFit)
                    ArrangeForHorizontalMode(finalSize);
                else
                    ArrangeForHorizontalFillMode(finalSize);
            }
            return finalSize;
        }



        public void ArrangeForHorizontalMode(Size finalSize)
        {
            // TotalColumnsWithinViewPort = (int)Math.Floor(finalSize.Width / cellwidth);
            if (TotalColumns <= TotalColumnsWithinViewPort)
            {
                int count = 1;
                double VerticalOffset = finalSize.Height - (cellheight * (TotalRows - 1) + LastRowcellheight);
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
                    if (VerticalOffset > 0)
                        y += (VerticalOffset / 2);
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
                    if (VerticalOffset > 0)
                        y += (VerticalOffset / 2);
                    Point anchorPoint = new Point(x, y);
                    child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                    count++;
                }
            }
            else
            {
                double x, y;
                if (TotalRows < 1)
                    TotalRows = 1;
                if (Children.Count == 0)
                    return;
                int TotalItemsWithinViewport = TotalColumnsWithinViewPort * TotalRows;
                double VerticalOffset = finalSize.Height - (cellheight * (TotalRows - 1) + LastRowcellheight);
                for (int i = 0; i < TotalItemsWithinViewport; i++)
                {
                    UIElement child = Children[i];
                    x = (i) % TotalColumnsWithinViewPort * child.DesiredSize.Width;
                    y = (i) / TotalColumnsWithinViewPort * child.DesiredSize.Height;
                    if (VerticalOffset > 0)
                        y += (VerticalOffset / 2);
                    Point anchorPoint = new Point(x, y);
                    child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                }
                int count = 1;
                int remainingCount = Children.Count - TotalItemsWithinViewport;
                for (int i = TotalItemsWithinViewport; i < Children.Count; i++)
                {
                    UIElement child = Children[i];
                    int row = (i) % TotalRows;
                    int column = ((i) / TotalRows);
                    x = column * child.DesiredSize.Width;
                    y = row * child.DesiredSize.Height;
                    if (VerticalOffset > 0)
                        y += (VerticalOffset / 2);
                    Point anchorPoint = new Point(x, y);
                    child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                    count++;
                }
            }
        }
        public void ArrangeForHorizontalFillMode(Size finalSize)
        {
            // TotalColumnsWithinViewPort = (int)Math.Floor(finalSize.Width / cellwidth);
            int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
            bool isScrollEnabled = true;
            if (TotalColumns <= TotalColumnsWithinViewPort)
            {
                isScrollEnabled = false;
            }
            if (!isScrollEnabled)//&& LastRowTotalItems <= Math.Ceiling(TotalColumns / 2.0)
            {
                int count = 1;
                double VerticalOffset = finalSize.Height - (cellheight * (TotalRows));
                double x, y;
                if (Children.Count == 0)
                    return;
                //int LastRowTotalItems = Children.Count - ((TotalRows - 1) * TotalColumns);
                int LastRowStartIndex;
                if (TotalRows <= 1)
                {
                    LastRowStartIndex = 0;
                }
                else
                {
                    LastRowStartIndex = (TotalRows - 1) * TotalColumns;
                }
                if (LastRowTotalItems <= Math.Ceiling(TotalColumns / 2.0))
                {
                    if (TotalRows > 1)
                        VerticalOffset += cellheight;
                    for (int i = 0; i < LastRowStartIndex; i++)
                    {
                        UIElement child = Children[i];
                        x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                        y = (count - 1) / TotalColumns * cellheight;
                        if (VerticalOffset > 0)
                            y += (VerticalOffset / 2);
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
                        //int offset = (LastRowTotalItems < CenterItemIndex) ? LastRowTotalItems : CenterItemIndex;
                        //int rightPanelOffset = LastRowTotalItems - offset;
                        //int leftPanelOffset = offset - 1;
                        //int horizontalOffset = leftPanelOffset - rightPanelOffset;
                        //double horizontalOffsetValue = 0;
                        //if (horizontalOffset > 0)
                        //{
                        //    horizontalOffsetValue = horizontalOffset * (child.DesiredSize.Width / 2);
                        //}
                        //if (TotalColumns == LastRowTotalItems)
                        //{
                        //    x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                        //}
                        //else if (i > CenterItemIndex)
                        //{
                        //    x = centerX + ((int)((i - CenterItemIndex - 1) * child.DesiredSize.Width)) + ((int)child.DesiredSize.Width / 2) + horizontalOffsetValue;
                        //}
                        //else
                        //{
                        //    x = centerX - ((int)((offset - i) * child.DesiredSize.Width)) - ((int)child.DesiredSize.Width / 2) + horizontalOffsetValue;
                        //}

                        //if (x < 0)
                        //    x = 0;

                        double offset = finalSize.Width - TotalColumns * cellwidth;
                        x = (TotalColumns - LastRowTotalItems) * cellwidth + (i - 1) * cellwidth;
                        if (offset > 0)
                            x += (offset / 2);
                        if (x < 0)
                            x = 0;
                        if (y >= (cellheight / 2.0))
                            y -= (cellheight / 2.0);
                        if (child is GridViewItem item)
                        {
                            item.CornerRadius = new CornerRadius(25);
                        }
                        if (VerticalOffset > 0)
                            y += (VerticalOffset / 2);
                        Point anchorPoint = new Point(x, y);
                        child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                        count++;
                    }
                }
                else
                {
                    ArrangeForHorizontalMode(finalSize);
                }
            }
            else
            {
                double x, y;
                if (TotalRows < 1)
                    TotalRows = 1;
                if (Children.Count == 0)
                    return;
                int TotalItemsWithinViewport = TotalColumnsWithinViewPort * TotalRows;
                double VerticalOffset = finalSize.Height - (cellheight * (TotalRows - 1) + LastRowcellheight);
                for (int i = 0; i < TotalItemsWithinViewport; i++)
                {
                    UIElement child = Children[i];
                    x = (i) % TotalColumnsWithinViewPort * child.DesiredSize.Width;
                    y = (i) / TotalColumnsWithinViewPort * child.DesiredSize.Height;
                    if (VerticalOffset > 0)
                        y += (VerticalOffset / 2);
                    Point anchorPoint = new Point(x, y);
                    child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                }
                int count = 1;
                int remainingCount = Children.Count - TotalItemsWithinViewport;
                for (int i = TotalItemsWithinViewport; i < Children.Count; i++)
                {
                    UIElement child = Children[i];
                    int row = (i) % TotalRows;
                    int column = ((i) / TotalRows);
                    x = column * child.DesiredSize.Width;
                    y = row * child.DesiredSize.Height;
                    if (VerticalOffset > 0)
                        y += (VerticalOffset / 2);
                    Point anchorPoint = new Point(x, y);
                    child.Arrange(new Rect(anchorPoint, child.DesiredSize));
                    count++;
                }
            }
        }


        public void ArrangeForVerticalFitMode(Size finalSize)
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
                double offset = finalSize.Width - TotalColumns * cellwidth;
                x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                y = (count - 1) / TotalColumns * child.DesiredSize.Height;
                if (offset > 0)
                    x += (offset / 2);

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
                    if (MainPage.GridMode == GridMode.AspectFit)
                    {
                        double offset1 = finalSize.Width - TotalColumns * cellwidth;
                        if (offset1 > 0)
                            x += (offset1 / 2);
                    }
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
        public void ArrangeForVerticalFillMode(Size finalSize)
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
            bool isScrollEnabled = false;
            if ((TotalRows - MaxRowsWithinViewPort) > 1)
            {
                isScrollEnabled = true;
            }
            else if ((TotalRows - MaxRowsWithinViewPort) == 1 && LastRowTotalItems > Math.Ceiling(TotalColumns / 2.0))
            {
                isScrollEnabled = true;
            }
            if (LastRowTotalItems > Math.Ceiling(TotalColumns / 2.0) || isScrollEnabled)
            {
                for (int i = 0; i < LastRowStartIndex; i++)
                {
                    UIElement child = Children[i];
                    double offset = finalSize.Width - TotalColumns * cellwidth;
                    x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                    y = (count - 1) / TotalColumns * child.DesiredSize.Height;
                    if (offset > 0)
                        x += (offset / 2);

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
                        //if (MainPage.GridMode == GridMode.AspectFit)
                        //{
                        //    double offset1 = finalSize.Width - TotalColumns * cellwidth;
                        //    if (offset1 > 0)
                        //        x += (offset1 / 2);
                        //}
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
            else
            {
                for (int i = 0; i < LastRowStartIndex; i++)
                {
                    UIElement child = Children[i];
                    double offset = finalSize.Width - TotalColumns * cellwidth;
                    x = (count - 1) % TotalColumns * child.DesiredSize.Width;
                    y = (count - 1) / TotalColumns * cellheight;
                    if (offset > 0)
                        x += (offset / 2);

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
                    //int centerX = (int)finalSize.Width / 2;
                    y = (count - 1) / TotalColumns * cellheight;
                    if (y >= (cellheight / 2.0))
                        y -= (cellheight / 2.0);
                    double offset = finalSize.Width - TotalColumns * cellwidth;
                    x = (TotalColumns - LastRowTotalItems) * cellwidth + (i - 1) * cellwidth;
                    if (offset > 0)
                        x += (offset / 2);
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
}
