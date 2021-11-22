using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animals
{
    class Deer : Herbivorous
    {
        public Deer() : base()
        {
            targets.Add(Species.Carrot);
        }
    }
}
