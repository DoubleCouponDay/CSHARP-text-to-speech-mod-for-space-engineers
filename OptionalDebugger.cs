﻿using System;
using System.Text;
using System.IO; //filewriter
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using SETextToSpeechMod.LookUpTables;

namespace SETextToSpeechMod
{
    public class OptionalDebugger
    {         
        //const string currentComputer = "pavilion";
        const string currentComputer = "thinkpad";

        const string pavilionAddress = @"C:\Users\power\Desktop\scripting\SpaceEngineersTextToSpeechMod\AdjacentResults.txt";       
        const string thinkpadAddress = @"C:\Users\sjsui\Desktop\Workshop\text-to-speech-mod-for-space-engineers\AdjacentResults.txt";

        const string AEE = PrettyScaryDictionary.AEE;
        const string AHH = PrettyScaryDictionary.AHH; 
        const string AWW = PrettyScaryDictionary.AWW; 
        const string BIH = PrettyScaryDictionary.BIH; 
        const string CHI = PrettyScaryDictionary.CHI; 
        const string DIH = PrettyScaryDictionary.DIH; 
        const string EEE = PrettyScaryDictionary.EEE; 
        const string EHH = PrettyScaryDictionary.EHH; 
        const string ERR = PrettyScaryDictionary.ERR; 
        const string EYE = PrettyScaryDictionary.EYE; 
        const string FIH = PrettyScaryDictionary.FIH; 
        const string GIH = PrettyScaryDictionary.GIH; 
        const string HIH = PrettyScaryDictionary.HIH; 
        const string HOH = PrettyScaryDictionary.HOH; 
        const string IHH = PrettyScaryDictionary.IHH; 
        const string JIH = PrettyScaryDictionary.JIH; 
        const string KIH = PrettyScaryDictionary.KIH; 
        const string KSS = PrettyScaryDictionary.KSS; 
        const string LIH = PrettyScaryDictionary.LIH; 
        const string MIH = PrettyScaryDictionary.MIH; 
        const string NIH = PrettyScaryDictionary.NIH; 
        const string OOO = PrettyScaryDictionary.OOO; 
        const string OWE = PrettyScaryDictionary.OWE; 
        const string PIH = PrettyScaryDictionary.PIH; 
        const string RIH = PrettyScaryDictionary.RIH; 
        const string SHI = PrettyScaryDictionary.SHI; 
        const string SIH = PrettyScaryDictionary.SIH; 
        const string THI = PrettyScaryDictionary.THI; 
        const string TIH = PrettyScaryDictionary.TIH; 
        const string UHH = PrettyScaryDictionary.UHH; 
        const string VIH = PrettyScaryDictionary.VIH; 
        const string WIH = PrettyScaryDictionary.WIH; 
        const string YIH = PrettyScaryDictionary.YIH; 
        const string ZIH = PrettyScaryDictionary.ZIH;
        static readonly string[] ROW = PrettyScaryDictionary.ROW; //in order to put this in the table it must be static.

