using BasicSpeechRecConsole;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Speech.Recognition;
using System.Speech.Synthesis;


internal class Program
{
    SpeechSynthesizer Zira;
    public static void Main(string[] args)
    {
        
        
        SpeakOut speakOut = new SpeakOut();
        speakOut.FgBgColorGrammar();
        


        //ColorGrammar colorGrammar = new ColorGrammar();
        //Grammar cGrammar = colorGrammar.CreateColorGrammar();

        //IWantToFlyTo toFlyTo = new IWantToFlyTo();
        //toFlyTo.FlyTo();

        // I want to fly from chicago to Miami
        //"Chicago", "ORD";
        //"Boston", "BOS";
        //"Miami", "MIA";
        //"Dallas", "DFW";


        
    }

    

}