﻿using CortanaExtension.Shared.Model;
using CortanaExtension.Shared.Utility;
using CortanaExtension.Shared.Utility.Cortana;
using FlawlessCowboy.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static CortanaExtension.Shared.Utility.FileHelper;

namespace FlawlessCowboy
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application, IModelHolder
    {
        private const string modelFileName = "Model.txt";

        public SharedModel Model { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(Microsoft.ApplicationInsights.WindowsCollectors.Metadata | Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public Page GetCurrentPage()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            Page currentPage = rootFrame.Content as Page;
            return currentPage;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            CustomizeDebugSettings();
            InitializeFrame(e);
            await Cortana.Setup(this);
            //await Save();
            try {
                await SetupLocalFolder(); //hack
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task StoreLocalFolderInResourceFiles()
        {
            StorageFolder dest = await StorageFolders.ResourceFiles();
            StorageFolder source = StorageFolders.LocalFolder;
            var filenames = await FileHelper.GetFiles(await StorageFolders.ResourceFiles());
            IStorageFile file;
            foreach (string filename in filenames)
            {
                file = await source.GetFileAsync(filename);
                await FileHelper.CopyToFolder(file, source, dest);
            }
        }

        public static async Task SetupLocalFolder()
        {
            StorageFolder source = await StorageFolders.ResourceFiles();
            StorageFolder dest = StorageFolders.LocalFolder;
            var filenames = await FileHelper.GetFiles(await StorageFolders.ResourceFiles());
            IStorageFile file;
            foreach (string filename in filenames)
            {
                file = await source.GetFileAsync(filename);
                await FileHelper.CopyToFolder(file, source, dest);
            }
        }

        protected async override void OnActivated(IActivatedEventArgs e)
        {
            IAppPage page = GetCurrentPage() as IAppPage;

            ActivationKind kind = e.Kind;
            switch (kind)
            {
                case Windows.ApplicationModel.Activation.ActivationKind.VoiceCommand:

                    //await Cortana.InstallFilenamePhrase();

                    RespondToForegroundVoiceCommand(e as VoiceCommandActivatedEventArgs, page);
                    break;
                //case ActivationKind.Protocol:
                //    RespondToBackgroundVoiceCommand(e as ProtocolActivatedEventArgs, page);
                //    break;
            }
        }

        private void RespondToForegroundVoiceCommand(VoiceCommandActivatedEventArgs e, IAppPage page)
        {
            CortanaCommand command = Cortana.ProcessCommand(e);
            page.RespondToVoice(command); // see MainPage for example
        }

        private void RespondToBackgroundVoiceCommand(ProtocolActivatedEventArgs e, IAppPage page)
        {
            var commandArgs = e as ProtocolActivatedEventArgs;
            Windows.Foundation.WwwFormUrlDecoder decoder = new Windows.Foundation.WwwFormUrlDecoder(commandArgs.Uri.Query);
            var passedArg = decoder.GetFirstValueByName("LaunchContext");

            // Ensure the current window is active.
            Window.Current.Activate();
        }

        private void InitializeFrame(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(FlawlessCowboy.View.MasterPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async Task Save()
        {
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            IList<Type> knownTypes = new List<Type>();
            foreach(CortanaCommand task in Model.AvailableTasks)
            {
                knownTypes.Add(task.GetType());
            }
            settings.KnownTypes = knownTypes;
            string json = Serializer.Serialize(Model, settings);
            StorageFolder folder = StorageFolders.LocalFolder;
            await FileHelper.CreateFile(modelFileName, folder);
            await FileHelper.WriteTo(modelFileName, folder, json);
        }

        public async Task<SharedModel> Load()
        {
            SharedModel model = new SharedModel();
            try {
                string json = await FileHelper.ReadFrom(modelFileName, await StorageFolders.ResourceFiles());
                model = Serializer.Deserialize<SharedModel>(json);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            
            return model;
        }

        public static Page GetPage()
        {
            var frame = (Frame)Window.Current.Content;
            var page = (Page)frame.Content;
            return page;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// </summary>
        /// of memory still intact.
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            //TODO: Save application state and stop any background activity
            await Save();
            //await StoreLocalFolderInResourceFiles();

            deferral.Complete();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary> Customize settings for Debug launch mode </summary>
        [Conditional("DEBUG")]
        private void CustomizeDebugSettings()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
        }
    }
}
