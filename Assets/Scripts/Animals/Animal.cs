using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;
using Agents;
using UnityEngine.AI;

namespace Animals
{
    [Serializable]
    public abstract class Animal : Entity
    {
        public enum ReproductionState { NONE, BIRTH_GIVER, PARTNER }

        public List<Species> targets = new List<Species>();
        public Transform transform;
        public Transform target;
        public GameObject targetGO;
        private Animator animator;
        //private bool goToDrink;
        private bool goToMate = false;

        private ReproductionState reproductionState = ReproductionState.NONE;
        private int nbRep = 0;
        private int maxRep = 2;

        private readonly float fausseCouche = 0.5f;

        public Animal() : base()
        {
            parameters.Add("hunger", new Parameters.ParameterEntry("hunger", 0.0, false)); // actual hunger (if low will seek a target, if 0 dies)
            parameters.Add("MAX_HUNGER", new Parameters.ParameterEntry("MAX_HUNGER", "Niveau de satiété", 0, Parameters.ParameterEntry.Type.Slider)); // hunger cap (if reached will not try to eat more)
            parameters.Add("thirst", new Parameters.ParameterEntry("thirst", 0.0, false)); // actuel thirst (if low will seek water, if 0 dies)
            parameters.Add("MAX_THIRST", new Parameters.ParameterEntry("MAX_THIRST", "Niveau de satiété hydrique", 0, Parameters.ParameterEntry.Type.Slider)); // thirst cap (if reached will not try to drink more)
            parameters.Add("runningSpeed", new Parameters.ParameterEntry("runningSpeed", 0.0, false)); // the actual speed
            parameters.Add("MAX_RUN_SPEED", new Parameters.ParameterEntry("MAX_RUN_SPEED", "Vitesse maximale", 0, Parameters.ParameterEntry.Type.Slider)); // the maximum speed
            parameters.Add("isMale", new Parameters.ParameterEntry("isMale", false, false)); // gender (true = male, false = female)
            parameters.Add("pregnancyTime", new Parameters.ParameterEntry("pregnancyTime", "Temps de gestation", 0, Parameters.ParameterEntry.Type.Slider)); // duration of pregnancy
            parameters.Add("nbOfBabyPerLitter", new Parameters.ParameterEntry("nbOfBabyPerLitter", "Nombre d'enfant par gestation", 0, Parameters.ParameterEntry.Type.Slider)); // how many babies are born in one go
            parameters.Add("interactionLevel", new Parameters.ParameterEntry("interactionLevel", "Niveau d'interaction", 0, Parameters.ParameterEntry.Type.Slider)); // measures how the animal interact with other animals (negative = afraid, 0 = neutral, positive = aggressive)
            parameters.Add("HPMax", new Parameters.ParameterEntry("HPMax", "Niveau de vie maximal", 0, Parameters.ParameterEntry.Type.Slider)); // Hit points of the animal
            parameters.Add("HP", new Parameters.ParameterEntry("HP", 0.0, false)); // Current hit points of the animal
            parameters.Add("Atk", new Parameters.ParameterEntry("Atk", "Niveau d'attaque maximal", 0, Parameters.ParameterEntry.Type.Slider)); // Atk points of the animal
        }

        /// <summary>
        /// lancement des animations
        /// </summary>
        public void Start()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        /// <summary>
        /// author : Ghali Boucetta
        /// Vérification de l'état de soif d'un agent
        /// </summary>
        /// <returns>true si la valeur de soif actuelle est inférieure ou égale aux 2/3 de la valeur maximale de l'espèce</returns>

        public bool isThirsty()
        {
            //return true;
            return parameters["thirst"].value < parameters["MAX_THIRST"].value * .6; // the animal is thirsty if its current thirst level is under the third of the max
        }

        /// <summary>
        /// author : Ghali Boucetta
        /// Vérification de l'état de faim d'un agent
        /// </summary>
        /// <returns>true si la valeur de faim actuelle est inférieure ou égale aux 2/3 de la valeur maximale de l'espèce</returns>
        /// 
        public bool isHungry()
        {
            return parameters["hunger"].value < parameters["MAX_HUNGER"].value * .6; // the animal is hungry if its current hunger level is under the third of the max
        }

