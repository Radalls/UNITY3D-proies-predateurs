using UnityEngine;

namespace Menu
{
    public class AgentAction : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            LoadScene.Load("AgentParametersMenu");
        }
    }
}
