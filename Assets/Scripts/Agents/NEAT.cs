using System.Collections.Generic;
using UnityEngine;
using Animals;
using UnityEngine.AI;

// Tuto of Underpower Jet : https://www.youtube.com/watch?v=Yq0SfuiOVYE

namespace Agents
{
    class NEAT : MonoBehaviour
    {
        private NeuralNetwork nn;
        private Entity animal;
        public Species species;

        public Entity Animal
        {
            get
            {
                return animal;
            }
        }
        public double hunger;
        public double thirst;
        public float age;
        public float hp;
        public float timeSinceDeath;
        public bool gender;

        private void Start()
        {
            Environment.Parameters parameters = Menu.EditAction.parameters ?? Environment.Parameters.Load();
            List<Entity> entities = parameters.parameters["entities"].value;

            foreach (Entity e in entities)
            {
                if ((e is Rabbit && species == Species.Rabbit) || (e is Wolf && species == Species.Wolf)
                    || (e is Carrot && species == Species.Carrot) || (e is Deer && species == Species.Deer)
                    || (e is Bear && species == Species.Bear) || (e is Cat && species == Species.Cat))
                {
                    animal = Instantiate(e);
                    animal.parameters = e.parameters;
                }
            }

            animal.gameObject = gameObject;
            animal.parameters["MAX_AGE"].value *= parameters.parameters["timeMax"].value / 100.0;
            if (animal is Animal)
            {
                animal.parameters["thirst"].value = animal.parameters["MAX_THIRST"].value;
                animal.parameters["hunger"].value = animal.parameters["MAX_HUNGER"].value;
                animal.parameters["HP"].value = animal.parameters["HPMax"].value;
                (animal as Animal).Start();
                GetComponent<NavMeshAgent>().enabled = true;
            }

            //nn = new NeuralNetwork(new int[] { });
        }

        private void FixedUpdate()
        {
            animal.FixedUpdate();
            if (animal is Animal)
            {
                hunger = animal.parameters["hunger"].value;
                thirst = animal.parameters["thirst"].value;
                age = animal.parameters["age"].value;
                hp = animal.parameters["HP"].value;
                timeSinceDeath = animal.parameters.ContainsKey("timeSinceDeath") ? animal.parameters["timeSinceDeath"].value : 0;
                gender = animal.parameters["isMale"].value;
            }
        }

        private class NeuralNetwork
        {
            // TODO : Edit attributes to create a fully editable network (weights, biases, layers, the neurons per layer and the connections between them)
            private int[] layers;
            private float[][][] weights;
            private float[][] neurons;
            private float[][] biases;

            private System.Random random;

            public NeuralNetwork(int[] layers) // 1. inputSize (senseurs/capteurs) 2. hiddenSize 3. outputSize (effecteurs)
            {
                this.layers = new int[layers.Length];
                for (int i = 0; i < this.layers.Length; i++)
                    this.layers[i] = layers[i];

                random = new System.Random(System.DateTime.Today.Millisecond);

                InitNeurons();
                InitWeights();
                InitBiases();
            }

            private void InitNeurons()
            {
                List<float[]> neuronsList = new List<float[]>();

                for (int i = 0; i < layers.Length; i++)
                    neuronsList.Add(new float[layers[i]]);
                neurons = neuronsList.ToArray();
            }

            private void InitWeights()
            {
                List<float[][]> weightsList = new List<float[][]>();

                for (int i = 1; i < layers.Length; i++)
                {
                    List<float[]> layerWList = new List<float[]>();

                    for (int j = 0; j < neurons[i].Length; j++)
                    {
                        float[] neuronWeights = new float[layers[i - 1]];

                        for (int k = 0; k < layers[i - 1]; k++)
                            neuronWeights[k] = (float)random.NextDouble() - 0.5f;
                        layerWList.Add(neuronWeights);
                    }
                    weightsList.Add(layerWList.ToArray());
                }
                weights = weightsList.ToArray();
            }

            private void InitBiases()
            {
                List<float[]> biasesList = new List<float[]>();

                for (int i = 0; i < layers.Length; i++)
                {
                    biasesList.Add(new float[layers[i]]);

                    for (int j = 0; j < layers[i]; j++)
                        biasesList[i][j] = (float)random.NextDouble() - .5f;
                }
                biases = biasesList.ToArray();
            }

            public float[] feedForward(float[] input)
            {
                for (int i = 0; i < layers[0]; i++)
                    neurons[0][i] = input[i];

                for (int i = 1; i < layers.Length; i++)
                {
                    for (int j = 0; j < neurons[i].Length; j++)
                    {
                        float sum = biases[i][j];

                        for (int k = 0; k < neurons[i - 1].Length; k++)
                            sum += weights[i - 1][j][k] * neurons[i - 1][k];

                        neurons[i][j] = (float)System.Math.Tanh(sum);
                    }
                }
                return neurons[layers.Length - 1];
            }
        }
    }
}
