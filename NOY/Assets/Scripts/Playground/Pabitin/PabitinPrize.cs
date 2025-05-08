using UnityEngine;
using UnityEngine.EventSystems;

public class PabitinPrize : MonoBehaviour, IPointerDownHandler
{
    private PabitinGameManager gameManager;

    [Header("Rope Setup")]
    [SerializeField] private Transform ropeTransform; // Assign the 'Rope' child in the Inspector
    public Transform anchorTransform; // Assigned during spawn

    private Rigidbody2D ropeRb; // Reference to the Rope's Rigidbody2D

    void Start()
    {
        // Initialize Rigidbody2D for rope
        ropeRb = ropeTransform.GetComponent<Rigidbody2D>();
        if (ropeRb == null)
        {
            ropeRb = ropeTransform.gameObject.AddComponent<Rigidbody2D>(); // In case Rigidbody2D is not assigned
            ropeRb.bodyType = RigidbodyType2D.Kinematic; // Ensure rope follows the prize
        }
    }

    public void Initialize(PabitinGameManager manager)
    {
        gameManager = manager;
    }

    private void Update()
    {
        UpdateRope();
    }

    private void UpdateRope()
    {
        if (ropeTransform == null || anchorTransform == null) return;

        // Update rope to follow the prize
        Vector3 direction = anchorTransform.position - transform.position;
        float distance = direction.magnitude;

        // Position rope midway between prize and anchor
        ropeTransform.position = transform.position + direction / 2f;

        // Stretch rope and rotate to match direction
        ropeTransform.up = direction.normalized;
        ropeTransform.localScale = new Vector3(1f, distance, 1f);

        // Apply physics to make rope "swing"
        ropeRb.MovePosition(ropeTransform.position); // Move the rope's Rigidbody2D to the correct position
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.AddScore(1);
        Destroy(gameObject);
    }
}
