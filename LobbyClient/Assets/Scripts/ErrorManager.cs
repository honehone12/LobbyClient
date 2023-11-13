using UnityEngine;

namespace Lobby
{
    public class ErrorManager : MonoBehaviour
    {
        public static ErrorManager Singleton { get; private set; }

        void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Error(string message)
        {
            Debug.LogError(message);
        }

        public void Exception(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
