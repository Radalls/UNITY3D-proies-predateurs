using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Environment;
using Animals;

namespace Menu
{
    class DisplayOpt : MonoBehaviour
    {
        private Parameters p;

        private void Start()
        {
            EditAction.parameters = EditAction.parameters ?? Parameters.Load();
            p = EditAction.parameters;

            if (SceneManager.GetActiveScene().name == "EnvironmentalParametersMenu")
                DisplayEnvironmentOpt();
            else if (SceneManager.GetActiveScene().name == "AgentParametersMenu")
                DisplayAgentOpt();
        }

        private void DisplayEnvironmentOpt()
        {
            foreach (var entry in p.parameters)
            {
                if (entry.Key == "entities" || !entry.Value.serialize)
                    continue;
                if (entry.Value.type == Parameters.ParameterEntry.Type.Slider)
                {
                    GameObject prefab = Instantiate(Resources.Load<GameObject>("Prefabs/EnvOptElement"), transform);

                    prefab.name = entry.Key;
                    prefab.GetComponentInChildren<Text>().text = entry.Value.menuText;
                    prefab.GetComponentInChildren<Slider>().value = entry.Value.value;
                }
                else if (entry.Value.type == Parameters.ParameterEntry.Type.Input)
                {
                    GameObject prefab = Instantiate(Resources.Load<GameObject>("Prefabs/InputLayout"), transform);
                    prefab.name = entry.Key + "Layout";
                    prefab.GetComponentInChildren<Text>().text = entry.Value.menuText;
                    prefab.GetComponentInChildren<InputField>().text = ((int)entry.Value.value).ToString();
                }
            }
        }

        private void DisplayAgentOpt()
        {
            List<Parameters.ParameterEntry> entityParameters = new List<Parameters.ParameterEntry>();
            List<Parameters.ParameterEntry> animalParameters = new List<Parameters.ParameterEntry>();
            List<Parameters.ParameterEntry> plantParameters = new List<Parameters.ParameterEntry>();

            foreach (Entity entry in p.parameters["entities"].value)
            {
                if (animalParameters.Count == 0 && entry is Animal)
                    animalParameters.AddRange(entry.parameters.Values);
                else if (plantParameters.Count == 0 && entry is Plant)
                    plantParameters.AddRange(entry.parameters.Values);

                if (animalParameters.Count > 0 && plantParameters.Count > 0)
                    break;
            }

            for (int i = 0; i < animalParameters.Count; i++)
            {
                Parameters.ParameterEntry pe = plantParameters.Find(e => e.id == animalParameters[i].id);

                if (pe != null && pe.id == animalParameters[i].id)
                {
                    plantParameters.Remove(pe);
                    entityParameters.Add(animalParameters[i]);
                    animalParameters.RemoveAt(i);
                    i--;
                }
            }

            DisplayList(entityParameters, "EntityOptions");
            DisplayList(animalParameters, "AnimalOptions");
            DisplayList(plantParameters, "PlantOptions");
        }

        private void DisplayList(List<Parameters.ParameterEntry> parameters, string tag)
        {
            foreach(Parameters.ParameterEntry entry in parameters)
            {
                if (!entry.serialize)
                    continue;
                if (entry.type == Parameters.ParameterEntry.Type.Slider)
                {
                    GameObject prefab = Instantiate(Resources.Load<GameObject>("Prefabs/AgentOptElement"), transform);
                    prefab.tag = tag;

                    prefab.name = entry.id;
                    prefab.GetComponentInChildren<Text>().text = entry.menuText;
                    prefab.GetComponentInChildren<Slider>().value = entry.value;
                }
                else if (entry.type == Parameters.ParameterEntry.Type.Input)
                {
                    GameObject prefab = Instantiate(Resources.Load<GameObject>("Prefabs/InputLayout"), transform);
                    prefab.name = entry.id + "Layout";
                    prefab.GetComponentInChildren<Text>().text = entry.menuText;
                    prefab.GetComponentInChildren<InputField>().text = ((int)entry.value).ToString();
                }
            }
        }
    }
}
