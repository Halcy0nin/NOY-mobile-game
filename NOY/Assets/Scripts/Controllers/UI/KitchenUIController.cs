using UnityEngine;
using UnityEngine.UI;
using System;

public class KitchenUIController : MonoBehaviour
{
public Sprite[] healthyFoodImages;
public Sprite[] junkFoodImages;

public Button foodA, foodB, foodC;
public TMPro.TextMeshProUGUI food1, food2, food3;
public string food1Value, food2Value, food3Value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomizeFood();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeFood()
    {
        string[] foodOptions = { "Healthy Food", "Junk Food" };

        // Ensure at least one is healthy
        int healthyIndex = UnityEngine.Random.Range(0, 3); // Pick 0, 1, or 2 to be healthy

        food1Value = (healthyIndex == 0) ? "Healthy Food" : foodOptions[UnityEngine.Random.Range(0, 2)];
        food2Value = (healthyIndex == 1) ? "Healthy Food" : foodOptions[UnityEngine.Random.Range(0, 2)];
        food3Value = (healthyIndex == 2) ? "Healthy Food" : foodOptions[UnityEngine.Random.Range(0, 2)];

        food1.text = food1Value;
        food2.text = food2Value;
        food3.text = food3Value;

        SetRandomFoodSprite();
    }

    public void SetSpriteBasedOnType(Image targetImage, string foodType)
    {
    if (foodType == "Healthy Food")
    {
        targetImage.sprite = healthyFoodImages[UnityEngine.Random.Range(0, healthyFoodImages.Length)];
    }
    else if (foodType == "Junk Food")
    {
        targetImage.sprite = junkFoodImages[UnityEngine.Random.Range(0, junkFoodImages.Length)];
    }
    }

    public void SetRandomFoodSprite()
    {
        SetSpriteBasedOnType(foodA.GetComponent<Image>(), food1Value);
        SetSpriteBasedOnType(foodB.GetComponent<Image>(), food2Value);
        SetSpriteBasedOnType(foodC.GetComponent<Image>(), food3Value);
    }
}
