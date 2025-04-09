using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public static float gameTimer;
    public float timeLength;
    private void Update(){
        if(gameTimer <= 0){
            gameTimer = timeLength;
        }
        else{
            gameTimer -= Time.deltaTime;
        }
    }
}