        readonly OrderedDictionary adjacentWords = new OrderedDictionary // ordered dictionary only allows you to access its values by int index; nothing more.
        {
            {"", new string[]{  }},
            {"_A_", ROW}, {"ABLE", new string[]{ AEE, BIH, LIH, }}, {"A", new string[]{ UHH, }}, {"AVAILABLE", new string[]{ UHH, VIH, AEE, LIH, UHH, BIH, LIH, }}, {"AUTOGRAPH", new string[]{ AWW, TIH, OWE, GIH, RIH, AHH, FIH, }}, {"ACTIVITIES", new string[]{ AHH, KIH, TIH, IHH, VIH, IHH, TIH, EEE, SIH, }}, {"AGGRESSION", new string[]{ UHH, GIH, RIH, EHH, SHI, UHH, NIH, }}, {"ABORIGINE", new string[]{ AHH, BIH, OWE, RIH, IHH, JIH, IHH, NIH, EEE, }}, {"ANNOINT", new string[]{ UHH, NIH, AWW, EEE, NIH, TIH, }}, {"ASTRONAUT", new string[]{ AHH, SIH, TIH, RIH, OWE, NIH, AWW, TIH, }}, {"ASSAULT", new string[]{ UHH, SIH, HOH, LIH, TIH, }}, {"ABOITEAU", new string[]{ AHH, BIH, AWW, EEE, TIH, OWE, }}, {"ABOITEAUX", new string[]{ AHH, BIH, AWW, EEE, TIH, OWE, }}, {"ABILITY", new string[]{ UHH, BIH, IHH, LIH, IHH, TIH, EEE, }}, {"AGAIN", new string[]{ UHH, GIH, AEE, NIH, }}, {"ACTIVATE", new string[]{ AHH, KIH, TIH, IHH, VIH, AEE, TIH, }}, {"ACOUSTIC", new string[]{ UHH, KIH, OOO, SIH, TIH, IHH, KIH, }}, {"ADVANTAGE", new string[]{ AHH, DIH, VIH, AHH, NIH, TIH, IHH, JIH, }},
            {"_B_", ROW}, {"BREAK", new string[]{ BIH, RIH, AEE, KIH, }}, {"BREW", new string[]{ BIH, RIH, OOO, }}, {"BE", new string[]{ BIH, EEE, }},  {"BIKE", new string[]{ BIH, EYE, KIH, }}, {"BESTOW", new string[]{ BIH, EHH, SIH, TIH, OWE, }}, {"BOTH", new string[]{ BIH, OWE, THI, }}, {"BOT", new string[]{ BIH, HOH, TIH, }}, {"BRUTE", new string[]{ BIH, RIH, OOO, TIH, }}, {"BUT", new string[]{ BIH, UHH, TIH, }}, {"BUY", new string[]{ BIH, EYE, }}, {"BICYCLE", new string[]{ BIH, EYE, SIH, EYE, KIH, LIH, }}, {"BABY", new string[]{ BIH, AEE, BIH, EEE, }}, {"BY", new string[]{ BIH, EYE, }}, {"BARGAIN", new string[]{ BIH, UHH, RIH, GIH, AEE, NIH, }}, {"BIKIES", new string[]{ BIH, EYE, KIH, EEE, SIH, }},
            {"_C_", ROW}, {"COMPARE", new string[]{ KIH, HOH, MIH, PIH, EHH, RIH, }}, {"COMPLICIT", new string[]{ KIH, HOH, MIH, PIH, LIH, IHH, SIH, IHH, TIH, }}, {"CAT", new string[]{ KIH, AHH, TIH, }}, {"CALLER", new string[]{ KIH, AWW, LIH, ERR, }}, {"COMPUTER", new string[]{ KIH, HOH, MIH, PIH, YIH, OOO, TIH, ERR, }}, {"CHAMPION", new string[]{ CHI, AHH, MIH, PIH, EEE, UHH, NIH, }}, {"COLLECTED", new string[]{ KIH, HOH, LIH, EHH, KIH, TIH, EHH, DIH, }}, {"CAUGHT", new string[]{ KIH, AWW, TIH, }}, {"CRUELTY", new string[]{ KIH, RIH, OOO, EHH, LIH, TIH, EEE, }}, {"CRUD", new string[]{ KIH, RIH, UHH, DIH, }}, {"CUT", new string[]{ KIH, UHH, TIH, }}, {"CHIVALRY", new string[]{ CHI, IHH, VIH, AHH, LIH, RIH, EEE, }},
            {"_D_", ROW}, {"DEAL", new string[]{ DIH, EEE, LIH, }}, {"DRESSES", new string[]{ DIH, RIH, EHH, SIH, EHH, SIH, }}, {"DOUGH", new string[]{ DIH, OWE, }}, {"DRUMMER", new string[]{ DIH, RIH, UHH, MIH, ERR, }}, {"DONE", new string[]{ DIH, UHH, NIH, }}, {"DESIGN", new string[]{ DIH, EHH, SIH, EYE, NIH, }},
            {"_E_", ROW}, {"EYE", new string[]{ EYE }}, {"ENGINEER", new string[]{ EHH, NIH, JIH, IHH, NIH, EEE, RIH, }}, {"EULOGY", new string[]{ YIH, OOO, LIH, HOH, JIH, EEE, }}, {"ELISABETH", new string[]{ EHH, LIH, IHH, SIH, UHH, BIH, EHH, THI, }},
            {"_F_", ROW}, {"FAITH", new string[]{ FIH, AEE, THI, }}, {"FAR", new string[]{ FIH, UHH, RIH, }}, {"FAIR", new string[]{ FIH, EHH, RIH, }}, {"FOLLOW", new string[]{ FIH, HOH, LIH, OWE, }}, {"FILED", new string[]{ FIH, EYE, LIH, DIH, }}, {"FELICITY", new string[]{ FIH, EHH, LIH, IHH, SIH, IHH, TIH, EEE, }}, {"FOUR", new string[]{ FIH, AWW, RIH, }}, {"FOUL", new string[]{ FIH, AHH, HOH, LIH, }}, {"FOOL", new string[]{ FIH, OOO, LIH, }}, {"FLY", new string[]{ FIH, LIH, EYE, }}, {"FLAKY", new string[]{ FIH, LIH, AEE, KIH, EEE, }}, {"FLIES", new string[]{ FIH, LIH, EYE, SIH, }},
            {"_G_", ROW}, {"GREAT", new string[]{ GIH, RIH, AEE, TIH, }}, {"GYM", new string[]{ JIH, IHH, MIH, }}, {"GIN", new string[]{ JIH, IHH, NIH, }}, {"GIVEN", new string[]{ GIH, IHH, VIH, EHH, NIH, }}, {"GUY", new string[]{ GIH, EYE, }}, {"GLADOS", new string[]{ GIH, LIH, AEE, DIH, HOH, SIH, }},
            {"_H_", ROW}, {"HAZE", new string[]{ HIH, AEE, ZIH, }}, {"HE", new string[]{ HIH, EEE, }}, {"HIGH", new string[]{ HIH, EYE, }}, {"HUB", new string[]{ HIH, UHH, BIH, }}, {"HYENA", new string[]{ HIH, EYE, EEE, NIH, UHH, }}, {"HOUSE", new string[]{ HIH, AHH, HOH, SIH, }}, {"HUSKIES", new string[]{ HIH, UHH, SIH, KIH, EEE, SIH, }}, {"HIVE", new string[]{ HIH, EYE, VIH, }},
            {"_I_", ROW}, {"I", new string[]{ EYE, }}, {"IMPROVE", new string[]{ IHH, MIH, PIH, RIH, OOO, VIH, }},  {"IMPROVISE", new string[]{ IHH, MIH, PIH, RIH, OWE, VIH, EYE, ZIH, }}, {"IMPROVISATION", new string[]{ IHH, MIH, PIH, RIH, OWE, VIH, EYE, ZIH, AEE, SHI, UHH, NIH, }}, {"INSTRUCTIONAL", new string[]{ IHH, NIH, SIH, TIH, RIH, UHH, KIH, SHI, UHH, NIH, AHH, LIH, }},
            {"_J_", ROW}, {"JUDGE", new string[]{ JIH, UHH, JIH, }}, {"JUDGEMENT", new string[]{ JIH, UHH, JIH, MIH, EHH, NIH, TIH, }}, {"JOHN", new string[]{ JIH, HOH, NIH, }}, {"JELLY", new string[]{ JIH, EHH, LIH, EEE, }},
            {"_K_", ROW}, {"KEY", new string[]{ KIH, EEE, }}, {"KNIGHT", new string[]{ NIH, EYE, TIH, }}, {"KITE", new string[]{ KIH, EYE, TIH, }},
            {"_L_", ROW}, {"LEAF", new string[]{ LIH, EEE, FIH, }}, {"LABEL", new string[]{ LIH, AEE, BIH, LIH, }}, {"LAST", new string[]{ LIH, AHH, SIH, TIH, }}, {"LADDER", new string[]{ LIH, AHH, DIH, ERR, }}, {"LOVELY", new string[]{ LIH, UHH, VIH, LIH, EEE, }}, {"LEAD", new string[]{ LIH, EEE, DIH, }}, {"LIGHT", new string[]{ LIH, EYE, TIH, }}, {"LORE", new string[]{ LIH, AWW, RIH, }}, {"LIKELY", new string[]{ LIH, EYE, KIH, LIH, EEE, }}, {"LEADER", new string[]{ LIH, EEE, DIH, ERR, }}, {"LUDICROUS", new string[]{ LIH, OOO, DIH, IHH, KIH, RIH, UHH, SIH, }}, {"LOVE", new string[]{ LIH, UHH, VIH, }},
            {"_M_", ROW}, {"MAPLE", new string[]{ MIH, AEE, PIH, LIH, }}, {"MAY", new string[]{ MIH, AEE, }}, {"ME", new string[]{ MIH, EEE, }}, {"MAYBE", new string[]{ MIH, AEE, BIH, EEE, }}, {"MOLTEN", new string[]{ MIH, HOH, LIH, TIH, EHH, NIH, }}, {"MANOEUVRE", new string[]{ MIH, AHH, NIH, OOO, VIH, ERR, }},
            {"_N_", ROW}, {"NICE", new string[]{ NIH, EYE, SIH, }}, {"NICKLE", new string[]{ NIH, IHH, KIH, LIH, }}, {"NARROW", new string[]{ NIH, AHH, RIH, OWE, }}, {"NEGATIVELY", new string[]{ NIH, EHH, GIH, UHH, TIH, IHH, VIH, LIH, EEE, }},
            {"_O_", ROW}, {"OSPREY", new string[]{ HOH, SIH, PIH, RIH, AEE, }}, {"OBJECTIVE", new string[]{ HOH, BIH, JIH, EHH, KIH, TIH, IHH, VIH, }}, {"OXYGEN", new string[]{ HOH, KSS, IHH, JIH, EHH, NIH, }}, {"OF", new string[]{ HOH, FIH, }}, {"OBSTRUCT", new string[]{ HOH, BIH, SIH, TIH, RIH, UHH, KIH, TIH, }}, {"OPULENCE", new string[]{ HOH, PIH, YIH, OOO, LIH, EHH, NIH, SIH, }}, {"OUT", new string[]{ AHH, HOH, TIH, }}, {"OUR", new string[]{ AHH, HOH, RIH, }}, {"OR", new string[]{ AWW, RIH, }}, {"OURSELVES", new string[]{ AHH, HOH, RIH, SIH, EHH, LIH, VIH, SIH, }},
            {"_P_", ROW}, {"PHRASE", new string[]{ FIH, RIH, AEE, SIH, }}, {"PLOTTABLE", new string[]{ PIH, LIH, HOH, TIH, UHH, BIH, LIH, }}, {"PLATED", new string[]{ PIH, LIH, AEE, TIH, EHH, DIH, }}, {"PLANET", new string[]{ PIH, LIH, AHH, NIH, EHH, TIH, }}, {"PHAROAH", new string[]{ FIH, EHH, RIH, OWE, }}, {"PIKE", new string[]{ PIH, EYE, KIH, }}, {"POINT", new string[]{ PIH, AWW, EEE, NIH, TIH, }}, {"PLANNER", new string[]{ PIH, LIH, AHH, NIH, ERR, }}, {"POUCH", new string[]{ PIH, AHH, HOH, CHI, }}, {"PRO", new string[]{ PIH, RIH, OWE, }}, {"PRISM", new string[]{ PIH, RIH, IHH, ZIH, MIH, }}, {"PURR", new string[]{ PIH, RIH, }}, {"PULL", new string[]{ PIH, OOO, LIH, }}, {"PIES", new string[]{ PIH, EYE, SIH, }}, {"PRUDE", new string[]{ PIH, RIH, OOO, DIH, }}, {"PROCLAMATION", new string[]{ PIH, RIH, HOH, KIH, LIH, UHH, MIH, AEE, SHI, UHH, NIH, }}, {"PATIO", new string[]{ PIH, AHH, TIH, EEE, OWE, }}, {"PENIS", new string[]{ PIH, EEE, NIH, IHH, SIH, }},
            {"_Q_", ROW}, {"QUEUE", new string[]{ KIH, YIH, OOO, }},
            {"_R_", ROW}, {"RAW", new string[]{ RIH, AWW, }}, {"ROW", new string[]{ RIH, OWE, }}, {"ROB", new string[]{ RIH, HOH, BIH, }}, {"RUBBER", new string[]{ RIH, UHH, BIH, ERR, }}, {"REMEMBER", new string[]{ RIH, EEE, MIH, EHH, MIH, BIH, ERR, }}, {"RUNNING", new string[]{ RIH, UHH, NIH, EEE, NIH, }}, {"RUDE", new string[]{ RIH, OOO, DIH, }}, {"RUIN", new string[]{ RIH, OOO, IHH, NIH, }}, {"REAL", new string[]{ RIH, EEE, LIH, }}, {"RESPITE", new string[]{ RIH, EHH, SIH, PIH, EYE, TIH, }}, {"REIN", new string[]{ RIH, AEE, NIH, }},
            {"_S_", ROW}, {"SAUL", new string[]{ SIH, AWW, LIH, }}, {"STEAK", new string[]{ SIH, TIH, AEE, KIH, }}, {"SPACE", new string[]{ SIH, PIH, AEE, SIH, }}, {"STACY", new string[]{ SIH, TIH, AEE, SIH, EEE, }}, {"SICILY", new string[]{ SIH, IHH, SIH, IHH, LIH, EEE, }}, {"SPEECH", new string[]{ SIH, PIH, EEE, CHI, }}, {"STEIN", new string[]{ SIH, TIH, EEE, NIH, }}, {"SIGN", new string[]{ SIH, EYE, NIH, }}, {"SPITE", new string[]{ SIH, PIH, EYE, TIH, }}, {"SOUR", new string[]{ SIH, AHH, HOH, RIH, }}, {"SLOUCH", new string[]{ SIH, LIH, AHH, HOH, CHI, }}, {"SOUL", new string[]{ SIH, OWE, LIH, }}, {"SOLE", new string[]{ SIH, HOH, LIH, }}, {"SOLO", new string[]{ SIH, OWE, LIH, OWE, }}, {"SLOTH", new string[]{ SIH, LIH, HOH, THI, }}, {"SUBMIT", new string[]{ SIH, UHH, BIH, MIH, IHH, TIH, }}, {"STYLE", new string[]{ SIH, TIH, EYE, LIH, }}, {"SOLILOQUY", new string[]{ SIH, HOH, LIH, IHH, LIH, OWE, KIH, WIH, EEE, }}, {"SAME", new string[]{ SIH, AEE, MIH, }}, {"SQUARE", new string[]{ SIH, KIH, WIH, EHH, RIH, }}, {"SLEUTH", new string[]{ SIH, LIH, OOO, THI, }}, {"STATION", new string[]{ SIH, TIH, AEE, SHI, UHH, NIH, }},
            {"_T_", ROW}, {"TABLE", new string[]{ TIH, AEE, BIH, LIH, }}, {"THE", new string[]{ THI, UHH, }}, {"TRIBE", new string[]{ TIH, RIH, EYE, BIH, }}, {"THESE", new string[]{ THI, EEE, SIH, }}, {"TREKKIES", new string[]{ TIH, RIH, EHH, KIH, EEE, SIH, }}, {"TRIGGER", new string[]{ TIH, RIH, IHH, GIH, ERR, }}, {"TALKING", new string[]{ TIH, AWW, KIH, EEE, NIH, }}, {"THIGH", new string[]{ THI, EYE, }}, {"TRACTION", new string[]{ TIH, RIH, AHH, KIH, SHI, UHH, NIH, }}, {"TOUCH", new string[]{ TIH, UHH, CHI, }}, {"TOLD", new string[]{ TIH, HOH, LIH, DIH, }}, {"THINK", new string[]{ THI, IHH, NIH, KIH, }}, {"TOTALLY", new string[]{ TIH, OWE, TIH, UHH, LIH, EEE, }}, {"THIS", new string[]{ THI, IHH, SIH, }}, {"TITANITE", new string[]{ TIH, EYE, TIH, AHH, NIH, EYE, TIH, }}, {"TALL", new string[]{ TIH, AWW, LIH, }}, {"TECHNOLOGY", new string[]{ TIH, EHH, KIH, NIH, HOH, LIH, HOH, JIH, EEE, }}, {"TECHNO", new string[]{ TIH, EHH, KIH, NIH, OWE }},
            {"_U_", ROW}, {"UPDATE", new string[]{ UHH, PIH, DIH, AEE, TIH, }},
            {"_V_", ROW}, {"VETO", new string[]{ VIH, EEE, TIH, OWE, }}, {"VISA", new string[]{ VIH, EEE, ZIH, UHH, }},
            {"_W_", ROW}, {"WATER", new string[]{ WIH, AWW, TIH, ERR, }}, {"WHAT", new string[]{ WIH, HOH, TIH, }}, {"WE", new string[]{ WIH, EEE, }},
            {"_X_", ROW}, {"XYLOPHONE", new string[]{ ZIH, EYE, LIH, OWE, FIH, OWE, NIH, }},
            {"_Y_", ROW}, {"YOU", new string[]{ YIH, OOO, }}, {"YAM", new string[]{ YIH, AHH, MIH, }},
            {"_Z_", ROW},
        };            

