using System.Collections.Generic;

namespace OCR.GoogleLens.NetFramework
{
    public class Constants
    {
        public const string LENS_ENDPOINT = "https://lens.google.com/v3/upload";

        public static readonly List<string> SUPPORTED_MIMES = new List<string>
        {
            "image/x-icon",
            "image/bmp",
            "image/jpeg",
            "image/png",
            "image/tiff",
            "image/webp",
            "image/heic"
        };

        public static readonly Dictionary<string, string> MIME_TO_EXT = new Dictionary<string, string>
        {
            { "image/x-icon", "ico" },
            { "image/bmp", "bmp" },
            { "image/jpeg", "jpg" },
            { "image/png", "png" },
            { "image/tiff", "tiff" },
            { "image/webp", "webp" },
            { "image/heic", "heic" }
        };

    }
}
