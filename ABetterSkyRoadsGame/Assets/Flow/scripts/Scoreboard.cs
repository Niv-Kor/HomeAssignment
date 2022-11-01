using BetterSkyRoads.Util;
using UnityEngine.Events;

namespace BetterSkyRoads.Flow
{
    public class Scoreboard : Singleton<Scoreboard>
    {
        #region Properties
        protected override bool GuardEntireObject => true;
        public int Highscore { get; private set; }
        #endregion

        #region Events
        public event UnityAction<int, bool> RegistrationEvent;
        #endregion

        protected override void Awake() {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Register the latest score.
        /// </summary>
        /// <param name="score">The last received score</param>
        public void RegisterScore(int score) {
            bool beatHighscore = score > Highscore;
            if (beatHighscore) Highscore = score;
            RegistrationEvent?.Invoke(score, beatHighscore);
        }
    }
}