        int lowerCaseWords = 0;
        string resultsFile;

        bool duplicateExists = default (bool);

        OrderedDictionary emptiesRemoved = new OrderedDictionary(); //needed for checking for duplicate entries
        OrderedDictionary tabledResults = new OrderedDictionary();
        ICollection adjacentKeys;
        ICollection resultKeys;

        public OptionalDebugger()
        {
            emptiesRemoved = adjacentWords;
            adjacentKeys = emptiesRemoved.Keys;
            resultKeys = tabledResults.Keys;

            switch (currentComputer)
            {
                case "pavilion":
                    resultsFile = pavilionAddress;
                    break;

                case "thinkpad":
                    resultsFile = thinkpadAddress;
                    break;
            }
        }

        static void Main()
        {
            OptionalDebugger debugger = new OptionalDebugger();
            ChatEntryPoint entryPoint = new ChatEntryPoint (true);
            Encoding encode = Encoding.Unicode;
            AttendanceManager.Debugging = true;                        
            entryPoint.Initialise();
            string testString = debugger.RollOutAdjacentWords(); //OptionalDebugger is designed to only take the table adjacentWords as input. replacing this string wont work.            
            string upperCase = testString.ToUpper();   
            string signatureBuild = entryPoint.OutputManager.LocalPlayersVoice.ToString();
            int leftoverSpace = PossibleOutputs.AutoSignatureSize - entryPoint.OutputManager.LocalPlayersVoice.ToString().Length;

            for (int i = 0; i < leftoverSpace; i++)
            {
                signatureBuild += " ";
            }
            string packaged = signatureBuild + upperCase;                
            byte[] packet = encode.GetBytes (packaged);
            entryPoint.OnReceivedPacket (packet);
            
            while (entryPoint.OutputManager.IsProcessingOutputs)
            {
                entryPoint.UpdateBeforeSimulation();              
                debugger.StoreResults (entryPoint.OutputManager.Speeches[5].MainProcess.Pronunciation.WordIsolator.CurrentWord, //use marek voice since we dont want intonations in the algorithm test. 
                                       entryPoint.OutputManager.Speeches[5].MainProcess.CurrentResults, 
                                       entryPoint.OutputManager.Speeches[5].MainProcess.Pronunciation.UsedDictionary);         
            }                               
            debugger.PrintResults (entryPoint.OutputManager.Speeches[5].MainProcess.Pronunciation.WrongFormatMatches, entryPoint.OutputManager.Speeches[0].MainProcess.Pronunciation.WrongFormatNonMatches);
        }

