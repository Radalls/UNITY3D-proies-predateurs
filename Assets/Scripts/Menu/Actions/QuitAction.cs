using UnityEngine;

namespace Menu
{
    class QuitAction : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            Application.Quit();
        }
    }
}
