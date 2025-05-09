using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class PabitinPrize : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private SpriteRenderer spriteRenderer; // manually assign in Inspector
    [SerializeField] private PabitinGameManager gameManager;

    public void Initialize(PabitinGameManager manager)
    {
        gameManager = manager;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || gameManager == null)
            return;

        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        // Calculate score based on sprite
        gameManager.AddScore(GetScoreFromSprite());

        spriteRenderer.sprite = null;
    }

    private int GetScoreFromSprite()
    {
        var sprite = spriteRenderer.sprite;
        if (sprite == null) return 0;

        string name = sprite.name.ToLower();
        if (name.Contains("rare")) return 5;
        if (name.Contains("special")) return 3;
        return 1;
    }
}
