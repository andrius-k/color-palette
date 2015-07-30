using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ColorPalette
{
	public class ColorPaletteGenerator
	{
		// =========================== Constants ============================ //

		// 0 - 2√2. Colors within this range will be treated as equal
		private const double RANGE = 0.075;

		// 0 - 1. Mininum distance of color's Y component for colors not to blend in with each other
		private const float MIN_CONTRAST_DISTANCE = 0.5f;

		// 0 - 1. By this distance color's Y component will be increased/decreased when generating lighter and darker version of colors
		private const float VARIANT_COLOR_DISTANCE = 0.3f;

		// Maximum image width and height. If image is bigger, it will be scaled down to fit
		private const int MAX_IMAGE_SIZE = 40;

		// ======================== Public properties ======================= //

		/// <summary>
		/// Dominant muted color
		/// </summary>
		public UIColor MutedColor { get; private set; }

		/// <summary>
		/// Dominant vibrant color
		/// </summary>
		public UIColor VibrantColor { get; private set; }

		/// <summary>
		/// Dominant light muted color
		/// </summary>
		public UIColor LightMutedColor { get; private set; }

		/// <summary>
		/// Dominant light vibrant color
		/// </summary>
		public UIColor LightVibrantColor { get; private set; }

		/// <summary>
		/// Dominant dark muted color
		/// </summary>
		public UIColor DarkMutedColor { get; private set; }

		/// <summary>
		/// Dominant light vibrant color
		/// </summary>
		public UIColor DarkVibrantColor { get; private set; }

		// ========================= private fields ========================= //

		private List<PixelColor> listOfColors = new List<PixelColor>();

		// ================================================================= //

		/// <summary>
		/// Generates color palette
		/// </summary>
		public void Generate (UIImage image)
		{
			// Clear list at first
			listOfColors.Clear ();

			// Scale image down if needed
			image = ScaleImageDown (image);

			for (int y = 1; y < image.Size.Height - 1; y++)
			{
				for (int x = 1; x < image.Size.Width - 1; x++)
				{
					var color = GetPixelColor (new CGPoint (x, y), image);
					var index = listOfColors.IndexOfItemWithColorFromRange (color.ToYUV (), RANGE);

					if(index == -1)
					{
						// Add new color to the list
						listOfColors.Add (new PixelColor() {Color = color.ToYUV(), NumberOfOccurrences = 0});
					}
					else
					{
						// This color already exists. Just increase NumberOfOccurrences
						listOfColors [index].NumberOfOccurrences++;
					}
				}
			}

			// Find the color palette
			findColorPalette ((float)(image.Size.Width * image.Size.Height));
		}

		private UIImage ScaleImageDown(UIImage sourceImage)
		{
			// Scale image to fit max size preserving aspect ratio

			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Min(MAX_IMAGE_SIZE / sourceSize.Width, MAX_IMAGE_SIZE / sourceSize.Height);

			if (maxResizeFactor > 1) 
				return sourceImage;
			
			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;

			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));

			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return resultImage;
		}

		private UIColor GetPixelColor(CGPoint point, UIImage image)
		{
			var rawData = new byte[4];
			var handle = GCHandle.Alloc(rawData);
			UIColor resultColor = null;
			try
			{
				using (var colorSpace = CGColorSpace.CreateDeviceRGB())
				{
					using (var context = new CGBitmapContext(rawData, 1, 1, 8, 4, colorSpace, CGImageAlphaInfo.PremultipliedLast))
					{
						context.DrawImage(new CGRect(-point.X, point.Y - image.Size.Height, image.Size.Width, image.Size.Height), image.CGImage);
						float red   = (rawData[0]) / 255.0f;
						float green = (rawData[1]) / 255.0f;
						float blue  = (rawData[2]) / 255.0f;
						float alpha = (rawData[3]) / 255.0f;
						resultColor = UIColor.FromRGBA(red, green, blue, alpha);
					}
				}
			}
			finally
			{
				handle.Free();
			}

			return resultColor;
		}

		private void findColorPalette(float numberOfPixels)
		{
			YUVColor dominantColor;
			YUVColor accentColor;

			YUVColor mutedColor;
			YUVColor vibrantColor;
			YUVColor lightMutedColor;
			YUVColor lightVibrantColor;
			YUVColor darkMutedColor;
			YUVColor darkVibrantColor;


			// Sort list

			int count = listOfColors.Count;

			if(count >= 2)
			{
				bool isInOrder = false;

				while (isInOrder == false)
				{
					isInOrder = true;

					for (int i = 1; i < count; i++)
					{
						if(listOfColors[i - 1].NumberOfOccurrences < listOfColors[i].NumberOfOccurrences)
						{
							var temp = listOfColors [i - 1];
							listOfColors [i - 1] = listOfColors [i];
							listOfColors [i] = temp;

							isInOrder = false;
						}
					}
				}
			}

			// Dominant color is the most frequent one
			dominantColor = listOfColors[0].Color;

			// Find accent color
			double biggestDifference = 0;
			int accentColorIndex = 1;

			if (count >= 2)
			{
				for (int i = 1; i < count; i++)
				{
					// Accent color is the most distant color from dominant color
					if(listOfColors[i].Color.DistanceTo(listOfColors[0].Color) > biggestDifference)
					{
						biggestDifference = listOfColors [i].Color.DistanceTo (listOfColors [0].Color);
						accentColorIndex = i;
					}
				}

				accentColor = listOfColors [accentColorIndex].Color;
			}
			else
			{
				// Only one color in the image. We are screwed :)
				accentColor = listOfColors[0].Color;
			}

			if(accentColor.DistanceTo(dominantColor) < MIN_CONTRAST_DISTANCE)
			{
				// Colors are to close and might blend with each other. We need to seperate them
				accentColor = accentColor.ColorAtDistanceFrom(dominantColor, MIN_CONTRAST_DISTANCE);
			}

			// Determine which color is darker
			if(dominantColor.Y < accentColor.Y)
			{
				// dominant color is darker
				mutedColor = dominantColor;
				vibrantColor = accentColor;
			}
			else
			{
				// accent color is darker
				mutedColor = accentColor;
				vibrantColor = dominantColor;
			}

			// Create lighter version of muted and vibrant colors
			lightMutedColor = mutedColor.LighttenByDistane (VARIANT_COLOR_DISTANCE);
			lightVibrantColor = vibrantColor.LighttenByDistane (VARIANT_COLOR_DISTANCE);

			// Create darker version of muted and vibrant colors
			darkMutedColor = mutedColor.DarkenByDistane (VARIANT_COLOR_DISTANCE);
			darkVibrantColor = vibrantColor.DarkenByDistane (VARIANT_COLOR_DISTANCE);

			// Assign
			MutedColor = mutedColor.ToRGBUIColor ();
			VibrantColor = vibrantColor.ToRGBUIColor ();

			LightMutedColor = lightMutedColor.ToRGBUIColor ();
			LightVibrantColor = lightVibrantColor.ToRGBUIColor ();

			DarkMutedColor = darkMutedColor.ToRGBUIColor ();
			DarkVibrantColor = darkVibrantColor.ToRGBUIColor ();
		}
	}
}
