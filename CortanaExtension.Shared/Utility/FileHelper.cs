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

        public static async Task<bool> Launch(string filename)
        {
            IStorageFile file = await FileIO.GetFile(filename);

            LauncherOptions options = new LauncherOptions();
            //options.DisplayApplicationPicker = true;
            //options.FallbackUri = GetUri(cole_protocol);

            bool result = await Launcher.LaunchFileAsync(file, options);
            return result;
        }

        public static async Task CreateFile(string filename)
        {
            // Create sample file; replace if exists.
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }

        public static async Task WriteTo(string filename, string content)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            await FileIO.SaveText(file, content);
        }

        public static async Task<string> ReadFrom(string filename)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            return await FileIO.LoadText(file);
        }

        public static async Task<IStorageFile> GetFile(string filename)
        {
            IStorageFile file = await FileIO.GetFile(filename);
            return file;
        }

        public static async Task<string[]> GetFiles(string folderName, string extension)
        {
            return await FileIO.GetFiles(folderName, extension);
        }

        public static async Task CloseActive()
        {
            await Launch("CloseActiveFile.ahk");
        }

        public static async Task MaximizeActive()
        {
            await Launch("Maximize.ahk");
        }

        public static async Task PlayFor(string filename, int msToPlayFor)
        {
            await Launch(filename);
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
            public static readonly StorageFolder LocalData = ApplicationData.Current.LocalFolder;

            public static async Task<StorageFolder> ResourceFiles()
            {
                return await FlawlessCowboy.GetFolderAsync("");
            }
        }

    }
}
