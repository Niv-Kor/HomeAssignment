using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterSkyRoads.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        /// <summary>
        /// Start the game.
        /// Activate when the "Play" button is clicked.
        /// </summary>
        public void StartGame() => SceneManager.LoadScene(1);
    }
}