using UnityEngine;

public class NeedsController : MonoBehaviour
{
   public int food, sleep ,energy, hygiene;
   public bool Sleeping = false;
   public int SleepRecoveryRate;
   public int foodTickRate, sleepTickRate, energyTickRate, hygieneTickRate;
   public void Initialize( int food, int sleep, int energy, int hygiene)
   {
       this.food = food;
       this.sleep = sleep;
       this.energy = energy;
       this.hygiene = hygiene;
   }
   private void Update()
   {
        if(TimingManager.gameHourTimer <= 0)
        {
            ChangeFoodStats(-foodTickRate);
            if (sleep > 0 && Sleeping == false )
            {
            ChangeSleepStats(-sleepTickRate);
            ChangeEnergyStats(-energyTickRate);
            }
            else if (Sleeping == true)
            {
                Sleep(SleepRecoveryRate);
            }
            else{
                Debug.Log("Sleep not calculated properly");
            }
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
   public void ChangeSleepStats(int amount)
   {
    sleep += amount;
    if (sleep < 0)
    {
        PetManager.Instance.Death();
    }
    else if (sleep > 100)
    {
        sleep = 100;
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
    public void JunkFood()
    {
        
        food += 20;
        energy += 10;
        Debug.Log("I ate nigga food");
    }
    public void HealthyFood()
    {
        
        Debug.Log("I ate white food");
        food += 20;
        energy += 20;
    }
    public void Sleep(int amount){
        
        Debug.Log("I am sleeping n");
        Sleeping = true;
        sleep += amount;
        energy += amount;
        if (sleep > 100)
        {
            sleep = 100;
        }
        if (energy > 100)
        {
            energy = 100;
        }
    }
    public void WakingUp(){
        Sleeping = false;
    }
    public void CharacterSleeping()
    {
        if (sleep == 0)
        {
            Debug.Log("Can't play Minigames");
        }
    }
    public void ToothbrushAction()
    {
        
        Debug.Log("I am toothbrushing");
        hygiene += 20;
    }
    public void ShowerAction()
    {
        Debug.Log("I am taking a bath");
        hygiene += 20;
    }
}
