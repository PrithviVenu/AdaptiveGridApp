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
            if (ParticipantHomeControl.GridMode == GridMode.AspectFit)
                return new Size(availableSize.Width, (availableSize.Width * ParticipantHomeControl.CurrentAspectHeightRatio) / ParticipantHomeControl.CurrentAspectWidthRatio);
            else
                return availableSize;
        }
    }
}
