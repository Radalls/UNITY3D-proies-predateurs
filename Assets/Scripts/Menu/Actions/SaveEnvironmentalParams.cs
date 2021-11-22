using UnityEngine;
using UnityEngine.UI;
using Environment;

namespace Menu
{
    class SaveEnvironmentalParams : MonoBehaviour, ButtonAction
    {
        public void Action()
        {
            Parameters parameters = EditAction.parameters ?? Parameters.Load();

            foreach (var entry in parameters.parameters)
            {
                if (entry.Key == "entities")
                    continue;
                if (entry.Value.type == Parameters.ParameterEntry.Type.Slider)
                    parameters.parameters[entry.Key].value = GameObject.Find(entry.Key).GetComponentInChildren<Slider>().value;
                else if (entry.Value.type == Parameters.ParameterEntry.Type.Input)
                    parameters.parameters[entry.Key].value = int.Parse(GameObject.Find(entry.Key + "Layout").GetComponentInChildren<InputField>().text);
            }

            EditAction.parameters = parameters;
        }
    }
}
