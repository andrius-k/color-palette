using System;
using UIKit;

namespace ColorPalette
{
	// This holds color of one pixel of the image and number of its occurrences
	public class PixelColor
	{
		// Color of one pixel
		public YUVColor Color { get; set; } = UIColor.Red.ToYUV();

		// Number of its occurrences in the image
		public int NumberOfOccurrences { get; set; }

		public override string ToString ()
		{
			return $"[PixelColor: Color={Color}, NumberOfOccurrences={NumberOfOccurrences}]";
		}
	}
}