        /// <summary>
        /// Removes whitespace and null from the table and returns every item as a single string in alphabetical order.
        /// </summary>
        /// <param name="tableToRollOut"></param>
        /// <returns></returns>
        public string RollOutAdjacentWords()
        {
            string rolledOut = string.Empty;
            
foreach (KeyValuePair <string, string[]> entry in adjacentWords)
{
                

                while ()
                {

                }
                if (entry.Value.IsNullOrEmpty() == false &&
                    )
                {
                    adjacentWords.Remove (entry.Key);
                }
}

            for (int i = 0; i < emptiesRemoved.Count; i++) 
            {                    
                string[] currentAdjacentValue = emptiesRemoved[i] as string[];

                if (currentAdjacentValue.IsNullOrEmpty())
                {
                    emptiesRemoved.RemoveAt (i);
                    i--;
                }
            }

            //these two loops need to be separate due to the nature ordered dictionary enumerators.
            IEnumerator addingIndex = adjacentKeys.GetEnumerator(); 

            for (int i = 0; i < emptiesRemoved.Count; i++) 
            {                    
                addingIndex.MoveNext();             
                string key = addingIndex.Current.ToString();          
                rolledOut += key + " ";
            }
            return rolledOut;
        }

        public void StoreResults (string currentWord, IList <string> phonemes, bool UsedDictionary)
        {
            if (currentWord != " ")
            {
                List <string> newReference = new List <string> (phonemes);                
                currentWord += " " + UsedDictionary.ToString();
                              
                for (int i = 0; i < newReference.Count; i++)
                {
                    if (newReference[i] == " " ||
                        newReference[i] == "")
                    {
                        newReference.RemoveAt (i);
                        i--;
                    }
                }               
                string[] formattedPhonemes = new string[newReference.Count];

                for (int i = 0; i < newReference.Count; i++)
                {
                    formattedPhonemes[i] = newReference[i];
                }

                if (tabledResults.Contains (currentWord))
                {
                    string[] previousEntry = tabledResults[currentWord] as string[];
                    string[] largerAccommodation = new string[previousEntry.Length + formattedPhonemes.Length];
                        
                    for (int i = 0; i < largerAccommodation.Length; i++)
                    {
                        if (i < previousEntry.Length)
                        {
                            largerAccommodation[i] = previousEntry[i];
                        }

                        else
                        {
                            largerAccommodation[i] = formattedPhonemes[i - previousEntry.Length];
                        }
                    }                    
                    tabledResults[currentWord] = largerAccommodation;
                }

                else
                {
                    tabledResults.Add (currentWord, formattedPhonemes);
                }
            }
        }

