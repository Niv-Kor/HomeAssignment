using UnityEngine;

namespace BetterSkyRoads.Flow
{
    public abstract class LevelPaceListener : MonoBehaviour
    {
        protected virtual void Start() {
            LevelFlow.Instance.PaceChangeEvent += delegate (float speed) {
                Enhance(speed, LevelFlow.Instance.LevelTime);
            };
        }

        /// <summary>
        /// Enhance the listener based on the new level flow values.
        /// </summary>
        /// <param name="levelSpeed">The new speed of the level</param>
        /// <param name="levelTime">The time [s] passed since the beginning of the game</param>
        protected abstract void Enhance(float levelSpeed, float levelTime);
    }
}