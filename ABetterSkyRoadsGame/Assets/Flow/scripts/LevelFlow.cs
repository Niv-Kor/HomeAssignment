using BetterSkyRoads.Input;
using BetterSkyRoads.Player;
using BetterSkyRoads.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BetterSkyRoads.Flow
{
    public class LevelFlow : Singleton<LevelFlow>
    {
        #region Exposed Editor Parameters
        [Header("Game Pace")]
        [Tooltip("The initial speed of the level.")]
        [SerializeField] private float initialSpeed;

        [Tooltip("The amount of time [s] needed to wait before the level's speed increases")]
        [SerializeField] private float milestoneInterval;

        [Tooltip("The rate at which the level's speed increases with each milestone.")]
        [SerializeField] private float speedChangePerMilestone;
        #endregion

        #region Class Members
        private float m_currentSpeed;
        #endregion

        #region Properties
        public bool GameRunning { get; private set; }
        public float LevelTime { get; private set; }
        public float CurrentSpeed {
            get => m_currentSpeed;
            set {
                m_currentSpeed = value;
                PaceChangeEvent?.Invoke(CurrentSpeed);
            }
        }
        #endregion

        #region Events
        public event UnityAction<float> PaceChangeEvent;
        public event UnityAction GameStartEvent;
        public event UnityAction GameEndEvent;
        #endregion

        protected override void Awake() {
            base.Awake();
            this.CurrentSpeed = initialSpeed;
            this.GameRunning = false;
        }

        private void Start() {
            GameEndEvent += delegate { GameRunning = false; };
            GameStartEvent += delegate {
                GameRunning = true;
                LevelTime = 0;
            };

            PlayerHitSensor hitSensor = FindObjectOfType<PlayerHitSensor>();
            InputController.Instance.EscapeEvent += ForceEndGame;
            hitSensor.HitEvent += ForceEndGame;
            GameStartEvent?.Invoke();
            StartCoroutine(ChangePace());
        }

        private void FixedUpdate() {
            if (GameRunning) LevelTime += Time.deltaTime;
            else LevelTime = 0;
        }

        /// <summary>
        /// Gradually change the pace of the game.
        /// </summary>
        private IEnumerator ChangePace() {
            while (Application.isPlaying) {
                if (GameRunning) CurrentSpeed += speedChangePerMilestone;
                else CurrentSpeed = 0;

                yield return new WaitForSeconds(milestoneInterval);
            }
        }

        /// <summary>
        /// Cause a gameover.
        /// </summary>
        public void ForceEndGame() => GameEndEvent?.Invoke();
    }
}