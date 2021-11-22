using UnityEngine.SceneManagement;
using UnityEngine;

namespace Menu
{
    public class LoadScene
    {
        /// <summary>
        /// Charge une scène donnée.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="previous"></param>
        public static void Load(string scene, bool previous = false)
        {
            if (!previous)
                PreviousMenu.sceneStack.Push(SceneManager.GetActiveScene().name);
            if (scene == "World")
            {
                PreviousMenu.sceneStack.Clear();
            }
            SceneManager.LoadScene(scene);
        }
    }
}