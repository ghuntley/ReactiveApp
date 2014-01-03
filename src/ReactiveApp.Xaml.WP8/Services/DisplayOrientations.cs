﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Xaml.Services
{   
    /// <summary>
    /// Describes the orientation of a rectangular monitor.
    /// </summary>
    [Flags]
    public enum DisplayOrientations
    { 
        /// <summary>
        /// No display orientation is specified.
        /// </summary>
        None = 0,
   
        /// <summary>
        /// Specifies that the monitor is oriented in landscape mode where the width
        /// of the display viewing area is greater than the height.
        /// </summary>
        Landscape = 1,
 
        /// <summary>
        /// Specifies that the monitor rotated 90 degrees in the clockwise direction
        /// to orient the display in portrait mode where the height of the display viewing
        /// area is greater than the width.
        /// </summary>
        Portrait = 2,

        /// <summary>
        /// Specifies that the monitor rotated another 90 degrees in the clockwise direction
        /// (to equal 180 degrees) to orient the display in landscape mode where the
        /// width of the display viewing area is greater than the height. This landscape
        /// mode is flipped 180 degrees from the Landscape mode.
        /// </summary>
        LandscapeFlipped = 4,

        /// <summary>
        /// Specifies that the monitor rotated another 90 degrees in the clockwise direction
        /// (to equal 270 degrees) to orient the display in portrait mode where the height
        /// of the display viewing area is greater than the width. This portrait mode
        /// is flipped 180 degrees from the Portrait mode.
        /// </summary>
        PortraitFlipped = 8,
    }
}
