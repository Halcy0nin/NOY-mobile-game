using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class NeedsController : MonoBehaviour
{
   public KitchenUIController kitchenUI;
   private int cooldownsRemaining = 0;
   public float CoRoutineValue;
   public string food1Value, food2Value, food3Value;
   public int foodTickRate, sleepTickRate, energyTickRate, hygieneTickRate;
   public TMPro.TextMeshProUGUI food1, food2, food3;

   private SaveManager saveManager;

   public int food, sleep ,energy, hygiene;


   public bool Sleeping = false;
   public int SleepRecoveryRate;

   public void Initialize( int food, int sleep, int energy, int hygiene)
   {
       this.food = food;
       this.sleep = sleep;
       this.energy = energy;
       this.hygiene = hygiene;
   }
   private void Update()
   {
        if(TimingManager.gameTimer <= 0)
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
        if (food > 100)
        {
            food = 100;
        }
        if (hygiene > 100)
        {
            hygiene = 100;
        }
        if (sleep > 100)
        {
            sleep = 100;
        }
        if (energy > 100)
        {
            energy = 100;
        }
   }
    public void Start()
    {
        saveManager = FindFirstObjectByType<SaveManager>();

        // Load data from JSON
        PetSaveData data = saveManager.LoadData();
        
        if (data != null)
        {
            // Restore stats from loaded data
            Initialize(data.food, data.sleep, data.energy, data.hygiene);

            // Calculate the time away using the saved timestamp
            DateTime lastSavedTime = DateTime.FromBinary(Convert.ToInt64(data.lastSavedTime));
            TimeSpan timeAway = DateTime.Now - lastSavedTime;

            // Get the minutes away
            float minutesAway = (float)timeAway.TotalMinutes;
            Debug.Log("Time away: " + minutesAway + " minutes");

            // Apply offline decay based on the time away
            ApplyOfflineDecay(minutesAway);
        }
    }
   public void ChangeFoodStats(int amount)
   {
    food += amount;
    if (food < 0)
    {
        PetManager.Instance.Death();
        food= 5;
    }
   }
   public void ChangeSleepStats(int amount)
   {
    sleep += amount;
    if (sleep < 0)
    {
        PetManager.Instance.Death();
        sleep=5;
    }
   }
   public void ChangeEnergyStats(int amount)
   {
    energy += amount;
    if (energy < 0)
    {
        PetManager.Instance.Death();
        energy=5;
    }
   }
   public void ChangeHygieneStats(int amount)
   {
    hygiene += amount;
    if (hygiene < 0)
    {
        PetManager.Instance.Death();
        hygiene=5;
    }
   
   }
   public void FoodChoice1()
    {
        ProcessFood(kitchenUI.food1Value, kitchenUI.foodA);
    }

    public void FoodChoice2()
    {
        ProcessFood(kitchenUI.food2Value, kitchenUI.foodB);
    }

    public void FoodChoice3()
    {
        ProcessFood(kitchenUI.food3Value, kitchenUI.foodC);
    }

    void ProcessFood(string foodType, Button button)
    {
        if (!button.interactable) return;
        if (foodType == "Healthy Food") HealthyFood();
        else if (foodType == "Junk Food") JunkFood();
        cooldownsRemaining++;
        StartCoroutine(FoodCooldown(button, CoRoutineValue)); 
    }

    IEnumerator FoodCooldown(Button button, float cooldownDuration)
    {
        button.interactable = false;
        yield return new WaitForSeconds(cooldownDuration);
        button.interactable = true;

        cooldownsRemaining--;

        // If all food buttons have finished cooldown, randomize again
        if (cooldownsRemaining <= 0)
        {
            kitchenUI.RandomizeFood();
        }
    }

    public void JunkFood()
    {
        food += 20;
        energy += 10;
        Debug.Log("Ate junk food");
    }

    public void HealthyFood()
    {
        food += 20;
        energy += 20;
        Debug.Log("Ate healthy food");
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
    void OnApplicationQuit()
    {
        saveManager.SaveData(this);
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            saveManager.SaveData(this);
    }
    public void ApplyOfflineDecay(float minutesAway)
    {
        int foodLoss = Mathf.RoundToInt(foodTickRate * minutesAway);
        int sleepLoss = Mathf.RoundToInt(sleepTickRate * minutesAway);
        int energyLoss = Mathf.RoundToInt(energyTickRate * minutesAway);
        int hygieneLoss = Mathf.RoundToInt(hygieneTickRate * minutesAway);

        ChangeFoodStats(-foodLoss);
        
        if (!Sleeping)
        {
            ChangeSleepStats(-sleepLoss);
            ChangeEnergyStats(-energyLoss);
        }
        else
        {
            Sleep(Mathf.RoundToInt(SleepRecoveryRate * minutesAway));
        }

        ChangeHygieneStats(-hygieneLoss);
    }
}
