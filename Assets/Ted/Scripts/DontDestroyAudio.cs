using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(audio);
    }
}
