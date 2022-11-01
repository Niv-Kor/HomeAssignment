using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace BetterSkyRoads.Util.Audio
{
    public class Jukebox : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("A list containing all of the object's tunes.")]
        [SerializeField] private List<Tune> tunes;

        [Tooltip("True to destroy tunes once they are done playing.")]
        [SerializeField] private bool destroyStoppedTunes = false;
        #endregion

        #region Constants
        private static readonly string PARENT_NAME = "Audio";
        #endregion

        #region Class Members
        private TunesLimiter limiter;
        private GameObject audioParent;
        private DisposableAudioManager disposableAudio;
        #endregion

        #region Properties
        public List<Tune> Tunes { get { return tunes; } }
        public List<string> TuneNames {
            get {
                List<string> names = new List<string>();
                foreach (Tune tune in tunes) names.Add(tune.Name);
                return names;
            }
        }
        #endregion

        private void Awake() {
            this.limiter = TunesLimiter.Instance;
            this.disposableAudio = DisposableAudioManager.Instance;
            this.audioParent = new GameObject(PARENT_NAME);
            audioParent.transform.SetParent(transform);
            audioParent.transform.localPosition = Vector3.zero;

            //create an audio source component for each tune
            foreach (Tune tune in tunes) BakeTune(tune);
        }

        private void OnDestroy() {
            foreach (Tune tune in tunes)
                if (!tune.IsExportable) Stop(tune);
        }

        /// <summary>
        /// Establish the tune's connection with the jukebox.
        /// </summary>
        /// <param name="tune">The tune to bake</param>
        private void BakeTune(Tune tune) {
            AudioSource audioSource = audioParent.AddComponent<AudioSource>();
            tune.Source = audioSource;
            audioSource.loop = tune.IsLoop;
            audioSource.playOnAwake = false;
            AudioMixerGroup mixerGroup = VolumeController.Instance.GetGenreGroup(tune.Genre);
            audioSource.outputAudioMixerGroup = mixerGroup;
            limiter.Subscribe(tune);
            if (!tune.IsExternal) tune.OrganicParent = this;

            //auto play the tune
            if (tune.PlayOnAwake) Play(tune);
        }

        /// <summary>
        /// Add a tune to the jukebox.
        /// </summary>
        /// <param name="tune">The tune to add</param>
        public void Add(Tune tune) {
            if (tunes == null) tunes = new List<Tune>();

            tunes.Add(tune);
            BakeTune(tune);
        }

        /// <summary>
        /// Remove a tune from the jukebox.
        /// </summary>
        /// <param name="tune">The tune to remove</param>
        public void Remove(Tune tune) {
            if (tune != null) {
                limiter.Unsubscribe(tune);
                Destroy(tune.Source);
                tunes.Remove(tune);
            }
        }

        /// <see cref="Remove(Tune)"/>
        /// <param name="tuneName"></param>
        public void Remove(string tuneName) {
            Tune tune = tunes.Find(x => x.Name == tuneName);
            Remove(tune);
        }

        /// <summary>
        /// Get a tune with a specific name.
        /// If there are multiple tunes that use the same name,
        /// this method returns one of them randomly.
        /// </summary>
        /// <param name="name">The name of the tune</param>
        /// <returns>
        /// A random tune that consists of the specified name,
        /// or null if nont exist.
        /// </returns>
        public Tune Get(string name) {
            List<Tune> list = (from Tune tune in tunes
                               where tune.Name == name
                               select tune).ToList();

            if (list.Count > 0) {
                int index = Random.Range(0, list.Count);
                return list[index];
            }
            else return null;
        }

        /// <summary>
        /// Play a random tune.
        /// </summary>
        public void PlayRandom() {
            int index = Random.Range(0, Tunes.Count);
            Tune tune = Tunes[index];
            Play(tune);
        }

        /// <summary>
        /// Play a tune.
        /// </summary>
        /// <param name="tune">The tune to play</param>
        public void Play(Tune tune) {
            if (tune != null) {
                //move the tune to an external source
                if (tune.IsExportable && !tune.IsExternal) {
                    disposableAudio.ExportTune(tune, this);
                    return;
                }

                if (limiter.GetPermission(tune)) {
                    if (tune.FadeIn) StartCoroutine(FadeTuneVolumeIn(tune));
                    tune.Source.PlayDelayed(tune.Delay);

                    //schedule the tune's stop trigger
                    if (!tune.IsLoop) {
                        float time = tune.Delay + tune.Duration;
                        tune.Coroutine = StartCoroutine(StopAfterSeconds(tune, time));
                    }
                }
            }
        }

        /// <summary>
        /// Gradually fade a tune's volume in.
        /// </summary>
        /// <param name="tune">The tune to fade in</param>
        /// <param name="time">The time [s] it takes to reach the target volumne</param>
        private IEnumerator FadeTuneVolumeIn(Tune tune, float time = .5f) {
            float timer = 0;
            float targetVol = tune.Volume;
            tune.Source.volume = 0;

            while (timer <= time) {
                timer += Time.deltaTime;
                float vol = Mathf.Lerp(0, targetVol, timer / time);
                tune.SetVolume(vol);

                yield return null;
            }
        }

        /// <see cref="Play(string)"/>
        /// <param name="name">The tune's name</param>
        public void Play(string name) { Play(Get(name)); }

        /// <summary>
        /// Stop a tune.
        /// </summary>
        /// <param name="name">The tune's name</param>
        public void Stop(Tune tune) {
            if (tune == null) return;

            if (tune.Coroutine != null) StopCoroutine(tune.Coroutine);
            tune.Coroutine = null;
            tune.Stop();
        }

        /// <see cref="Stop(string)"/>
        /// <param name="name">The tune's name</param>
        public void Stop(string name) { Stop(Get(name)); }

        /// <summary>
        /// Stop the tune after a fixed amount of seconds.
        /// </summary>
        /// <param name="seconds">Amount of seconds after which the tune is stopped</param>
        private IEnumerator StopAfterSeconds(Tune tune, float seconds) {
            yield return new WaitForSeconds(seconds);
            tune.Stop();
            if (destroyStoppedTunes) Remove(tune);
        }
    }
}