using UnityEngine;

public class NeedsController : MonoBehaviour
{
   public int food, happiness ,energy, hygiene;
   public int foodTickRate, happinessTickRate, energyTickRate, hygieneTickRate;
   public void Initialize( int food, int happiness, int energy, int hygiene)
   {
       this.food = food;
       this.happiness = happiness;
       this.energy = energy;
       this.hygiene = hygiene;
   }
   private void Update()
   {
        if(TimingManager.gameHourTimer <= 0)
        {
            ChangeFoodStats(-foodTickRate);
            ChangeHappinessStats(-happinessTickRate);
            ChangeEnergyStats(-energyTickRate);
            ChangeHygieneStats(-hygieneTickRate);
        }
   }
   public void ChangeFoodStats(int amount)
   {
    food += amount;
    if (food < 0)
    {
        PetManager.Instance.Death();
    }
    else if (food > 100)
    {
        food = 100;
    }
   }
   public void ChangeHappinessStats(int amount)
   {
    happiness += amount;
    if (happiness < 0)
    {
        PetManager.Instance.Death();
    }
    else if (happiness > 100)
    {
        happiness = 100;
    }
   }
   public void ChangeEnergyStats(int amount)
   {
    energy += amount;
    if (energy < 0)
    {
        PetManager.Instance.Death();
    }
    else if (energy > 100)
    {
        energy = 100;
    }
   }
   public void ChangeHygieneStats(int amount)
   {
    hygiene += amount;
    if (hygiene < 0)
    {
        PetManager.Instance.Death();
    }
    else if (hygiene > 100)
    {
        hygiene = 100;
    }
   }
}
