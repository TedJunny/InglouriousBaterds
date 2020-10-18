using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolingManager : MonoBehaviour
{
    //싱글톤 
    public static EnemyPoolingManager Instance;
    public GameObject[] zombies = null;

    public int spawnQueue;
    //좀비들의 Queue
    public Queue<GameObject>[] ZombiesQueue = { new Queue<GameObject>(), new Queue<GameObject>(), new Queue<GameObject>(), new Queue<GameObject>() };



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPoolZombies();

    }
    
    void SpawnPoolZombies()
    {

        //사용할 오브젝트(노멀좀비)를 오브젝트 풀 화 시킨것 
        for (int i = 0; i < spawnQueue; i++)
        {
            //노멀 좀비를 노멀 좀비오브젝트 풀에 넣어준다 . 
            GameObject zombie = Instantiate(zombies[0], Vector3.zero, Quaternion.identity);
            ZombiesQueue[0].Enqueue(zombie);
            zombie.SetActive(false);
        }
        for (int i = 0; i < spawnQueue; i++)
        {
            //스피디 좀비를 스피디 좀비오브젝트 풀에 넣어준다 . 
            GameObject speedyZombie = Instantiate(zombies[1], Vector3.zero, Quaternion.identity);
            ZombiesQueue[1].Enqueue(speedyZombie);
            speedyZombie.SetActive(false);
        }
        for (int i = 0; i < spawnQueue; i++)
        {
            //탱크 좀비를 오브젝트 풀에 넣어준다 . 
            GameObject tankZombie = Instantiate(zombies[2], Vector3.zero, Quaternion.identity);
            ZombiesQueue[2].Enqueue(tankZombie);
            tankZombie.SetActive(false);
        }
        for (int i = 0; i < spawnQueue; i++)
        {
            //어택커 좀비를 오브젝트 풀에 넣어준다 . 
            GameObject attackerZombie = Instantiate(zombies[3], Vector3.zero, Quaternion.identity);
            ZombiesQueue[3].Enqueue(attackerZombie);
            attackerZombie.SetActive(false);
        }

    }

    //사용한 객체를 풀(큐)에 반납시키는 함수 
    public void InsertQueue(GameObject p_zombie, Queue<GameObject> zombiepool)
    {
        zombiepool.Enqueue(p_zombie);
        p_zombie.SetActive(false);
    }
    //큐에서 사용할 객체를 꺼내오는 함수
    //GetQueue(GameObject t_zombie) 이런식으로 바꾸어주고 생성 매니저에서 오브젝트를 관리한다. 
    public GameObject GetQueue(Queue<GameObject> zombiepool)
    {
        GameObject t_zombie = zombiepool.Dequeue();
        t_zombie.SetActive(true);
        return t_zombie;
    }

}
