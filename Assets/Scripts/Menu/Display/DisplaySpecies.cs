using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Animals;
using Environment;

namespace Menu
{
    /// <summary>
    /// Permet d'afficher les boutons des relatifs à chaque type d'espèce (proies ou prédateurs)
    /// </summary>
    public class DisplaySpecies : MonoBehaviour
    {
        public enum SpeciesType { Prey, Predator }

        // Définit quel onglet est sélectionné.
        [HideInInspector]
        public SpeciesType speciesType = SpeciesType.Prey;
        // Indique qu'il faut redessiner le contenu du Viewport
        [HideInInspector]
        public bool redraw = true;

        private void Update()
        {
            if (redraw)
            {
                switch (speciesType)
                {
                    case SpeciesType.Predator:
                        DrawPredator();
                        break;
                    case SpeciesType.Prey:
                        DrawPrey();
                        break;
                }
                redraw = false;
            }
        }

        private void DrawPredator()
        {
            List<string> species = new List<string>();
            EditAction.parameters = EditAction.parameters ?? Parameters.Load();

            foreach (Entity entity in (List<Entity>)EditAction.parameters.parameters["entities"].value)
                if (entity is Carnivorous)
                    species.Add(entity.parameters["id"].value);

            DrawSpecies(species);
        }

        private void DrawPrey()
        {
            List<string> species = new List<string>();
            EditAction.parameters = EditAction.parameters ?? Parameters.Load();

            foreach (Entity entity in (List<Entity>)EditAction.parameters.parameters["entities"].value)
                if (entity is Herbivorous || entity is Plant)
                    species.Add(entity.parameters["id"].value);

            DrawSpecies(species);
        }

        private void DrawSpecies(List<string> species)
        {
            for (int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);

            foreach (string creature in species)
            {
                string crea = creature.ToLower(), text = crea.Substring(0, 1).ToUpper() + crea.Substring(1);
                GameObject instance = Instantiate(Resources.Load<GameObject>("Prefabs/Type"), transform);
                instance.name = text;

                AddScript(crea, instance);
                instance.GetComponentInChildren<Text>().text = text;
            }
        }

        private void AddScript(string creature, GameObject instance)
        {
            GameObject script = new GameObject();
            EntityAction entityAction = script.AddComponent<EntityAction>();
            Button b = instance.GetComponent<Button>();

            script.name = instance.name + "Script";

            entityAction.id = creature;
            script.transform.SetParent(instance.transform);

            b.onClick.AddListener(entityAction.Action);
        }
    }
}
