using UnityEngine;
using UnityEngine.UI;

public class PetUIController : MonoBehaviour
{
    public Image hungerMeter, energyMeter, sleepMeter, hygieneMeter;
    public NeedsController needsController;

    void Update()
    {
        if (needsController != null)
        {
            UpdateMeter(hungerMeter, needsController.food);
            UpdateMeter(energyMeter, needsController.energy);
            UpdateMeter(hygieneMeter, needsController.hygiene);
            UpdateMeter(sleepMeter, needsController.sleep);
        }
    }

    void UpdateMeter(Image meter, float value)
    {
        float fillAmount = value / 100f;
        meter.fillAmount = fillAmount;
        meter.color = value < 50 ? Color.red : Color.green;
    }
}
