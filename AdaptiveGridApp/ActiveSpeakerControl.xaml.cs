﻿using System;
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
    public sealed partial class ActiveSpeakerControl : UserControl
    {
        // Using a DependencyProperty as the backing store for HasRoundedCorner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotoItemsProperty =
            DependencyProperty.Register("PhotoItems", typeof(ObservableCollection<PhotoItem>), typeof(ActiveSpeakerControl), new PropertyMetadata(null));
        public ObservableCollection<PhotoItem> PhotoItems
        {
            get { return (ObservableCollection<PhotoItem>)GetValue(PhotoItemsProperty); }
            set { SetValue(PhotoItemsProperty, value); }
        }
        public ActiveSpeakerControl()
        {
            this.InitializeComponent();
        }

        private void CustomPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ActiveSpeakerCustomPanel panel)
            {
                panel.ListingControl = AdaptiveGridViewControl;
            }
        }
    }
}
