using BetterSkyRoads.Asteroids;
using UnityEngine;

namespace BetterSkyRoads.Track
{
    public class Lane : MonoBehaviour
    {
        /// <summary>
        /// Assign an asteroid to this lane.
        /// </summary>
        /// <param name="asteroid">The asteroid to assign</param>
        public void Adopt(Asteroid asteroid) {
            asteroid.transform.position = transform.position;
        }
    }
}