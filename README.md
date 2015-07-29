# color-palette
Android's Palette API implementation for Xamarin.iOS
This libraty allows you to extract an array of colors from single image.

# Available colors

Muted Color, Vibrant Color, Light Muted Color, Light Vibrant Color, Dark Muted Color, Dark Vibrant Color

<ul>
    <li>hi</li>
</ul>

# Usage

    var palette = new ColorPaletteGenerator();
    var image = UIImage.FromBundle ("Images/sampleImage.jpeg");
    palette.Generate (image);

    // Use generated colors
    Something.Color = palette.MutedColor;
