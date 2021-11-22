using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Animals;

namespace Environment
{
    public class Parameters
    {
        public Dictionary<string, ParameterEntry> parameters = new Dictionary<string, ParameterEntry>();

        public static Parameters Load()
        {
            Carrot carrot = ScriptableObject.CreateInstance<Carrot>();
            carrot.parameters["id"].value = "carrot";
            carrot.parameters["ADULT_AGE"].value = 1;
            carrot.parameters["MAX_AGE"].value = 10;

            Wolf wolf = ScriptableObject.CreateInstance<Wolf>();
            wolf.parameters["id"].value = "wolf";
            wolf.parameters["ADULT_AGE"].value = 4;
            wolf.parameters["MAX_AGE"].value = 20;
            wolf.parameters["MAX_HUNGER"].value = 45; //15 // 40
            wolf.parameters["MAX_THIRST"].value = wolf.parameters["MAX_HUNGER"].value * 1.5;
            wolf.parameters["MAX_RUN_SPEED"].value = 5; // 50
            wolf.parameters["pregnancyTime"].value = 4;
            wolf.parameters["nbOfBabyPerLitter"].value = 3;
            wolf.parameters["interactionLevel"].value = 5;
            wolf.parameters["HPMax"].value = 50;
            wolf.parameters["Atk"].value = 55;

            Rabbit rabbit = ScriptableObject.CreateInstance<Rabbit>();

            rabbit.parameters["id"].value = "rabbit";
            rabbit.parameters["ADULT_AGE"].value = 4;
            rabbit.parameters["MAX_AGE"].value = 22;
            rabbit.parameters["MAX_HUNGER"].value = 75; //25
            rabbit.parameters["MAX_THIRST"].value = rabbit.parameters["MAX_HUNGER"].value * 1.5;
            rabbit.parameters["MAX_RUN_SPEED"].value = 5; // 45 is the true value
            rabbit.parameters["pregnancyTime"].value = 4;
            rabbit.parameters["nbOfBabyPerLitter"].value = 3;
            rabbit.parameters["interactionLevel"].value = -8;
            rabbit.parameters["HPMax"].value = 10;
            rabbit.parameters["Atk"].value = 1;

            Deer deer = ScriptableObject.CreateInstance<Deer>();

            deer.parameters["id"].value = "deer";
            deer.parameters["ADULT_AGE"].value = 3;
            deer.parameters["MAX_AGE"].value = 20;
            deer.parameters["MAX_HUNGER"].value = 75; //75
            deer.parameters["MAX_THIRST"].value = deer.parameters["MAX_HUNGER"].value * 1.5;
            deer.parameters["MAX_RUN_SPEED"].value = 45; // 45 is the true value
            deer.parameters["pregnancyTime"].value = 4;
            deer.parameters["nbOfBabyPerLitter"].value = 3;
            deer.parameters["interactionLevel"].value = -8;
            deer.parameters["HPMax"].value = 60;
            deer.parameters["Atk"].value = 40;

            Bear bear = ScriptableObject.CreateInstance<Bear>();

            bear.parameters["id"].value = "bear";
            bear.parameters["ADULT_AGE"].value = 16;
            bear.parameters["MAX_AGE"].value = 80;
            bear.parameters["MAX_HUNGER"].value = 75; //75
            bear.parameters["MAX_THIRST"].value = bear.parameters["MAX_HUNGER"].value * 1.5;
            bear.parameters["MAX_RUN_SPEED"].value = 45; // 45 is the true value
            bear.parameters["pregnancyTime"].value = 4;
            bear.parameters["nbOfBabyPerLitter"].value = 3;
            bear.parameters["interactionLevel"].value = -8;
            bear.parameters["HPMax"].value = 100;
            bear.parameters["Atk"].value = 70;

            Cat cat = ScriptableObject.CreateInstance<Cat>();

            cat.parameters["id"].value = "cat";
            cat.parameters["ADULT_AGE"].value = 4;
            cat.parameters["MAX_AGE"].value = 25;
            cat.parameters["MAX_HUNGER"].value = 75; //25
            cat.parameters["MAX_THIRST"].value = cat.parameters["MAX_HUNGER"].value * 1.5;
            cat.parameters["MAX_RUN_SPEED"].value = 5; // 45 is the true value
            cat.parameters["pregnancyTime"].value = 4;
            cat.parameters["nbOfBabyPerLitter"].value = 3;
            cat.parameters["interactionLevel"].value = -8;
            cat.parameters["HPMax"].value = 20;
            cat.parameters["Atk"].value = 15;

            Parameters p = new Parameters();

            p.parameters.Add("aridity", new ParameterEntry("aridity", "Aridité", 20, ParameterEntry.Type.Slider));
            p.parameters.Add("fertility", new ParameterEntry("fertility", "Fertilité des sols", 50, ParameterEntry.Type.Slider));
            p.parameters.Add("amplitude", new ParameterEntry("amplitude", "Amplitude", 15, ParameterEntry.Type.Slider));
            p.parameters.Add("resourcesQuantity", new ParameterEntry("resourcesQuantity", "Quantité de ressources", 70, ParameterEntry.Type.Slider));
            p.parameters.Add("seed", new ParameterEntry("seed", "Seed", 0, ParameterEntry.Type.Input));
            p.parameters.Add("timeMax", new ParameterEntry("timeMax", 375, false));
            p.parameters.Add("entities", new ParameterEntry("entities", new List<Entity>{
                    rabbit,
                    wolf,
                    deer,
                    bear,
                    cat,
                    carrot
            }
            ));

            return p;
        }

        public static Parameters Load(int id)
        {
            Parameters parameters = Load();

            if (id < 0 || id > 2)
            return parameters;

            parameters.parameters["seed"].value = new int[] { 332, 355, 384 }[id];
            return parameters;
        }

        [Serializable]
        private struct ParametersList
        {
            List<ParameterEntry> entries;

            public Dictionary<string, ParameterEntry> toDict()
            {
                Dictionary<string, ParameterEntry> result = new Dictionary<string, ParameterEntry>();

                foreach (ParameterEntry entry in entries)
                    result.Add(entry.id, entry);

                return result;
            }

            public static ParametersList toList(Dictionary<string, ParameterEntry> dict)
            {
                ParametersList result = new ParametersList();

                foreach (KeyValuePair<string, ParameterEntry> entry in dict)
                    result.entries.Add(entry.Value);

                return result;
            }
        }

        [Serializable]
        public class ParameterEntry
        {
            public string id;
            public string menuText = null;
            public dynamic value;
            public bool serialize = true;
            public Type type = Type.None;

            public enum Type { None, Input, Slider }

            public ParameterEntry(string id, string menuText, dynamic value, Type type, bool serialize = true)
            {
                this.id = id;
                this.value = value;
                this.menuText = menuText;
                this.serialize = serialize;
                this.type = type;
            }

            public ParameterEntry(string id, dynamic value, bool serialize = true)
            {
                this.id = id;
                this.value = value;
            }
        }
    }
}
