using UnityEngine;

public class PabitinPrizeSpawner : MonoBehaviour
{
    [Header("Prefabs & Visuals")]
    [SerializeField] private GameObject prizePrefab;         // Root prefab with Rope & PrizeVisual children
    [SerializeField] private Sprite[] prizeSprites;          // Variety of prize looks
    [SerializeField] private GameObject anchorPrefab;        // Invisible static body to attach the rope
    [SerializeField] private Transform prizesParent;         // Organize spawned prizes
    [SerializeField] private Transform[] spawnPoints;        // Anchor positions

    public void SpawnAllPrizes(PabitinGameManager gameManager)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // 1. Create anchor point (invisible object with Static Rigidbody2D)
            GameObject anchor = Instantiate(anchorPrefab, spawnPoint.position, Quaternion.identity, prizesParent);
            Rigidbody2D anchorRb = anchor.GetComponent<Rigidbody2D>();
            anchorRb.bodyType = RigidbodyType2D.Static;

            // 2. Create prize at offset position (hangs below the anchor)
            Vector3 prizePosition = spawnPoint.position + Vector3.down * 1f;
            GameObject prize = Instantiate(prizePrefab, prizePosition, Quaternion.identity, prizesParent);

            // 3. Set a random sprite for prize visual
            SpriteRenderer prizeSR = prize.transform.Find("PrizeVisual")?.GetComponent<SpriteRenderer>();
            if (prizeSR != null && prizeSprites.Length > 0)
            {
                prizeSR.sprite = prizeSprites[Random.Range(0, prizeSprites.Length)];
            }

            // 4. Add and configure Rigidbody2D
            Rigidbody2D prizeRb = prize.GetComponent<Rigidbody2D>();
            if (prizeRb == null) prizeRb = prize.AddComponent<Rigidbody2D>();
            prizeRb.gravityScale = 1f;
            prizeRb.mass = 1f;
            prizeRb.AddTorque(Random.Range(-30f, 30f)); // Slight swing

            // 5. Create and connect the SpringJoint2D
            SpringJoint2D joint = prize.AddComponent<SpringJoint2D>();
            joint.connectedBody = anchorRb; // Connect to the anchor
            joint.autoConfigureDistance = false;  // We set this manually to control rope length
            joint.distance = 2.5f;  // Max length of the rope
            joint.dampingRatio = 0.7f;  // Controls the settling speed of the rope
            joint.frequency = 3.0f;     // Controls the stiffness of the rope

            // 6. Initialize prize script
            PabitinPrize prizeScript = prize.GetComponent<PabitinPrize>();
            prizeScript.Initialize(gameManager);
            prizeScript.anchorTransform = anchor.transform;
        }
    }
}
