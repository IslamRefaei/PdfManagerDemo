using System.IO;
using Microsoft.Win32;

namespace PdfManager.Core
{
    public class Utilities
    {
        public static string SelectFileFromDialog(string filter)
        {
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }

        public static Stream GetResourceStream(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            return stream;
        }

        public static byte[] GetResourceByteArray(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            return fileBytes;
        }

    }
}
