using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using System.Diagnostics;
using Windows.ApplicationModel;

namespace CortanaExtension.Shared.Utility
{
    public static class FileHelper
    {
        public const string resourceFolderName = "ResourceFiles";
        public const string sharedModelPathRelative = @"CortanaExtension.Shared\Model";
        public const string resourcePathRelative = sharedModelPathRelative + @"\" + resourceFolderName;

        public static async Task<bool> Launch(string filename, StorageFolder folder)
        {
            IStorageFile file = await folder.GetFileAsync(filename);

            LauncherOptions options = new LauncherOptions();
            //options.DisplayApplicationPicker = true;
            //options.FallbackUri = GetUri(cole_protocol);

            bool result = await Launcher.LaunchFileAsync(file, options);
            return result;
        }

        public static async Task CreateFile(string filename, StorageFolder folder)
        {
            // Create sample file; replace if exists.
            StorageFile sampleFile = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }

        public static async Task WriteTo(string filename, StorageFolder folder, string content)
        {
            StorageFile file = await folder.GetFileAsync(filename);
            await FileIO.SaveText(file, content);
        }

        public static async Task<string> ReadFrom(string filename, StorageFolder folder)
        {
            StorageFile file = await folder.GetFileAsync(filename);
            return await FileIO.LoadText(file);
        }

        public static async Task<IStorageFile> GetFile(string filename, StorageFolder folder)
        {
            IStorageFile file = await folder.GetFileAsync(filename);
            return file;
        }

        public static async Task<string[]> GetFiles(StorageFolder folder, string extension)
        {
            return await FileIO.GetFiles(folder, extension);
        }

        public static async Task CloseActive()
        {
            await Launch("CloseActiveFile.ahk", await StorageFolders.ResourceFiles());
        }

        public static async Task MaximizeActive()
        {
            await Launch("Maximize.ahk", await StorageFolders.ResourceFiles());
        }

        public static async Task PlayFor(string filename, int msToPlayFor)
        {
            await Launch(filename, await StorageFolders.ResourceFiles());
            await MaximizeActive();
            await Task.Delay(msToPlayFor);
            await CloseActive();
        }

        public static string ConvertToProperFilename(string spokenFilename)
        {
            const string extension = ".ahk"; // assume autohotkey extension

            string[] words = spokenFilename.Split(null); // split based on spaces
            string properFilename = "";
            foreach (string word in words)
            {
                string temp = char.ToUpper(word[0]) + word.Substring(1); // Capitalize 1st letter
                properFilename += temp + "_";
            }
            properFilename = properFilename.TrimEnd('_'); // eliminate extra underscore
            properFilename += extension;
            return properFilename;
        }

        /// <summary>
        /// Formats the filename like you would say it aloud (ex. "cortana_settings.ahk" to "cortana settings") 
        /// </summary>
        public static string ColloquializeFilename(string filename)
        {
            string colloquialFilename = "";
            string[] words = filename.Split(new char[] { '_', '.' }); // split based on underscores and the extension
            for (int i = 0; i < words.Length - 1; i++) // keep all but last word (remove extension)
            {
                colloquialFilename += words[i] + " ";
            }
            colloquialFilename = colloquialFilename.TrimEnd(' '); // eliminate extra space
            return colloquialFilename;
        }

        public class StorageFolders
        {
            public static readonly StorageFolder FlawlessCowboy = Package.Current.InstalledLocation;
            public static readonly StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;

            public static async Task<StorageFolder> ResourceFiles()
            {
                return await FlawlessCowboy.GetFolderAsync(FileHelper.resourcePathRelative);
            }
        }

    }
}
