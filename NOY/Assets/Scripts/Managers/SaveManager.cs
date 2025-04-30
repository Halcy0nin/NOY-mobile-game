using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string savePath;

    // This is called when the SaveManager is created/initialized
    void Awake()
    {
        // Set the path where the JSON file will be saved (inside Scripts/SaveFile)
        savePath = Path.Combine(Application.persistentDataPath, "pet_save.json");

        // Log the save path for debugging purposes
        Debug.Log("Saving to: " + savePath);
    }

    // Save the data from NeedsController into a JSON file
    public void SaveData(NeedsController needs)
    {
        // Create a PetSaveData object and populate it with the values from NeedsController
        PetSaveData data = new PetSaveData
        {
            food = needs.food,
            sleep = needs.sleep,
            energy = needs.energy,
            hygiene = needs.hygiene,
            lastSavedTime = System.DateTime.Now.ToBinary().ToString() // Save the current time
        };

        // Convert the data to a JSON string
        string json = JsonUtility.ToJson(data, true);

        // Ensure the directory exists before saving the file
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

        // Write the JSON string to the file
        File.WriteAllText(savePath, json);
        Debug.Log("Data saved to: " + savePath);
    }

    // Load the saved data from the JSON file
    public PetSaveData LoadData()
    {
        if (File.Exists(savePath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(savePath);

            // Convert the JSON string back to a PetSaveData object
            PetSaveData data = JsonUtility.FromJson<PetSaveData>(json);
            Debug.Log("Data loaded from: " + savePath);
            return data;
        }
        else
        {
            Debug.Log("No saved data found.");
            return null;
        }
    }
        // Save just the high score
    public void SaveHighScore(int score)
    {
        PetSaveData data = LoadData() ?? new PetSaveData();
        data.highScore = score;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("High score saved to: " + savePath);
    }

    // Load just the high score
    public int LoadHighScore()
    {
        PetSaveData data = LoadData();
        return data != null ? data.highScore : 0;
    }

}