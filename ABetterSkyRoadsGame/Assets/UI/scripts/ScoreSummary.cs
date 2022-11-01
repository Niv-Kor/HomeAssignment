using BetterSkyRoads.Flow;
using TMPro;
using UnityEngine;

namespace BetterSkyRoads.UI
{
    public class ScoreSummary : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Header("References")]
        [Tooltip("The label that indicates the latest achieved score.")]
        [SerializeField] private TextMeshProUGUI latestScoreLabel;

        [Tooltip("The label that indicates the highest achieved score until now.")]
        [SerializeField] private TextMeshProUGUI highestScoreLabel;
        #endregion

        private void Start() {
            Scoreboard scoreboard = Scoreboard.Instance;
            scoreboard.RegistrationEvent += delegate (int newScore, bool _) {
                latestScoreLabel.text = newScore.ToString();
                highestScoreLabel.text = scoreboard.Highscore.ToString();
            };
        }
    }
}