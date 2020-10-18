using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource smAudio;
    [SerializeField] AudioClip explosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        smAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void OnExplosionSound()
    {
        if (!smAudio.isPlaying)
        {
            smAudio.clip = explosionAudio;
            smAudio.Play();
        }
    }
}
