using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace AdaptiveGridApp
{
    public class AspectContentControl : ContentControl
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            if (MainPage.GridMode == GridMode.AspectFit)
                return new Size(availableSize.Width, (availableSize.Width * MainPage.CurrentAspectHeightRatio) / MainPage.CurrentAspectWidthRatio);
            else
                return availableSize;
        }
    }
}
