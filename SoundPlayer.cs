﻿using System.Collections.Generic;

using Sandbox.ModAPI;
using Sandbox.Game.Entities;

using VRage.ModAPI;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRageMath;

namespace SETextToSpeechMod
{
    static class SoundPlayer //i only need one emitter per player.
    {
        static MyEntity3DSoundEmitter TTSEmitter;
        const float DEFAULT_VOLUME = 0.6f;

        public static void InitialiseEmitter()
        {
            IMyEntity emitterEntity = new MyEntity() as IMyEntity; //couldnt instantiate MyEntity so had to use its cast.
            TTSEmitter = new MyEntity3DSoundEmitter (emitterEntity as MyEntity);
            TTSEmitter.CustomMaxDistance = 500.0f; //since emitter position updates every interval, the distance should be large.
            TTSEmitter.SourceChannels = 1;
            TTSEmitter.Force3D = true;
            TTSEmitter.CustomVolume = DEFAULT_VOLUME; //my sounds are already clipping; people dont want it that loud.
        }

        public static void UpdatePosition (List <IMyPlayer> players)
        {
            for (int i = 0; i < players.Count; i++) //performance danger
            {
                if (players[i].SteamUserId == MyAPIGateway.Multiplayer.MyId) //finds the client in the pool of players.
                {
                    Vector3D pos3D = players[i].GetPosition(); //some formatting is required
                    Vector3I pos3I = new Vector3I (pos3D);
                    Vector3 pos3 = new Vector3 (pos3I);
                    TTSEmitter.SetPosition (pos3);
                }
            }
        }

        public static void PlayClip (bool debugging, string clip)
        {
            if (debugging == false)
            {
                MySoundPair sound = new MySoundPair (clip);
                TTSEmitter.PlaySound (sound, false, false, false, true, false); //this overload enables sound in realistic mode.
            }
        }
    }
}
