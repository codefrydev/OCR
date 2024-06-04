using OCR.WindowMediaOcr.NetCore.Helper;
using System.Drawing;

namespace OCR.WindowMediaOcr.NetCore
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
