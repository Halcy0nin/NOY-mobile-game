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
        food1.text = foodOptions[UnityEngine.Random.Range(0, 2)]; // Corrected range to include both indices
        food2.text = foodOptions[UnityEngine.Random.Range(0, 2)]; // Corrected range to include both indices
        food3.text = foodOptions[UnityEngine.Random.Range(0, 2)]; // Corrected range to include both indices
        food1Value = food1.text;
        food2Value = food2.text;
        food3Value = food3.text;

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
