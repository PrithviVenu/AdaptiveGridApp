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
        public ScreensharePanel RightPanel = null;
        public ScreensharePanel LeftPanel = null;
        public ScreensharePanel TopPanel = null;
        public ScreensharePanel BottomPanel = null;

        public ObservableCollection<PhotoItem> PhotoItems
        {
            get { return (ObservableCollection<PhotoItem>)GetValue(PhotoItemsProperty); }
            set { SetValue(PhotoItemsProperty, value); }
        }
        public ScreenshareControl()
        {
            this.InitializeComponent();
        }
        public VideoPosition videoPosition = VideoPosition.Right;

        public void VideoModeToggled(VideoPosition videoPosition = VideoPosition.Right)
        {
            this.videoPosition = videoPosition;
            if (videoPosition == VideoPosition.Left)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(400, GridUnitType.Pixel);
                //LeftGrid.ColumnDefinitions[0].Width = new GridLength(400, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 300;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 650;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Auto);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[3].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[4].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[4].MaxWidth = 0;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 0;
                ScreenGrid.RowDefinitions[0].MaxHeight = 0;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                ScreenGrid.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[4].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[4].MinHeight = 0;
                ScreenGrid.RowDefinitions[4].MaxHeight = 0;
                SetVideoPositionVisibility(videoPosition);
                //LeftPanel.ListingControl = AdaptiveGridViewControlLeft;
            }
            else if (videoPosition == VideoPosition.Right)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 0;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Auto);
                ScreenGrid.ColumnDefinitions[4].Width = new GridLength(400, GridUnitType.Pixel);
                //RightGrid.ColumnDefinitions[4].Width = new GridLength(400, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[4].MinWidth = 300;
                ScreenGrid.ColumnDefinitions[4].MaxWidth = 650;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 0;
                ScreenGrid.RowDefinitions[0].MaxHeight = 0;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                ScreenGrid.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[4].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[4].MinHeight = 0;
                ScreenGrid.RowDefinitions[4].MaxHeight = 0;
                SetVideoPositionVisibility(videoPosition);
                //RightPanel.ListingControl = AdaptiveGridViewControlRight;
            }
            else if (videoPosition == VideoPosition.Top)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 0;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[3].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[4].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[4].MaxWidth = 0;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(250, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 250;
                ScreenGrid.RowDefinitions[0].MaxHeight = 250;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                ScreenGrid.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[4].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[4].MinHeight = 0;
                ScreenGrid.RowDefinitions[4].MaxHeight = 0;
                SetVideoPositionVisibility(videoPosition);
                //TopPanel.ListingControl = AdaptiveGridViewControlTop;
            }
            else if (videoPosition == VideoPosition.Botton)
            {
                ScreenGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[0].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[0].MaxWidth = 0;
                ScreenGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                ScreenGrid.ColumnDefinitions[3].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.ColumnDefinitions[4].MinWidth = 0;
                ScreenGrid.ColumnDefinitions[4].MaxWidth = 0;
                ScreenGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[0].MinHeight = 0;
                ScreenGrid.RowDefinitions[0].MaxHeight = 0;
                ScreenGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                ScreenGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Auto);
                ScreenGrid.RowDefinitions[4].Height = new GridLength(250, GridUnitType.Pixel);
                ScreenGrid.RowDefinitions[4].MinHeight = 250;
                ScreenGrid.RowDefinitions[4].MaxHeight = 250;
                SetVideoPositionVisibility(videoPosition);
                //BottomPanel.ListingControl = AdaptiveGridViewControlBottom;
            }
        }


        private void SetVideoPositionVisibility(VideoPosition VideoPosition = VideoPosition.Right)
        {
            if (videoPosition == VideoPosition.Left)
            {
                FindName("AdaptiveGridViewControlLeft");
                UnloadObject(AdaptiveGridViewControlRight);
                UnloadObject(AdaptiveGridViewControlTop);
                UnloadObject(AdaptiveGridViewControlBottom);
            }
            else if (videoPosition == VideoPosition.Right)
            {
                FindName("AdaptiveGridViewControlRight");
                UnloadObject(AdaptiveGridViewControlLeft);
                UnloadObject(AdaptiveGridViewControlTop);
                UnloadObject(AdaptiveGridViewControlBottom);
            }
            else if (videoPosition == VideoPosition.Top)
            {
                FindName("AdaptiveGridViewControlTop");
                UnloadObject(AdaptiveGridViewControlLeft);
                UnloadObject(AdaptiveGridViewControlRight);
                UnloadObject(AdaptiveGridViewControlBottom);
            }
            else if (videoPosition == VideoPosition.Botton)
            {
                FindName("AdaptiveGridViewControlBottom");
                UnloadObject(AdaptiveGridViewControlLeft);
                UnloadObject(AdaptiveGridViewControlRight);
                UnloadObject(AdaptiveGridViewControlTop);
            }
        }


        private void panel_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ScreensharePanel panel)
            {
                //if (panel.Name == "LeftPanel")
                //{
                //    LeftPanel = panel;
                //}
                //else if (panel.Name == "RightPanel")
                //{
                //    RightPanel = panel;
                //}
                //else if (panel.Name == "TopPanel")
                //{
                //    TopPanel = panel;
                //}
                //else if (panel.Name == "BottomPanel")
                //{
                //    BottomPanel = panel;
                //}
                if (videoPosition == VideoPosition.Right)
                    panel.ListingControl = AdaptiveGridViewControlRight;
                else if (videoPosition == VideoPosition.Left)
                    panel.ListingControl = AdaptiveGridViewControlLeft;
                else if (videoPosition == VideoPosition.Top)
                    panel.ListingControl = AdaptiveGridViewControlTop;
                else if (videoPosition == VideoPosition.Botton)
                    panel.ListingControl = AdaptiveGridViewControlBottom;
                panel.Margin = new Thickness(1, 1, 1, 1);
                panel.Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }
}
