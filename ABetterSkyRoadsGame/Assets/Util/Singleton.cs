using UnityEngine;

namespace BetterSkyRoads.Util
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Class Members
        private static T m_instance = null;
        #endregion

        #region Properties
        protected virtual bool GuardEntireObject => false;
        public static T Instance {
            get {
                if (m_instance == null) {
                    m_instance = FindObjectOfType<T>();
                    return m_instance;
                }

                return m_instance;
            }
        }
        #endregion

        protected virtual void Awake() {
            if (Instance != this) {
                if (GuardEntireObject) Destroy(gameObject);
                else Destroy(this);
            }
        }
    }
}