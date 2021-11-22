using UnityEngine;
using System;
using System.Collections.Generic;
using Environment;

namespace Agents
{
    class Gender : MonoBehaviour
    {
        private enum State { NOT_READY, READY, DONE }

        private Dictionary<string, int[]> needs = new Dictionary<string, int[]>();
        private State state;
        private ObjectOnTerrainGenerator spawner;

        private void Start()
        {
            foreach (string name in Enum.GetNames(typeof(Species)))
            {
                if (name == Enum.GetName(typeof(Species), Species.Carrot))
                    continue;
                needs.Add(name, new int[2]);
                needs[name][0] = 0;
                needs[name][1] = 0;
            }

            spawner = GameObject.Find("Species").GetComponent<ObjectOnTerrainGenerator>();
            state = State.NOT_READY;
        }

        private void Update()
        {
            if (state == State.NOT_READY && spawner.done)
            {
                foreach (var n in needs)
                {
                    GameObject[] pop = GameObject.FindGameObjectsWithTag(n.Key);

                    n.Value[0] = pop.Length / 2;
                    n.Value[1] = pop.Length - n.Value[0];
                }
                state = State.READY;
            }
            else if (state == State.READY)
            {
                foreach (var n in needs)
                {
                    GameObject[] pop = GameObject.FindGameObjectsWithTag(n.Key);
                    
                    foreach (GameObject individual in pop)
                    {
                        bool gender = UnityEngine.Random.value < 0.5;

                        if (n.Value[0] <= 0)
                            gender = true;
                        if (n.Value[1] <= 0)
                            gender = false;

                        individual.GetComponent<NEAT>().Animal.parameters["isMale"].value = gender;

                        if (gender)
                            n.Value[1]--;
                        else
                            n.Value[0]--;
                    }
                }

                state = State.DONE;
            }
        }
    }
}
