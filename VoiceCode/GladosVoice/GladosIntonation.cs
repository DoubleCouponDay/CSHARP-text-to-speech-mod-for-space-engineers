﻿using SETextToSpeechMod.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETextToSpeechMod.VoiceCode.GladosVoice
{
    class GladosIntonation// : Intonation
    {
        //        protected virtual int smallSize { get { return 6; } }
        //protected virtual int mediumSize { get { return 50; } }
        //protected virtual int largeSize { get { return OutputManager.MAX_LETTERS; } }
        //protected int[] allSizes;

        //protected virtual int[][] smallIntonationPatterns { get; } //an intonation pattern should be designed to loop on itself. it can be any size.
        //protected virtual int[][] mediumIntonationPatterns { get; }
        //protected virtual int[][] largeIntonationPatterns { get; }
        //protected int[][][] allPitchOptions;


            //allSizes = new int[]
            //{
            //    smallSize,
            //    mediumSize,
            //    largeSize,
            //};
            
            //allPitchOptions = new int[][][]     
            //{
            //    smallIntonationPatterns,
            //    mediumIntonationPatterns,
            //    largeIntonationPatterns,
            //};

        //protected virtual int voiceRange { get; }



        //protected void ChoosePitchPattern (int sizeIndex)
        //{
        //    int currentLimit = allPitchOptions[sizeIndex].Length;
        //    int randomPattern = rng.Next (currentLimit);
        //    intonationArrayChosen = allPitchOptions[sizeIndex][randomPattern];
        //}

        //protected override int[][] smallIntonationPatterns { get { return smallOptions; } }
        //protected override int[][] mediumIntonationPatterns { get { return mediumOptions; } }
        //protected override int[][] largeIntonationPatterns { get { return largeOptions; } }

        //protected override int voiceRange { get { return 11; } }

        //readonly int[][] smallOptions = new int[][] 
        //{
        //    new int[] { 0, 1, 2, },          
        //};

        //readonly int[][] mediumOptions = new int[][] 
        //{
        //    new int[] { 0, 1, 2, },          
        //};

        //readonly int[][] largeOptions = new int[][] 
        //{
        //    new int[] { 0, 1, 2, },          
        //};

        //public GLADOSVoice (SoundPlayer inputEmitter) : base (inputEmitter, new GladosIntonation()){}

        //void AddIntonations (int timelineIndex)
        //{
        //    string intonation = " ";

        //    if (timeline[timelineIndex].ClipsSound != SPACE)
        //    {                        
        //        if (intonationArrayChosen == null)
        //        {
        //            for (int u = 0; u < allSizes.Length; u++)
        //            {
        //                if (timeline.Count <= allSizes[u])
        //                {
        //                    //ChoosePitchPattern (u);
        //                }

        //                else if (u == allSizes.Length - 1)
        //                {
        //                    //ChoosePitchPattern (allSizes.Length - 1); //assuming the array is ordered from largest to smallest!
        //                }
        //            }
        //        }

        //        if (arraysIndex >= intonationArrayChosen.Length)
        //        {
        //            arraysIndex = 0;
        //        }        
                
        //        if (intonationArrayChosen[arraysIndex] > voiceRange)
        //        {
        //            intonationArrayChosen[arraysIndex] = voiceRange;
        //        }                
        //        intonation += intonationArrayChosen[arraysIndex];
        //        timeline[timelineIndex] = new TimelineClip (timeline[timelineIndex].StartPoint, timeline[timelineIndex].ClipsSound + intonation);
        //        arraysIndex++;
        //    }            
        //}
    }
}
