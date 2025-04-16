using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RandomizerScript : MonoBehaviour
{
    public RectTransform parentContainer;
    public float minX = -90f;
    public float maxX = 180f;
    public float minY = -240f;
    public float maxY = 335f;
    public NeedsController needsController;
    public List<Sprite> buttonSprites;

    public bool isInBathroom = false;

    private List<GameObject> spawnedButtons = new List<GameObject>();
    private int currentRange = -1;

    void Start()
    {
        UpdateButtonsBasedOnHygiene();
    }

    void Update()
    {
        if (needsController.hygiene > 100)
            needsController.hygiene = 100;

        UpdateButtonsBasedOnHygiene();
        UpdateButtonInteractivity();
    }

    void UpdateButtonsBasedOnHygiene()
    {
        int newRange = GetRangeFromHygiene(needsController.hygiene);
        if (newRange != currentRange)
        {
            currentRange = newRange;
            SpawnButtonsForRange(currentRange);
            // No more call to RandomizeButtonPositions()
        }
    }


    int GetRangeFromHygiene(float hygiene)
    {
        if (hygiene >= 80) return 0;
        else if (hygiene >= 60) return 1;
        else if (hygiene >= 40) return 2;
        else if (hygiene >= 20) return 3;
        else return 4;
    }

    int GetButtonCountForRange(int range)
    {
        switch (range)
        {
            case 0: return 2;
            case 1: return 4;
            case 2: return 6;
            case 3: return 8;
            case 4: return 10;
            default: return 2;
        }
    }

    void SpawnButtonsForRange(int range)
    {
        int numberOfButtons = GetButtonCountForRange(range);
        int existingCount = spawnedButtons.Count;

        // Create only the missing buttons
        for (int i = existingCount; i < numberOfButtons; i++)
        {
            GameObject newButton = CreateButton();
            AddClickToHide(newButton);
            spawnedButtons.Add(newButton);

            // Only randomize position for newly spawned
            RandomizePosition(newButton);
        }

        // Enable only up to the required number
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            spawnedButtons[i].SetActive(i < numberOfButtons);
        }
    }

    void RandomizePosition(GameObject btn)
    {
        RectTransform rt = btn.GetComponent<RectTransform>();
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        rt.anchoredPosition = new Vector2(randomX, randomY);
    }


    private GameObject CreateButton()
    {
        GameObject button = new GameObject("Button", typeof(RectTransform), typeof(Button), typeof(Image));
        button.transform.SetParent(parentContainer, false);
        RectTransform rt = button.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(118, 110);

        Image img = button.GetComponent<Image>();
        if (buttonSprites != null && buttonSprites.Count > 0)
        {
            img.sprite = buttonSprites[Random.Range(0, buttonSprites.Count)];
        }
        return button;
    }

    void AddClickToHide(GameObject btn)
    {
        btn.GetComponent<Button>().onClick.AddListener(() =>
        {
            needsController.hygiene += 5;
            btn.SetActive(false);
        });
    }

    void UpdateButtonInteractivity()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            if (btn != null)
            {
                Button buttonComp = btn.GetComponent<Button>();
                buttonComp.interactable = isInBathroom;
            }
        }
    }

    public void SetIsInBathroom(bool value)
    {
        isInBathroom = value;
    }
}
