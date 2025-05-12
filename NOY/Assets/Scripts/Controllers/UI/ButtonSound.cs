using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip eatSound;
    public AudioClip fridgeSound;
    public AudioClip sleepSound;
    public AudioClip cleaningSound;

    public void PlayClickSound()
    {
        Debug.Log("Button clicked");
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip!");
        }
    }

     public void PlayRefSound()
    {
        Debug.Log("Button clicked");
        if (audioSource != null && fridgeSound != null)
        {
            audioSource.PlayOneShot(fridgeSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip!");
        }
    }

     public void PlayEatSound()
    {
        Debug.Log("Button clicked");
        if (audioSource != null && eatSound != null)
        {
            audioSource.PlayOneShot(eatSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip!");
        }
    }

     public void PlaySleepSound()
    {
        Debug.Log("Button clicked");
        if (audioSource != null && sleepSound != null)
        {
            audioSource.PlayOneShot(sleepSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip!");
        }
    }

         public void PlayCleaningSound()
    {
        Debug.Log("Button clicked");
        if (audioSource != null && cleaningSound != null)
        {
            audioSource.PlayOneShot(cleaningSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip!");
        }
    }
}
