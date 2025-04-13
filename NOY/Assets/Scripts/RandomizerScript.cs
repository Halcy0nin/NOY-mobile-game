using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class RandomizerScript : MonoBehaviour
{
    public GameObject buttonPrefab;
    public RectTransform parentContainer;
    public float minX = -90f;
    public float maxX = 180f;
    public float minY = -240f;
    public float maxY = 335f;
    public NeedsController needsController; 
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Start()
    {
    }
    void Update(){
        if (needsController.hygiene > 100)
        {
            needsController.hygiene = 100;
        }
    }
    public void SpawnButtonsBasedOnPercentage()
    {
        int numberOfButtons = Mathf.Clamp(2 + (100 - needsController.hygiene) / 20 * 2, 2, 10);

        while (spawnedButtons.Count < numberOfButtons)
        {
            GameObject newButton = Instantiate(buttonPrefab, parentContainer);
            newButton.SetActive(false);
            AddClickToHide(newButton);
            spawnedButtons.Add(newButton);
        }

        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            if (i < numberOfButtons)
            {
                GameObject btn = spawnedButtons[i];
                btn.SetActive(true);

                RectTransform rt = btn.GetComponent<RectTransform>();
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);
                rt.anchoredPosition = new Vector2(randomX, randomY);
            }
            else
            {
                spawnedButtons[i].SetActive(false);
            }
        }
    }

    void AddClickToHide(GameObject btn)
    {
        btn.GetComponent<Button>().onClick.AddListener(() => {needsController.hygiene += 5;btn.SetActive(false);});
    }
}
