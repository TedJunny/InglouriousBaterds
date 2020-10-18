using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



    enum WaveSystemState
    {
        OnWave,
        OnCreateTime,
        ending
    }
    WaveSystemState waveState;


    //Wave 함수 
    //최대 wave
    public int maxWave = 10;
    //현재 wave
    int currentWave = 0;
    //wave 프로퍼티 
    public int _WAVE
    {
        get
        {
            return currentWave;
        }
        set
        {
            currentWave = value;

            AddLevelDesign();

            //wave가 올라가면 에너미생성 코루틴을 wave당 레벨디자인에 따라 한번 출력
            if (waveState == WaveSystemState.OnWave)
            {
                StartCoroutine("CreateCoroutine");
            }

        }
    }

    //KillPoint 프로퍼티
    //적을 잡으면 킬 포인트가 오름
    int currentKillPoint = 0;
    public int _KILLPOINT
    {
        get
        {
            return currentKillPoint;
        }
        set
        {
            currentKillPoint = value;
            remainEnemy.GetComponent<Text>().text = "Zombies Left :" + currentKillPoint;
        }
    }

    //현재 시간 
    private float currentTime;
    #region "좀비 생성에 필요한 속성"
    //enemywave 1 일때 , 2일때 , 3일때 생성을 다르게 한다. 

    //지정된 SpawnArea에서 생성한다.
    //SpawnArea의 선정은 랜덤 
    //필요요소 : SpawnArea 의 배열(트랜스 폼) , 랜덤 함수
    //SpawnArea 의 배열(트랜스 폼)
    public Transform[] spawnArea;
    // 랜덤함수
    int r_value;
    //총 에너미의 개수 
    private int enemyValue;
    //생성 반복 횟수  
    private int repeatEnemySpawn;
    //에너미의 생성 단위
    private int enemySpawn;
    //wave딜레이 시간 
    private int waveDelayTime;
    #endregion
    #region "바리게이트 생성 시간 관련 속성"
    //바리게이트 생성시간 종료
    public int endCreateTime = 30;
    //WAVE로 넘어가기 버튼
    #endregion
    #region "WAVE시스템 관련 속성"
    //WAVE시간 
    public int waveTime = 60;
    //남은시간 
    public GameObject readyText;
    //추가할 에너미의 공격력
    [HideInInspector]
    public float addEnemyAttack;
    //추가할 에너미의 HP
    [HideInInspector]
    public float addEnemyHP;
    //추가할 에너미의 스피드
    [HideInInspector]
    public float addEnemySpeed;

    FollowingEnemyAI FA;
    ////시간(WAVE, 바리게이트 생성 남은시간)을 표시해주는 UI
    //public Text remainingTime;
    //킬포인트를 표시해주는 UI
    public GameObject remainEnemy;

    #endregion

    //에너미 죽을 때 상황 
    private bool isEnd = false;
    void Start()
    {
        // 처음에는 바리게이트 생성 타임
        waveState = WaveSystemState.OnCreateTime;
        currentTime = endCreateTime;
    }

    // Update is called once per frame
    void Update()
    {
        //switch case 말고 bool함수를 이용해서 CreateTime을 제어하고 wave는 프로퍼티 (getset)안에 코루틴을 넣어 사용한다. 
        switch (waveState)
        {
            case WaveSystemState.OnWave:
                OnWave();
                break;
            case WaveSystemState.OnCreateTime:
                OnCreateTime();
                break;
            case WaveSystemState.ending:
                OnEnding();
                break;
        }
    }

    void OnWave()
    {
        readyText.SetActive(false);
        currentTime -= Time.deltaTime;

        //남은 적표시를 켜준다
        remainEnemy.SetActive(true);
        if (currentWave <= 9)
        {
            //만약 waveTime이 지나면 
            if (currentTime <= 0)
            {
                //다시 생성 시간으로 
                waveState = WaveSystemState.OnCreateTime;
                // _KILLPOINT = enemyValue;
                currentTime = endCreateTime;

            }

            ////만약 생성한 모든 적을 잡으면 (enemyValue = killvalue) 생성시간으로 이동 
            //if (currentKillPoint == 0)
            //{
            //    currentTime = endCreateTime;
            //    waveState = WaveSystemState.OnCreateTime;
            //    //_KILLPOINT = enemyValue;
            //}
        }
        if (currentWave == 10)
        {
            print("WAVE 10!!!!");
            if (currentTime <= 0)
            {
                isEnd = true;
                waveState = WaveSystemState.ending;
            }
        }


    }
    //void 저장() =>엔딩씬 대비 
    //{
    //    //만약 생성된 좀비들이 있다면 
    //    if (GameObject.FindGameObjectsWithTag("Enemy") != null)
    //    {
    //        //생성을 멈추고
    //        StopCoroutine("CreateCoroutine");
    //        GameObject[] trashEnemy = GameObject.FindGameObjectsWithTag("Enemy");
    //        for (int i = 0; i < trashEnemy.Length; i++)
    //        {
    //            //죽인다 로 바꾸고 싶다.
    //            trashEnemy[i].GetComponent<FollowingEnemyAI>().TimeOver();
    //            //모두 오브젝트풀로 돌려준다.
    //            //EnemyPoolingManager.Instance.InsertQueue(trashEnemy[i]);
    //        }
    //       // waveState = WaveSystemState.OnCreateTime;
    //    }
    //}

    //생성 가능 시간 
    void OnCreateTime()
    {
        //곧 좀비가 출현합니다 .
        readyText.SetActive(true);

        //30초(수정 가능)의 시간을 두고 시간이 지나면 Onwave로 돌아가게 하고 싶다.
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            //WAVE로 넘어가기 버튼의 SetActive를 False로 바꾸어주기
            waveState = WaveSystemState.OnWave;
            _WAVE++;
            currentTime = waveTime;

            _KILLPOINT += enemyValue;
        }
        //필요속성
        //- 현재 시간, createTime(생성가능 시간) , 상점을 껏다킬수있는 기능 
    }

    //앤딩 
    private void OnEnding()
    {
        if (isEnd == true)
        {
            //만약 생성된 좀비들이 있다면
            if (GameObject.FindGameObjectsWithTag("Enemy") != null)
            {
                //생성을 멈추고
                StopCoroutine("CreateCoroutine");
                GameObject[] trashEnemy = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < trashEnemy.Length; i++)
                {
                    //죽인다 로 바꾸고 싶다.
                    trashEnemy[i].GetComponent<FollowingEnemyAI>().TimeOver();
                    //모두 오브젝트풀로 돌려준다.

                }
                isEnd = false;
            }
        }
    }


    //레벨 디자인 값 넣어주기
    void AddLevelDesign()
    {

        repeatEnemySpawn = LevelDesign.Instance.repeatEnemySpawn[currentWave - 1];//repeatEnemySpawn[0]
        enemySpawn = LevelDesign.Instance.enemySpawn[currentWave - 1];
        waveDelayTime = LevelDesign.Instance.waveDelayTime[currentWave - 1];
        addEnemyAttack = LevelDesign.Instance.addEnemyAttack[currentWave - 1];
        addEnemyHP = LevelDesign.Instance.addEnemyHP[currentWave - 1];
        addEnemySpeed = LevelDesign.Instance.addEnemySpeed[currentWave - 1];
        //총 에너미의 개수는 반복횟수 * 생성 단위 
        enemyValue = repeatEnemySpawn * enemySpawn;

    }



    //에너미 생성 코루틴 
    IEnumerator CreateCoroutine()
    {
        //에너미 생성 반복 횟수
        for (int i = 0; i < repeatEnemySpawn; i++)
        {
            //에너미 생성 지연시간 
            yield return new WaitForSeconds(waveDelayTime);
            //에너미 생성단위 만큼 
            for (int j = 0; j < enemySpawn; j++)
            {
                r_value = Random.Range(0, spawnArea.Length);
                yield return new WaitForSeconds(.5f);
                //여기서 (좀비의 종류 => GetQueue(해당 좀비의 queue))관리
                waveSpawn();
                yield return null;
            }
        }
    }
    //WAVE에 따라 좀비의 생성 종류가 랜덤바뀌도록 한다.
    void waveSpawn()
    {
        //1~2 단계 에서는 노멀좀비
        if (currentWave >= 1 && currentWave < 3)
        {
            getzombie(1);
        }
        //3~6단계에서는 노멀 +스피디
        if (currentWave >= 3 && currentWave < 7)
        {
            getzombie(2);
        }
        //7~8 노멀 +스피디 + 탱커
        if (currentWave >= 7 && currentWave < 9)
        {
            getzombie(3);
        }
        // 9이상 전부다 
        if (currentWave >= 9)
        {
            getzombie(4);
        }

    }
    void getzombie(int range)
    {
        int r_enemy = Random.Range(0, range);

        GameObject t_zombie = EnemyPoolingManager.Instance.GetQueue(EnemyPoolingManager.Instance.ZombiesQueue[r_enemy]);
        t_zombie.transform.position = (spawnArea[r_value].position);
        t_zombie.transform.GetChild(0).transform.localPosition = new Vector3(0, -1, 0);
        t_zombie.transform.GetChild(0).transform.localRotation = Quaternion.identity;

        FA = t_zombie.GetComponent<FollowingEnemyAI>();
        FA.enemyDamage = FA.firstDamage * addEnemyAttack;
        FA.enemyMaxHP = FA.firstMaxHP * addEnemyHP;
        FA.F_enemy.speed = FA.firstSpeed * addEnemySpeed;
        //print(FA.enemyDamage);

    }
}
