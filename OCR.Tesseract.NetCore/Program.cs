using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing; 
using Tesseract;

namespace OCR.Tesseract.NetCore
{
    internal class Program
    {
        static readonly string imagPath = "imagetwo.jpg";
        static string extractedTextImageSharp = string.Empty;
        static readonly string modifiedImagePath = "modified_image.jpg";

        static readonly int times = 1;
        static void Main()
        {
            ExtrnalLib(imagPath);
            Console.WriteLine("Without Any Modification of Image");
            Console.WriteLine(extractedTextImageSharp);

            ImagSharpMethod(imagPath);
            ExtrnalLib(modifiedImagePath);
            Console.WriteLine("With Modification of Image");
            Console.WriteLine(extractedTextImageSharp);
        }
        static void ExtrnalLib(string path)
        {  
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.LstmOnly))
            {

                using var img = Pix.LoadFromFile(path);
                using var page = engine.Process(img);
                extractedTextImageSharp = page.GetText();
            } 
        }
        static void ImagSharpMethod(string path)
        {
            using (Image<Rgba32> image = Image.Load<Rgba32>(path))
            {
                image.Mutate(x => x.Resize(image.Width * times, image.Height * times));
                // Iterate through each pixel
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        // Get the pixel color
                        Rgba32 pixelColor = image[x, y];

                        // Check if the pixel color matches the target colors
                        if (IsTargetColor(pixelColor))
                        {
                            // Set to absolute white
                            image[x, y] = Rgba32.ParseHex("#fff");
                        }
                    }
                }
                 
                // Save the modified image
                image.Save(modifiedImagePath);
            } 
        }
        static bool IsTargetColor(Rgba32 color)
        { 

            // Blue and light blue range
            if (color.B > 128 && color.R < 128 && color.G < 128)
                return true;

            // Grey range
            if (color.R > 128 && color.R < 200 && color.G > 128 && color.G < 200 && color.B > 128 && color.B < 200) return true;

            // Yellow range
            if (color.R > 200 && color.G > 200 && color.B < 128)
                return true;

            // Add more color checks as needed

            return false;
        }
    }
}