        /// <summary>
        /// author : Anis Koraichi
        /// Vérifie si un agent a besoin de se reproduire
        /// </summary>
        /// <returns>true si l'agent est d'âge adulte et s'il est en période de chaleur</returns>
        public bool needsToReproduce()
        {
            System.Random rnd = new System.Random();
            return (parameters["age"].value >= parameters["ADULT_AGE"].value) && (rnd.Next() % 2 == 0);
        }

        /// <summary>
        /// author : Ghali Boucetta
        /// </summary>
        /// <returns></returns>
        public bool targetIsMate()
        {
            return targetGO.GetComponent<NEAT>().Animal.GetType() == GetType();
        }

        /// <summary>
        /// author : Ghali Boucetta & Anis Koraichi
        /// État par défaut d'un agent, il se déplace dans des direction aléatoire
        /// </summary>
        public void lookAround()
        {
            NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();

            if (!agent.hasPath)
            {
                MapGenerator mapGenerator = MapGenerator.instance;
                Vector3 dest;
                do
                {
                    dest = MapGenerator.GetRandomPointOnMesh(gameObject.transform.position, gameObject.transform.localScale.z * parameters["MAX_RUN_SPEED"].value);
                } while (!agent.SetDestination(dest));

                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
        }
        /// <summary>
        /// author : Anis Koraichi
        /// Recherche d'un partenaire dans le but de se reproduire. L'agent se déplace dans une direction aléatoire jusqu'à trouver un partenaire. Une fois le partenaire trouver, il se déplacera vers lui
        /// </summary>
        private void lookForMate()
        {
            if (nbRep >= maxRep)
                return;

            SightManager sightManager = gameObject.GetComponent<SightManager>();
            Dictionary<GameObject, Vector3> sight = sightManager.gameObjects;

            foreach (var entry in sight)
            {
                GameObject sightable = entry.Key;
                if (sightable != null && gameObject.tag == sightable.tag)
                {
                    Animal animal = sightable.GetComponent<NEAT>().Animal as Animal;
                    if (animal.nbRep >= maxRep && animal.target != null && !animal.needsToReproduce())
                        continue;

                    target = sightable.transform;
                    targetGO = sightable;
                    animal.target = gameObject.transform;
                    animal.targetGO = gameObject;
                    //goToDrink = false;
                    goToMate = true;
                    break;
                }
            }
        }

        /// <summary>
        /// author : Ghali Boucetta & Anis Koraichi
        /// Recherche de nourriture
        /// </summary>
        private void lookForFood()
        {
            SightManager sightManager = gameObject.GetComponent<SightManager>();
            Dictionary<GameObject, Vector3> sight = sightManager.gameObjects;

            foreach (var entry in sight)
            {
                GameObject sightable = entry.Key;
                if (sightable != null && targets.Contains(sightable.GetComponent<NEAT>().species))
                {
                    target = sightable.transform;
                    targetGO = sightable;
                    //goToDrink = false;
                    goToMate = false;
                    break;
                }
            }
        }

        /// <summary>
        /// author : Ghali Boucetta
        /// Mécanisme de fuite de l'agent. S'il voit un prédateur, il doit aller dans la direction opposé de celle de ce dernier
        /// </summary>
        private void flee()
        {
            SightManager sightManager = gameObject.GetComponent<SightManager>();
            Dictionary<GameObject, Vector3> sight = sightManager.gameObjects;

            if (sight == null)
                return;
            foreach (var entry in sight)
            {
                GameObject sightable = entry.Key;

                if (sightable == null || sightable.GetComponent<NEAT>() == null)
                    continue;
                Entity entity = sightable.GetComponent<NEAT>().Animal;

                if (entity is Animal)
                {
                    bool isAnimalSightedPredator = (entity as Animal).targets.Contains(gameObject.GetComponent<NEAT>().species);
                    if (isAnimalSightedPredator)
                    {
                        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
                        Vector3 predator = sightable.transform.position;
                        Vector3 current = gameObject.transform.position;

                        agent.SetDestination(current + (current - predator).normalized * parameters["MAX_RUN_SPEED"].value);
                        //goToDrink = false;
                        goToMate = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// author : Anis Koraichi
        /// Recherche d'eau
        /// </summary>
        private void lookForWater()
        {
            Debug.Log(gameObject.tag + " is looking for water");
            SightManager sightManager = gameObject.GetComponent<SightManager>();
            Dictionary<GameObject, Vector3> sight = sightManager.gameObjects;

            foreach (var entry in sight)
            {
                if (entry.Key.name == "Water")
                {
                    Debug.Log(gameObject.tag + " found water");
                    targetGO = entry.Key;
                    target = targetGO.transform;
                    //goToDrink = true;
                    break;
                }
            }
        }
        /// <summary>
        /// author : Ghali Boucetta
        /// Mécanisme de traque. L'agent se déplace vers la cible en question et si celle-ci est un partenaire, appelle la fonction reproduce(), sinon appelle attack()
        /// </summary>
        private void moveToTarget()
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(target.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);

            Entity entity = target.gameObject.GetComponent<NEAT>().Animal;
            if (targetIsMate())
                reproduce(entity as Animal);
            else
                attack(entity);
        }

        float timeSinceRep = 0;

        /// <summary>
        /// author : Ghali Boucetta
        /// Reproduction de deux agents. L'agent courant doit se déplacer vers sa cible dans le but de se reproduire
        /// </summary>
        private void reproduce(Animal animal)
        {
#if UNITY_EDITOR
            float t = 30;
#else
            float t = 1000;
#endif

            goToMate = false;

            if (reproductionState != ReproductionState.NONE)
                return;
            if ((timeSinceRep == 0 || Time.realtimeSinceStartup - timeSinceRep >= t) && Vector3.Distance(gameObject.transform.position, target.position) <= target.localScale.z)
            {
                if (nbRep >= maxRep || animal.nbRep >= maxRep)
                {
                    targetGO = null;
                    target = null;
                    return;
                }

                reproductionState = UnityEngine.Random.value < 0.5f ? ReproductionState.BIRTH_GIVER : ReproductionState.PARTNER;
                if (reproductionState == ReproductionState.BIRTH_GIVER)
                    animal.reproductionState = ReproductionState.PARTNER;
                else
                    animal.reproductionState = ReproductionState.BIRTH_GIVER;
            }
        }

        /// <summary>
        /// author : Anis Koraichi
        /// Méthode représentant le combat entre le prédateur et sa proie. Lorsque le prédateur réussi à s'approcher de sa proie, il la frappe retirant à ses HP une valeur égale à celle d'ATK
        /// </summary>
    
        private void attack(Entity entity)
        {
            if (entity is Carrot)
            {
                entity.parameters["isAlive"].value = false;
                return;
            }
            if (Vector3.Distance(gameObject.transform.position, target.position) <= 10)
            {
                animator.SetBool("isAttacking", true);
                int HP;
                int ATK;
                if (entity.parameters.ContainsKey("HP"))
                {
                    HP = entity.parameters["HP"].value;
                }
                else HP = 1;

                if (parameters.ContainsKey("ATK"))
                {
                    ATK = parameters["ATK"].value;

                }
                else ATK = 1;

                HP -= ATK;
                entity.parameters["HP"].value = HP;

                animator.SetBool("isAttacking", false);
            }
        }

        /// <summary>
        /// author : Anis Koraichi
        /// Acte de manger d'un agent, sa valeur de satiété remonte au maximum et le gamObject représentant sa nourriture est détruit
        /// </summary>
        /// <param name="go"></param>
        public void eat(GameObject go)
        {
            parameters["hunger"].value = parameters["MAX_HUNGER"].value; // hunger is refilled
            gameObject.GetComponent<SightManager>().gameObjects.Remove(targetGO);
            Destroy(go);

            target = null;
            targetGO = null;
            gameObject.GetComponent<NavMeshAgent>().ResetPath();
        }

        /// <summary>
        /// author : Anis Koraichi
        /// Acte de boire d'un agent, sa valeur de soif remonte au maximum
        /// </summary>
        /// <param name="go"></param>
        public void drink()
        {
            parameters["thirst"].value = parameters["MAX_THIRST"].value; // thirst is refilled
        }

        /// <summary>
        /// author : Ghali Boucetta
        /// Une fois que deux agents sont prêts à se reproduire, un nouveau gameObject est créé représentant l'enfant
        /// </summary>
        private void giveBirth()
        {
            if (targetIsMate())
            {
                if (UnityEngine.Random.value < 1 - fausseCouche)
                    GameObject.Instantiate(gameObject, gameObject.transform.parent);
                (targetGO.GetComponent<NEAT>().Animal as Animal).reproductionState = ReproductionState.NONE;
                timeSinceRep = Time.realtimeSinceStartup;

                nbRep++;
                (targetGO.GetComponent<NEAT>().Animal as Animal).nbRep++;
            }
            reproductionState = ReproductionState.NONE;

            target = null;
            targetGO = null;
        }

        /// <summary>
        /// author : Anis Koraichi
        /// Méthode représentant la mort et les différentes raisons qui peuvent la causer. Lorsqu'une raison est respectée, lancement de l'animation de mort puis initialisation de "timeSinceDeath"
        /// </summary>
        private void death()
        {
            if (!parameters["isAlive"].value)
                return;
            if (parameters["HP"].value <= 0 || parameters["age"].value >= parameters["MAX_AGE"].value
                || parameters["hunger"].value <= 0 || parameters["thirst"].value <= 0)
            {
                animator.SetBool("died", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                parameters["isAlive"].value = false;
                parameters.Add("timeSinceDeath", new Parameters.ParameterEntry("timeSinceDeath", 0, false));

                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
        }

        /// <summary>
        /// author : Ghali Boucetta & Anis Koraichi
        /// Méthode de simulation du temps, à chaque appel, incrémente les valeurs nécessaires par coeff
        /// </summary>
        private void time()
        {
#if UNITY_EDITOR
            float coeff = 1f;
#else
            float coeff = 0.02f;
#endif

            //parameters["thirst"].value -= Time.fixedDeltaTime;
            if (parameters["isAlive"].value)
            {
                parameters["hunger"].value -= Time.fixedDeltaTime * coeff;
                parameters["age"].value += Time.fixedDeltaTime * coeff;
            }
            else
                parameters["timeSinceDeath"].value += Time.fixedDeltaTime * coeff;

            death();
        }

        public float lastAction = -1;

        /// <summary>
        /// author : Ghali Boucetta & Anis Koraichi
        /// Boucle principale de la simulation, appel des méthodes nécessaires lorsque les conditions sont respectées.
        /// </summary>
        override public void FixedUpdate()
        {
            base.FixedUpdate();

            time();

            if (!parameters["isAlive"].value)
            {
#if UNITY_EDITOR
                if (parameters["timeSinceDeath"].value >= 30)
#else
               
                if (parameters["timeSinceDeath"].value >= 90)
#endif
                    Destroy(gameObject);
                return;
            }

            if (target != null && isHungry() && goToMate)
            {
                target = null;
                goToMate = false;
            }

            if (target == null)
            {
                if (isHungry())
                    lookForFood();
                //else if (isThirsty())
                //    lookForWater();
                else if (needsToReproduce())
                    lookForMate();
                else
                {
                    target = null;
                    targetGO = null;
                }
                lookAround();
            }
            else
            {
                moveToTarget();

                if (reproductionState == ReproductionState.BIRTH_GIVER)
                    giveBirth();
                if (targetGO != null && !targetIsMate() && Vector3.Distance(target.position, gameObject.transform.position) < 2 && !target.gameObject.GetComponent<NEAT>().Animal.parameters["isAlive"].value)
                {
                    //if (goToDrink)
                    //    drink();
                    //else
                    eat(target.gameObject);
                }
            }

            flee();
        }
    }
}

