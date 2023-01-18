using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace BasicSpeechRecConsole
{
    public class IWantToFlyTo
    {
       public void FlyTo()
        {

            using (SpeechRecognitionEngine recognizer =
              new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US")))
            {


                // Create SemanticResultValue objects that contain cities and airport codes.
                SemanticResultValue chicago = new SemanticResultValue("Chicago", "ORD");
                SemanticResultValue boston = new SemanticResultValue("Boston", "BOS");
                SemanticResultValue miami = new SemanticResultValue("Miami", "MIA");
                SemanticResultValue dallas = new SemanticResultValue("Dallas", "DFW");

                // Create a Choices object and add the SemanticResultValue objects.
                Choices cities = new Choices();
                cities.Add(new Choices(new GrammarBuilder[] { chicago, boston, miami, dallas }));

                // Build the phrase and add SemanticResultKeys.
                GrammarBuilder chooseCities = new GrammarBuilder();
                chooseCities.Append("I want to fly from");
                chooseCities.Append(new SemanticResultKey("origin", cities));
                chooseCities.Append("to");
                chooseCities.Append(new SemanticResultKey("destination", cities));

                // Build a Grammar object from the GrammarBuilder.
                Grammar bookFlight = new Grammar(chooseCities);
                bookFlight.Name = "Book Flight";

                // Add a handler for the SpeechRecognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

                // Load the grammar object to the recognizer.
                recognizer.LoadGrammarAsync(bookFlight);

                // Set the input to the recognizer.
                recognizer.SetInputToDefaultAudioDevice();

                // Start recognition.
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                Console.WriteLine("Starting asynchronous recognition...");

                // Keep the console window open.
                Console.ReadLine();

                Console.ReadKey();
            }
        }

        // Handle the SpeechRecognized event.
        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognition result summary:");
            Console.WriteLine(
              "  Recognized phrase: {0}\n" +
              "  Confidence score {1}\n" +
              "  Grammar used: {2}\n",
              e.Result.Text, e.Result.Confidence, e.Result.Grammar.Name);

            // Display the semantic values in the recognition result.
            Console.WriteLine("  Semantic results:");
            foreach (KeyValuePair<String, SemanticValue> child in e.Result.Semantics)
            {
                Console.WriteLine("    The {0} city is {1}",
                  child.Key, child.Value.Value ?? "null");
            }
            Console.WriteLine();

            // Display information about the words in the recognition result.
            Console.WriteLine("  Word summary: ");
            foreach (RecognizedWordUnit word in e.Result.Words)
            {
                Console.WriteLine(
                  "    Lexical form ({1})" +
                  " Pronunciation ({0})" +
                  " Display form ({2})",
                  word.Pronunciation, word.LexicalForm, word.DisplayAttributes);
            }

            // Display information about the audio in the recognition result.
            Console.WriteLine("  Input audio summary:\n" +
                  "    Candidate Phrase at:       {0} mSec\n" +
                  "    Phrase Length:             {1} mSec\n" +
                  "    Input State Time:          {2}\n" +
                  "    Input Format:              {3}\n",
                  e.Result.Audio.AudioPosition,
                  e.Result.Audio.Duration,
                  e.Result.Audio.StartTime,
                  e.Result.Audio.Format.EncodingFormat);

            // Display information about the alternate recognitions in the recognition result.
            Console.WriteLine("  Alternate phrase collection:");
            foreach (RecognizedPhrase phrase in e.Result.Alternates)
            {
                Console.WriteLine("    Phrase: " + phrase.Text);
                Console.WriteLine("    Confidence score: " + phrase.Confidence);
            }

            // Display information about text that was replaced during normalization.
            if (e.Result.ReplacementWordUnits.Count != 0)
            {
                Console.WriteLine("  Replacement text:\n");
                foreach (ReplacementText rep in e.Result.ReplacementWordUnits)
                {
                    Console.WriteLine("      At index {0} for {1} words. Text: {2}\n",
                    rep.FirstWordIndex, rep.CountOfWords, rep.Text);
                }
                //label.Text += String.Format("\n\n");

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("No text was replaced");
            }
        }
    }
}
