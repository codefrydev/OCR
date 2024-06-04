using OCR.WindowMediaOcr.Library.Helpers;
using System.Drawing;

namespace OCR.WindowMediaOcr.Library
{
    public class Extract
    {
        public static string ExtractText(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            return ImageMethods.ExtractText(bitmap, null).Result;
        }
    }
}
