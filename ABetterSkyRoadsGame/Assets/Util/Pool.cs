using System.Collections.Generic;
using UnityEngine;

namespace BetterSkyRoads.Util
{
    public abstract class Pool<TElement> : MonoBehaviour where TElement : MonoBehaviour
    {
        #region Exposed Editor Parameters
        [Tooltip("The object that contains all created items (if empty, the current object is used).")]
        [SerializeField] protected Transform elementsParent;

        [Tooltip("A list of possible elements to be picked randomly.")]
        [SerializeField] protected List<TElement> list;

        [Tooltip("An initial amount of elements that will be created along with the pool.")]
        [SerializeField] protected uint initialAmount = 0;

        [Tooltip("The maximum allowed amount of elements (active or non-active) at all times.")]
        [SerializeField] protected uint maxTotalAmount = 100;
        #endregion

        #region Class Members
        protected Queue<TElement> pool;
        protected int totalItems;
        #endregion

        #region Properties
        public Transform Parent => (elementsParent != null) ? elementsParent : transform;
        public bool MaxTotalExceeded => totalItems >= maxTotalAmount;
        public int PoolSize => pool.Count;
        #endregion

        protected virtual void Awake() {
            this.pool = new Queue<TElement>();
            this.totalItems = 0;
        }

        protected virtual void Start() {
            Insert(initialAmount);
        }

        /// <summary>
        /// Manually insert a fixed amount of items into the pool.
        /// </summary>
        /// <param name="amount">The amount of items to insert</param>
        protected virtual void Insert(uint amount) {
            for (int i = 0; i < amount; i++) {
                TElement item = Make();

                if (item != null) {
                    item.gameObject.SetActive(false);
                    pool.Enqueue(item);
                }
            }
        }

        /// <summary>
        /// Generate a randome element from the list.
        /// </summary>
        /// <returns>The newly created item (or null if no more items can be created due to the limit being exceeded).</returns>
        protected virtual TElement Make() {
            if (list.Count == 0 || MaxTotalExceeded) return null;

            TElement selected = Pick();

            //instantiate
            if (selected != null) {
                TElement instance = Instantiate(selected, Parent);
                instance.name = instance.name.Replace("(Clone)", $"({gameObject.GetHashCode()})");
                totalItems++;

                return instance;
            }

            return null;
        }

        /// <summary>
        /// Return an item to the pool.
        /// </summary>
        /// <param name="item">The item to return</param>
        public virtual void Return(TElement item) {
            if (!pool.Contains(item)) pool.Enqueue(item);
            item.gameObject.SetActive(false);
        }

        /// <summary>
        /// Take an item from the pool,
        /// or generate one if the pool is empty.
        /// </summary>
        /// <returns>The retrieved item (or null if no more items can be created due to the limit being exceeded).</returns>
        public virtual TElement Take() {
            TElement item;

            if (PoolSize > 0) {
                item = pool.Dequeue();
                item.gameObject.SetActive(true);
            }
            else item = Make();

            return item;
        }

        /// <summary>
        /// Pick a random (weighted) item from the list, without instantiating it.
        /// </summary>
        /// <returns>A random element from the list.</returns>
        public virtual TElement Pick() {
            int index = Random.Range(0, list.Count);
            return list[index];
        }
    }
}