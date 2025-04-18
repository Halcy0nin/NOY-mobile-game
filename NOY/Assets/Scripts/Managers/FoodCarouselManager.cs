using UnityEngine;
using UnityEngine.UI;

public class FoodCarouselManager : MonoBehaviour
{
    public GameObject buttonPrefab;          
    public Transform contentPanel;           
    public Sprite[] foodSprites;            

    void Start()
    {
        buttonPrefab.SetActive(false);
        GenerateFoodButtons();
    }

    void GenerateFoodButtons()
    {
        foreach (Sprite foodSprite in foodSprites)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentPanel);
            newButton.SetActive(true);
            Image img = newButton.GetComponent<Image>();
            if (img != null)
                img.sprite = foodSprite;

            // Optional: Hook up a click event
            Button btn = newButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnFoodSelected(foodSprite));
            }
        }
    }

    void OnFoodSelected(Sprite selectedFood)
    {
        Debug.Log("Selected food sprite: " + selectedFood.name);
        // Do something with the selected food (e.g., add to fridge)
    }
}
