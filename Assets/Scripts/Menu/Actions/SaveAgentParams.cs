using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    class SaveAgentParams : MonoBehaviour, ButtonAction
    {
        public static EntityAction entityAction = null;

        public void Action()
        {
            if (entityAction == null)
                return;
            entityAction.Action();
        }
    }
}
