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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AdaptiveGridApp
{
    public sealed partial class ScreenshareControl : UserControl
    {
        // Using a DependencyProperty as the backing store for HasRoundedCorner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotoItemsProperty =
            DependencyProperty.Register("PhotoItems", typeof(ObservableCollection<PhotoItem>), typeof(ScreenshareControl), new PropertyMetadata(null));
        public ScreensharePanel panel = null;
        public ObservableCollection<PhotoItem> PhotoItems
        {
            get { return (ObservableCollection<PhotoItem>)GetValue(PhotoItemsProperty); }
            set { SetValue(PhotoItemsProperty, value); }
        }
        public ScreenshareControl()
        {
            this.InitializeComponent();
        }

        public void VideoModeToggled(VideoPosition videoPosition = VideoPosition.Right)
        {
            if (videoPosition == VideoPosition.Left)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(400, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 300;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 650;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[2].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[2].MaxWidth = 0;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 0;
                ScreenGrid.RowDefinitions[0].MaxHeight = 0;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(3, GridUnitType.Star);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[2].MinHeight = 0;
                ScreenGrid.RowDefinitions[2].MaxHeight = 0;
            }
            else if (videoPosition == VideoPosition.Right)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 0;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(400, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[2].MinWidth = 300;
                ScreenGrid.ColumnDefinitions[2].MaxWidth = 650;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 0;
                ScreenGrid.RowDefinitions[0].MaxHeight = 0;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(3, GridUnitType.Star);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[2].MinHeight = 0;
                ScreenGrid.RowDefinitions[2].MaxHeight = 0;

            }
            else if (videoPosition == VideoPosition.Top)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 0;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[2].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[2].MaxWidth = 0;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(400, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 300;
                ScreenGrid.RowDefinitions[0].MaxHeight = 650;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(3, GridUnitType.Star);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[2].MinHeight = 0;
                ScreenGrid.RowDefinitions[2].MaxHeight = 0;
            }
            else if (videoPosition == VideoPosition.Botton)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 0;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[2].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[2].MaxWidth = 0;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 0;
                ScreenGrid.RowDefinitions[0].MaxHeight = 0;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(3, GridUnitType.Star);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(400, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[2].MinHeight = 300;
                ScreenGrid.RowDefinitions[2].MaxHeight = 650;
            }
        }


        //private void panel_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (sender is ScreensharePanel panel)
        //    {
        //        this.panel = panel;
        //        panel.ListingControl = AdaptiveGridViewControl;
        //        panel.Margin = new Thickness(1, 1, 1, 1);
        //        panel.Margin = new Thickness(0, 0, 0, 0);
        //    }
        //}
    }
}
