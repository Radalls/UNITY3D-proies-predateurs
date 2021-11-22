using UnityEngine;

namespace Menu
{
    class EnvironmentalAction : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            LoadScene.Load("EnvironmentalParametersMenu");
        }
    }
}
