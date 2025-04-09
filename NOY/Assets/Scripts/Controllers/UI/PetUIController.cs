using UnityEngine;
using UnityEngine.UI;

public class PetUIController : MonoBehaviour
{

    public Image hungerMeter,energyMeter,sleepMeter,hygieneMeter;
    public NeedsController needsController;

    void Update()
    {
        if (needsController != null)
        {      
            float hungerfillAmount = (float) needsController.food / 100;
            hungerMeter.fillAmount= hungerfillAmount;
            float energyfillAmount = (float) needsController.energy / 100;
            energyMeter.fillAmount= energyfillAmount;
            float hygienefillAmount = (float) needsController.hygiene / 100;
            hygieneMeter.fillAmount= hygienefillAmount;
            float sleepfillAmount = (float) needsController.sleep / 100;
            sleepMeter.fillAmount= sleepfillAmount;

        }
    }
    
}
