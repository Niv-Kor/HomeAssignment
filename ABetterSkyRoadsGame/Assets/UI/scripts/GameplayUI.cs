using BetterSkyRoads.Flow;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterSkyRoads.UI
{
    [RequireComponent(typeof(Animator))]
    public class GameplayUI : MonoBehaviour
    {
        #region Constants
        private static readonly string INTRO_PARAM = "intro";
        private static readonly string OUTRO_PARAM = "outro";
        #endregion

        #region Class Members
        private Animator animator;
        #endregion

        private void Awake() {
            this.animator = GetComponent<Animator>();
        }

        private void Start() {
            LevelFlow.Instance.GameEndEvent += delegate { animator.SetTrigger(INTRO_PARAM); };
        }

        /// <summary>
        /// Back to the main menu.
        /// Activate when the "Restart" button is clicked.
        /// </summary>
        public void BackToMenu() {
            animator.SetTrigger(OUTRO_PARAM);
            SceneManager.LoadScene(0);
        }
    }
}