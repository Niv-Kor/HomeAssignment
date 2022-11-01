using System;
using UnityEngine;
using UnityEngine.Events;

namespace BetterSkyRoads.Util.Audio
{
    [Serializable]
    public class Tune
    {
        #region Exposed Editor Parameters
        [Tooltip("The name of the tune.")]
        [SerializeField] private string name;

        [Tooltip("The audio file to play.")]
        [SerializeField] private AudioClip clip;

        [Tooltip("The volume of the sound [0:1].")]
        [SerializeField] private float volume = .5f;

        [Tooltip("The pitch value of the sound (1 is natural).")]
        [SerializeField] private float pitch = 1;

        [Tooltip("A delay to add before the sound plays [s].")]
        [SerializeField] private float delay = 0;

        [Tooltip("The cooldown time within which the tune cannot be played twice.")]
        [SerializeField] private float cooldown = 0;

        [Tooltip("True to fade the tune in gradually.")]
        [SerializeField] private bool fadeIn = false;

        [Tooltip("True to play this tune infinitely.")]
        [SerializeField] private bool loop = false;

        [Tooltip("True to play this automatically as soon as it's created.")]
        [SerializeField] private bool playOnAwake = false;

        [Tooltip("True to set the volume of the tune relative to the distance of its object from the scene anchor.")]
        [SerializeField] private bool relyOnDistance = false;

        [Tooltip("True to export this tune to an extenral source.\n"
               + "Use this flag for tunes that are ought to continue playing "
               + "even after their object is destroyed.\n"
               + "Exporting too many tunes might result in performance issues.")]
        [SerializeField] private bool exportAsExternal = false;

        [Tooltip("The mixer to which this tune belongs.")]
        [SerializeField] private Genre genre;
        #endregion

        #region Class Members
        private AudioSource m_source;
        #endregion

        #region Events
        public event UnityAction StopEvent;
        #endregion

        #region Properties
        public string Name => name;
        public float Delay => delay;
        public float Cooldown => cooldown;
        public bool FadeIn => fadeIn;
        public bool PlayOnAwake => playOnAwake;
        public bool RelateOnDistance => relyOnDistance;
        public bool IsExportable => exportAsExternal;
        public bool IsExternal { get; private set; }
        public Jukebox OrganicParent { get; set; }
        public Genre Genre { get => genre; }
        public AudioClip Clip { get => clip; }
        public Coroutine Coroutine { get; set; }
        public AudioSource Source {
            get => m_source;
            set {
                m_source = value;
                m_source.clip = clip;
                m_source.volume = volume;
                m_source.pitch = pitch;
            }
        }

        public bool IsLoop {
            get => loop;
            set { loop = value; }
        }


        public float Volume {
            get => volume;
            set {
                if (Source != null) {
                    Source.volume = value;
                    volume = value;
                }
            }
        }

        public float Pitch {
            get => pitch;
            set {
                if (Source != null) {
                    Source.pitch = value;
                    pitch = value;
                }
            }
        }

        public float Duration {
            get {
                if (clip == null) return 0;
                else return clip.length;
            }
        }

        public bool IsPlaying {
            get {
                if (Source == null) return false;
                else return Source.isPlaying;
            }
        }
        #endregion

        public Tune(Tune tune) {
            this.name = tune.name;
            this.clip = tune.clip;
            this.volume = tune.volume;
            this.pitch = tune.pitch;
            this.delay = tune.delay;
            this.cooldown = tune.cooldown;
            this.fadeIn = tune.fadeIn;
            this.loop = tune.loop;
            this.playOnAwake = tune.playOnAwake;
            this.relyOnDistance = tune.relyOnDistance;
            this.exportAsExternal = tune.exportAsExternal;
            this.genre = tune.genre;
            this.OrganicParent = OrganicParent;
        }

        /// <summary>
        /// Externalize a tune (make it orphan).
        /// </summary>
        /// <param name="parent">The organic parent of the tune</param>
        public void Externalize(Jukebox parent) {
            IsExternal = true;
            OrganicParent = parent;
        }

        /// <summary>
        /// Stop the tune.
        /// </summary>
        public void Stop() {
            Source.Stop();
            StopEvent?.Invoke();
        }

        /// <param name="vol">The new volume of the tune</param>
        public void SetVolume(float vol) {
            Source.volume = vol;
            volume = vol;
        }

        /// <param name="ptch">The new pitch of the tune</param>
        public void SetPitch(float ptch) {
            Source.pitch = ptch;
            pitch = ptch;
        }
    }
}