using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SlipperThrow2D : MonoBehaviour
{
    [Header("Throw Settings")]
    [SerializeField] private float throwForceMultiplier = 10f;
    [SerializeField] private float maxDragDistance = 5f;

    [Header("UI Elements")]
    private Transform arrowTransform;
    private Image forceBar; // UI Image with Fill Method set

    private Rigidbody2D rb;
    private bool thrown = false;
    private Vector2 startTouch;
    private Camera mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        rb.bodyType = RigidbodyType2D.Kinematic;

        if (arrowTransform != null)
            arrowTransform.gameObject.SetActive(false);

        if (forceBar != null)
            forceBar.fillAmount = 0f;
    }

    void Update()
    {
        if (!thrown)
        {
            if (Touchscreen.current != null)
            {
                HandleTouchInput();
            }
            else
            {
                HandleMouseInput();
            }
        }
        else
        {
            // Check if slipper fell off screen
            if (transform.position.y < -10f || Mathf.Abs(transform.position.x) > 15f)
            {
                GameManager.Instance.RegisterMiss();
                Destroy(gameObject);
            }
        }
    }

    public void SetUIReferences(Transform arrow, Image force)
    {
        arrowTransform = arrow;
        forceBar = force;
    }

    void HandleMouseInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startTouch = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (arrowTransform != null)
                arrowTransform.gameObject.SetActive(true);
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 currentTouch = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            UpdateAimingVisuals(currentTouch);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector2 endTouch = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Launch(endTouch);
        }
    }

    void HandleTouchInput()
    {
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            startTouch = mainCam.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            if (arrowTransform != null)
                arrowTransform.gameObject.SetActive(true);
        }

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentTouch = mainCam.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            UpdateAimingVisuals(currentTouch);
        }

        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
        {
            Vector2 endTouch = mainCam.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            Launch(endTouch);
        }
    }

    void UpdateAimingVisuals(Vector2 currentTouch)
    {
        Vector2 direction = startTouch - currentTouch;
        float distance = Mathf.Clamp(direction.magnitude, 0, maxDragDistance);
        float normalizedForce = distance / maxDragDistance;

        if (arrowTransform != null)
        {
            // Only rotate the arrow to show direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle);

            // Keep position and scale unchanged
        }

        if (forceBar != null)
            forceBar.fillAmount = normalizedForce;
    }

    void Launch(Vector2 endTouch)
    {
        Vector2 direction = startTouch - endTouch;
        float distance = Mathf.Clamp(direction.magnitude, 0, maxDragDistance);
        float normalizedForce = distance / maxDragDistance;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction.normalized * normalizedForce * throwForceMultiplier, ForceMode2D.Impulse);
        thrown = true;

        if (arrowTransform != null)
            arrowTransform.gameObject.SetActive(false);
        if (forceBar != null)
            forceBar.fillAmount = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Wall"))
        {
            GameManager.Instance.RegisterMiss();
            Destroy(gameObject);
        }
    }
}
