﻿using System.Text;

namespace SETextToSpeechMod
{
    class OoptionalDebugger
    {
        static void Main()
        {
            MessageEventHandler handlers = new MessageEventHandler();
            Encoding test_encode = Encoding.Unicode;
            string test_string = "[ very";
            string upper = test_string.ToUpper();
            byte[] test_array = test_encode.GetBytes (upper);
            handlers.OnReceivedPacket (test_array);

            while (true)
            {
                for (int i = 0; i < handlers.speeches.Count; i++) //performance danger.
                {
                    handlers.speeches[i].Load();

                    if (handlers.speeches[i].finished == true)
                    {
                        handlers.speeches.RemoveAt (i);
                        i--; //the for loop is about to increment so i dont want to skip a speech.
                    }
                }
            }
        }
    }
}
