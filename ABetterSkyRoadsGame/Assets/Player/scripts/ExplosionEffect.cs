using BetterSkyRoads.Util.Audio;
using UnityEngine;

namespace BetterSkyRoads.Player
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ExplosionEffect : MonoBehaviour
    {
        #region Constants
        private static readonly string TUNE_NAME = "explosion";
        #endregion

        #region Class Members
        private bool exploded;
        private Jukebox jukebox;
        #endregion

        private void Awake() {
            this.jukebox = GetComponent<Jukebox>();
        }

        private void Start() {
            ParticleSystem partSys = GetComponent<ParticleSystem>();
            PlayerHitSensor hitSensor = GetComponentInParent<PlayerHitSensor>();
            hitSensor.HitEvent += delegate {
                if (exploded) return;

                jukebox?.Play(TUNE_NAME);
                partSys.Play();
                exploded = true;
            };
        }
    }
}