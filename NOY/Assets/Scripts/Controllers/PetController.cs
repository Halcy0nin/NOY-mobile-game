using UnityEngine;

public class PetController : MonoBehaviour
{
    //Move Pet when idle
    private Vector3 destination;
    public float speed;

    private void Awake()
    {

    }

    //Function to move pet in idle
    public void Move(Vector3 destination)
    {
        this.destination = destination;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if(Vector3.Distance(transform.position,destination) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
        }
    }
}
