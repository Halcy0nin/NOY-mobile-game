using UnityEngine;
using UnityEngine.UI;

public class BedUIController : MonoBehaviour
{
    public Sprite[] sleepingCharacter; // 0 = sleeping, 1 = awake
    public Image characterImage;       // The UI Image to update
    public bool sleeping;

    public void ToggleSleep(bool sleeping)
    {
    if (sleeping)
    {
        characterImage.sprite = sleepingCharacter[0];
    }
    else
    {
        characterImage.sprite = sleepingCharacter[1];
    }
}
}