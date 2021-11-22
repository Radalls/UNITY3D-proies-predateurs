using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.UI.Extensions;
using System;

using Animals;

namespace Environment
{
    class Stats : MonoBehaviour
    {
        private Dictionary<string, Parameters.ParameterEntry> parameters;

        public GameObject basicStats;
        public GameObject midStats;
        public GameObject advancedStats;
        public GameObject individualStats;

        public GameObject midStatsEntry;

        [Header("Advanced stats parameters")]
        [Range(10, 100)]
        public int valuesThreshold = 50;
        public GameObject advancedViewport;
        public GameObject advancedViewportElement;
        public GameObject legendElement;
        public GameObject legendViewport;
        public List<Color> legendColor;

        [Header("Individual stats parameters")]
        public GameObject hungerGO;
        public GameObject thirstGO;
        public GameObject ageGO;

        private Dictionary<string, GraphData> populations = new Dictionary<string, GraphData>();
        private const int inc = 5;

        private bool isMidActive = false;
        private bool isAdvActive = false;

        private GameObject target = null;

        private void Start()
        {
            parameters = (Menu.EditAction.parameters ?? Parameters.Load()).parameters;

            SetBasicStats();

            basicStats.SetActive(false);
            midStats.SetActive(false);
            advancedStats.SetActive(false);
            individualStats.SetActive(false);

            string[] species = Enum.GetNames(typeof(Species));
            List<Color> colors = new List<Color>(legendColor);
            foreach (string sp in species)
            {
                int currentPop = GameObject.FindGameObjectsWithTag(sp).Length;
                GameObject vp = GameObject.Instantiate(advancedViewportElement, advancedViewport.transform);
                GameObject legend = GameObject.Instantiate(legendElement, legendViewport.transform);
                UILineRenderer renderer = vp.GetComponent<UILineRenderer>();

                vp.name = sp;
                renderer.Points = new Vector2[1];
                renderer.Points[0] = new Vector2(0, 0);

                SetColor(legend, ref colors);

                renderer.color = legend.GetComponentInChildren<ProceduralImage>().color;
                legend.GetComponentInChildren<Text>().text = sp;

                GraphData graphData = new GraphData(new List<int> { currentPop }, renderer, null);
                populations.Add(sp, graphData);
            }
        }

        private void Update()
        {
            bool f3 = Input.GetKeyDown(KeyCode.F3);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            UpdateTarget();

            if (!ctrl && !shift && f3)
                basicStats.SetActive(!basicStats.activeSelf);
            else if ((!ctrl && shift && f3) || isMidActive)
            {
                SetMidStats();
                if ((ctrl && shift && f3) || !isMidActive)
                    isMidActive = !isMidActive;
                basicStats.SetActive(isMidActive);
                midStats.SetActive(isMidActive);
            }
            else if ((ctrl && shift && f3) || isAdvActive)
            {
                SetAdvancedStats();
                SetMidStats();

                if ((ctrl && shift && f3) || !isAdvActive)
                    isAdvActive = !isAdvActive;

                basicStats.SetActive(isAdvActive);
                midStats.SetActive(isAdvActive);
                advancedStats.SetActive(isAdvActive);
            }
            if (target != null)
                SetIndividualStats();

            individualStats.SetActive(target != null);

            if (Time.frameCount % inc == 0)
                UpdatePop();
        }

        private void SetMidStats()
        {
            if (midStats.transform.childCount == 0)
            {
                foreach (string species in Enum.GetNames(typeof(Species)))
                {
                    GameObject go = Instantiate(midStatsEntry, midStats.transform);

                    go.name = species;
                    go.GetComponent<Text>().text = species + " : " + GameObject.FindGameObjectsWithTag(species).Length;
                }
                return;
            }
            for (int i = 0; i < midStats.transform.childCount; i++)
            {
                GameObject go = midStats.transform.GetChild(i).gameObject;
                go.GetComponent<Text>().text = go.name + " : " + populations[go.name].values[populations[go.name].values.Count - 1];
            }
        }

