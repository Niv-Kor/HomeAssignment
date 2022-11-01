using BetterSkyRoads.Flow;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BetterSkyRoads.Asteroids
{
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("The speed at which the asteroid rotates while moving.")]
        [SerializeField] private float rotationSpeed;
        #endregion

        #region Class Members
        private AsteroidsPool parentPool;
        private Vector3 initialScale;
        private bool released;
        private float speed;
        #endregion

        #region Events
        public event UnityAction DestroyEvent;
        #endregion

        private void Awake() {
            this.released = false;
            this.speed = LevelFlow.Instance.CurrentSpeed;
            this.initialScale = transform.localScale;
        }

        private void Start() {
            LevelFlow.Instance.PaceChangeEvent += delegate (float newSpeed) { speed = newSpeed; };
            LevelFlow.Instance.GameEndEvent += delegate { released = false; };
        }

        /// <summary>
        /// Gradually move the astroid towards the player.
        /// </summary>
        private IEnumerator Move() {
            while (released) {
                Vector3 dir = Vector3.forward * -1;
                transform.Translate(dir * speed);
                yield return null;
            }
        }

        /// <summary>
        /// Scale the asteroid up to its initial value.
        /// </summary>
        private IEnumerator ScaleUp() {
            Vector3 startScale = transform.localScale;
            float timer = 0;

            while (timer <= speed) {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, initialScale, timer / speed);
                yield return null;
            }
        }

        /// <summary>
        /// Release the asteroid on the track.
        /// </summary>
        /// <param name="pool">The pool to which the asteroid should return when destoryed</param>
        public void Release(AsteroidsPool pool) {
            parentPool = pool;
            released = true;
            transform.localScale = Vector3.zero;
            StartCoroutine(ScaleUp());
            StartCoroutine(Move());
        }

        /// <summary>
        /// Destroy the asteroid (and return it to the pool).
        /// </summary>
        public void Destroy() {
            released = false;
            transform.localScale = Vector3.zero;
            DestroyEvent?.Invoke();
            parentPool.Return(this);
        }
    }
}