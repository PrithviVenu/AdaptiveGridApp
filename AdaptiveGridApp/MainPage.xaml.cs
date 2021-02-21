﻿using Microsoft.Toolkit.Uwp.UI.Controls;
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
        public static int TotalColumns = 1;
        public static int TotalRows = 1;
        public int MinimumWidth = 250;
        public static int CurrentAspectWidthRatio = 16;
        public static int CurrentAspectHeightRatio = 9;

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
        }
        private void AddParticipant_Click(object sender, RoutedEventArgs e)
        {
            PhotoItem item = PhotoItemsList[GetCurrentIndex()];
            PhotoItems.Add(item);
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
            Reset();
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
        private void Reset()
        {
            PhotoItems.Clear();
            CurrentIndex = 0;
            TotalColumns = 1;
            TotalRows = 1;
        }
        private void AdaptiveGridViewControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ComputeAndSetDimension();
        }

        private void ToggleAspectRatio_Click(object sender, RoutedEventArgs e)
        {
            if (AspectRatio.Text.Contains("16:9"))
            {
                AspectRatio.Text = " 4:3";
                CurrentAspectWidthRatio = 4;
                CurrentAspectHeightRatio = 3;
            }
            else if (AspectRatio.Text.Contains("4:3"))
            {
                AspectRatio.Text = " 16:9";
                CurrentAspectWidthRatio = 16;
                CurrentAspectHeightRatio = 9;
            }
            Reset();
        }
    }
}
