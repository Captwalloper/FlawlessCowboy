
using System;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Media.SpeechRecognition;

namespace CortanaExtension.Shared.Utility.Cortana
{
    public class CommandDiagnostics
    {
        public string RawText { get; private set; }
        public string Mode { get; private set; }

        public CommandDiagnostics(VoiceCommandActivatedEventArgs commandArgs)
        {
            OrganizeFeedback(commandArgs);
        }

        private void OrganizeFeedback(VoiceCommandActivatedEventArgs commandArgs)
        {
            if (commandArgs != null)
            {
                SpeechRecognitionResult speech = commandArgs.Result;
                RawText = speech.Text;
                Mode = SemanticInterpretation("commandMode", speech);

                SpeechRecognitionConfidence confidence = speech.Confidence;
                double rawConfidence = speech.RawConfidence;
                TimeSpan duration = speech.PhraseDuration;
                DateTimeOffset start = speech.PhraseStartTime;
                SpeechRecognitionResultStatus status = speech.Status;
            }
            else
            {
                RawText = Mode = null;
            }
        }

        /// <summary> Returns the semantic interpretation of a speech result. Returns null if there is no interpretation for that key. </summary>
        /// <param name="interpretationKey">The interpretation key.</param>
        /// <param name="speechRecognitionResult">The result to get an interpretation from.</param>
        private static string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }
    }

}
