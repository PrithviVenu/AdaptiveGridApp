﻿using System;
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
            return new Size(availableSize.Width, (availableSize.Width * 9.0) / 16.0);
        }
    }
}
