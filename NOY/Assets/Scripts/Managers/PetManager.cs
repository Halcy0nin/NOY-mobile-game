using UnityEngine;

public class PetManager : MonoBehaviour
{
    public PetController pet;
    public NeedsController needsController;

    public static PetManager Instance;

 

    private void Awake()
    {
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
  
    }
    public void Death()
    {
        Debug.Log("Pet has died. Game Over.");
    }
}
