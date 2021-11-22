using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    class LaunchSimulationAction : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            LoadScene.Load("LaunchSimulationMenu");
        }
    }
}
