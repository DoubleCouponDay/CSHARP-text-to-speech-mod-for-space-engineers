﻿using System.Text.RegularExpressions; //needed to match wildcard string matches simplify the rule based phoneme approach AdjacentEvaluation().
using System.Collections.Generic;
using SETextToSpeechMod.LookUpTables;

namespace SETextToSpeechMod.Processing
{
    public class Pronunciation : ISentenceReset
    {    
        //Constants
        /// <summary>
        /// The length of evaluation chunks in the AdjacentEvaluation algorithm.
        /// </summary>
        public const int ALGORITHM_PHRASE_SIZE = 5;
        public const int SENTENCE_END_ZONES_LENGTH = 3;
        const string SPACE = " ";

        //objects
        public WordIsolator WordIsolator {get; private set;}
        private readonly Intonation intonationGen;

        //public feedback
        public bool PreviousProcessUsedDictionary {get; private set;}
        public int WrongFormatMatches {get; private set;}
        public int WrongFormatNonMatches {get; private set;}        

        //internal state
        string[] dictionaryMatch;
        string surroundingPhrase;
        List <string> currentResults = new List <string>(); //re used a lot so dont put dictonary and adjacent results in at the same time!        
        bool pushingDictionaryWordOut;
        readonly int sentenceLength;
        
        Sentence tempSentence; //temps should remain conventionally readonly for each separate letter analysis.

        public Pronunciation (Intonation intonationType)
        {
            this.WordIsolator = new WordIsolator();
            this.intonationGen = intonationType;
        }

        public void FactoryReset(Sentence newSentence)
        {
            surroundingPhrase = string.Empty;
            PreviousProcessUsedDictionary = false;
            WrongFormatMatches = 0;
            WrongFormatNonMatches = 0;            
            currentResults.Clear();
            dictionaryMatch = null;

            tempSentence = newSentence;
            tempLetterIndex = 0;

            pushingDictionaryWordOut = false;

            WordIsolator.FactoryReset(newSentence);
        }

        /// <summary>
        /// first searches the dictionary, then tries adjacent evaluation if no match found.
        /// Returns new list.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="letterIndex"></param>
        /// <returns>returns new list.</returns>
        public List <string> GetLettersPronunciation (int letterIndex) 
        {            
            pushingDictionaryWordOut = false;   
            currentResults = new List <string>();
            WordIsolator.MoveNext(); //Incrementing the WordIsolator must happen at the beginning of a new letter analysis. This is so optional debugger can pick up accurate properties after each letter analysis.             
            currentResults.Clear();        
            surroundingPhrase = string.Empty;                                      

            if (WordIsolator.Current != SPACE)
            {
                if (WordIsolator.CurrentWordIsNew == true)
                {                    
                    dictionaryMatch = null;
                    PreviousProcessUsedDictionary = false; //prevent false positives
                    PreviousProcessUsedDictionary = PrettyScaryDictionary.TTS_DICTIONARY.TryGetValue (WordIsolator.Current, out dictionaryMatch);

                    if (PreviousProcessUsedDictionary)
                    {
                        TakeFromDictionary();
                    }
            
                    else
                    {
                        AdjacentEvaluation (letterIndex);
                    }
                }

                else if (PreviousProcessUsedDictionary)
                {
                    TakeFromDictionary();
                }

                else
                {
                    AdjacentEvaluation (letterIndex);
                }
            }

            else
            {
                currentResults.Add (SPACE); //avoids setting WordIsolator.LetterIndex in this scenario since an empty space cant reset it when needed.
            }
            
            if (OutputManager.IsDebugging == false) //debugger only checks that the correct phonemes were selected; doesnt care about intonation
            {
                bool sentenceIsEnding = (letterIndex >= tempSentence.Length - SENTENCE_END_ZONES_LENGTH) ? true : false;

                for (int i = default (int); i < currentResults.Count; i++)
                { 
                    bool lastVowelSoon = default (bool);

                    if (sentenceIsEnding && 
                        pushingDictionaryWordOut &&
                        i < currentResults.Count - SENTENCE_END_ZONES_LENGTH) 
                    {
                        lastVowelSoon = false; //prevents TakeFromDictionary() from applying the sentence end state to its entire word push.
                    }              
                    currentResults[i] = intonationGen.GetPhonemesIntonation (currentResults[i], surroundingPhrase, lastVowelSoon);
                }
            }
            return currentResults;
        }

