using BetterSkyRoads.Asteroids;
using BetterSkyRoads.Flow;
using BetterSkyRoads.Util;
using System.Collections.Generic;
using UnityEngine;

namespace BetterSkyRoads.Track
{
    [RequireComponent(typeof(MeshFilter))]
    public class Road : Singleton<Road>
    {
        #region Exposed Editor Parameters
        [Tooltip("A lane prefab.")]
        [SerializeField] private Lane lanePrototype;

        [Tooltip("The object that spawns asteroids on the track.")]
        [SerializeField] private AsteroidSpawner asteroidSpawner;

        [Tooltip("The amount of track lanes to create.")]
        [SerializeField] [Range(1, 5)] private int lanesAmount = 1;
        #endregion

        #region Constants
        private static readonly float ROAD_SPEED_MULTIPLIER = -7;
        private static readonly float MIN_ROAD_OFFSET = -1000;
        #endregion

        #region Class Members
        private List<Lane> lanes;
        private Lane lastAssignedLane;
        private Material roadMat;
        private Vector3 size;
        #endregion

        #region Properties
        public int LanesAmount => lanesAmount;
        #endregion

        protected override void Awake() {
            base.Awake();

            Renderer mesh = GetComponent<Renderer>();
            this.lanes = new List<Lane>();
            this.size = mesh.bounds.size;
            this.roadMat = mesh.material;

            BuildLanes();
        }

        private void FixedUpdate() {
            if (roadMat.mainTextureOffset.y < MIN_ROAD_OFFSET) roadMat.mainTextureOffset = Vector2.zero;

            float paceParameter = LevelFlow.Instance.LevelTime * LevelFlow.Instance.CurrentSpeed;
            roadMat.mainTextureOffset = new Vector2(0, paceParameter * ROAD_SPEED_MULTIPLIER);
        }

        /// <summary>
        /// Create the track lanes.
        /// </summary>
        private void BuildLanes() {
            if (asteroidSpawner == null)
                throw new MissingReferenceException("No AsteroidSpawner object found.");

            Vector3 spawnerPos = asteroidSpawner.transform.position;
            Vector3 initialYZPos = Vector3.Scale(spawnerPos, Vector3.up + Vector3.forward);
            float laneWidth = size.x / lanesAmount;
            float roadStart = size.x / -2;

            //instantiate lanes
            for (int i = 0; i < lanesAmount; i++) {
                float laneX = roadStart + laneWidth * (i + .5f);
                Vector3 position = initialYZPos + Vector3.right * laneX;
                Lane instance = Instantiate(lanePrototype, position, Quaternion.identity, transform);
                instance.gameObject.name = $"Lane #{i + 1}";
                lanes.Add(instance);
            }
        }

        /// <summary>
        /// Assign an available lane to an asteroid.
        /// </summary>
        /// <param name="asteroid">The requesting asteroid</param>
        public void AssignLane(Asteroid asteroid) {
            Lane selectedLane;

            do { selectedLane = lanes[Random.Range(0, lanes.Count)]; }
            while (lanesAmount > 1 && lastAssignedLane == selectedLane);

            lastAssignedLane = selectedLane;
            selectedLane.Adopt(asteroid);
        }
    }
}