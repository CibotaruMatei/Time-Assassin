using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource myFX;
    public AudioClip hoverFX;
    public AudioClip clickFX;


    public void HoverSound()
    {
        myFX.PlayOneShot(hoverFX);
    }

    public void ClickSound()
    {
        myFX.PlayOneShot(clickFX);
    }
}
