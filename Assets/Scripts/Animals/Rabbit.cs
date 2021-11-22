using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animals
{
    [Serializable]
    public class Rabbit : Herbivorous
    {
        public Rabbit() : base()
        {
            targets.Add(Species.Carrot);
        }
    }
}
