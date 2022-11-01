using BetterSkyRoads.Asteroids;
using BetterSkyRoads.Util;
using UnityEngine;
using UnityEngine.Events;

namespace BetterSkyRoads.Player
{
    public class PlayerHitSensor : MonoBehaviour
    {
        #region Events
        public event UnityAction HitEvent;
        #endregion

        private void OnCollisionEnter(Collision col) {
            if (Layers.ASTEROID.ContainsLayer(col.gameObject.layer)) {
                Asteroid asteroidCmp = col.gameObject.GetComponentInParent<Asteroid>();

                if (asteroidCmp != null) {
                    asteroidCmp.Destroy();
                    HitEvent?.Invoke();
                }
            }
        }
    }
}