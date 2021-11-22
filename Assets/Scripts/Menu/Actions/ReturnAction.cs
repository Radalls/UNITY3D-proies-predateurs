using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    class ReturnAction : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            string scene = SceneManager.GetActiveScene().name;
            if (scene == "EnvironmentalParametersMenu")
            {
                SaveEnvironmentalParams save = GameObject.Find("SaveParameters").GetComponent<SaveEnvironmentalParams>();
                save.Action();
            }
            else if (scene == "AgentParametersMenu")
            {
                SaveAgentParams save = GameObject.Find("SaveParameters").GetComponent<SaveAgentParams>();
                save.Action();
                EntityAction.ResetTargets = true;
            }

            if (PreviousMenu.sceneStack.Count != 0)
                LoadScene.Load(PreviousMenu.sceneStack.Pop(), true);
            else
                LoadScene.Load("ParameterMenu", true);
        }
    }
}
