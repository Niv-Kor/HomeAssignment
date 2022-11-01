using BetterSkyRoads.Util;
using UnityEngine;
using UnityEngine.Events;

namespace BetterSkyRoads.Input
{
    public class InputController : Singleton<InputController>
    {
        #region Class Members
        private Controls controls;
        #endregion

        #region Properties
        public Vector2 MovementVector => controls.Player.Movement.ReadValue<Vector2>();
        #endregion

        #region Events
        public event UnityAction EscapeEvent;
        #endregion

        protected override void Awake() {
            base.Awake();

            this.controls = new Controls();
            controls.Enable();
        }

        private void Start() {
            controls.UI.Quit.performed += delegate { EscapeEvent?.Invoke(); };
        }
    }
}