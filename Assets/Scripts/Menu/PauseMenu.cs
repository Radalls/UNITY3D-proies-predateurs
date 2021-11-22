using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    class PauseMenu : MonoBehaviour
    {
        private Canvas menu;
        private static bool paused = false;

        public static bool Paused { get
            {
                return paused;
            }
        }

        public void Start()
        {
            paused = false;
            menu = GetComponent<Canvas>();
            menu.enabled = false;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = !paused;
                Time.timeScale = paused ? 0 : 1;
                menu.enabled = paused;
            }
        }

        public void Unpause()
        {
            paused = false;
            Time.timeScale = 1;
            menu.enabled = false;
        }
    }
}
