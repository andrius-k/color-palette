using System;
using UIKit;

namespace ColorPalette
{
	public static class UIColorExtensions
	{
		public static YUVColor ToYUV(this UIColor color)
		{
			var cgColor = color.CGColor;

			// RGB values
			float[] rgb = {0,0,0};

			//Y'UV values
			float y = 0;
			float u = 0;
			float v = 0;

			// Last component is alpha
			for(int i = 0; i < cgColor.NumberOfComponents - 1; i++)
			{
				// Fill rgb array
				rgb [i] = (float)cgColor.Components [i];
			}
			
			// Calculate Y'UV color:
			y = 0.299f * rgb[0] + 0.587f * rgb[1] + 0.114f * rgb[2];
			u = 0.492f * (rgb[2] - y);
			v = 0.877f * (rgb[0] - y);

			return new YUVColor () { Y = y, U = u, V = v };
		}
	}
}

