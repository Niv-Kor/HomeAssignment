using BetterSkyRoads.Flow;
using UnityEngine;

namespace BetterSkyRoads.Player
{
    [RequireComponent(typeof(ParticleSystem))]
    public class WindEffect : LevelPaceListener
    {
        #region Exposed Editor Parameters
        [SerializeField] private float simulationSpeedMultiplier;
        #endregion

        #region Class Members
        private ParticleSystem partSys;
        private float initialSimulationSpeed;
        #endregion

        private void Awake() {
            this.partSys = GetComponent<ParticleSystem>();
            this.initialSimulationSpeed = partSys.main.simulationSpeed;

            LevelFlow.Instance.GameStartEvent += delegate { partSys.Play(); };
            LevelFlow.Instance.GameEndEvent += delegate { partSys.Stop(); };
        }

        /// <inheritdoc/>
        protected override void Enhance(float _, float levelTime) {
            var partSysMain = partSys.main;
            partSysMain.simulationSpeed = initialSimulationSpeed + levelTime / 2;
        }
    }
}