using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuAlphaThreshold : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        }
    }
}
