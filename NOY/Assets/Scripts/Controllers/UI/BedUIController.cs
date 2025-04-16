using UnityEngine;
using UnityEngine.UI;

public class BedUIController : MonoBehaviour
{
    public Sprite[] sleepingCharacter; // 0 = sleeping, 1 = awake
    public Image characterImage;       // The UI Image to update
    public bool sleeping;

    public NeedsController NeedsController;

    public void ToggleSleep(bool sleeping)
    {
        if (sleeping && NeedsController.petGender == "M")
        {
            characterImage.sprite = sleepingCharacter[0];
        }
        else if (!sleeping && NeedsController.petGender == "M")
        {
            characterImage.sprite = sleepingCharacter[1];
        }
        else if (sleeping && NeedsController.petGender == "F")
        {
            characterImage.sprite = sleepingCharacter[2];
        }
        else if (!sleeping && NeedsController.petGender == "F")
        {
            characterImage.sprite = sleepingCharacter[3];
        }
    }

}