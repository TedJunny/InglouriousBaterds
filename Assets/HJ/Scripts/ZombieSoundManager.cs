using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundManager : MonoBehaviour
{
    private AudioSource zombieSound;
    public AudioClip zombieIdle;
    public AudioClip zombieAttack;
    public AudioClip zombieDie;
    public AudioClip zombieDamage;

   
    // Start is called before the first frame update
    void Start()
    {
        zombieSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

}
