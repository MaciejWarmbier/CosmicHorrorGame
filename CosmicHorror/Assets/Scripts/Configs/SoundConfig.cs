using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundConfig", menuName = "ScriptableObjects/SoundConfig", order = 1)]
public class SoundConfig : ScriptableObject
{
    public List<AucioData> audioData;

    public AudioClip GetAudioClip(AudioEnum audioType)
    {
        return audioData.FirstOrDefault(x => x.Enum == audioType).audioClip;
    }

    [Serializable]
    public class AucioData
    {
        public AudioEnum Enum;
        public AudioClip audioClip;
    }

    public enum AudioEnum
    {
        None = 0,
        Hmmm = 1,
        whisper = 2,
        bubble = 3,
        bird = 4,
        string_noise = 5,
        iron = 6, 
        ambient1 = 7,
        ambient2 = 8,
        door_closing = 9,
        tentacle_hit = 10,
        wet_footsteps = 11,
        wet_footsteps2 = 12,
        teleport = 13,
        moving = 14,
        enemy_machete = 15,
        machete_player = 16,
        shot = 17,
        reload = 18,
        empty_gun = 19,
        pickup = 20,
        aaah_hmmm = 21,
        cave_sound = 22,
        scrattching = 23,
    }
}