        public void PrintResults (int wrongFormatMatchers, int wrongFormatNonMatchers)
        {
            string[] previousReadings = File.ReadAllLines(resultsFile);   
            previousReadings = previousReadings[1].Split(' '); 
   
            string[] tallies = {"Total Words: ",
                                "Total Incorrect: ",
                                previousReadings[2],
                                "From Dictionary: ",
                                "Lowercase Keys: ",
                                "Wrong Format Matchers: ",
                                "Wrong Format NonMatchers: ",
                                "",
                                };                           
            string[] lines = new string[2 * emptiesRemoved.Count + tallies.Length];
            int errorCount = 0;
            int UsageCount = 0;

            IEnumerator adjacentIndex = adjacentKeys.GetEnumerator(); //assumes both collections will always be the same size.
            IEnumerator resultsIndex = resultKeys.GetEnumerator();
            Process[] processes;

            for (int i = 0 + tallies.Length; i < lines.Length; i++) //note that i skip a couple lines to save the tally.
            {
                adjacentIndex.MoveNext();
                resultsIndex.MoveNext();

                string currentAdjacentKey = adjacentIndex.Current.ToString();
                string currentResultsKey = resultsIndex.Current.ToString();
                string[] splitReading = currentResultsKey.Split(' ');
                bool UsedDictionary = Convert.ToBoolean (splitReading[1]);

                string[] currentAdjacentValue = emptiesRemoved[currentAdjacentKey] as string[];
                string[] currentResultsValue = tabledResults[currentResultsKey] as string[];
                bool isMatch = true;

                if (char.IsLower (currentAdjacentKey[0]))
                {
                    lowerCaseWords++;
                    currentAdjacentKey += "____Lowercase Key____";
                }

                if (UsedDictionary)
                {
                        UsageCount++;
                        currentAdjacentKey += "____Used Dictionary____";
                }                       
                lines[i] = currentAdjacentKey + "      { ";                

                for (int f = 0; f < currentAdjacentValue.Length; f++)
                {
                    lines[i] += currentAdjacentValue[f] + ", ";

                    if (currentAdjacentValue.Length == currentResultsValue.Length)
                    {
                        if (currentAdjacentValue[f] == currentResultsValue[f] &&
                            isMatch != false)
                        {
                            isMatch = true;
                        }

                        else
                        {
                            isMatch = false;
                        }
                    }    
                        
                    else
                    {
                        isMatch = false;
                    }                  
                }
                lines[i] += "} { ";

                for (int k = 0; k < currentResultsValue.Length; k++)
                {
                    lines[i] += currentResultsValue[k] + ", ";
                }
                lines[i] += "} ";

                switch (isMatch)
                {
                    case true:
                    lines[i] = "Correct ------ " + lines[i];
                        break;

                    case false:
                        errorCount++;
                        lines[i] = "Not Correct -- " + lines[i];
                        break;
                }
                i++;
            }
            lines[0] = tallies[0] + emptiesRemoved.Count;
            lines[1] = tallies[1] + errorCount;
            lines[2] = "Previous Incorrect: " + tallies[2];
            lines[3] = tallies[3] + UsageCount;
            lines[4] = tallies[4] + lowerCaseWords;
            lines[5] = tallies[5] + wrongFormatMatchers;
            lines[6] = tallies[6] + wrongFormatNonMatchers;

            File.WriteAllLines (resultsFile, lines);

            processes = Process.GetProcessesByName ("notepad");

            for (int i = 0; i < processes.Length; i++)
            {
                processes[i].Kill();
            }             
            Process.Start (resultsFile);
        }
    }
}
