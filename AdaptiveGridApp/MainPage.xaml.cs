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
        public static int CurrentAspectWidthRatio = 16;
        public static int CurrentAspectHeightRatio = 9;
        public static GridMode GridMode = GridMode.AspectFit;
        public static ScrollMode ScrollMode = ScrollMode.Vertical;
        public static int TotalColumns = 1;
        public static int TotalRows = 1;
        public int MinimumWidth = 250;

        public MainPage()
        {
            this.InitializeComponent();
        }
        private int GetCurrentIndex()
        {
            return CurrentIndex % 26;
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
            bool isToggled = false;
            if (Ratio.Contains("4:3") && CurrentAspectWidthRatio == 16)
            {
                isToggled = true;
                RatioGridTextBox.Text = " 4:3";
                CurrentAspectWidthRatio = 4;
                CurrentAspectHeightRatio = 3;
            }
            else if (Ratio.Contains("16:9") && CurrentAspectWidthRatio == 4)
            {
                isToggled = true;
                RatioGridTextBox.Text = " 16:9";
                CurrentAspectWidthRatio = 16;
                CurrentAspectHeightRatio = 9;
            }
            if (isToggled)
            {
                PhotoItems.Clear();
                for (int i = 0; i < CurrentIndex; i++)
                {
                    PhotoItems.Add(PhotoItemsList[i % 26]);
                }
            }
        }
        private void SetGridModeVisibility(GridMode GridMode)
        {
            if (GridMode == GridMode.Fill || GridMode == GridMode.AspectFit)
            {
                FindName("AdaptiveGridViewControl");
                UnloadObject(OverlayControl);
                UnloadObject(ActiveSpeakerControl);
                UnloadObject(AudioConferenceControl);
                UnloadObject(PoppedoutControl);
                UnloadObject(ScreenshareControl);
            }
            else if (GridMode == GridMode.Overlay)
            {
                UnloadObject(AdaptiveGridViewControl);
                UnloadObject(ActiveSpeakerControl);
                UnloadObject(AudioConferenceControl);
                UnloadObject(PoppedoutControl);
                UnloadObject(ScreenshareControl);
            }
            else if (GridMode == GridMode.ActiveSpeaker)
            {
                FindName("ActiveSpeakerControl");
                UnloadObject(AdaptiveGridViewControl);
                UnloadObject(OverlayControl);
                UnloadObject(AudioConferenceControl);
                UnloadObject(PoppedoutControl);
                UnloadObject(ScreenshareControl);
            }
            else if (GridMode == GridMode.Screenshare)
            {
                FindName("ScreenshareControl");
                UnloadObject(AdaptiveGridViewControl);
                UnloadObject(OverlayControl);
                UnloadObject(AudioConferenceControl);
                UnloadObject(PoppedoutControl);
                UnloadObject(ActiveSpeakerControl);
            }
            else if (GridMode == GridMode.PoppedOut)
            {
                FindName("PoppedoutControl");
                UnloadObject(AdaptiveGridViewControl);
                UnloadObject(OverlayControl);
                UnloadObject(AudioConferenceControl);
                UnloadObject(ActiveSpeakerControl);
                UnloadObject(ScreenshareControl);
            }
            else if (GridMode == GridMode.AudioConference)
            {
                FindName("AudioConferenceControl");
                UnloadObject(AdaptiveGridViewControl);
                UnloadObject(OverlayControl);
                UnloadObject(ActiveSpeakerControl);
                UnloadObject(PoppedoutControl);
                UnloadObject(ScreenshareControl);
            }
        }
        private void ToggleGridMode(GridMode GridMode)
        {
            if (MainPage.GridMode != GridMode)
            {
                if (GridMode == GridMode.Fill)
                {
                    MainPage.GridMode = GridMode.Fill;
                    ModeGridTextBox.Text = "Fill";
                    ScrollModeGrid.Visibility = Visibility.Visible;
                    AspectRatioGrid.Visibility = Visibility.Visible;
                    AdaptiveGridViewControl.ItemsPanelRoot.Margin = new Thickness(1, 1, 1, 1);
                    AdaptiveGridViewControl.ItemsPanelRoot.Margin = new Thickness(0, 0, 0, 0);
                }
                else if (GridMode == GridMode.AspectFit)
                {
                    MainPage.GridMode = GridMode.AspectFit;
                    ModeGridTextBox.Text = "Aspect Fit";
                    ScrollModeGrid.Visibility = Visibility.Visible;
                    AspectRatioGrid.Visibility = Visibility.Visible;
                    AdaptiveGridViewControl.ItemsPanelRoot.Margin = new Thickness(1, 1, 1, 1);
                    AdaptiveGridViewControl.ItemsPanelRoot.Margin = new Thickness(0, 0, 0, 0);
                }
                else if (GridMode == GridMode.Overlay)
                {
                    MainPage.GridMode = GridMode.Overlay;
                    ModeGridTextBox.Text = "Overlay";
                    ScrollModeGrid.Visibility = Visibility.Collapsed;
                    AspectRatioGrid.Visibility = Visibility.Collapsed;
                    SetVerticalScroll();
                }
                else if (GridMode == GridMode.ActiveSpeaker)
                {
                    MainPage.GridMode = GridMode.ActiveSpeaker;
                    ModeGridTextBox.Text = "Active Speaker";
                    ScrollModeGrid.Visibility = Visibility.Collapsed;
                    AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
                else if (GridMode == GridMode.AudioConference)
                {
                    MainPage.GridMode = GridMode.AudioConference;
                    ModeGridTextBox.Text = "Audio Conference";
                    ScrollModeGrid.Visibility = Visibility.Collapsed;
                    AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
                else if (GridMode == GridMode.Screenshare)
                {
                    MainPage.GridMode = GridMode.Screenshare;
                    ModeGridTextBox.Text = "Screenshare";
                    ScrollModeGrid.Visibility = Visibility.Collapsed;
                    AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
                else if (GridMode == GridMode.PoppedOut)
                {
                    MainPage.GridMode = GridMode.PoppedOut;
                    ModeGridTextBox.Text = "Popped Out";
                    ScrollModeGrid.Visibility = Visibility.Collapsed;
                    AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
                SetGridModeVisibility(GridMode);
            }
        }

        private void MinWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int result))
            {
                CustomPanel.MinimumWidth = result;
                CustomPanel.MinimumHeight = (result * CurrentAspectHeightRatio) / CurrentAspectWidthRatio;
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


        private void Overlay_Click(object sender, RoutedEventArgs e)
        {
            ToggleGridMode(GridMode.Overlay);
        }

        private void ActiveSpeaker_Click(object sender, RoutedEventArgs e)
        {
            ToggleGridMode(GridMode.ActiveSpeaker);
        }

        private void Screenshare_Click(object sender, RoutedEventArgs e)
        {
            ToggleGridMode(GridMode.Screenshare);
        }
        private void PoppedOut_Click(object sender, RoutedEventArgs e)
        {
            ToggleGridMode(GridMode.PoppedOut);
        }

        private void AudioConference_Click(object sender, RoutedEventArgs e)
        {
            ToggleGridMode(GridMode.AudioConference);
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

        private void HorizontalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            if (ScrollMode != ScrollMode.Horizontal)
            {
                SetHorizontalScroll();
            }
        }

        public void SetHorizontalScroll()
        {
            ScrollMode = ScrollMode.Horizontal;
            ScrollModeGridTextBox.Text = "Horizontal";
            if (AdaptiveGridViewControl == null)
                return;
            Border border = VisualTreeHelper.GetChild(AdaptiveGridViewControl, 0) as Border;
            // get scrollviewer
            ScrollViewer scrollviewer = border.Child as ScrollViewer;
            scrollviewer.VerticalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Disabled;
            scrollviewer.HorizontalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Enabled;
            scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            PhotoItems.Clear();
            for (int i = 0; i < CurrentIndex; i++)
            {
                PhotoItems.Add(PhotoItemsList[i % 26]);
            }
        }

        public void SetVerticalScroll()
        {
            ScrollMode = ScrollMode.Vertical;
            ScrollModeGridTextBox.Text = "Vertical";
            if (AdaptiveGridViewControl == null)
                return;
            Border border = VisualTreeHelper.GetChild(AdaptiveGridViewControl, 0) as Border;
            // get scrollviewer
            ScrollViewer scrollviewer = border.Child as ScrollViewer;
            scrollviewer.VerticalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Enabled;
            scrollviewer.HorizontalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Disabled;
            scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            PhotoItems.Clear();
            for (int i = 0; i < CurrentIndex; i++)
            {
                PhotoItems.Add(PhotoItemsList[i % 26]);
            }
        }


        private void VerticalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            if (ScrollMode != ScrollMode.Vertical)
            {
                SetVerticalScroll();
            }
        }

        private void CustomPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is CustomPanel panel)
            {
                panel.ListingControl = AdaptiveGridViewControl;
                panel.Margin = new Thickness(1, 1, 1, 1);
                panel.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void RemoveLast_Click(object sender, RoutedEventArgs e)
        {
            PhotoItems.RemoveAt(CurrentIndex - 1);
            CurrentIndex--;
        }
    }
}
