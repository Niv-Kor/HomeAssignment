using BetterSkyRoads.Util.Audio;
using UnityEngine;

namespace BetterSkyRoads.UI
{
    public class UIButton : MonoBehaviour
    {
        #region Constants
        private static readonly string HOVER_TUNE = "hover";
        #endregion

        #region Class Members
        private Jukebox jukebox;
        #endregion

        private void Awake() {
            this.jukebox = GetComponent<Jukebox>();
        }

        /// <summary>
        /// Play an hover sound for when the pointer enters the button.
        /// </summary>
        public void PlayHoverSound() => jukebox?.Play(HOVER_TUNE);
    }
}