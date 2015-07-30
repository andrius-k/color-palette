# color-palette
Android's Palette API implementation for Xamarin.iOS.
This libraty allows you to extract an array of colors from single image.

# Available colors

<ul>
    <li>Muted Color</li>
    <li>Vibrant Color</li>
    <li>Light Muted Color</li>
    <li>Light Vibrant Color</li>
    <li>Dark Muted Color</li>
    <li>Dark Vibrant Color</li>
</ul>

# Samples

Background color is MutedColor, Title is VibrantColor and smaller text is LightVibrantColor.

![Alt text](https://cloud.githubusercontent.com/assets/1687466/8980111/f64028f4-36b6-11e5-93db-b661f13788f3.png)
![Alt text](https://cloud.githubusercontent.com/assets/1687466/8980111/f64028f4-36b6-11e5-93db-b661f13788f3.png)

# Usage
```c#
var palette = new ColorPaletteGenerator();
var image = UIImage.FromBundle ("Images/sampleImage.jpeg");
palette.Generate (image);

// Use generated colors
Something.Color = palette.MutedColor;
```
