﻿using System;
using System.Text;
using System.IO; //filewriter
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace SETextToSpeechMod
{
    class OptionalDebugger
    {
        static void Main()
        {
            MessageEventHandler handler = new MessageEventHandler();
            Encoding test_encode = Encoding.Unicode;
            AdjacentTester methodDebugger = new AdjacentTester();

            string testString = methodDebugger.RollOutAdjacentWords();
            string upper = testString.ToUpper();
            byte[] testBytes = test_encode.GetBytes (upper);
            handler.debugging = true;
            handler.OnReceivedPacket (testBytes);
            handler.speeches[0].debugging = true;

            while (handler.speeches[0].finished == false)
            {
                handler.speeches[0].Load();

                if (handler.speeches[0].loading == true)
                {
                    methodDebugger.StoreResults (handler.speeches[0].pronunciation.wordCounter.currentWord, handler.speeches[0].results);
                }
            }
            methodDebugger.PrintResults();
        }

        class AdjacentTester
        {            
            const string AEE = PrettyScaryDictionary.AEE;
            const string AHH = PrettyScaryDictionary.AHH; 
            const string AWW = PrettyScaryDictionary.AWW; 
            const string BIH = PrettyScaryDictionary.BIH; 
            const string DIH = PrettyScaryDictionary.DIH; 
            const string EEE = PrettyScaryDictionary.EEE; 
            const string EHH = PrettyScaryDictionary.EHH; 
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
            const string SIH = PrettyScaryDictionary.SIH; 
            const string THI = PrettyScaryDictionary.THI; 
            const string TIH = PrettyScaryDictionary.TIH; 
            const string UHH = PrettyScaryDictionary.UHH; 
            const string VIH = PrettyScaryDictionary.VIH; 
            const string WIH = PrettyScaryDictionary.WIH; 
            const string YIH = PrettyScaryDictionary.YIH; 
            const string ZIH = PrettyScaryDictionary.ZIH;
            static readonly string[] row = PrettyScaryDictionary.row;

            readonly OrderedDictionary adjacentWords = new OrderedDictionary
            {
                {"", new string[]{  }},
                {"_A_", row}, {"ABLE", new string[]{ AEE, BIH, LIH, }}, {"A", new string[]{ UHH, }}, {"AVAILABLE", new string[]{ UHH, VIH, AEE, LIH, UHH, BIH, LIH, }}, {"AUTOGRAPH", new string[]{ AWW, TIH, OWE, GIH, RIH, AHH, FIH, }}, {"ACTIVITIES", new string[]{ AHH, KIH, TIH, IHH, VIH, IHH, TIH, EEE, SIH, }}, {"AGGRESSION", new string[]{ UHH, GIH, RIH, EHH, SIH, HIH, UHH, NIH, }}, {"ABORIGINE", new string[]{ AHH, BIH, AWW, RIH, IHH, JIH, NIH, EEE, }}, {"ANNOINT", new string[]{ UHH, NIH, AWW, EEE, NIH, TIH, }}, {"ASTRONAUT", new string[]{ AHH, SIH, TIH, RIH, OWE, NIH, AWW, TIH, }}, {"ASSAULT", new string[]{ UHH, SIH, HOH, LIH, TIH, }}, {"ABOITEAU", new string[]{ UHH, BIH, AWW, EEE, TIH, OWE, }}, {"ABOITEAUX", new string[]{ UHH, BIH, AWW, EEE, TIH, OWE, }}, {"ABILITY", new string[]{ UHH, BIH, IHH, LIH, IHH, TIH, EEE, }},
                {"_B_", row}, {"BREAK", new string[]{ BIH, RIH, AEE, KIH, }}, {"BREW", new string[]{ BIH, RIH, OOO, }}, {"BE", new string[]{ BIH, EEE, }},  {"BIKE", new string[]{ BIH, EYE, KIH, }}, {"BESTOW", new string[]{ BIH, EHH, SIH, TIH, OWE, }}, {"BOTH", new string[]{ BIH, OWE, THI, }}, {"BOT", new string[]{ BIH, HOH, TIH, }}, {"BRUTE", new string[]{ BIH, RIH, OOO, TIH, }}, {"BUT", new string[]{ BIH, UHH, TIH, }}, {"BUY", new string[]{ BIH, EYE, }}, {"BICYCLE", new string[]{ BIH, EYE, SIH, EYE, KIH, LIH, }}, {"BABY", new string[]{ BIH, AEE, BIH, EEE, }},
                {"_C_", row}, {"COMPARE", new string[]{ KIH, HOH, MIH, PIH, EHH, RIH, }}, {"COMPLICIT", new string[]{ KIH, HOH, MIH, PIH, IHH, LIH, SIH, IHH, TIH, }}, {"CAT", new string[]{ KIH, AHH, TIH, }}, {"CALLER", new string[]{ KIH, AWW, LIH, RIH, }}, {"COMPUTER", new string[]{ KIH, HOH, MIH, PIH, YIH, OOO, TIH, RIH, }}, {"CHAMPION", new string[]{ KIH, HIH, AHH, MIH, PIH, EEE, UHH, NIH, }}, {"COLLECTED", new string[]{ KIH, HOH, LIH, EHH, KIH, TIH, EHH, DIH, }}, {"CAUGHT", new string[]{ KIH, AWW, TIH, }}, {"CRUELTY", new string[]{ KIH, RIH, OOO, LIH, TIH, EEE, }}, {"CRUD", new string[]{ KIH, RIH, UHH, DIH, }}, {"CUT", new string[]{ KIH, UHH, TIH, }}, {"CHIVALRY", new string[]{ KIH, HIH, IHH, VIH, AHH, LIH, RIH, EEE, }},
                {"_D_", row}, {"DEAL", new string[]{ DIH, EEE, LIH, }}, {"DRESSES", new string[]{ DIH, RIH, EHH, SIH, EHH, SIH, }}, {"DESIGN", new string[]{ DIH, EEE, SIH, EYE, NIH, }}, {"DOUGH", new string[]{ DIH, OWE, }}, {"DRUMMER", new string[]{ DIH, RIH, UHH, MIH, RIH, }},
                {"_E_", row}, {"EYE", new string[]{ EYE }}, {"ENGINEER", new string[]{ EHH, NIH, JIH, IHH, NIH, EEE, RIH, }}, {"EULOGY", new string[]{ YIH, OOO, LIH, HOH, JIH, EEE, }}, {"END", new string[]{ EHH, NIH, DIH, }},
                {"_F_", row}, {"FAITH", new string[]{ FIH, AEE, THI, }}, {"FAR", new string[]{ FIH, UHH, RIH, }}, {"FAIR", new string[]{ FIH, EHH, RIH, }}, {"FOLLOW", new string[]{ FIH, HOH, LIH, OWE, }}, {"FILED", new string[]{ FIH, EYE, LIH, DIH, }}, {"FELICITY", new string[]{ FIH, EHH, LIH, IHH, SIH, IHH, TIH, EEE, }}, {"FOUR", new string[]{ FIH, AWW, }}, {"FOUL", new string[]{ FIH, UHH, WIH, IHH, LIH, }}, {"FOOL", new string[]{ FIH, OOO, LIH, }}, {"FLY", new string[]{ FIH, LIH, EYE, }}, {"FLAKY", new string[]{ FIH, LIH, AEE, KIH, EEE, }},
                {"_G_", row}, {"GREAT", new string[]{ GIH, RIH, AEE, TIH, }}, {"GIBBERISH", new string[]{ JIH, IHH, BIH, RIH, IHH, SIH, HIH, }}, {"GYM", new string[]{ JIH, IHH, MIH, }}, {"GIN", new string[]{ JIH, IHH, NIH, }}, {"GIVEN", new string[]{ GIH, IHH, VIH, EHH, NIH, }}, {"GUY", new string[]{ GIH, EYE, }},
                {"_H_", row}, {"HAZE", new string[]{ HIH, AEE, ZIH, }}, {"HE", new string[]{ HIH, EEE, }}, {"HIGH", new string[]{ HIH, EYE, }}, {"HUB", new string[]{ HIH, UHH, BIH, }}, {"HYENA", new string[]{ HIH, EYE, EEE, NIH, UHH, }},
                {"_I_", row}, {"I", new string[]{ EYE, }}, {"IMPROVE", new string[]{ IHH, MIH, PIH, RIH, OOO, VIH, }},
                {"_J_", row}, {"JUDGE", new string[]{ JIH, UHH, DIH, JIH, }}, {"JUDGEMENT", new string[]{ JIH, UHH, DIH, JIH, MIH, EHH, NIH, TIH, }}, {"JOHN", new string[]{ JIH, HOH, NIH, }}, {"JELLY", new string[]{ JIH, EHH, LIH, EEE, }},
                {"_K_", row}, {"KEY", new string[]{ KIH, EEE, }}, {"KNIGHT", new string[]{ NIH, EYE, TIH, }}, {"KITE", new string[]{ KIH, EYE, TIH, }},
                {"_L_", row}, {"LEAF", new string[]{ LIH, EEE, FIH, }}, {"LABEL", new string[]{ LIH, AEE, BIH, LIH, }}, {"LAST", new string[]{ LIH, AHH, SIH, TIH, }}, {"LADDER", new string[]{ LIH, AHH, DIH, RIH, }}, {"LOVELY", new string[]{ LIH, UHH, VIH, LIH, EEE, }}, {"LEAD", new string[]{ LIH, EEE, DIH, }}, {"LIGHT", new string[]{ LIH, EYE, TIH, }}, {"LORE", new string[]{ LIH, AWW, RIH, }}, {"LIKELY", new string[]{ LIH, EYE, KIH, LIH, EEE, }},
                {"_M_", row}, {"MAPLE", new string[]{ MIH, AEE, PIH, LIH, }}, {"MAY", new string[]{ MIH, AEE, }}, {"ME", new string[]{ MIH, EEE, }}, {"MAYBE", new string[]{ MIH, AEE, BIH, EEE, }}, {"MOLTEN", new string[]{ MIH, HOH, LIH, TIH, EHH, NIH, }},
                {"_N_", row}, {"NICE", new string[]{ NIH, EYE, SIH, }}, {"NICKLE", new string[]{ NIH, IHH, KIH, LIH, }}, {"NARROW", new string[]{ NIH, AHH, RIH, OWE, }}, {"NEGATIVELY", new string[]{ NIH, EHH, GIH, UHH, TIH, IHH, VIH, LIH, EEE, }},
                {"_O_", row}, {"OSPREY", new string[]{ HOH, SIH, PIH, RIH, AEE, }}, {"OBJECTIVE", new string[]{ HOH, BIH, JIH, EHH, KIH, TIH, IHH, VIH, }}, {"OXYGEN", new string[]{ HOH, KIH, SIH, IHH, JIH, EHH, NIH, }}, {"OF", new string[]{ HOH, FIH, }}, {"OBSTRUCT", new string[]{ HOH, BIH, SIH, TIH, RIH, UHH, KIH, TIH, }}, {"OPULENCE", new string[]{ HOH, PIH, YIH, OOO, LIH, EHH, NIH, SIH, }},
                {"_P_", row}, {"PHRASE", new string[]{ FIH, RIH, AEE, SIH, }}, {"PLOTTABLE", new string[]{ PIH, LIH, HOH, TIH, UHH, BIH, LIH, }}, {"PLATED", new string[]{ PIH, LIH, AEE, TIH, EHH, DIH, }}, {"PLANET", new string[]{ PIH, LIH, AHH, NIH, EHH, TIH, }}, {"PHAROAH", new string[]{ FIH, EHH, RIH, OWE, }}, {"PIKE", new string[]{ PIH, EYE, KIH, }}, {"POINT", new string[]{ PIH, AWW, EEE, NIH, TIH, }}, {"PLANNER", new string[]{ PIH, LIH, AHH, NIH, RIH, }}, {"POUCH", new string[]{ PIH, AHH, OOO, KIH, HIH, }}, {"PRO", new string[]{ PIH, RIH, OWE, }}, {"PRISM", new string[]{ PIH, RIH, IHH, SIH, MIH, }}, {"PURR", new string[]{ PIH, RIH, }}, {"PULL", new string[]{ PIH, OOO, LIH, }},
                {"_Q_", row}, {"QUEUE", new string[]{ KIH, YIH, OOO, }}, {"QUERY", new string[]{ KIH, WIH, EHH, RIH, EEE, }},
                {"_R_", row}, {"RAW", new string[]{ RIH, AWW, }}, {"ROW", new string[]{ RIH, OWE, }}, {"ROB", new string[]{ RIH, HOH, BIH, }}, {"RUBBER", new string[]{ RIH, UHH, BIH, RIH, }}, {"REMEMBER", new string[]{ RIH, EEE, MIH, EHH, MIH, BIH, RIH, }}, {"RUNNING", new string[]{ RIH, UHH, NIH, EEE, NIH, }}, {"ROUTE", new string[]{ RIH, OOO, TIH, }}, {"RUDE", new string[]{ RIH, OOO, DIH, }}, {"RUIN", new string[]{ RIH, OOO, IHH, NIH, }},
                {"_S_", row}, {"SAUL", new string[]{ SIH, AWW, LIH, }}, {"STEAK", new string[]{ SIH, TIH, AEE, KIH, }}, {"SPACE", new string[]{ SIH, PIH, AEE, SIH, }}, {"STACY", new string[]{ SIH, TIH, AEE, SIH, EEE, }}, {"SICILY", new string[]{ SIH, IHH, SIH, IHH, LIH, EEE, }}, {"SPEECH", new string[]{ SIH, PIH, EEE, EEE, KIH, HIH, }}, {"STEIN", new string[]{ SIH, TIH, EEE, NIH, }}, {"SKIES", new string[]{ SIH, KIH, EYE, SIH, }}, {"SIGN", new string[]{ SIH, EYE, NIH, }}, {"SPITE", new string[]{ SIH, PIH, EYE, TIH, }}, {"SOUR", new string[]{ SIH, AHH, HOH, WIH, UHH, }}, {"SLOUCH", new string[]{ SIH, LIH, AHH, OOO, KIH, HIH, }}, {"SOUL", new string[]{ SIH, OWE, WIH, IHH, LIH, }}, {"SOLE", new string[]{ SIH, OWE, WIH, IHH, LIH, }}, {"SOLO", new string[]{ SIH, OWE, LIH, OWE, }}, {"SLOTH", new string[]{ SIH, LIH, HOH, THI, }}, {"SUBMIT", new string[]{ SIH, UHH, BIH, MIH, IHH, TIH, }}, {"SKY", new string[]{ SIH, KIH, EYE, }}, {"STYLE", new string[]{ SIH, TIH, EYE, LIH, }}, {"SOLILOQUY", new string[]{ SIH, HOH, LIH, IHH, LIH, HOH, KIH, WIH, EEE, }},
                {"_T_", row}, {"TABLE", new string[]{ TIH, AEE, BIH, LIH, }}, {"THE", new string[]{ THI, UHH, }}, {"TRIBE", new string[]{ TIH, RIH, EYE, BIH, }}, {"THESE", new string[]{ THI, EEE, SIH, }}, {"TREKKIES", new string[]{ TIH, RIH, EHH, KIH, EEE, SIH, }}, {"THERE", new string[]{ THI, EHH, RIH, }}, {"TRIGGER", new string[]{ TIH, RIH, IHH, GIH, RIH, }}, {"TALKING", new string[]{ TIH, AWW, KIH, EEE, NIH, }}, {"THIGH", new string[]{ THI, EYE, }}, {"TRACTION", new string[]{ TIH, RIH, AHH, KIH, SIH, HIH, UHH, NIH, }}, {"TOUCH", new string[]{ TIH, UHH, TIH, SIH, HIH, }}, {"TOLD", new string[]{ TIH, HOH, LIH, DIH, }}, {"TODAY", new string[]{ TIH, OOO, DIH, AEE, }}, {"THINK", new string[]{ THI, IHH, NIH, KIH, }}, {"TOTALLY", new string[]{ TIH, OWE, TIH, UHH, LIH, EEE, }},
                {"_U_", row}, {"UNDEVELOPED", new string[]{ UHH, NIH, DIH, EHH, VIH, EHH, LIH, HOH, PIH, DIH, }}, {"UPDATE", new string[]{ UHH, PIH, DIH, AEE, TIH, }},
                {"_V_", row}, {"VETO", new string[]{ VIH, EEE, TIH, OWE, }},
                {"_W_", row}, {"WATER", new string[]{ WIH, AWW, TIH, RIH, }}, {"WHAT", new string[]{ WIH, HOH, TIH, }}, {"WE", new string[]{ WIH, EEE, }}, {"WORD", new string[]{ WIH, RIH, DIH, }},
                {"_X_", row}, {"XYLOPHONE", new string[]{ ZIH, EYE, LIH, UHH, FIH, OWE, NIH, }},
                {"_Y_", row}, {"YOU", new string[]{ YIH, OOO, }}, {"YAM", new string[]{ YIH, AHH, MIH, }},
                {"_Z_", row},
            };
            const string fileAddress = @"C:\Users\power\Desktop\scripting\SpaceEngineersTextToSpeechMod\AdjacentResults.txt";                        

            OrderedDictionary emptiesRemoved = new OrderedDictionary();            
            OrderedDictionary tabledResults = new OrderedDictionary();
            ICollection adjacentKeys;
            ICollection resultKeys;

            public AdjacentTester()
            {
                emptiesRemoved = adjacentWords;
                adjacentKeys = emptiesRemoved.Keys;
                resultKeys = tabledResults.Keys;
            }

            public string RollOutAdjacentWords()
            {
                string rolledOut = "[ ";                

                //removing row markers
                for (int i = 0; i < emptiesRemoved.Count; i++) 
                {                    
                    string[] currentAdjacentValue = emptiesRemoved[i] as string[];

                    if (currentAdjacentValue.IsNullOrEmpty())
                    {
                        emptiesRemoved.RemoveAt (i);
                        i--;
                    }
                }

                //adding
                //these two loops need to be seperate due to the nature ordered dictionary enumerators.
                IEnumerator addingIndex = adjacentKeys.GetEnumerator(); 

                for (int i = 0; i < emptiesRemoved.Count; i++) 
                {                    
                    addingIndex.MoveNext();                       
                    rolledOut += addingIndex.Current.ToString() + " ";
                }
                return rolledOut;
            }

            public void StoreResults (string currentWord, List <string> phonemes)
            {
                if (currentWord != " ")
                {                    
                    for (int i = 0; i < phonemes.Count; i++)
                    {
                        if (phonemes[i] == " " ||
                            phonemes[i] == "")
                        {
                            phonemes.RemoveAt (i);
                            i--;
                        }
                    }
                    string[] formattedPhonemes = phonemes.ToArray();

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

            public void PrintResults()
            {                
                string[] lines = new string[emptiesRemoved.Count];
                IEnumerator adjacentIndex = adjacentKeys.GetEnumerator();
                IEnumerator resultsIndex = resultKeys.GetEnumerator();

                for (int i = 0; i < lines.Length; i++)
                {
                    adjacentIndex.MoveNext();
                    resultsIndex.MoveNext();
                    string currentAdjacentKey = adjacentIndex.Current.ToString();
                    string currentResultsKey = resultsIndex.Current.ToString();
                    string[] currentAdjacentValue = emptiesRemoved[currentAdjacentKey] as string[];
                    string[] currentResultsValue = tabledResults[currentResultsKey] as string[];
                    bool isMatch = false;

                    lines[i] = adjacentIndex.Current.ToString() + "      { ";

                    for (int f = 0; f < currentAdjacentValue.Length; f++)
                    {
                        lines[i] += currentAdjacentValue[f] + ", ";

                        if (currentAdjacentValue.Length == currentResultsValue.Length)
                        {
                            if (currentAdjacentValue[f] == currentResultsValue[f])
                            {
                                isMatch = true;
                            }
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
                        lines[i] = "correct ------ " + lines[i];
                            break;

                        case false:
                            lines[i] = "incorrect ------ " + lines[i];
                            break;
                    }
                }
                File.WriteAllLines (fileAddress, lines);
            }
        }
    }
}
