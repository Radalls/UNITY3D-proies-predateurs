using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    [Serializable]
    public class Wolf : Carnivorous
    {
        public Wolf() : base()
        {
            targets.Add(Species.Rabbit);
            targets.Add(Species.Deer);
        }
    }
}
