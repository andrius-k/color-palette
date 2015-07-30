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

# Usage
    c#
    var palette = new ColorPaletteGenerator();
    var image = UIImage.FromBundle ("Images/sampleImage.jpeg");
    palette.Generate (image);

    // Use generated colors
    Something.Color = palette.MutedColor;
