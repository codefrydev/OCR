using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using BitmapDecoder = Windows.Graphics.Imaging.BitmapDecoder;

namespace OCR.WindowMediaOcr.Library.Helpers;

internal sealed class ImageMethods
{


    internal static readonly char[] Separator = ['\n', '\r'];

    public static async Task<string>
        ExtractText(Bitmap bmp, System.Windows.Point? singlePoint = null)
    {
        //Language? selectedLanguage = GetOCRLanguage();
        Language? selectedLanguage = OcrEngine.AvailableRecognizerLanguages.First();
        if (selectedLanguage == null)
        {
            return string.Empty;
        }

        XmlLanguage lang = XmlLanguage.GetLanguage(selectedLanguage.LanguageTag);
        CultureInfo culture = lang.GetEquivalentCulture();

        bool isSpaceJoiningLang = IsLanguageSpaceJoining(selectedLanguage);

        bool scaleBMP = true;

        if (singlePoint != null
            || bmp.Width * 1.5 > OcrEngine.MaxImageDimension)
        {
            scaleBMP = false;
        }

        using Bitmap scaledBitmap = scaleBMP ? ScaleBitmapUniform(bmp, 1.5) : ScaleBitmapUniform(bmp, 1.0);
        StringBuilder text = new();

        await using MemoryStream memoryStream = new();
        using WrappingStream wrappingStream = new(memoryStream);

        scaledBitmap.Save(wrappingStream, ImageFormat.Bmp);
        wrappingStream.Position = 0;
        BitmapDecoder bmpDecoder = await BitmapDecoder.CreateAsync(wrappingStream.AsRandomAccessStream());
        SoftwareBitmap softwareBmp = await bmpDecoder.GetSoftwareBitmapAsync();

        OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(selectedLanguage);
        OcrResult ocrResult = await ocrEngine.RecognizeAsync(softwareBmp);

        await memoryStream.DisposeAsync();
        await wrappingStream.DisposeAsync();
        GC.Collect();

        if (singlePoint == null)
        {
            foreach (OcrLine ocrLine in ocrResult.Lines)
            {
                ocrLine.GetTextFromOcrLine(isSpaceJoiningLang, text);
            }
        }
        else
        {
            Windows.Foundation.Point fPoint = new(singlePoint.Value.X, singlePoint.Value.Y);
            foreach (OcrLine ocrLine in ocrResult.Lines)
            {
                foreach (OcrWord ocrWord in ocrLine.Words)
                {
                    if (ocrWord.BoundingRect.Contains(fPoint))
                    {
                        _ = text.Append(ocrWord.Text);
                    }
                }
            }
        }

        if (culture.TextInfo.IsRightToLeft)
        {
            string[] textListLines = text.ToString().Split(Separator);

            _ = text.Clear();
            foreach (string textLine in textListLines)
            {
                List<string> wordArray = textLine.Split().ToList();
                wordArray.Reverse();
                _ = text.Append(string.Join(' ', wordArray));

                if (textLine.Length > 0)
                {
                    _ = text.Append('\n');
                }
            }

            return text.ToString();
        }

        return text.ToString();
    }

    public static bool IsLanguageSpaceJoining(Language selectedLanguage)
    {
        if (selectedLanguage.LanguageTag.StartsWith("zh", StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }
        else if (selectedLanguage.LanguageTag.Equals("ja", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
    public static Bitmap ScaleBitmapUniform(Bitmap passedBitmap, double scale)
    {
        using MemoryStream memoryStream = new();
        using WrappingStream wrappingStream = new(memoryStream);
        passedBitmap.Save(wrappingStream, ImageFormat.Bmp);
        wrappingStream.Position = 0;
        BitmapImage bitmapImage = new();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = wrappingStream;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        TransformedBitmap transformedBmp = new();
        transformedBmp.BeginInit();
        transformedBmp.Source = bitmapImage;
        transformedBmp.Transform = new ScaleTransform(scale, scale);
        transformedBmp.EndInit();
        transformedBmp.Freeze();

        memoryStream.Dispose();
        wrappingStream.Dispose();
        GC.Collect();
        return BitmapSourceToBitmap(transformedBmp);
    }

    public static Bitmap BitmapSourceToBitmap(BitmapSource source)
    {
        Bitmap bmp = new(
          source.PixelWidth,
          source.PixelHeight,
          System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
        BitmapData data = bmp.LockBits(
          new Rectangle(System.Drawing.Point.Empty, bmp.Size),
          ImageLockMode.WriteOnly,
          System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
        source.CopyPixels(
          Int32Rect.Empty,
          data.Scan0,
          data.Height * data.Stride,
          data.Stride);
        bmp.UnlockBits(data);
        GC.Collect();
        return bmp;
    }

    public static Language? GetOCRLanguage()
    {
        // use currently selected Language
        string inputLang = InputLanguageManager.Current.CurrentInputLanguage.Name;

        Language? selectedLanguage = new(inputLang);
        List<Language> possibleOcrLanguages = [.. OcrEngine.AvailableRecognizerLanguages];

        if (possibleOcrLanguages.Count < 1)
        {
            System.Windows.MessageBox.Show("No possible OCR languages are installed.", "Text Grab");
            return null;
        }
        return selectedLanguage;
    }
}