        private void SetAdvancedStats()
        {
            List<Vector2> points = new List<Vector2>();
            float w = advancedViewport.GetComponent<RectTransform>().rect.width;
            float increment = w / valuesThreshold;

            GetMinMax(out int min, out int max);

            foreach (GraphData data in populations.Values)
            {
                points.Clear();

                foreach (int value in data.values)
                {
                    Vector2 point = new Vector2(0, 0);
                    if (points.Count != 0)
                    {
                        float trueX = Mathf.Lerp(0, w, points[points.Count - 1].x);
                        point.x = Mathf.InverseLerp(0, w, trueX + increment);
                    }
                    point.y = Mathf.InverseLerp(min, max, value);
                    points.Add(point);
                }

                data.renderer.Points = points.ToArray();
            }
        }

        private void SetBasicStats()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("BasicStats");

            foreach (GameObject go in gos)
            {
                go.GetComponent<Text>().text += " ";
                if (go.name == "entities")
                {
                    go.GetComponent<Text>().text += ((List<Entity>)parameters["entities"].value).Count;
                    continue;
                }
                go.GetComponent<Text>().text += parameters[go.name].value;
            }
        }

        private void SetIndividualStats()
        {
            var neat = target.GetComponent<Agents.NEAT>();
            if (neat == null)
                return;
            if (neat.Animal == null)
                return;

            var parameters = neat.Animal.parameters;

            float hunger = Mathf.InverseLerp(0.0f, (float)parameters["MAX_HUNGER"].value, (float)parameters["hunger"].value);
            float thirst = Mathf.InverseLerp(0.0f, (float)parameters["MAX_THIRST"].value, (float)parameters["thirst"].value);
            float age = Mathf.InverseLerp(0.0f, (float)parameters["MAX_AGE"].value, (float)parameters["age"].value);

            SetChild(hunger, (float)parameters["hunger"].value, hungerGO);
            SetChild(thirst, (float)parameters["thirst"].value, thirstGO);
            SetChild(age, (float)parameters["age"].value, ageGO, false);
        }

        private void UpdatePop()
        {
            string[] species = Enum.GetNames(typeof(Species));

            foreach (string sp in species)
            {
                int currentPop = GetLength(GameObject.FindGameObjectsWithTag(sp));
                populations[sp].values.Add(currentPop);
                if (populations[sp].values.Count > valuesThreshold)
                    populations[sp].values.RemoveAt(0);
            }
        }

        private int GetLength(GameObject[] pop)
        {
            int length = 0;

            foreach (GameObject go in pop)
            {
                Agents.NEAT comp = go.GetComponent<Agents.NEAT>();
                if (comp != null && comp.Animal != null && comp.Animal.parameters["isAlive"].value)
                    length++;
            }

            return length;
        }

        private void GetMinMax(out int min, out int max)
        {
            min = int.MaxValue;
            max = int.MinValue;

            foreach (GraphData species in populations.Values)
            {
                foreach (int pop in species.values)
                {
                    if (pop < min)
                        min = pop;
                    else if (pop > max)
                        max = pop;
                }
            }
        }

        private void SetColor(GameObject legend, ref List<Color> colors)
        {
            ProceduralImage image = legend.GetComponentInChildren<ProceduralImage>();

            int index = UnityEngine.Random.Range(0, colors.Count);
            image.color = colors[index];

            colors.RemoveAt(index);
        }

        private void UpdateTarget()
        {
            if (Input.GetMouseButton(1))
                target = null;
            if (!Input.GetMouseButton(0))
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
                    target = hit.collider.gameObject;
        }

        private void SetChild(float val, float trueVal, GameObject go, bool slider = true)
        {
            Text text = go.GetComponentInChildren<Text>();

            int index = text.text.IndexOf(':');
            if (index > 0)
                text.text = text.text.Substring(0, index + 1) + " " + Mathf.RoundToInt(trueVal);

            if (slider)
                go.GetComponent<Slider>().value = val;
        }
    }

    internal struct GraphData
    {
        public List<int> values;
        public UILineRenderer renderer;

        public GraphData(List<int> vals, UILineRenderer render, GameObject legend)
        {
            values = vals;
            renderer = render;
        }
    }
}
