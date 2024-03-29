﻿using System.Collections.Generic;

namespace ArtemisFlyout.Models.Configuration
{
    public class LedColorPickerLedSettings
    {
        public string DefaultColor { get; set; }
        public bool KeepColorInSync { get; set; }
        public List<LedColorPickerLed> LedColorPickerLeds { get; set; }
        public byte ColorMaskOpacity { get; set; }
        public bool Enabled { get; set; }
    }
}
