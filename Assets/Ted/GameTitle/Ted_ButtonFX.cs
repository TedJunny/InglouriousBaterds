using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Ted_ButtonFX : MonoBehaviour
{
    [SerializeField] private AudioSource btnFX;
    [SerializeField] private AudioClip hoverFX;
    [SerializeField] private AudioClip clickFX;

    // Start is called before the first frame update
    public void HoverSound()
    {
        btnFX.PlayOneShot(hoverFX);
    }

   public void ClickSound()
    {
        btnFX.PlayOneShot(clickFX);
    }
}
