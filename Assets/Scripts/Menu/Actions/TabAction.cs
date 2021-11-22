using UnityEngine;

namespace Menu
{
    class TabAction : MonoBehaviour, ButtonAction
    {
        public DisplaySpecies.SpeciesType species;
        public DisplaySpecies display;

        public void Action()
        {
            display.redraw = true;
            display.speciesType = species;
        }
    }
}
