using UnityEngine;

namespace BetterSkyRoads.Util.Audio
{
    public struct TuneSettings
    {
        public string Name;
        public AudioClip Clip;
        public float Volume;
        public float Pitch;
        public float Delay;
        public bool Loop;
        public Genre Genre;
    }
}