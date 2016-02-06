using CortanaExtension.Shared.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Xaml;
using static CortanaExtension.Shared.Utility.FileHelper;

namespace CortanaExtension.Shared.Utility.Cortana
{
    public interface IModelHolder
    {
        SharedModel Model { get; }
    }

    public static class Cortana
    {
        /// <summary>
        /// Note: this class and the vcd file are heavily coupled.
        /// </summary>
        private const string vcdFilename = "CortanaCommands.xml";

        private static IModelHolder ModelHolder;

        public static async Task Setup(IModelHolder modelHolder=null)
        {
            ModelHolder = modelHolder;
            await InstallVoiceCommands();
            await InstallPhrases();
        }

        /// <summary>
        /// Converts VoiceCommandActivatedEvent into specific CortanaCommand. 2 purposes:
        /// 1) Parse the "argument" of the command (ex. filename "My Cool Script", notepad note "feed the cat", etc.) into usable format.
        /// 2) Separate the details of "capturing" a voice command (here) from the command's business logic (in it's subclass of CortanaCommand) 
        /// </summary>
        public static CortanaCommand ProcessCommand(VoiceCommandActivatedEventArgs commandArgs)
        {
            SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;
            CommandDiagnostics diagnostics = new CommandDiagnostics(commandArgs);
            return ProcessCommand(speechRecognitionResult, diagnostics);
        }

        public static CortanaCommand ProcessCommand(SpeechRecognitionResult speechRecognitionResult, CommandDiagnostics diagnostics)
        {
            // Get the name of the voice command and the text spoken
            string voiceCommandName = speechRecognitionResult.RulePath[0];
            string textSpoken = speechRecognitionResult.Text;

            string argument = null;
            CortanaCommand processedCommand = null;

            bool modelUsed = ModelHolder != null;
            if (modelUsed) {
                UserCortanaCommand userCommand = ProcessUserCommand(voiceCommandName, speechRecognitionResult, diagnostics);
                bool wasUserCommand = userCommand != null;
                if (wasUserCommand) {
                    return userCommand;
                }
            }

            switch (voiceCommandName)
            {
                case CortanaCommand.Execute:
                    argument = GetPhraseArg(speechRecognitionResult, "filename"); // filename
                    processedCommand = new ExecuteCortanaCommand(argument, diagnostics);
                    break;

                case CortanaCommand.ToggleListening:
                    processedCommand = new ToggleListeningCortanaCommand(null, diagnostics); // no argument needed
                    break;

                default:
                    Debug.WriteLine("Command Name Not Found:  " + voiceCommandName);
                    break;
            }
            return processedCommand;
        }

        public static UserCortanaCommand ProcessUserCommand(string voiceCommandName, SpeechRecognitionResult speechRecognitionResult, CommandDiagnostics commandArgs)
        {
            SharedModel model = ModelHolder.Model;
            UserCortanaCommand command = null;
            if (model != null) {
                IList<UserCortanaCommand> commands = model.UserCortanaCommands;
                command = commands.Where( c => c.Name.Equals(voiceCommandName) ).First();
                if (command != null) {
                    command = command.Spawn(speechRecognitionResult);
                }
            }
            return command;
        }

        /// <summary>
        /// Run the command in text mode (as opposed to voice mode). 
        /// </summary>
        public static async Task RunAsTextCommand(CortanaCommand command)
        {
            // Store argument in clipboard
            string rawInput = command.ToInputString();
            ClipboardHelper.CopyToClipboard(rawInput);
            // Run the closest thing to Cortana command line I have
            const string filename = "Cortanahk.ahk";
            await FileHelper.Launch(filename, await StorageFolders.ResourceFiles());
        }

        /// <summary>
        /// Updates Cortana with new commands and phrases from a VCD file
        /// </summary>
        private static async Task InstallVoiceCommands()
        {
            StorageFile file = await StorageFolders.FlawlessCowboy.GetFileAsync(vcdFilename);
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);
        }

        private static async Task InstallPhrases()
        {
            await InstallFilenamePhrase();
        }

        private static async Task InstallFilenamePhrase()
        {
            // Get the names of the files of interest (right now, the autohotkey files in the resource folder)
            string[] filenames = await FileHelper.GetFiles(await StorageFolders.ResourceFiles(), ".ahk");

            // colloquialize (so you don't have to say the underscore(s) and extension aloud)
            for (int i = 0; i < filenames.Length; i++)
            {
                string filename = filenames[i];
                filenames[i] = FileHelper.ColloquializeFilename(filename);
            }

            const string filenamePhraseListLabel = "filename"; // must match vcd file
            AugmentPhraseList(filenamePhraseListLabel, filenames);
        }

        /// <summary>
        /// Updates the VCD file with new prhases at runtime
        /// </summary>
        private static async void AugmentPhraseList(string phraseLabel, string[] newList)
        {
            try {
                VoiceCommandDefinition commandDefinitions;
                const string nameTag = "CortanaCommandSet_en-us"; // must match vcd file
                bool commandDefFound = VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue(nameTag, out commandDefinitions);
                if (commandDefFound) {
                    await commandDefinitions.SetPhraseListAsync(phraseLabel, newList);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Updating Phrase list for VCDs: " + ex.ToString());
            }
        }

        /// <summary>
        /// Retrieves the best match from phraseName's list of options (declared in VCD file)
        /// </summary>
        private static string GetPhraseArg(SpeechRecognitionResult speechRecognitionResult, string phraseName)
        {
            string phraseArg = speechRecognitionResult.SemanticInterpretation.Properties[phraseName][0];
            return phraseArg;
        }

    }
}
