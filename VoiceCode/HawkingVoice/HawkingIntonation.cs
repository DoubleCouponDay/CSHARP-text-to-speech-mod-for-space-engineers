﻿using SETextToSpeechMod.LookUpTables;
using SETextToSpeechMod.Processing;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SETextToSpeechMod.VoiceCode.HawkingVoice
{
    class HawkingIntonation : Intonation
    {
        const string NORMAL_PHONEME = " 0";
        const string SENTENCE_END_ID = "-E";
        const string QUESTION_PHONEME_ID = "-Q";
        const string EXCLAMATION_PHONEME_ID = "-!";
        const string COMMA_PHONEME_ID = "-,";          

        const string QUESTION_PATTERN = @"....\?|...\? ";
        const string COMMA_PATTERN = @"....,|..., ";
        const string EXCLAMATION_PATTERN = "....!|...! ";
        const string ENDING_PATTERN = @"....\.|...\. ";

        public override string VoiceId { get { return "-H"; } }        
        

        private static readonly Dictionary <string, string> PATTERN_ASSOCIATIONS = new Dictionary <string, string>()
        {
            {QUESTION_PATTERN, QUESTION_PHONEME_ID},
            {COMMA_PATTERN, COMMA_PHONEME_ID},
            {EXCLAMATION_PATTERN, EXCLAMATION_PHONEME_ID},
            {ENDING_PATTERN, SENTENCE_END_ID}
        };

        protected override string DerivedIntonationChoice (string phoneme, string surroundingPhrase, bool sentenceEndInPhrase)
        {
            string intonation = phoneme + VoiceId + NORMAL_PHONEME;

            for (int i = 0; i < Vowels.TABLE.Count; i++)
            {
                if (Vowels.TABLE[i].Equals (phoneme)) //meshes well with AdjacentEvaluation since a there will only be one vowel phoneme per word ending.
                {
                    foreach (KeyValuePair <string, string> item in PATTERN_ASSOCIATIONS)
                    {
                        if (Regex.IsMatch (surroundingPhrase, item.Key))
                        {
                            intonation = intonation.Replace (NORMAL_PHONEME, item.Value);
                            break;
                        }

                        else if (sentenceEndInPhrase)
                        {
                            intonation = string.Concat (intonation, SENTENCE_END_ID);
                            break;
                        }
                    }  
                    break;              
                }
            }
            return intonation;
        }
    }
}
