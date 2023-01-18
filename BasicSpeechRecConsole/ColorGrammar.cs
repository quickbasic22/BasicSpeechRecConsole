using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace BasicSpeechRecConsole
{
    public class ColorGrammar
    {
        public Grammar CreateColorGrammar()
        {
            Choices colorChoice = new Choices();
            GrammarBuilder colorBuilder = new GrammarBuilder("red");
            SemanticResultValue colorValue = new SemanticResultValue(colorBuilder, "#FF0000");
            colorChoice.Add(new GrammarBuilder(colorValue));

            colorBuilder = new GrammarBuilder("green");
            colorValue = new SemanticResultValue(colorBuilder, "#00FF00");
            colorChoice.Add(new GrammarBuilder(colorValue));

            colorBuilder = new GrammarBuilder("blue");
            colorValue = new SemanticResultValue(colorBuilder, "#0000FF");
            colorChoice.Add(new GrammarBuilder(colorValue));

            GrammarBuilder colorElement = new GrammarBuilder(colorChoice);

            GrammarBuilder makePhrase = new GrammarBuilder("Make background");
            makePhrase.Append(colorElement);
            GrammarBuilder setPhrase = new GrammarBuilder("Set background to");
            setPhrase.Append(colorElement);

            Choices bothChoices = new Choices(new GrammarBuilder[] { makePhrase, setPhrase });
            GrammarBuilder bothPhrases = new GrammarBuilder(bothChoices);

            SemanticResultKey colorKey = new SemanticResultKey("ColorCode", bothPhrases);
            bothPhrases = new GrammarBuilder(colorKey);

            Grammar grammar = new Grammar(bothPhrases);
            grammar.Name = "backgroundColor";
            return grammar;

        }
    }
}
