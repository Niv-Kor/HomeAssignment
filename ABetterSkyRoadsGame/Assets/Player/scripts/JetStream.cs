using BetterSkyRoads.Flow;
using UnityEngine;

namespace BetterSkyRoads.Player
{
    [RequireComponent(typeof(ParticleSystem))]
    public class JetStream : LevelPaceListener
    {
        #region Class Members
        private ParticleSystem partSys;
        #endregion

        private void Awake() {
            this.partSys = GetComponent<ParticleSystem>();
            LevelFlow.Instance.GameStartEvent += delegate { partSys.Play(); };
            LevelFlow.Instance.GameEndEvent += delegate { partSys.Stop(); };
        }

        /// <inheritdoc/>
        protected override void Enhance(float _, float levelTime) {
            var partSysMain = partSys.main;
            partSysMain.simulationSpeed = levelTime;
        }
    }
}