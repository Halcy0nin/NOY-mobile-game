using UnityEngine;

public class CanTarget : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Slipper"))
        {
            Debug.Log("Hit the Can!");
            GameManager.Instance.RegisterHit();
            Destroy(collision.gameObject);
        }
    }
}