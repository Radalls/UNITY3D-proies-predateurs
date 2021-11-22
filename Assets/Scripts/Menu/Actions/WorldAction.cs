using UnityEngine;

namespace Menu
{
    class WorldAction : MonoBehaviour, ButtonAction
    {
        public enum WorldOptions { WorldPreset1, WorldPreset2, WorldPreset3, NewWorld }

        public WorldOptions worldPreset;
        public static WorldOptions preset;

        public void Action()
        {
            preset = worldPreset;
            switch (worldPreset)
            {
                case WorldOptions.WorldPreset1:
                    EditAction.parameters = Environment.Parameters.Load(0);
                    LoadScene.Load("World");
                    break;
                case WorldOptions.WorldPreset2:
                    EditAction.parameters = Environment.Parameters.Load(1);
                    LoadScene.Load("World");
                    break;
                case WorldOptions.WorldPreset3:
                    EditAction.parameters = Environment.Parameters.Load(2);
                    LoadScene.Load("World");
                    break;
                case WorldOptions.NewWorld:
                    EditAction.parameters = Environment.Parameters.Load();
                    EditAction.parameters.parameters["seed"].value = Random.Range(0, int.MaxValue);
                    LoadScene.Load("ParameterMenu");
                    break;
                default:
                    Debug.LogError("This option is not supported.");
                    break;
            }
        }
    }
}
