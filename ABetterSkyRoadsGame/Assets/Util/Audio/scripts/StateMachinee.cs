using System.Collections.Generic;
using UnityEngine;

namespace BetterSkyRoads.Util.Audio
{
    public abstract class StateMachine : MonoBehaviour
    {
        #region Class Members
        protected List<string> states;
        #endregion

        protected virtual void Awake() {
            this.states = RetrieveStates();
        }

        /// <returns>A list of the available states in the machine.</returns>
        protected abstract List<string> RetrieveStates();

        /// <summary>
        /// Activate or deactivate a state.
        /// </summary>
        /// <param name="state">The state to activate or deactivate</param>
        /// <param name="flag">True to activate the state or false to stop it</param>
        public abstract void Activate(string state, bool flag);

        /// <summary>
        /// Activate a state.
        /// </summary>
        /// <param name="state">The state to activate</param>
        public virtual void Activate(string state) => Activate(state, true);

        /// <param name="state">The state to check</param>
        /// <returns>True if the state's parameter is currently on</returns>
        public abstract bool IsAtState(string state);

        /// <returns>True if no state is currently activated.</returns>
        protected virtual bool IsIdling() {
            foreach (string state in states)
                if (IsAtState(state)) return false;

            return true;
        }

        /// <summary>
        /// Cancel all parameters and return to Idle state.
        /// </summary>
        public virtual void Idlize() {
            foreach (string state in states) Activate(state, false);
        }
    }
}