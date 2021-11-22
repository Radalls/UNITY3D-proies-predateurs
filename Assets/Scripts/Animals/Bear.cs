using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animals
{
    class Bear : Carnivorous
    {
        public Bear() : base()
        {
            targets.Add(Species.Wolf);
            targets.Add(Species.Deer);
            targets.Add(Species.Cat);
            targets.Add(Species.Rabbit);
        }
    }
}
