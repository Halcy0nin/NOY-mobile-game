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
            if(hungerfillAmount <= .3f){
                hungerMeter.color = Color.red;
            }else if (hungerfillAmount > .3f){
                hungerMeter.color = Color.green;
            }
            float energyfillAmount = (float) needsController.energy / 100;
            energyMeter.fillAmount= energyfillAmount;
            if(energyfillAmount <= .3f){
                energyMeter.color = Color.red;
            }else if (energyfillAmount > .3f){
                hungerMeter.color = Color.green;
            }
            float hygienefillAmount = (float) needsController.hygiene / 100;
            hygieneMeter.fillAmount= hygienefillAmount;
            if(hygienefillAmount <= .3f){
                hygieneMeter.color = Color.red;
            }else if (hygienefillAmount > .3f){
                hungerMeter.color = Color.green;
            }
            float sleepfillAmount = (float) needsController.sleep / 100;
            sleepMeter.fillAmount= sleepfillAmount;
            if(sleepfillAmount <= .3f){
                sleepMeter.color = Color.red;
            }else if (sleepfillAmount > .3f){
                hungerMeter.color = Color.green;
            }

        }
    }
    
}
