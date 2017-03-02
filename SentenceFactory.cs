﻿using System;
using System.Collections.Generic;

namespace SETextToSpeechMod
{   
    public abstract class SentenceFactory : VoiceTemplate
    {   
        protected const string SPACE = "SPACE";    

        //voice template
        public virtual string Name { get { return "SentenceFactory";} }
        public virtual string FileID { get; }
        public virtual int SpaceSize { get; }
        public virtual int ClipLength { get; }
        public virtual int SyllableSize { get; }        

        //pitch        
        protected virtual int smallSize { get { return 6; } }
        protected virtual int mediumSize { get { return 50; } }
        protected virtual int largeSize { get { return OutputManager.MAX_LETTERS; } }
        protected int[] allSizes;

        protected virtual int[][] smallIntonationPatterns { get; } //an intonation pattern should be designed to loop on itself. it can be any size.
        protected virtual int[][] mediumIntonationPatterns { get; }
        protected virtual int[][] largeIntonationPatterns { get; }
        protected int[][][] allPitchOptions;

        protected virtual int voiceRange { get; }

        //external feedback        
        public bool FinishedPlaying {get; protected set;}

        //index
        protected int letterIndex; 
        protected int optionsIndex;                      
        protected int arraysIndex; 
        
        //loading data            
        bool Loading;
        protected string sentence;
        bool previousWasSpace;
        int syllableMeasure;          
        protected int[] intonationArrayChosen;

        //play data
        int currentTick;

        public IList <string> Results
        {
            get
            {
                return resultsField.AsReadOnly();
            }
        }
        List <string> resultsField;

        //objects     
        public Pronunciation Pronunciation { get; private set; }   
        protected List <TimelineClip> timeline = new List <TimelineClip>();      
        protected Random rng = new Random();
        private SoundPlayer soundPlayerRef; 

        public SentenceFactory (SoundPlayer inputEmitter)
        {            
            Loading = false;
            FinishedPlaying = true;
            Pronunciation = new Pronunciation();
            resultsField = new List <string>();
            timeline.Capacity = OutputManager.MAX_LETTERS; //lists resize constantly when filling. better to know its limit and prevent that to increase performance;            
            soundPlayerRef = inputEmitter; //the reason im using a pointer is there is no need for a SoundPlayer per SentenceFactory.
                                                             
            allSizes = new int[]
            {
                smallSize,
                mediumSize,
                largeSize,
            };
            
            allPitchOptions = new int[][][]     
            {
                smallIntonationPatterns,
                mediumIntonationPatterns,
                largeIntonationPatterns,
            };
        }

        public void FactoryReset (string inputSentence)
        {
            Loading = true;
            FinishedPlaying = false;           

            letterIndex = 0; 
            optionsIndex = 0;
            arraysIndex = 0;

            sentence = inputSentence;
            
            previousWasSpace = false;
            syllableMeasure = 0;
            intonationArrayChosen = null;

            currentTick = 0;
            resultsField.Clear();

            Pronunciation.FactoryReset();
            timeline.Clear();            
        }

        //this function will extract what phonemes it can from the sentence and save performance by taking its sweet time.
        public void Run()
        {    
            if (Loading == true)
            {   
                if (letterIndex < sentence.Length)
                {                                    
                    AddPhonemes(); 
                    letterIndex++;
                }   

                else
                {
                    IterateIntonationsOnTimeLine();
                    Loading = false;
                }        
            }

            else 
            {
                FinishedPlaying = soundPlayerRef.PlaySentence (timeline, currentTick);
                currentTick++;
            }    
        } 
        
        private void AddPhonemes()
        {   
            resultsField = Pronunciation.GetLettersPronunciation (sentence, letterIndex);

            for (int i = 0; i < resultsField.Count; i++)
            {
                if (resultsField[i] != "") //AdjacentEvaluation() can return an empty string sometimes.
                {
                    if (resultsField[i] != " ")
                    {                                                   
                        string soundChoice = resultsField[i] + FileID;
                        AddToTimeline (soundChoice);                                            

                        if (syllableMeasure >= SyllableSize - 1)
                        {                            
                            IncrementSyllables();                       
                        }   

                        else
                        {
                            previousWasSpace = false;
                            syllableMeasure++;
                        }
                    }

                    else
                    {
                        if (previousWasSpace == false)
                        {
                            IncrementSyllables();
                        }       
                        
                        else
                        {
                            previousWasSpace = false;
                        }                 
                    }
                }
            }
        }

        private void AddToTimeline (string inputSound)
        {
            int startPoint = 0;                                                                      
                        
            if (timeline.Count != 0)
            {
                startPoint = timeline[timeline.Count - 1].StartPoint + ClipLength;         
            }  
            timeline.Add (new TimelineClip (startPoint, inputSound));
        }

        private void IncrementSyllables()
        {
            previousWasSpace = true; //pronunciation class inserts spaces for low energy letters. i dont want double spaces so thats the purpose of this var.        {
            AddToTimeline (SPACE);
            syllableMeasure = 0;
        } 

        protected virtual void IterateIntonationsOnTimeLine()
        {
            for (int i = 0; i < timeline.Count; i++)
            {
                AddIntonations (i);
            }
        }

        //this has been separated from the initial timeline construction because it must know the total number of phonemes first.
        protected virtual void AddIntonations (int timelineIndex)
        {        
            TimelineClip previousClip = timeline[timelineIndex];
            timeline[timelineIndex] = new TimelineClip (previousClip.StartPoint, previousClip.ClipsSound);
        }         

        protected void ChoosePitchPattern (int sizeIndex)
        {
            int currentLimit = allPitchOptions[sizeIndex].Length;
            int randomPattern = rng.Next (currentLimit);
            intonationArrayChosen = allPitchOptions[sizeIndex][randomPattern];
        }
    }

    public struct TimelineClip
    {
        public int StartPoint { get; }
        public string ClipsSound { get; }

        internal TimelineClip (int inputPoint, string inputSound)
        {
            StartPoint = inputPoint;
            ClipsSound = inputSound;
        }
    } 
}
