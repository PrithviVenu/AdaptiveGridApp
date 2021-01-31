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
        public int Dimension = 1;
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
            Dimension = (int)Math.Ceiling(Math.Sqrt(PhotoItems.Count));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PhotoItems.Add(PhotoItemsList[GetCurrentIndex()]);
            CurrentIndex++;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeChanged += MainPage_SizeChanged;
            PhotoItems.CollectionChanged += PhotoItems_CollectionChanged;
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

        private void PhotoItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ComputeAndSetDimension();
            AdaptiveGridViewControl.DesiredWidth = ActualWidth / Dimension;
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdaptiveGridViewControl.DesiredWidth = e.NewSize.Width / Dimension;
        }
    }
}
