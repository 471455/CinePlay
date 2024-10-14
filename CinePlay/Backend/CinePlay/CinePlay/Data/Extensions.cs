namespace MyMDb.Data
{
    public class Extensions
    {
        static readonly List<string> videoExtensions = new List<string> 
        {
            ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".mpeg", ".mpg",
            ".m4v", ".3gp", ".3g2", ".ogv", ".ogg", ".mxf", ".vob", ".m2ts", ".mts",
            ".ts", ".dv", ".m2v", ".f4v", ".f4p", ".f4a", ".f4b", ".rm", ".rmvb",
            ".asf", ".amv", ".svi", ".yuv", ".mpe", ".mpv", ".qt", ".divx", ".vid",
            ".h264", ".h265", ".xvid"
        };

        static readonly List<string> imageExtensions = new List<string>
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".svg", ".webp",
            ".ico", ".heic", ".heif", ".raw", ".nef", ".cr2", ".orf", ".sr2", ".arw"
        };

        public static bool IsVideoFile(string fileName)
        {
            return CheckFileExtension(fileName, videoExtensions);
        }

        public static bool IsImageFile(string fileName)
        {
            return CheckFileExtension(fileName, imageExtensions);
        }

        static bool CheckFileExtension(string fileName, List<string> validExtensions)
        {
            int dotPos = fileName.LastIndexOf('.');

            if (dotPos == -1)
            {
                return false;
            }

            string extension = fileName.Substring(dotPos).ToLower();

            return validExtensions.Contains(extension);
        }
    }
}
