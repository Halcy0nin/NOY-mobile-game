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
    private bool hasSpawned = false;
    private const int dirtButtonCount = 10;

    void Update()
    {
        // Clamp hygiene
        if (needsController.hygiene > 100)
            needsController.hygiene = 100;

        // Allow dirt to spawn regardless of bathroom state
        if (needsController.hygiene <= 70 && !hasSpawned)
        {
            SpawnDirtButtons();
        }

        // Update interactivity (only clickable if in bathroom)
        UpdateButtonInteractivity();
    }

    private void SpawnDirtButtons()
    {
        hasSpawned = true;

        for (int i = 0; i < dirtButtonCount; i++)
        {
            GameObject newButton = CreateButton();
            AddClickToDestroy(newButton);
            RandomizePosition(newButton);
            spawnedButtons.Add(newButton);
        }
    }

    private GameObject CreateButton()
    {
        GameObject button = new GameObject("DirtButton", typeof(RectTransform), typeof(Button), typeof(Image));
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

    private void AddClickToDestroy(GameObject btn)
    {
        btn.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!isInBathroom) return;

            spawnedButtons.Remove(btn);
            Destroy(btn);

            if (spawnedButtons.Count == 0)
            {
                needsController.hygiene = 100;
                hasSpawned = false; // Allow re-spawn if hygiene decays again
            }
        });
    }

    private void RandomizePosition(GameObject btn)
    {
        RectTransform rt = btn.GetComponent<RectTransform>();
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        rt.anchoredPosition = new Vector2(randomX, randomY);
    }

    private void UpdateButtonInteractivity()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            if (btn != null)
            {
                btn.GetComponent<Button>().interactable = isInBathroom;
            }
        }
    }

    public void SetIsInBathroom(bool value)
    {
        isInBathroom = value;
    }
}
