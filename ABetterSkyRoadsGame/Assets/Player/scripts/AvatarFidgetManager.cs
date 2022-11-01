using BetterSkyRoads.Flow;
using UnityEngine;

namespace BetterSkyRoads.Player
{
    [RequireComponent(typeof(Animator))]
    public class AvatarFidgetManager : LevelPaceListener
    {
        #region Exposed Editor Parameters
        [Tooltip("The minimum available fidget speed.")]
        [SerializeField] private float minFidgetSpeed;

        [Tooltip("The maximum available fidget speed.")]
        [SerializeField] private float maxFidgetSpeed;

        [Tooltip("The level speed at which the fidget speed is at its maximum value.")]
        [SerializeField] private float peakLevelSpeed;
        #endregion

        #region Constants
        private static readonly string SPEED_PARAM = "speed";
        #endregion

        #region Class Members
        private Animator animator;
        #endregion

        private void Awake() {
            this.animator = GetComponent<Animator>();
            LevelFlow.Instance.GameStartEvent += delegate { animator.enabled = true; };
            LevelFlow.Instance.GameEndEvent += delegate { animator.enabled = false; };
        }

        /// <inheritdoc/>
        protected override void Enhance(float levelSpeed, float _) {
            float pacePercent = Mathf.Min(levelSpeed / peakLevelSpeed, 1);
            float diff = maxFidgetSpeed - minFidgetSpeed;
            float speed = pacePercent * diff + minFidgetSpeed;
            animator.SetFloat(SPEED_PARAM, speed);
        }
    }
}