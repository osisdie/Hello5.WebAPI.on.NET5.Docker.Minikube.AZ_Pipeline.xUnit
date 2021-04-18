using System.IO;

namespace CoreFX.Common.Extensions
{
    public static class FileIO_Extension
    {
        public static string AddingBeforeExtension(this string fileName, string adding, bool checkFileExisting = false)
        {
            try
            {
                var newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.{adding}{Path.GetExtension(fileName)}";
                if (checkFileExisting && !File.Exists(newFileName))
                {
                    return fileName;
                }

                return newFileName;
            }
            catch { }

            return fileName;
        }
    }
}
