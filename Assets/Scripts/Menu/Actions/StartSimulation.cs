using UnityEngine;

namespace Menu
{
    class StartSimulation : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            LoadScene.Load("World");
        }
    }
}
