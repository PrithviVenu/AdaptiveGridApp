using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AdaptiveGridApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<PhotoItem> PhotoItems = new ObservableCollection<PhotoItem>();
        IList<PhotoItem> PhotoItemsList = new List<PhotoItem>();
        private int CurrentIndex = 0;
        public int TotalColumns = 1;
        public int TotalRows = 1;
        public int MinimumWidth = 350;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private int GetCurrentIndex()
        {
            return CurrentIndex %= 26;
        }

        private void ComputeAndSetDimension()
        {
            int cols = (int)Math.Ceiling(Math.Sqrt(PhotoItems.Count));
            if (cols > 0)
                TotalColumns = cols;
            double itemWidth = AdaptiveGridViewControl.ActualWidth / TotalColumns;
            if (itemWidth < MinimumWidth)
            {
                int MaxColumns = (int)(AdaptiveGridViewControl.ActualWidth / MinimumWidth);
                TotalColumns = MaxColumns;
            }
            if (TotalColumns > 0)
                TotalRows = (int)Math.Ceiling(PhotoItems.Count / (double)TotalColumns);
            AdaptiveGridViewControl.DesiredWidth = AdaptiveGridViewControl.ActualWidth / TotalColumns;
            // ComputeAndSetMargins();
        }
        private void AddParticipant_Click(object sender, RoutedEventArgs e)
        {
            PhotoItems.Add(PhotoItemsList[GetCurrentIndex()]);
            CurrentIndex++;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AdaptiveGridViewControl.Items.VectorChanged += Items_VectorChanged;
            for (int i = 0; i < 26; i++)
            {
                PhotoItem item = new PhotoItem
                {
                    Title = ((i + 1).ToString() + ".jpg")
                };
                string ImageURI = "ms-appx:///Photos/" + item.Title;
                Uri uri = new Uri(ImageURI);
                BitmapImage bitmapImage = new BitmapImage
                {
                    UriSource = uri
                };
                item.ImageURI = bitmapImage;
                PhotoItemsList.Add(item);
            }
        }

        private void Items_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            ComputeAndSetDimension();
        }

        private void ClearParticipants_Click(object sender, RoutedEventArgs e)
        {
            PhotoItems.Clear();
            CurrentIndex = 0;
            TotalColumns = 1;
            TotalRows = 1;
        }

        private void ComputeAndSetMargins()
        {
            int LastRowTotalItems = PhotoItems.Count - ((TotalRows - 1) * TotalColumns);
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
                DependencyObject containerAtIndex = AdaptiveGridViewControl.ContainerFromIndex(i);
                if (containerAtIndex is GridViewItem gvItem)
                {
                    gvItem.Margin = new Thickness(0, 0, 10, 0);
                }
            }
            int CenterItemIndex = TotalColumns / 2 + TotalColumns % 2;
            for (int i = 1; i <= LastRowTotalItems; i++)
            {
                int indexFactor = LastRowStartIndex - 1;
                int index = indexFactor + i;
                DependencyObject containerAtIndex = AdaptiveGridViewControl.ContainerFromIndex(index);
                if (containerAtIndex is GridViewItem gvItem)
                {
                    if (LastRowTotalItems > CenterItemIndex)
                    {
                        gvItem.Margin = new Thickness(0, 0, 10, 0);
                    }
                    else
                    {
                        int RightMargin = 10;
                        int centerX = (int)AdaptiveGridViewControl.ActualWidth / 2;
                        int LeftMargin = centerX - ((int)((CenterItemIndex - i) * AdaptiveGridViewControl.DesiredWidth + RightMargin)) - ((int)AdaptiveGridViewControl.DesiredWidth / 2);//- ((int)AdaptiveGridViewControl.DesiredWidth / 2)
                        if (LeftMargin < 0)
                            LeftMargin = 0;
                        gvItem.Margin = new Thickness(LeftMargin, 0, RightMargin, 0);
                        DependencyObject contai = AdaptiveGridViewControl.ContainerFromItem(gvItem);

                    }
                }
            }
        }

        private bool IsItemInLastRow(PhotoItem photoItem)
        {
            bool IsItemInLastRow = false;
            int CurrentRow = (int)Math.Ceiling((PhotoItems.IndexOf(photoItem) + 1) / (double)TotalColumns);

            if (CurrentRow == TotalRows)
            {
                IsItemInLastRow = true;
            }
            return IsItemInLastRow;
        }

        private void AdaptiveGridViewControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ComputeAndSetDimension();
        }
    }
}
