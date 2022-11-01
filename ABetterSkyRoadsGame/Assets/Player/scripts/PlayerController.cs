using BetterSkyRoads.Flow;
using BetterSkyRoads.Input;
using BetterSkyRoads.Util.Audio;
using System.Collections;
using UnityEngine;

namespace BetterSkyRoads.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerHitSensor))]
    public class PlayerController : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("The spaceship's horizontal speed (left and right).")]
        [SerializeField] private float turnSpeed;

        [Tooltip("The angle at which the spaceship tilts upon turning.")]
        [SerializeField] [Range(0, 90)] private float tiltAngle;

        [Tooltip("The speed at which the spaceship tilts.")]
        [SerializeField] private float tiltSpeed;

        [Tooltip("The force that's applied on the spaceship upon hit.")]
        [SerializeField] private float destructionForce;
        #endregion

        #region Constants
        private static readonly string FLIGHT_TUNE = "flight";
        #endregion

        #region Class Members
        private InputController input;
        private Jukebox jukebox;
        private Rigidbody rigidBody;
        private float nextTiltAngle;
        private bool controllable;
        #endregion

        private void Awake() {
            this.controllable = false;
            this.input = InputController.Instance;
            this.rigidBody = GetComponent<Rigidbody>();
            this.jukebox = GetComponent<Jukebox>();
        }

        private void Start() {
            PlayerHitSensor hitSensor = GetComponent<PlayerHitSensor>();
            LevelFlow.Instance.GameStartEvent += delegate {
                controllable = true;
                jukebox?.Play(FLIGHT_TUNE);
            };

            LevelFlow.Instance.GameEndEvent += delegate {
                controllable = false;
                jukebox?.Stop(FLIGHT_TUNE);
            };

            hitSensor.HitEvent += delegate {
                Vector3 destructionVec = Vector3.forward * destructionForce;
                rigidBody.constraints = RigidbodyConstraints.None;
                rigidBody.AddForce(destructionVec);
            };

            StartCoroutine(Tilt());
        }

        private void FixedUpdate() {
            if (!controllable) return;

            Vector3 vec = Vector3.Scale(input.MovementVector, Vector3.right);
            rigidBody.velocity = vec * turnSpeed;
            nextTiltAngle = input.MovementVector.x * tiltAngle * -1;
        }

        /// <summary>
        /// Regularly tilt the player's spaceship.
        /// </summary>
        private IEnumerator Tilt() {
            Vector3 xyMask = Vector3.right + Vector3.up;

            while (true) {
                if (!controllable) yield return null;

                Vector3 currRot = transform.localEulerAngles;
                float step = Time.deltaTime * tiltSpeed;
                float nextZ = nextTiltAngle;
                Vector3 nextEuler = Vector3.Scale(currRot, xyMask) + Vector3.forward * nextZ;
                Quaternion nextRot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(nextEuler), step);
                transform.localRotation = nextRot;

                yield return null;
            }
        }
    }
}