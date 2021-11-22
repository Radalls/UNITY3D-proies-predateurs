using UnityEngine;
using System.Collections.Generic;
using System;

namespace Menu
{
    class PreviousMenu : MonoBehaviour
    {
        public static Stack<string> sceneStack = new Stack<string>();
        public GameObject action;

        public void Update()
        {
            if (Input.GetButtonDown("Back"))
            {
                if (action != null)
                    action.GetComponent<ButtonAction>().Action();
                LoadScene.Load(sceneStack.Pop(), true);
            }
        }
    }
}
