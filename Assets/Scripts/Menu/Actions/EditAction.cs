using UnityEngine;
using Environment;

namespace Menu
{
    class EditAction : MonoBehaviour, ButtonAction
    {
        // Monde à éditer.
        public WorldAction.WorldOptions worldPreset;

        public static Parameters parameters;

        public void Action()
        {
            switch (worldPreset)
            {
                case WorldAction.WorldOptions.WorldPreset1:
                    parameters = Parameters.Load(0);
                    LoadScene.Load("ParameterMenu");
                    break;
                case WorldAction.WorldOptions.WorldPreset2:
                    parameters = Parameters.Load(1);
                    LoadScene.Load("ParameterMenu");
                    break;
                case WorldAction.WorldOptions.WorldPreset3:
                    parameters = Parameters.Load(2);
                    LoadScene.Load("ParameterMenu");
                    break;
                case WorldAction.WorldOptions.NewWorld:
                    parameters = Parameters.Load();
                    LoadScene.Load("ParameterMenu");
                    break;
                default:
                    Debug.LogError("This option is not supported.");
                    break;
            }
        }
    }
}
