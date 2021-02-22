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
        public static int CurrentAspectWidthRatio = 16;
        public static int CurrentAspectHeightRatio = 9;
        public static GridMode GridMode = GridMode.AspectFit;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private int GetCurrentIndex()
        {
            return CurrentIndex %= 26;
        }

        private void AddParticipant_Click(object sender, RoutedEventArgs e)
        {
            PhotoItem item = PhotoItemsList[GetCurrentIndex()];
            PhotoItems.Add(item);
            CurrentIndex++;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void ClearParticipants_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            PhotoItems.Clear();
            CurrentIndex = 0;
        }

        private void ToggleAspectRatio(string Ratio)
        {
            if (Ratio.Contains("4:3"))
            {
                RatioGridTextBox.Text = " 4:3";
                CurrentAspectWidthRatio = 4;
                CurrentAspectHeightRatio = 3;
            }
            else if (Ratio.Contains("16:9"))
            {
                RatioGridTextBox.Text = " 16:9";
                CurrentAspectWidthRatio = 16;
                CurrentAspectHeightRatio = 9;
            }
            Reset();
        }

        private void ToggleGridMode(GridMode GridMode)
        {
            if (GridMode == GridMode.Fill)
            {
                MainPage.GridMode = GridMode.Fill;
                ModeGridTextBox.Text = "Fill";
                ModeIcon.Margin = new Thickness(62, 0, 0, 0);
            }
            else if (GridMode == GridMode.AspectFit)
            {
                MainPage.GridMode = GridMode.AspectFit;
                ModeGridTextBox.Text = "Aspect Fit";
                ModeIcon.Margin = new Thickness(20, 0, 0, 0);
            }
        }

        private void MinWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int result))
            {
                CustomPanel.MinimumWidth = result;
            }
        }

        private void AspectFit_Click(object sender, RoutedEventArgs e)
        {
            ToggleGridMode(GridMode.AspectFit);
        }

        private void Fill_Click(object sender, RoutedEventArgs e)
        {
            ToggleGridMode(GridMode.Fill);
        }

        private void ModeGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                FlyoutBase.ShowAttachedFlyout(textBlock);
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Grid grid)
            {
                FlyoutBase.ShowAttachedFlyout(grid);
            }

        }

        private void Ratio16W9H_Click(object sender, RoutedEventArgs e)
        {
            ToggleAspectRatio("16:9");
        }

        private void Ratio4W3H_Click(object sender, RoutedEventArgs e)
        {
            ToggleAspectRatio("4:3");
        }
    }
}