        //Method returns one phoneme per run until it reaches the end of the current dictionary key. In which case it will dump all remaining.
        //Returns nothing if there is no dictionary match
        private void TakeFromDictionary()
        {
            int bookmark = dictionaryMatch.Length - WordIsolator.LettersLeftInWord;

            if (bookmark >= default (int)) //Just in case ive entered dictionary values with 0 length
            {
                if (bookmark == WordIsolator.WordsIndexLimit)
                {
                    pushingDictionaryWordOut = true;

                    for (int i = bookmark; i < dictionaryMatch.Length; i++)
                    {
                        currentResults.Add(dictionaryMatch[i]);
                    }
                }

                else
                {
                    currentResults.Add(dictionaryMatch[bookmark]);
                }
            }
        }

        /* Rule based processing algorithm which works one letter at a time.
         * It's more efficient than a dictionary attack but its structure is hard to understand without consulting the documentation.
         * The primary purpose of OptionalDebugger is to make sure this algorithm does not decrease in word coverage due to obscurity.
        */
        private void AdjacentEvaluation (int tempLetterIndex)
        {
            //const string VOWELS = "AEIOU";
            const string CONSONANTS = "BCDFGHJKLMNPQRSTVWXYZ";
            string primary = "";
            string secondary = "";

            int intBefore = (tempLetterIndex - 1 >= 0) ? (tempLetterIndex - 1) : tempLetterIndex; //these wil prevent out-of-bounds exception.
            int intAfter = (tempLetterIndex + 1 < tempSentence.Length) ? (tempLetterIndex + 1) : tempLetterIndex; 
            int intTwoAfter = (tempLetterIndex + 2 < tempSentence.Length) ? (tempLetterIndex + 2) : tempLetterIndex;
            int intTwoBefore = (tempLetterIndex - 2 >= 0) ? (tempLetterIndex - 2) : tempLetterIndex;

            string before = (intBefore != tempLetterIndex) ? tempSentence[intBefore].ToString() : SPACE; //these 4 strings ensure i can correctly identify separate words.
            string after = (intAfter != tempLetterIndex) ? tempSentence[intAfter].ToString() : SPACE; //using strings instead of chars saves lines since i need strings for Contains()
            string twoBefore = (intTwoBefore != tempLetterIndex && before != SPACE) ? tempSentence[intTwoBefore].ToString() : SPACE; //the false path must return a space string because spaces signify the start/end of a word.
            string twoAfter = (intTwoAfter != tempLetterIndex && after != SPACE) ? tempSentence[intTwoAfter].ToString() : SPACE;        
            string currentLetter = tempSentence[tempLetterIndex].ToString();

            surroundingPhrase = twoBefore + before + currentLetter + after + twoAfter; //must update here before UnwantedMatchBypassed is used in this method.
           
            switch (currentLetter)
            {
#region case A
                case "A":
                    if (UnwantedMatchBypassed ("..AK.") && //!steak
                        UnwantedMatchBypassed ("REAT.") && //!great
                        IsMatch (".EAF." + //leaf
                                "|.EAL." + //real
                                "|.EAD." + //lead
                                "|.EAT." //meat
                                ) ||

                       (IsMatch (".OAH."))) //pharoah 
                    {
                        ;
                    }

                    else if (IsMatch ("..AW." + //raw
                                     "|.WAT." + //water
                                     "|. AUT" + //autograph
                                     "|..AUT" + //astronaut
                                     "| CALL" + //caller
                                     "| TALL" + //tallest
                                     "|..AUG" + //caught
                                     "|..ALK" //talking
                                     ) ||

                            (UnwantedMatchBypassed ("SSAUL") && //!assault
                            IsMatch ("..AUL"))) //saul                                 

                    {
                        primary = PrettyScaryDictionary.AWW;
                    }

                    else if (IsMatch ("REA.." + //break
                                     "|.EAK." + //steak
                                     "| TAB." + //table
                                     "| LAB." + //lable
                                     "|..APL" + //maple
                                     "|..AY." + //may
                                     "|.HAZE" + //haze
                                     "|.RASE" + //phrase
                                     "|. ABL" + //able
                                     "|..ACE" + //space
                                     "|..ATE" + //activate
                                     "| BABY" + //baby
                                     "|..ADY" + //lady
                                     "|..AKY" + //flaky
                                     "|..ACY" + //stacy
                                     "|..AME" + //same
                                     "|.LADO" //glados
                                     ) ||
                            
                            (UnwantedMatchBypassed (".PATI") && //!patio
                             UnwantedMatchBypassed (".GATI") && //!negatively
                             IsMatch ("..ATI")) || //station

                            (IsMatch ("..AI.") && //faith
                             UnwantedMatchBypassed ("..A.R"))) // !fair,
                    {
                        primary = PrettyScaryDictionary.AEE;
                    }

                    else if (IsMatch (". A ." + //a
                                     "|. AV." + //available
                                     "|..ABL" + //available
                                     "|. AG." + //aggression
                                     "|. ANN" + //annoint
                                     "|. ASS" + //assault
                                     "|. ABI" + //ability
                                     "|OTAL." + //totally
                                     "|..A ." + //hyena
                                     "|..ARG" + //bargain
                                     "|.GATI" + //negatively
                                     "|..AMA" + //proclamation
                                     "|ISA.." //elisabeth
                                     ) ||

                            (UnwantedMatchBypassed ("..ACT") && //!activites
                             IsMatch (". AC.")) || //acoustic
                            
                            (IsMatch ("..AR ") && //far
                             UnwantedMatchBypassed ("..A.E"))) //!fare
                    {
                        primary = PrettyScaryDictionary.UHH;
                    }

                    else if (IsMatch ("..ARE" + //compare                             
                                     "|..AIR" + //fair
                                     "|..ARO" //pharoah
                                     ))
                    { 
                        primary = PrettyScaryDictionary.EHH;
                    }

                    else if (IsMatch ("SSAUL" + //assault
                                     "|WHAT." //what        
                                     )) 
                    {
                        primary = PrettyScaryDictionary.HOH;
                    }

                    else if (IsMatch ("TEAU.")) //chateau
                    {
                        primary = PrettyScaryDictionary.OWE;
                    }

                    else if (IsMatch (".TAGE")) //advantage
                    {
                        primary = PrettyScaryDictionary.IHH;
                    }

                    else    
                    {
                        primary = PrettyScaryDictionary.AHH; //plottable
                    }
                    break;
#endregion case A

#region case B
                case "B":
                    if (IsMatch (".MB ." + //bomb
                                "|.BB.." //cobber
                                ))                                                                      
                    {
                        ;
                    }

                    else if (IsMatch ("..BL.")) //able
                    {
                        primary = SPACE;
                        secondary = PrettyScaryDictionary.BIH;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.BIH;
                    }
                    break;
#endregion case B

#region case C
                case "C":
                    if (IsMatch (".CC..")) //!double C's 
                    {
                        ;
                    }
                     
                    else if (IsMatch ("..CE." + //nice
                                     "|..CI." + //complicit
                                     "|..CY." //stacy
                                     ))
                    {
                        primary = PrettyScaryDictionary.SIH; //sicily
                    }

                    else if (UnwantedMatchBypassed ("..CHN") && //technology
                             IsMatch ("..CH.")) //champion
                    {
                        primary = PrettyScaryDictionary.CHI;
                    }
            
                    else
                    {
                        primary = PrettyScaryDictionary.KIH;
                    } 
                    break;
#endregion case C

#region case D
                case "D":
                    if (IsMatch ("..DG." + //judge
                                "|.DD.." //ladder
                                ))
                    {
                        ; 
                    }
            
                    else
                    {
                        primary = PrettyScaryDictionary.DIH;
                    }
                    break;
#endregion case D

#region case E
                case "E":
                    if (IsMatch ("..EAK" + //break
                                "|..EU." + //queue
                                "|.EE.." + //speech
                                "|..ELY" + //lovely
                                "|.TEAU" + //aboiteau
                                "|OPED " + //undeveloped
                                "|.LED " + //filed
                                "|DGE.." + //judgement
                                "|.VES " + //ourselves
                                "|.BEL " //label
                                ) ||

                            (UnwantedMatchBypassed (".REAL") && //!real
                             IsMatch (".REA.")) || //great

                            (UnwantedMatchBypassed ("TIES.") && //!activities
                            IsMatch (".IES.")) || //flies

                            (UnwantedMatchBypassed (".VE..") && //!lover 
                             UnwantedMatchBypassed (".EE..") && //!veer
                             IsMatch ("..ER.")) || //rubber                            
                            
                            (UnwantedMatchBypassed ("THE .") && //!the
                             UnwantedMatchBypassed (" .E..") && //!He man    
                             UnwantedMatchBypassed ("YBE..") && //!maybe                     
                             IsMatch ("..E ."))) //tribe
                    {
                        ;
                    }    

                    else if (IsMatch ("THE .")) //the
                    {
                        primary = PrettyScaryDictionary.UHH;
                    } 
  
        
                    else if (IsMatch ("..EW.")) //brew
                    {
                        primary = PrettyScaryDictionary.OOO; 
                    }

                    else if (IsMatch ("..EYE")) //eye               
                    {
                        primary = PrettyScaryDictionary.EYE;
                    }         
       
                    else if (IsMatch ("..EE." + //engineer
                                     "|.DEA." + //deal
                                     "|..E.D" + //lead
                                     "| ME ." + //me
                                     "| HE ." + //he
                                     "| WE ." + //we
                                     "| BE ." + //be
                                     "|YBE ." + //maybe
                                     "|..ESE" + //these
                                     "|.KEY." + //key
                                     "|.IE. " + //trekkies
                                     "|.VETO" + //veto
                                     "|..ENA" + //hyena
                                     "|.TEI." + //stein
                                     "|..ENI" //penis
                                     ) ||

                            (UnwantedMatchBypassed (".RES.") && //!respite
                             UnwantedMatchBypassed (".REI.") && //!rein
                             IsMatch (" RE..")) || //remember
                                     
                            (UnwantedMatchBypassed ("ITE..") && //!aboiteau
                             IsMatch (".LEA."))) //lead
                    {                           
                        primary = PrettyScaryDictionary.EEE;
                    }  

                    else if (IsMatch (".REY." + //osprey
                                     "|.REI." //rein
                                     ))
                    {
                        primary = PrettyScaryDictionary.AEE;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.EHH;
                    }
                    break;
#endregion case E

#region case F
                case "F": 
                    primary = PrettyScaryDictionary.FIH; //follow
                    break;
#endregion case F

#region case G
                case "G":
                    if (IsMatch (".GG.." + //trigger
                                "|..GH." + //high
                                "|.NG ." + //talking
                                "|.IGN." + //design
                                "|..GHT" //caught
                                ))
                    {
                        ;
                    }

                    else if  (

                              (UnwantedMatchBypassed ("..G .") && //rig
                               IsMatch (".IGI.")) || //aborigine
                                
                               IsMatch (". GY." + //gym
                                       "|.DGE." + //judgement 
                                       "|ENGI." + //engineer
                                       "|.OGY " + //eulogy
                                       "|. GIN" + //gin
                                       "|.AGE." + //advantage
                                       "|..GEN" //oxygen
                                       ))
                    {   
                        primary = PrettyScaryDictionary.JIH;
                        secondary = SPACE; //such as "gin", judgement, 
                    }
            
                    else
                    {
                        primary = PrettyScaryDictionary.GIH; //given
                    }    
                    break;
#endregion case G

#region case H
                case "H":
                    if (IsMatch ("..HN." + //john
                                "|.GH.." + //dough
                                "|.PH.." + //autograph
                                "|.WH.." + //what
                                "|.CH.." //champion
                                ) ||

                       (UnwantedMatchBypassed (".CH..") && //!pouch
                        IsMatch ("..H .")) || //pharoah

                       (UnwantedMatchBypassed ("..HU.") && //!github
                        IsMatch (".TH.."))) //thigh
                    {
                        ;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.HIH;
                    }    
                    break;
#endregion case H

#region case I
                case "I":
                    if (UnwantedMatchBypassed (".LIES") && //!flies
                        UnwantedMatchBypassed (" PIES") && //!pies
                        UnwantedMatchBypassed (".KIES") && //!skies
                        IsMatch ("..IES") || //activities

                        IsMatch ("VAILA" + //available
                                "|GAIN." + //bargain
                                "|.AITH" + //faith
                                "|.AIR." + //fair
                                "|.EI.." //stein
                                ))
                    {
                        ;
                    }

                    else if (IsMatch (".TION" + //traction
                                     "|.SION" //accession
                                     ))
                    {
                        primary = PrettyScaryDictionary.SHI;                   
                    }

                    else if (IsMatch ("..IKE" + //pike
                                     "|KNI.." + //knight
                                     "|..IGH" + //light
                                     "|. I ." + //I
                                     "|..ILE" + //filed                                 
                                     "|..IGN" + //sign
                                     "| VITA" + //vitality                                                                   
                                     "|..ICY" + //bicycle
                                     "| TITA" + //titanite
                                     "|OVISA" + //improvisation
                                     "|OVISE" + //improvise
                                     "|.LIE." + //flies
                                     "| .IE." + //pies
                                     "| .IK." //bikie
                                     ) ||

                            (UnwantedMatchBypassed (".GINE") && //!aborigine
                             UnwantedMatchBypassed ("ENICE") && //!venice
                             UnwantedMatchBypassed (".OITE") && //!aboiteau
                             UnwantedMatchBypassed (".GIVE") && //!give
                             UnwantedMatchBypassed (".TIVE") && //!negatively
                             IsMatch ("..I.E")) || //nice
                            
                            (CONSONANTS.Contains (before) && //respite
                             IsMatch ("..ITE"))) //titanite
                    {   
                        primary = PrettyScaryDictionary.EYE;
                    }
            
                    else if (IsMatch (".OIN." + //point
                                     "|..ING" + //running
                                     "|.OITE" + //aboiteaux
                                     "|..ION" + //champion
                                     "|..IO." + //patio                                     
                                     "|SKIE." + //huskies
                                     "|.KIE." + //trekkies
                                     "| .ISA" //visa
                                     ))
                    {
                        primary = PrettyScaryDictionary.EEE;
                    }
    
                    else 
                    {
                        primary = PrettyScaryDictionary.IHH;  //felicity
                    }
                    break;
#endregion case I

#region case J
                case "J":   
                    primary = PrettyScaryDictionary.JIH; //jelly
                    break;
#endregion case J

#region case K
                case "K":
                    if (IsMatch (".CK.." + //two kih's
                                "|..KN." + //silent K
                                "|.KK..")) //double K's
                    {
                        ;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.KIH;
                    }    
                    break;
#endregion case K

#region case L
                case "L":
                    if (IsMatch ("..LK." + //silent L
                                "|..LF." + //silent L
                                "|.LL..")) //double L's
                    {
                        ;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.LIH;
                    }
                    break;
#endregion case L

#region case M
                case "M":
                    if (IsMatch (".MM..")) //double M's
                    {
                        ;                        
                    }

                    else
                    {
                        primary = SPACE;
                        secondary = PrettyScaryDictionary.MIH; //such as "molten", drummer,
                    }  
                    break;
#endregion case M

#region case N
                case "N":      
                    if (IsMatch (".NN..")) //double N's
                    {
                        ;
                    }

                    else if (IsMatch ("GINE ")) //aborigine
                    {
                        primary = PrettyScaryDictionary.NIH;
                        secondary = PrettyScaryDictionary.EEE;
                    }
                           
                        
                    else
                    {
                        primary = SPACE;
                        secondary = PrettyScaryDictionary.NIH;  //such as nickel,
                    }                         
                    break;
#endregion case N

#region case O
                case "O":    
                    if (IsMatch (".TOU." + //touch
                                "|.OO.." + //double O's
                                "|.WOR." //word
                                ) ||

                       (UnwantedMatchBypassed ("..OE ") && //tiptoe
                        IsMatch ("..OE.")) || //manoeuvre

                       (UnwantedMatchBypassed (".COUS") && //acoustic
                        UnwantedMatchBypassed (".HOUS") && //house
                        IsMatch (".IOUS" + //abstentious
                                "|.ROUS" + //ludicrous
                                "|.POUS" + //acarpous
                                "|.EOUS" + //advantageous
                                "|.LOUS" //acaulous
                                )))
                    {
                        ;
                    }                    
                        
                    else if (IsMatch ("..OI." + //annoint
                                     "|.FOUR" //four
                                     ) ||

                            (UnwantedMatchBypassed ("ABORI") && //aborigine
                             IsMatch ("..OR."))) //lore
                    {
                        primary = PrettyScaryDictionary.AWW;
                    }

                    else if (IsMatch (".FOUL" + //foul
                                     "|.POUC" + //pouch
                                     "|.LOUC" + //slouch
                                     "|.HOUS" + //house
                                     "|..OUT" + //out
                                     "|..OUR" //our
                                     ))
                    {
                        primary = PrettyScaryDictionary.AHH; 
                        secondary = PrettyScaryDictionary.HOH;
                    }
 
                    else if (IsMatch (".ION." + //champion
                                     "|.DONE" + //done
                                     "|.LOVE" //lovely
                                     ))
                    {
                        primary = PrettyScaryDictionary.UHH;
                    }   

                    else if (IsMatch ("..OHN" +  //john
                                     "|.BOT " + //bot
                                     "|..OM." + //computer
                                     "|..OGY" + //eulogy
                                     "|..OPU" + //opulence
                                     "|..OTT" + //plottable
                                     "|..OC." + //proclamation
                                     "|..OB " + //rob
                                     "|..OLD" + //told
                                     "|..OX." + //oxygen
                                     "|..OF." + //of
                                     "|ADO.." //glados
                                     ) ||

                            (UnwantedMatchBypassed ("..OO.") && //!oolacile
                             IsMatch (". O..")) || //objective

                            (UnwantedMatchBypassed (".SOLO") && //!solo
                             UnwantedMatchBypassed (".OO..") && //!cool
                             IsMatch ("..OL.")) || //collected

                            (UnwantedMatchBypassed (".BO..") && //!both
                             IsMatch ("..OTH"))) //sloth
                    {
                        primary = PrettyScaryDictionary.HOH;                      
                    }    
            
                    else if (IsMatch ("..OO." + //fool
                                     "|PROVE" + //improve
                                     "|.TOD." + //today
                                     "|PROVE" + //improve
                                     "|.COUS" + //acoustic
                                     "|..OU " //you
                                     ))
                    {
                        primary = PrettyScaryDictionary.OOO;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.OWE;
                    }
                    break;
#endregion case O

#region case P
                case "P":
                    if (IsMatch (".PP..")) //double P's
                    {
                        ;
                    }

                    else if (IsMatch ("..PH.")) //phrase
                    {
                        primary = PrettyScaryDictionary.FIH;
                    }     
                    
                    else
                    {
                        primary = PrettyScaryDictionary.PIH;
                    }                 
                    break;
#endregion case P

#region case Q
                case "Q": 
                    if (IsMatch ("..QUE"))
                    {
                        primary = PrettyScaryDictionary.KIH;
                        secondary = PrettyScaryDictionary.YIH;
                    }                   

                    else
                    {
                        primary = PrettyScaryDictionary.KIH; //query    
                        secondary = PrettyScaryDictionary.WIH;
                    }                    
                    break;
#endregion case Q

#region case R
                case "R":   
                    if (IsMatch (".RR..")) //!double R's
                    {                        
                        ;
                    }

                    else if ((UnwantedMatchBypassed ("EER..") && //!engineer
                              IsMatch (".ER..")) || //climber
                              
                              IsMatch (".VRE ")) //manoeuvre
                    {
                        primary = PrettyScaryDictionary.ERR;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.RIH;
                    }
                    break;
#endregion case R

#region case S
                case "S":
                    if (IsMatch (".SS.." + //!double S's
                                "|..SSI" //i need to place an SHI in case I so this will have to do.
                                ))
                    {
                        ;
                    }
                       
                    else if (IsMatch ("..SM " + //prism
                                     "|VIS.." //improvise
                                     ))
                    {
                        primary = PrettyScaryDictionary.ZIH;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.SIH;
                    }                  
                    break;
#endregion case S

#region case T
                case "T":
                    if (UnwantedMatchBypassed ("PATIO") && //!patio
                        IsMatch (".ATIO" + //proclamation                        
                                "|.CTIO" + //instructional 
                                "|.TT.." //!double T's
                                ))
                    {
                        ;
                    }
                             
                    else if (UnwantedMatchBypassed ("..T.U") && //!github
                        IsMatch ("..TH.")) //think
                    {
                        primary = SPACE;
                        secondary = PrettyScaryDictionary.THI;    
                    } 

                    else if (IsMatch (".ST..")) //emphasised T
                    {
                        primary = SPACE;
                        secondary = PrettyScaryDictionary.TIH;
                    }
                        
                    else
                    {
                        primary = PrettyScaryDictionary.TIH;
                    }                        
                    break;
#endregion case T

#region case U
                case "U":
                    if (IsMatch (".QU.." + //queue
                                "|.AU.." + //caught
                                "|.OUL." + //soul
                                "|YOU.." + //you
                                "|.AUT." + //astronaut
                                "|.AUL." + //assault
                                "|.OUGH" + //dough
                                "|.OUR." + //four
                                "|COUS." + //accoustic
                                "|HOUS." + //hous
                                "|.OUT." //out
                                ) ||
                        
                       (UnwantedMatchBypassed ("TOUCH") && //!touch
                        IsMatch (".OUCH")) || //pouch

                       (UnwantedMatchBypassed (".OU..") && //!your
                        IsMatch ("..URR"))) //purr
                    {
                        ;
                    }    

                    else if (IsMatch (".EU.." + //sleuth
                                     "|..UE." + //cruelty
                                     "|.AU ." + //aboiteau
                                     "|.AUX " + //aboiteaux
                                     "|.RU.E" + //rude
                                     "|..UI." + //ruin
                                     "|..UDI" + //ludicrous
                                     "|.PULL" //pull
                                     ))
                    {
                        if (IsMatch (" EU..")) //eulogy
                        {
                            primary = PrettyScaryDictionary.YIH;
                            secondary = PrettyScaryDictionary.OOO;
                        }

                        else
                        {
                            primary = PrettyScaryDictionary.OOO;
                        }                        
                    }

                    else if (IsMatch (" OUR." + //end
                                     "|.OUC." + //touch
                                     "|. UN." + //undeveloped
                                     "|. UP." + //update
                                     "|.SUB." + //submit
                                     "|.HUB." + //hub
                                     "|..UMM" //drummer                                     
                                     ) ||

                            (UnwantedMatchBypassed (".OUSE") && //!house
                             IsMatch (".OUS.")) || //ludicrous

                            (UnwantedMatchBypassed ("..UDE") && //!prude
                             UnwantedMatchBypassed ("..UDI") && //!ludicrous
                             IsMatch ("..UD.")) || //crud                                                    

                            (CONSONANTS.Contains (after) && //rubber
                             CONSONANTS.Contains (twoAfter)) || //instructional

                            (CONSONANTS.Contains (before) && //cut
                             UnwantedMatchBypassed ("..U.E") && //brute
                             IsMatch ("..UT." + //but
                                     "|..UC." //obstruct
                                     )))
                    {
                        primary = PrettyScaryDictionary.UHH;
                    }   

                    else if (IsMatch (".BUY." + //buy
                                     "|.GUY." //guy
                                     ))
                    {
                        primary = PrettyScaryDictionary.EYE;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.YIH;
                        secondary = PrettyScaryDictionary.OOO;
                    }
                    break;
#endregion case U

#region case V
                case "V":    
                    primary = PrettyScaryDictionary.VIH;
                    break;
#endregion case V

#region case W
                case "W":
                    if (IsMatch ("..W .")) //narrow
                    {
                        ;
                    }

                    else
                    {
                        primary = PrettyScaryDictionary.WIH;
                    }                   
                    break;
#endregion case W

#region case X
                case "X":
                    if (IsMatch ("AUX .")) //aboitaux
                    {
                        ;
                    }   

                    else if (IsMatch (". X..")) //xylophone
                    {
                        primary = PrettyScaryDictionary.ZIH; 
                    }    
        
                    else
                    {
                        primary = PrettyScaryDictionary.KSS;                 
                    }
                    break;
#endregion case X

#region case Y
                case "Y":
                    if (IsMatch (".EY ." + //key
                                "|.AY.." + //maybe
                                "|.EYE." //eye
                                ) ||
                                
                        (UnwantedMatchBypassed ("QUY .") && //!soliloquy 
                         IsMatch (".UY ."))) //buy                                              
                    {
                        ; 
                    }

                    else if (IsMatch (".CYC." + //bicycle
                                     "|.MY.." + //my
                                     "|.HY.." + //hyena
                                     "|FLY.." + //fly
                                     "| BY ." + //by
                                     "| XYL." //xylophone 
                                     ) ||              

                            (UnwantedMatchBypassed ("..Y .") && //!possibility     
                             IsMatch (".TY.."))) //style                             
                    {
                        primary = PrettyScaryDictionary.EYE;
                    }

                    else if (IsMatch ("..Y ." + //ability                                  
                                     "|QUY ." //soliloquy                            
                                     ))
                    {
                        primary = PrettyScaryDictionary.EEE;
                    }

                    else if (IsMatch (" XYS." + //xyster
                                     "|.GYM." + //gym
                                     "|.XYG." //oxygen
                                    ))   
                    {
                        primary = PrettyScaryDictionary.IHH;
                    }   
                           
                    else
                    {
                        primary = PrettyScaryDictionary.YIH;  //yam
                    }
                    break;
#endregion case Y

#region case Z
                case "Z":  
                    primary = PrettyScaryDictionary.ZIH;
                    break;
#endregion case Z
            }            
            currentResults.Add (primary);
            currentResults.Add (secondary);
        }

        //helps cut down on text needed and is easier to understand
        bool IsMatch (string pattern) //SURROUNDINGPHRASE MUST EXIST BEFORE USE.
        {
            string[] analyseDivisions = pattern.Split ('|');

            for (int i = 0; i < analyseDivisions.Length; i++)
            {
                if (analyseDivisions[i].Length != ALGORITHM_PHRASE_SIZE)
                {
                    WrongFormatMatches++;
                }
            }           
            return Regex.IsMatch (surroundingPhrase, pattern);
        }

        //returns true when the unwanted phrase
        bool UnwantedMatchBypassed (string pattern) 
        {
            if (pattern.Length != ALGORITHM_PHRASE_SIZE)
            {
                WrongFormatNonMatches++;
            }
            return !Regex.IsMatch (surroundingPhrase, pattern);
        }
    }
}