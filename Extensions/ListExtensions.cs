using System;
using System.Collections.Generic;

namespace ColorPalette
{
	public static class ListExtensions
	{
		public static int IndexOfItemWithColorFromRange (this List<PixelColor> list, YUVColor color, double range)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if(list[i].Color.DistanceTo(color) <= range)
				{
					// In range!
					return i;
				}
			}

			return -1;
		}
	}
}

