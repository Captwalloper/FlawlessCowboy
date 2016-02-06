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

        public static async Task<string[]> GetFiles(StorageFolder dir)
        {
            string[] filenames = null;
            try
            {
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

        public static async Task<string[]> GetFiles(StorageFolder dir, string extension)
        {
            string[] filenames = await GetFiles(dir);

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

    }
}
