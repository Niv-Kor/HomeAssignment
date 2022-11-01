using BetterSkyRoads.Flow;
using TMPro;
using UnityEngine;

namespace BetterSkyRoads.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Score : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("The track's rear wall, for asteroids detection.")]
        [SerializeField] private RearTrackWall rearTrackWall;

        [Tooltip("The score received after a single asteroid dodge.")]
        [SerializeField] private int scorePerAsteroid;
        #endregion

        #region Class Members
        private TextMeshProUGUI textMesh;
        private int m_currentScore;
        #endregion

        #region Properties
        public int CurrentScore {
            get => m_currentScore;
            set {
                m_currentScore = value;
                textMesh.text = CurrentScore.ToString();
            }
        }
        #endregion

        private void Awake() {
            this.textMesh = GetComponent<TextMeshProUGUI>();
            this.CurrentScore = 0;
        }

        private void Start() {
            LevelFlow.Instance.GameStartEvent += delegate { CurrentScore = 0; };
            LevelFlow.Instance.GameEndEvent += delegate {
                Scoreboard.Instance.RegisterScore(CurrentScore);
            };

            //collect
            rearTrackWall.ScoreEvent += delegate {
                if (LevelFlow.Instance.GameRunning)
                    CurrentScore += scorePerAsteroid;
            };
        }
    }
}