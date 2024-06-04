using System; 
using System.Drawing; 
using Tesseract; 

namespace OCR.Tesseract.NetFramework
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

            ImagSharpMethod(imagPath, modifiedImagePath, times);
            ExtrnalLib(modifiedImagePath);
            Console.WriteLine("With Modification of Image");
            Console.WriteLine(extractedTextImageSharp);
        }
        static void ExtrnalLib(string path)
        {
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.LstmOnly))
            {

                using (var img = Pix.LoadFromFile(path))
                {
                    using (var page = engine.Process(img))
                    { 
                        extractedTextImageSharp = page.GetText();
                    }

                }
            }
        }
        static void ImagSharpMethod(string path, string modifiedImagePath, int times)
        {
            using (Bitmap image = new Bitmap(path))
            {
                int newWidth = image.Width * times;
                int newHeight = image.Height * times;
                using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                    }

                    // Iterate through each pixel
                    for (int y = 0; y < resizedImage.Height; y++)
                    {
                        for (int x = 0; x < resizedImage.Width; x++)
                        {
                            // Get the pixel color
                            Color pixelColor = resizedImage.GetPixel(x, y);

                            // Check if the pixel color matches the target colors
                            if (IsTargetColor(pixelColor))
                            {
                                // Set to absolute white
                                resizedImage.SetPixel(x, y, Color.White);
                            }
                        }
                    }

                    // Save the modified image
                    resizedImage.Save(modifiedImagePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        static bool IsTargetColor(Color color)
        {
            // Blue and light blue range
            if (color.B > 128 && color.R < 128 && color.G < 128)
                return true;

            // Grey range
            if (color.R > 128 && color.R < 200 && color.G > 128 && color.G < 200 && color.B > 128 && color.B < 200)
                return true;

            // Yellow range
            if (color.R > 200 && color.G > 200 && color.B < 128)
                return true;

            // Add more color checks as needed

            return false;
        }
    }
}
