using UnityEngine;

namespace Menu
{
    class ReturnGame : MonoBehaviour, ButtonAction
    {
        public PauseMenu pauseMenu;

        public void Action()
        {
            if (PauseMenu.Paused)
                pauseMenu.Unpause();
        }
    }
}
