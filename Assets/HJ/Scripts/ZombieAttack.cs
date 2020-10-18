using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    FollowingEnemyAI FE;
    GameObject player;
    private AudioSource zombieAudio;
    private ZombieSoundManager zombieSound;

    private void Start()
    {
        FE = transform.parent.GetComponent<FollowingEnemyAI>();
        player = FE.player;
        zombieAudio = GetComponentInParent<AudioSource>();
        zombieSound = GetComponentInParent<ZombieSoundManager>();

    }

    public void Attack()
    {

        PlayerHP ph = FE.Target.GetComponent<PlayerHP>();
        ph.PlayerDamage(FE.enemyDamage);
        zombieAudio.clip = zombieSound.zombieAttack;
        zombieAudio.loop = false;
        zombieAudio.PlayOneShot(zombieAudio.clip);


    }
}
