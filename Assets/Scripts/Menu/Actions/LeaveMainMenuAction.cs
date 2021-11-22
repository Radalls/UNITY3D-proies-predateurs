using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class LeaveMainMenuAction : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            Time.timeScale = 1;
            PreviousMenu.sceneStack.Clear();
            LoadScene.Load("MainMenu", true);
        }
    }
}
