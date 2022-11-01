using BetterSkyRoads.Asteroids;
using BetterSkyRoads.Util;
using UnityEngine;
using UnityEngine.Events;

public class RearTrackWall : MonoBehaviour
{
    #region Events
    public event UnityAction ScoreEvent;
    #endregion

    private void OnTriggerEnter(Collider col) {
        if (Layers.ASTEROID.ContainsLayer(col.gameObject.layer)) {
            Asteroid asteroidCmp = col.GetComponentInParent<Asteroid>();

            if (asteroidCmp != null) {
                asteroidCmp.Destroy();
                ScoreEvent?.Invoke();
            }
        }
    }
}