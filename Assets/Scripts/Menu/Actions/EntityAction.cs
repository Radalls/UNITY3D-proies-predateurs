using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Animals;

namespace Menu
{
    class EntityAction : MonoBehaviour, ButtonAction
    {
        [HideInInspector]
        public string id;
        [HideInInspector]
        public bool active = false;

        private static GameObject[] targets = null;
        private static GameObject[] entityTargets = null;
        private static GameObject[] animalTargets = null;

        public static bool ResetTargets
        {
            set
            {
                if (value)
                {
                    targets = null; 
                    entityTargets = null;
                    animalTargets = null;
                }
            }
        }

        private void Start()
        {
            targets = targets ?? GameObject.FindGameObjectsWithTag("EntityOptions");
            entityTargets = entityTargets ?? GameObject.FindGameObjectsWithTag("EntityOptions");
            animalTargets = animalTargets ?? GameObject.FindGameObjectsWithTag("AnimalOptions");

            foreach (GameObject target in targets)
                target.SetActive(false);
            foreach (GameObject target in animalTargets)
                target.SetActive(false);
        }

        public void Action()
        {
            Entity entity = ((List<Entity>)EditAction.parameters.parameters["entities"].value).Find(e => e.parameters["id"].value == id);
            active = !active;

            if (active)
            {
                GameObject.Find("SaveParameters").GetComponent<SaveAgentParams>().Action();
                SaveAgentParams.entityAction = this;
                UpdateValues(entity);
            }
            else
            {
                SaveValues(entity);
                SaveAgentParams.entityAction = null;
                foreach (GameObject go in targets)
                    go.SetActive(false);
            }
        }

        private void SaveValues(Entity entity)
        {
            Debug.Log("Saving : " + id);
            foreach (GameObject gameObject in targets)
                entity.parameters[gameObject.name].value = gameObject.GetComponentInChildren<Slider>().value;
        }

        private void UpdateValues(Entity entity)
        {
            if (entity is Animal)
            {
                int l = targets.Length;
                Array.Resize(ref targets, targets.Length + animalTargets.Length);
                Array.Copy(animalTargets, 0, targets, l, animalTargets.Length);
            }
            else
                targets = entityTargets;

            foreach (GameObject gameObject in targets)
                gameObject.SetActive(true);

            foreach (GameObject gameObject in targets)
                gameObject.GetComponentInChildren<Slider>().value = entity.parameters[gameObject.name].value;
        }
    }
}
