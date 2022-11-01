using UnityEngine;

namespace BetterSkyRoads.Util
{
    public static class Layers
    {
        //masks
        public static readonly LayerMask DEFAULT = LayerMask.GetMask("Default");
        public static readonly LayerMask TRANSPARENT_FX = LayerMask.GetMask("TransparentFX");
        public static readonly LayerMask IGNORE_RAYCAST = LayerMask.GetMask("Ignore Raycast");
        public static readonly LayerMask PLAYER = LayerMask.GetMask("Player");
        public static readonly LayerMask WATER = LayerMask.GetMask("Water");
        public static readonly LayerMask UI = LayerMask.GetMask("UI");
        public static readonly LayerMask ASTEROID = LayerMask.GetMask("Asteroid");

        /// <summary>
        /// Check if a certain layer is contained in a layer mask.
        /// </summary>
        /// <param name="layer">The layer to check (as is "gameObject.layer")</param>
        /// <returns>True if the mask contains the layer.</returns>
        public static bool ContainsLayer(this LayerMask mask, int layer) => (mask & 1 << layer) == 1 << layer;
    }
}