using System;
using UIKit;

namespace ColorPalette
{
	// Color representation in Y'UV plane
	// Y component represents brigtnes: 0 - dark, 1 - bright
	public class YUVColor : ICloneable
	{
		public float Y { get; set; }
		public float U { get; set; }
		public float V { get; set; }

		internal UIColor ToRGBUIColor()
		{
			nfloat r = Y + (1.140f * V);
			nfloat g = Y - (0.395f * U) - (0.581f * V);
			nfloat b = Y + (2.032f * U);

			return new UIColor (r, g, b, 255);
		}

		public override string ToString ()
		{
			return $"YUVColor [Y={Y}, U={U}, V={V}]";
		}

		public object Clone()
		{
			return new YUVColor() {Y = this.Y, U = this.U, V = this.V};
		}
	}
}

