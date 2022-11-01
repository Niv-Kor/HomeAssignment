using System.Collections;
using BetterSkyRoads.Flow;
using BetterSkyRoads.Track;
using UnityEngine;

namespace BetterSkyRoads.Asteroids
{
    [RequireComponent(typeof(AsteroidsPool))]
    public class AsteroidSpawner : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Header("Spawn Interval")]
        [Tooltip("The initial time [s] to wait before spawning the next asteroid.")]
        [SerializeField] private float initialSpawnInterval = 1;

        [Tooltip("The change in the spawning interval time [s] over time.")]
        [SerializeField] private float intervalChangeRate;

        [Tooltip("The minimum interval time [s] possible.")]
        [SerializeField] private float minIntervalTime;

        [Header("Settings")]
        [Tooltip("The chance to spawn multiple asteroids at the same time.")]
        [SerializeField] [Range(0, 1)] private float doubleSpawnChance;
        #endregion

        #region Class Members
        private AsteroidsPool pool;
        #endregion

        private void Awake() {
            this.pool = GetComponent<AsteroidsPool>();
        }

        private void Start() {
            StartCoroutine(Spawn());
        }

        /// <summary>
        /// Regularly spawn asteroids over time.
        /// </summary>
        private IEnumerator Spawn() {
            while (Application.isPlaying) {
                while (LevelFlow.Instance.GameRunning) {
                    int lanesAmount = Road.Instance.LanesAmount;
                    bool multiChance = lanesAmount > 1 && Random.Range(0, 1f) <= doubleSpawnChance;

                    for (int i = 0; i == 0 || (i < lanesAmount - 1 && multiChance); i++) {
                        Asteroid asteroid = pool.Take();
                        Road.Instance.AssignLane(asteroid);
                        asteroid.Release(pool);
                    }

                    //calculate the time needed to wait until the next iteration
                    float paceChange = intervalChangeRate * LevelFlow.Instance.LevelTime;
                    float finalInterval = Mathf.Max(minIntervalTime, initialSpawnInterval - paceChange);
                    yield return new WaitForSeconds(finalInterval);
                }

                yield return null;
            }
        }
    }
}