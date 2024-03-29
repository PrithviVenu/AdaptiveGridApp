﻿using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
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
    public sealed partial class ParticipantHomeControl : Page
    {
        ObservableCollection<Participant> PhotoItems = new ObservableCollection<Participant>();
        IList<Participant> PhotoItemsList = new List<Participant>();
        private int CurrentIndex = 0;
        public static int CurrentAspectWidthRatio = 16;
        public static int CurrentAspectHeightRatio = 9;
        public static GridMode GridMode = GridMode.AspectFit;
        public static VideoPosition VideoPosition = VideoPosition.Right;
        public static GridMode PreviousGridMode = GridMode.AspectFit;
        public static ScrollMode ScrollMode = ScrollMode.Vertical;
        public static int TotalColumns = 1;
        public static int TotalRows = 1;
        public int MinimumWidth = 250;

        public CustomPanel panel = null;
        bool isScreenshareToggled = false;
        public ParticipantHomeControl()
        {
            this.InitializeComponent();
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(trickyTitleBar);
        }
        private int GetCurrentIndex()
        {
            return CurrentIndex % 26;
        }

        private void AddParticipant_Click(object sender, RoutedEventArgs e)
        {
            Participant item = PhotoItemsList[GetCurrentIndex()];
            PhotoItems.Add(item);
            CurrentIndex++;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 26; i++)
            {
                Participant item = new Participant
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
                //RatioGridTextBox.Text = " 4:3";
                CurrentAspectWidthRatio = 4;
                CurrentAspectHeightRatio = 3;
            }
            else if (Ratio.Contains("16:9") && CurrentAspectWidthRatio == 4)
            {
                isToggled = true;
                //RatioGridTextBox.Text = " 16:9";
                CurrentAspectWidthRatio = 16;
                CurrentAspectHeightRatio = 9;
            }
            if (isToggled)
            {
                CustomPanel.MinimumHeight = (CustomPanel.MinimumWidth * CurrentAspectHeightRatio) / CurrentAspectWidthRatio;
                if (panel == null)
                    return;
                panel.Margin = new Thickness(1, 1, 1, 1);
                panel.Margin = new Thickness(0, 0, 0, 0);
                //PhotoItems.Clear();
                //for (int i = 0; i < CurrentIndex; i++)
                //{
                //    PhotoItems.Add(PhotoItemsList[i % 26]);
                //}
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
                FindName("OverlayControl");
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
            if (ParticipantHomeControl.GridMode != GridMode)
            {
                SetGridModeVisibility(GridMode);
                if (GridMode == GridMode.Fill)
                {
                    ParticipantHomeControl.GridMode = GridMode.Fill;
                    VideoPositionItem.Visibility = Visibility.Collapsed;
                    AspectRatioItem.Visibility = Visibility.Collapsed;
                    VideoPositionItemSeperator.Visibility = Visibility.Collapsed;
                    AspectRatioItemSeperator.Visibility = Visibility.Collapsed;
                    //ModeGridTextBox.Text = "Fill";
                    //ScrollModeGrid.Visibility = Visibility.Visible;
                    //AspectRatioGrid.Visibility = Visibility.Visible;
                    //ModeGrid.Visibility = Visibility.Visible;
                    //MinWidthGrid.Visibility = Visibility.Visible;
                    if (AdaptiveGridViewControl != null)
                    {
                        panel.Margin = new Thickness(1, 1, 1, 1);
                        panel.Margin = new Thickness(0, 0, 0, 0);
                    }
                }
                else if (GridMode == GridMode.AspectFit)
                {
                    VideoPositionItem.Visibility = Visibility.Collapsed;
                    AspectRatioItem.Visibility = Visibility.Visible;
                    VideoPositionItemSeperator.Visibility = Visibility.Collapsed;
                    AspectRatioItemSeperator.Visibility = Visibility.Visible;
                    ParticipantHomeControl.GridMode = GridMode.AspectFit;
                    //ModeGridTextBox.Text = "Aspect Fit";
                    //ScrollModeGrid.Visibility = Visibility.Visible;
                    //AspectRatioGrid.Visibility = Visibility.Visible;
                    //ModeGrid.Visibility = Visibility.Visible;
                    //MinWidthGrid.Visibility = Visibility.Visible;
                    if (AdaptiveGridViewControl != null)
                    {
                        panel.Margin = new Thickness(1, 1, 1, 1);
                        panel.Margin = new Thickness(0, 0, 0, 0);
                    }
                }
                else if (GridMode == GridMode.Overlay)
                {
                    ParticipantHomeControl.GridMode = GridMode.Overlay;
                    VideoPositionItem.Visibility = Visibility.Collapsed;
                    AspectRatioItem.Visibility = Visibility.Collapsed;
                    VideoPositionItemSeperator.Visibility = Visibility.Collapsed;
                    AspectRatioItemSeperator.Visibility = Visibility.Collapsed;
                    //ModeGridTextBox.Text = "Overlay";
                    //ScrollModeGrid.Visibility = Visibility.Collapsed;
                    //AspectRatioGrid.Visibility = Visibility.Collapsed;
                    SetVerticalScroll();
                }
                else if (GridMode == GridMode.ActiveSpeaker)
                {
                    ParticipantHomeControl.GridMode = GridMode.ActiveSpeaker;
                    VideoPositionItem.Visibility = Visibility.Collapsed;
                    AspectRatioItem.Visibility = Visibility.Collapsed;
                    VideoPositionItemSeperator.Visibility = Visibility.Collapsed;
                    AspectRatioItemSeperator.Visibility = Visibility.Collapsed;
                    //ModeGridTextBox.Text = "Active Speaker";
                    //ScrollModeGrid.Visibility = Visibility.Collapsed;
                    //AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
                else if (GridMode == GridMode.AudioConference)
                {
                    ParticipantHomeControl.GridMode = GridMode.AudioConference;
                    VideoPositionItem.Visibility = Visibility.Collapsed;
                    AspectRatioItem.Visibility = Visibility.Collapsed;
                    VideoPositionItemSeperator.Visibility = Visibility.Collapsed;
                    AspectRatioItemSeperator.Visibility = Visibility.Collapsed;
                    //ModeGridTextBox.Text = "Audio Conference";
                    //ScrollModeGrid.Visibility = Visibility.Collapsed;
                    //AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
                else if (GridMode == GridMode.Screenshare)
                {
                    PreviousGridMode = ParticipantHomeControl.GridMode;
                    ParticipantHomeControl.GridMode = GridMode.Screenshare;
                    VideoPositionItem.Visibility = Visibility.Visible;
                    AspectRatioItem.Visibility = Visibility.Collapsed;
                    VideoPositionItemSeperator.Visibility = Visibility.Visible;
                    AspectRatioItemSeperator.Visibility = Visibility.Collapsed;
                    //MinWidthGrid.Visibility = Visibility.Collapsed;
                    //ModeGridTextBox.Text = "Screenshare";
                    //ModeGrid.Visibility = Visibility.Collapsed;
                    //ScrollModeGrid.Visibility = Visibility.Collapsed;
                    //AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
                else if (GridMode == GridMode.PoppedOut)
                {
                    ParticipantHomeControl.GridMode = GridMode.PoppedOut;
                    VideoPositionItem.Visibility = Visibility.Collapsed;
                    AspectRatioItem.Visibility = Visibility.Collapsed;
                    VideoPositionItemSeperator.Visibility = Visibility.Collapsed;
                    AspectRatioItemSeperator.Visibility = Visibility.Collapsed;
                    //ModeGridTextBox.Text = "Popped Out";
                    //ScrollModeGrid.Visibility = Visibility.Collapsed;
                    //AspectRatioGrid.Visibility = Visibility.Collapsed;
                }
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
            if (sender is Button button)
            {
                FlyoutBase.ShowAttachedFlyout(button);
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
            //ScrollModeGridTextBox.Text = "Horizontal";
            if (AdaptiveGridViewControl == null)
                return;
            Border border = VisualTreeHelper.GetChild(AdaptiveGridViewControl, 0) as Border;
            // get scrollviewer
            ScrollViewer scrollviewer = border.Child as ScrollViewer;
            scrollviewer.VerticalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Disabled;
            scrollviewer.HorizontalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Auto;
            scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            if (panel == null)
                return;
            panel.Margin = new Thickness(1, 1, 1, 1);
            panel.Margin = new Thickness(0, 0, 0, 0);
            //PhotoItems.Clear();
            //for (int i = 0; i < CurrentIndex; i++)
            //{
            //    PhotoItems.Add(PhotoItemsList[i % 26]);
            //}
        }

        public void SetVerticalScroll()
        {
            ScrollMode = ScrollMode.Vertical;
            //ScrollModeGridTextBox.Text = "Vertical";
            if (AdaptiveGridViewControl == null)
                return;
            Border border = VisualTreeHelper.GetChild(AdaptiveGridViewControl, 0) as Border;
            // get scrollviewer
            ScrollViewer scrollviewer = border.Child as ScrollViewer;
            scrollviewer.VerticalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Auto;
            scrollviewer.HorizontalScrollMode = Windows.UI.Xaml.Controls.ScrollMode.Disabled;
            scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            if (panel == null)
                return;
            panel.Margin = new Thickness(1, 1, 1, 1);
            panel.Margin = new Thickness(0, 0, 0, 0);
            //PhotoItems.Clear();
            //for (int i = 0; i < CurrentIndex; i++)
            //{
            //    PhotoItems.Add(PhotoItemsList[i % 26]);
            //}
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
                this.panel = panel;
                panel.ListingControl = AdaptiveGridViewControl;
                panel.Margin = new Thickness(1, 1, 1, 1);
                panel.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void RemoveLast_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentIndex > 0)
            {
                PhotoItems.RemoveAt(CurrentIndex - 1);
                CurrentIndex--;
            }
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {

        }

        private void UnlockedButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdaptiveGridViewControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((e.NewSize.Width == e.PreviousSize.Width && e.NewSize.Height != e.PreviousSize.Height) || (e.NewSize.Height == e.PreviousSize.Height && e.NewSize.Width != e.PreviousSize.Width))
            {
                if (panel == null)
                    return;
                panel.Margin = new Thickness(1, 1, 1, 1);
                panel.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((e.NewSize.Width == e.PreviousSize.Width && e.NewSize.Height != e.PreviousSize.Height) || (e.NewSize.Height == e.PreviousSize.Height && e.NewSize.Width != e.PreviousSize.Width))
            {
                if (panel == null)
                    return;
                panel.Margin = new Thickness(1, 1, 1, 1);
                panel.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void Screenshare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!isScreenshareToggled)
            {
                ToggleGridMode(GridMode.Screenshare);
                isScreenshareToggled = true;
            }
            else
            {
                ToggleGridMode(PreviousGridMode);
                isScreenshareToggled = false;
            }
        }

        private void Top_Click(object sender, RoutedEventArgs e)
        {
            VideoPosition = VideoPosition.Top;
            if (ScreenshareControl != null)
            {
                ScreenshareControl.VideoModeToggled(VideoPosition.Top);
            }
        }

        private void Bottom_Click(object sender, RoutedEventArgs e)
        {
            VideoPosition = VideoPosition.Botton;
            if (ScreenshareControl != null)
            {
                ScreenshareControl.VideoModeToggled(VideoPosition.Botton);
            }
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            VideoPosition = VideoPosition.Left;
            if (ScreenshareControl != null)
            {
                ScreenshareControl.VideoModeToggled(VideoPosition.Left);
            }
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            VideoPosition = VideoPosition.Right;
            if (ScreenshareControl != null)
            {
                ScreenshareControl.VideoModeToggled(VideoPosition.Right);
            }
        }

        private void BottomPane_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 600)
            {
                OptionsGrid.Visibility = Visibility.Visible;
                ChatGrid.Visibility = Visibility.Collapsed;
                ParticipantGrid.Visibility = Visibility.Collapsed;
                RightGrid.ColumnDefinitions[2].Width = GridLength.Auto;
            }
            else
            {
                OptionsGrid.Visibility = Visibility.Collapsed;
                ChatGrid.Visibility = Visibility.Visible;
                ParticipantGrid.Visibility = Visibility.Visible;
                RightGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
            }
        }

        private void Small_Click(object sender, RoutedEventArgs e)
        {
            CustomPanel.MinimumWidth = 200; ;
            CustomPanel.MinimumHeight = (200 * CurrentAspectHeightRatio) / CurrentAspectWidthRatio;
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            CustomPanel.MinimumWidth = 250;
            CustomPanel.MinimumHeight = (250 * CurrentAspectHeightRatio) / CurrentAspectWidthRatio;
        }

        private void Large_Click(object sender, RoutedEventArgs e)
        {
            CustomPanel.MinimumWidth = 300;
            CustomPanel.MinimumHeight = (300 * CurrentAspectHeightRatio) / CurrentAspectWidthRatio;
        }

        private void EndCall_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
