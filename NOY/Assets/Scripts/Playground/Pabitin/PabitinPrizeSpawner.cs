using UnityEngine;

public class PabitinPrizeSpawner : MonoBehaviour
{
    [Header("Prize Settings")]
    [SerializeField] private Sprite[] prizeSprites;
    [SerializeField] private Transform[] prizeSlots; // Array of prize slots or placeholders for prizes

    // This method will check and refill prizes when the ropes are raised
    public void CheckAndRefillPrizes()
    {
        Debug.Log("Checking and refilling prizes...");
        foreach (Transform prizeSlot in prizeSlots) // Iterate through each prize slot
        {
            SpriteRenderer spriteRenderer = prizeSlot.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogWarning("No SpriteRenderer found on prize slot: " + prizeSlot.name);
                continue;
            }

            // Check if the prize slot has no sprite (indicating it's empty)
            if (spriteRenderer.sprite == null)
            {
                // Assign a new random sprite from the prizeSprites array
                spriteRenderer.sprite = prizeSprites[Random.Range(0, prizeSprites.Length)];
            }
        }
    }
}
