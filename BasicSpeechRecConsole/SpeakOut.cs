using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace BasicSpeechRecConsole
{
    public class SpeakOut
    {
        public void FgBgColorGrammar()
        {
            using (SpeechRecognitionEngine recognizer =
             new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US")))
            {
                Grammar grammar = null;
                //Allow command to begin with set, alter, change.
                Choices introChoices = new Choices();
                foreach (string introString in new string[] { "Change", "Set", "Alter" })
                {
                    GrammarBuilder introGB = new GrammarBuilder(introString);
                    introChoices.Add(new SemanticResultValue(introGB,
                                        String.Format("Command: {0}", introString)));
                }
                GrammarBuilder cmdIntro = new GrammarBuilder(introChoices);
                //Now define the arguments to the command for Foreground or background and color as sementic Values
                Choices fgOrbgChoice = new Choices();
                GrammarBuilder backgroundGB = new GrammarBuilder("background");
                backgroundGB.Append(new SemanticResultValue(true));
                fgOrbgChoice.Add(backgroundGB);
                fgOrbgChoice.Add((GrammarBuilder)new SemanticResultValue("foreground", false));
                SemanticResultKey fgOrbgChoiceKey = new SemanticResultKey("BgOrFgBool", fgOrbgChoice);
                Choices colorChoice = new Choices();
                foreach (string colorName in System.Enum.GetNames(typeof(KnownColor)))
                {
                    string a = (Color.FromName(colorName)).A.ToString();
                    string r = (Color.FromName(colorName)).R.ToString();
                    string g = (Color.FromName(colorName)).G.ToString();
                    string b = (Color.FromName(colorName)).B.ToString();
                    colorChoice.Add((GrammarBuilder)
                                                    (new SemanticResultValue(colorName, "(r,g,b) " + r + " " + g + " " + b)));
                    //Uses implicit conversion of SemanticResultValue to GrammarBuilder    
                }

                //Create GrammarBuilder for CmdArgs to be appended to CmdInto using Semantic keys.
                GrammarBuilder cmdArgs = new GrammarBuilder();
                cmdArgs.Append(new SemanticResultKey("BgOrFgBool", fgOrbgChoice));
                cmdArgs.AppendWildcard();
                cmdArgs.Append(new SemanticResultKey("colorStringList", colorChoice));

                GrammarBuilder cmds = GrammarBuilder.Add(cmdIntro,
                                                         new GrammarBuilder(new SemanticResultKey("Cmd Args", cmdArgs)));
                grammar = new Grammar(cmds);
                grammar.Name = "Tree [Set,change,alter] [foreground,background] * color";

                
                // Add a handler for the SpeechRecognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(Speech_Recognized);

                // Load the grammar object to the recognizer.
                recognizer.LoadGrammarAsync(grammar);

                // Set the input to the recognizer.
                recognizer.SetInputToDefaultAudioDevice();

                // Start recognition.
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                Console.WriteLine("Starting asynchronous recognition...");
                               
                Console.ReadKey();

            }
        }

           void Speech_Recognized(object? sender, SpeechRecognizedEventArgs e)
        {

            Console.WriteLine(e.Result.Text);

            foreach (var value in e.Result.Semantics)
            {
                global::System.Console.WriteLine("Key = ");
                global::System.Console.WriteLine(value.Key);

                global::System.Console.WriteLine(value.Value.GetType());
                global::System.Console.WriteLine(value.Value["colorStringList"].Value);
                global::System.Console.WriteLine("is Bg? ");
                global::System.Console.WriteLine(value.Value["BgOrFgBool"].Value);


            }



            //if (e.Result.Semantics["BgOrFgBool"] != null)
            //    Console.WriteLine(e.Result.Semantics["BgOrFgBool"]);

            //if (e.Result.Semantics["colorStringList"] != null)
            //    Console.WriteLine(e.Result.Semantics["colorStringList"]);

            //if (e.Result.Semantics["Cmd Args"] != null)
            //    Console.WriteLine(e.Result.Semantics["Cmd Args"]);


            //if (e.Result.Semantics["red"] != null)
            //    Console.WriteLine(e.Result.Semantics["red"]);

            //if (e.Result.Semantics["blue"] != null)
            //    Console.WriteLine(e.Result.Semantics["blue"]);

        }

    }
}
