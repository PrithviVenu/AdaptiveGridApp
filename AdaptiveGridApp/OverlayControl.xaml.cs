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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AdaptiveGridApp
{
    public sealed partial class OverlayControl : UserControl
    {
        // Using a DependencyProperty as the backing store for HasRoundedCorner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PhotoItemsProperty =
            DependencyProperty.Register("PhotoItems", typeof(ObservableCollection<PhotoItem>), typeof(OverlayControl), new PropertyMetadata(null));
        public ObservableCollection<PhotoItem> PhotoItems
        {
            get { return (ObservableCollection<PhotoItem>)GetValue(PhotoItemsProperty); }
            set { SetValue(PhotoItemsProperty, value); }
        }


        // Using a DependencyProperty as the backing store for HasRoundedCorner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayParticipantProperty =
            DependencyProperty.Register("PhotoItem", typeof(PhotoItem), typeof(OverlayControl), new PropertyMetadata(null));
        public PhotoItem OverlayParticipant
        {
            get { return (PhotoItem)GetValue(OverlayParticipantProperty); }
            set { SetValue(OverlayParticipantProperty, value); }
        }

        public OverlayControl()
        {
            this.InitializeComponent();
        }

        private void CustomPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is OverlayPanel panel)
            {
                panel.ListingControl = AdaptiveGridViewControl;
                panel.Margin = new Thickness(1, 1, 1, 1);
                panel.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PhotoItem item = new PhotoItem
            {
                Title = ((20).ToString() + ".jpg")
            };
            string ImageURI = "ms-appx:///Photos/" + item.Title;
            Uri uri = new Uri(ImageURI);
            BitmapImage bitmapImage = new BitmapImage
            {
                UriSource = uri
            };
            item.ImageURI = bitmapImage;
            OverlayParticipant = item;
        }
    }
}
