using UnityEngine;
using UnityEngine.UI;
using System;

public class NeedsController : MonoBehaviour
{
   private KitchenUIController kitchenUI;
   private SaveManager saveManager;
   public int food, sleep ,energy, hygiene;
   public string food1Value, food2Value, food3Value;
   public bool Sleeping = false;
   public int SleepRecoveryRate;
   public int foodTickRate, sleepTickRate, energyTickRate, hygieneTickRate;
   public TMPro.TextMeshProUGUI food1, food2, food3;
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
        sleep=5;
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
        energy=5;
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
        hygiene=5;
    }
    else if (hygiene > 100)
    {
        hygiene = 100;
    }
   
   }
   public void FoodChoice()
{
    if (kitchenUI.food1Value == "Healthy Food") HealthyFood();
    else if (kitchenUI.food1Value == "Junk Food") JunkFood();

    if (kitchenUI.food2Value == "Healthy Food") HealthyFood();
    else if (kitchenUI.food2Value == "Junk Food") JunkFood();

    if (kitchenUI.food3Value == "Healthy Food") HealthyFood();
    else if (kitchenUI.food3Value == "Junk Food") JunkFood();
}


    public void JunkFood(){    
    public void FoodChoice()
    {
        if (food1Value == "Healthy Food")
        {
            HealthyFood();
        }
        else if (food1Value == "Junk Food")
        {
            JunkFood();
        }
        else if (food2Value == "Healthy Food")
        {
            HealthyFood();
        }
        else if (food2Value == "Junk Food")
        {
            JunkFood();
        }
        else if (food3Value == "Healthy Food")
        {
            HealthyFood();
        }
        else if (food3Value == "Junk Food")
        {
            JunkFood();
        }
    }
    public void JunkFood(){    
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
    public void RandomizeFood()
    {
        string[] foodOptions = { "Healthy Food", "Junk Food" };
        food1.text = foodOptions[UnityEngine.Random.Range(0, 2)]; // Corrected range to include both indices
        food2.text = foodOptions[UnityEngine.Random.Range(0, 2)]; // Corrected range to include both indices
        food3.text = foodOptions[UnityEngine.Random.Range(0, 2)]; // Corrected range to include both indices
        food1Value = food1.text;
        food2Value = food2.text;
        food3Value = food3.text;
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
