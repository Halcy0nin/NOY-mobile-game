using UnityEngine;

public class PetManager : MonoBehaviour
{
    public PetController pet;
    public NeedsController needsController;
    public float petMoveTimer, originalPetMoveTimer;
    public Transform[] waypoints;

    public static PetManager Instance;

    private void MovePetToRandomWaypoint()
    {
        int randomWaypoint = Random.Range(0,waypoints.Length);
        pet.Move(waypoints[randomWaypoint].position);
    }

    private void Awake()
    {
        originalPetMoveTimer = petMoveTimer;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
           Debug.LogWarning("Multiple instances of PetManager found.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(petMoveTimer > 0)
        {
            petMoveTimer -= Time.deltaTime;
        }
        else
        {
            MovePetToRandomWaypoint();
            petMoveTimer = originalPetMoveTimer;
        }
    }
    public void Death()
    {
        Debug.Log("Pet has died. Game Over.");
    }
}
