using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CortanaExtension.Shared.Utility
{
    class FileIO
    {

        public static async Task<IStorageFile> GetFile(string filename)
        {
            Uri uri = GetUri(filename);
            IStorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return file;
        }

        public static async Task<StorageFolder> GetDirectory(string name)
        {
            StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder dir = await installedLocation.GetFolderAsync(name);
            return dir;
        }

        public static async Task<string[]> GetFiles(string folderName)
        {
            string[] filenames = null;
            try
            {
                StorageFolder dir = await GetDirectory(FileHelper.sharedModelPathRelative + @"\" + folderName);
                var files = await dir.GetFilesAsync();
                IList<string> names = new List<string>();
                foreach (StorageFile file in files)
                {
                    string filename = file.Name;
                    names.Add(filename);
                }

                filenames = names.ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return filenames;
        }

        public static async Task<string[]> GetFiles(string folderName, string extension)
        {
            string[] filenames = await GetFiles(FileHelper.resourceFolderName);

            // filter out files not of the specified extension
            IList<string> filenamesOfSpecifiedExtension = new List<string>();
            foreach (string filename in filenames)
            {
                if (filename.Contains(extension))
                {
                    filenamesOfSpecifiedExtension.Add(filename);
                }
            }
            return filenamesOfSpecifiedExtension.ToArray();
        }

        public static async Task SaveText(IStorageFile file, string content)
        {
            await Windows.Storage.FileIO.WriteTextAsync(file, content);
        }

        public static async Task<string> LoadText(IStorageFile file)
        {
            return await Windows.Storage.FileIO.ReadTextAsync(file);
        }

        private static Uri GetUri(string filename)
        {
            string filepath = FileHelper.resourcePathRelative + @"\" + filename;

            Uri uri = new Uri(@"ms-appx:///" + filepath);
            return uri;
        }

    }
}
