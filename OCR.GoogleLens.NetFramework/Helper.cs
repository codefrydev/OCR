using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using Image = System.Drawing.Image;

namespace OCR.GoogleLens.NetFramework
{

    internal class Helper
    {
        public static (int Width, int Height) ImageDimensionsFromData(byte[] data)
        {
            using (var inputStream = new MemoryStream(data))
            {
                using (var image = Image.FromStream(inputStream))
                {
                    int width = image.Width;
                    int height = image.Height;
                    return (width, height);
                }
            }
        }
        public static byte[] DecompressDeflate(byte[] deflateData)
        {
            using (MemoryStream inputStream = new MemoryStream(deflateData))
            using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            using (MemoryStream outputStream = new MemoryStream())
            {
                deflateStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }
        public static byte[] DecompressGzip(byte[] gzipData)
        {
            using (MemoryStream inputStream = new MemoryStream(gzipData))
            using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (MemoryStream outputStream = new MemoryStream())
            {
                gzipStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }
        public static byte[] ResizeImage(byte[] imageBytes, int maxWidth, int maxHeight)
        {
            using (var inputStream = new MemoryStream(imageBytes))
            {
                using (var originalImage = Image.FromStream(inputStream))
                {
                    // Calculate the new width and height
                    int originalWidth = originalImage.Width;
                    int originalHeight = originalImage.Height;
                    float ratioX = (float)maxWidth / originalWidth;
                    float ratioY = (float)maxHeight / originalHeight;
                    float ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(originalWidth * ratio);
                    int newHeight = (int)(originalHeight * ratio);

                    // Create a new bitmap with the new dimensions
                    var resizedImage = new Bitmap(newWidth, newHeight);

                    // Draw the original image on the new bitmap, resizing it
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    // Save the resized image to a memory stream
                    using (var outputStream = new MemoryStream())
                    {
                        resizedImage.Save(outputStream, ImageFormat.Jpeg);
                        return outputStream.ToArray();
                    }
                }
            }
        }
        public static string GetImageMimeType(byte[] imageBytes)
        {
            using (var stream = new MemoryStream(imageBytes))
            {
                using (var image = Image.FromStream(stream))
                {
                    if (ImageFormat.Jpeg.Equals(image.RawFormat))
                    {
                        return "image/jpeg";
                    }
                    if (ImageFormat.Png.Equals(image.RawFormat))
                    {
                        return "image/png";
                    }
                    if (ImageFormat.Gif.Equals(image.RawFormat))
                    {
                        return "image/gif";
                    }
                    if (ImageFormat.Bmp.Equals(image.RawFormat))
                    {
                        return "image/bmp";
                    }
                    if (ImageFormat.Tiff.Equals(image.RawFormat))
                    {
                        return "image/tiff";
                    }
                    // Add more formats if needed

                    return "unknown";
                }
            }
        }
    }
}
