using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingEnemyAI : MonoBehaviour
{
    enum FollowEnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    }
    FollowEnemyState state;

    [HideInInspector]
    public GameObject player;
    CharacterController cc;
    [HideInInspector]
    public NavMeshAgent F_enemy = null;
    Animator anim;
    //현재 시간 
    private float currentTime;
    //좀비의 콜라이더
    public BoxCollider[] zombieColiders;
    //시간 오버로 죽을 때 이동 못하게 onoff;
    private bool timeOverdead = false;
    #region "Idle에 필요한 속성"
    private float idleDelayTime = 1f;
    #endregion
    #region "Move에 필요한 속성"
    //탐지 범위
    public float detectRange = 5f;
    //공격 범위
    public float attackRange = 2f;
    // 검출한 바리게이트 타겟
    [HideInInspector]
    public Transform Target = null;

    #endregion
    #region "Attack에 필요한속성"
    //공격 딜레이 시간 
    public float attackDelayTime = 2f;
    //바리게이트HP 함수를 담을 변수
    private BarricadeHP barricadeHP;
    #endregion
    #region "ending에 필요한 속성"
    public Material[] zombieMaterials;
    public GameObject zombieDieEffect;
    
    #endregion
    #region "에너미의 능력치"
    public int deadMoney = 30;
    [HideInInspector]
    public float enemyDamage ;
    [HideInInspector]
    public float enemyMaxHP ;
    [HideInInspector]
    public float enemySpeed;
    private float enemyCurrentHP;

    //enemy 스탯의 초기값을 담을 변수
    public float firstMaxHP;
    public float firstDamage;
    public float firstSpeed;
    #endregion
    #region "사운드 관련"
    private AudioSource zombieAudio;
    //좀비 사운드매니저
    private ZombieSoundManager zombieSound;
    //move일 때 사운드 출력 
    private bool playIdleAudio = true;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        F_enemy = GetComponent<NavMeshAgent>();
        barricadeHP = GetComponent<BarricadeHP>();
        player = GameObject.FindGameObjectWithTag("Player");
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        zombieAudio = GetComponent<AudioSource>();
        zombieSound = GetComponent<ZombieSoundManager>();
    }
    void Start()
    {
        //state = FollowEnemyState.Idle;
        ////에너미의 능력치 
        enemyCurrentHP = firstMaxHP;
        
        
        
    }
    //오브젝트풀 재사용시 바꿔줄 함수 
    void OnEnable()
    {
        if (cc.enabled == false)
        {
            cc.enabled = true;
            state = FollowEnemyState.Idle;
        }
        else
        {
            state = FollowEnemyState.Idle;
        }
        
        for (int i = 0; i < zombieColiders.Length; i++)
        {
            if (zombieColiders[i].enabled == false)
            {
                zombieColiders[i].enabled = true;
            }
        }

        timeOverdead = false;
        //에이전트 관리 
        F_enemy.enabled = false;
        //SearchBarricade를 .5초 마다 실행 시켜라
        //InvokeRepeating("SearchBarricade", 0f, 0.5f);
        
        enemyMaxHP = firstMaxHP;
        enemyDamage = firstDamage;
        F_enemy.speed = firstSpeed;
    }

    void Update()
    {
        
        switch (state)
        {
            case FollowEnemyState.Idle:
                Idle();
                break;
            case FollowEnemyState.Move:
                Move();
                break;
            case FollowEnemyState.Attack:
                Attack();
                break;
            case FollowEnemyState.Damage:
                Damage();
                break;
        }
    }



    private void Idle()
    {
        currentTime += Time.deltaTime;
        if (currentTime > idleDelayTime)
        {
            state = FollowEnemyState.Move;
            currentTime = 0;
            //여기다가 이동 애니메이션 추가
            anim.SetTrigger("Move");
            playIdleAudio = true;
        }
    }

    private void Move()
    {
        if (F_enemy.enabled == false)
        {
            F_enemy.enabled = true;
        }
        //3. 바리게이트가 감지 되지않으면 타겟을 플레이어로 바꿈 
        if (Target == null)
        {
            Target = player.transform;
            AttackDetect();

        }
        else
        {
            AttackDetect();

        }
        //평상시 사운드 
        if (playIdleAudio)
        {
            zombieAudio.clip = zombieSound.zombieIdle;
            zombieAudio.Play();
            zombieAudio.loop = true;
            playIdleAudio = false;
        }
    }
    //타겟이 공격 범위로 오면 상태를 공격으로 바꿈 
    void AttackDetect()
    {
        if (timeOverdead == false)
        {
            //타겟을 검출하면 타겟으로 이동
            F_enemy.SetDestination(Target.position);
            //이동 중에 타겟이 공격범위에 들어오면 상태를 공격으로 바꿈
            float a_distance = Vector3.SqrMagnitude(transform.position - Target.position);
            if (attackRange > a_distance)
            {
                F_enemy.enabled = false;
                state = FollowEnemyState.Attack;
                currentTime = attackDelayTime;
            }
        }

    }

    private void Attack()
    {

        if (Target == player.transform)
        {
            Target = player.transform;
        }
        // 타겟이 있으면 
        if (Target != null)
        {
            currentTime += Time.deltaTime;
            //검출한 바리게이트를 일정 간격으로 공격 
            if (currentTime > attackDelayTime)
            {
                // 이쪽에 공격 애니메이션 추가 
                anim.SetTrigger("Attack");
                currentTime = 0;
            }
            float distance = Vector3.Distance(Target.position, transform.position);
            // 플레이어가 공격범위 밖으로 넘어갔다면
            if (distance > attackRange)
            {
                state = FollowEnemyState.Move;
                //여기다가 이동 애니메이션 추가
                anim.SetTrigger("Move");
            }

        }
        //검출한 바리게이트가 없다면 
        else
        {
            state = FollowEnemyState.Move;
            //여기다가 이동 애니메이션 추가
            anim.SetTrigger("Move");
        }

    }
    //피격 딜레이시간
    private float damageDelayTime = 1f;
    private void Damage()
    {
        // 일정 시간이 지나면 상태를 Idle 로 전환
        // 1. 시간이 흘렀으니까
        currentTime += Time.deltaTime;
        // 2. 경과시간이 피격딜레이 시간을 초과했으니까
        if (currentTime > damageDelayTime)
        {
            // 3. 상태를 Idle로 전환
            state = FollowEnemyState.Idle;
            currentTime = 0;
        }
    }

    //기본 데미지 상태 
    public void OnNormalDamageProcess(int PlayerDamage, string DamagePart, string DiePartName)
    {
        if (DamagePart == "HeadDamage")
        {
            enemyCurrentHP -= PlayerDamage * 1.5f;
        }
        else
        {
            enemyCurrentHP -= PlayerDamage;

        }
        F_enemy.enabled = false;
        //뒤로 밀리게 하고싶다. 
        Vector3 direction = transform.position - player.transform.position;
        direction.Normalize();
        cc.SimpleMove(direction * 5);
        if (enemyCurrentHP > 0)
        {
            zombieAudio.clip = zombieSound.zombieDamage;
            zombieAudio.loop = false;
            zombieAudio.PlayOneShot(zombieAudio.clip);

            state = FollowEnemyState.Damage;
            //여기다 데미지 애니메이션을 넣어준다.
            anim.SetTrigger(DamagePart);
        }
        else
        {
            zombieAudio.clip = zombieSound.zombieDie;
            zombieAudio.loop = false;
            zombieAudio.PlayOneShot(zombieAudio.clip);
            state = FollowEnemyState.Die;
            StopAllCoroutines();
            //Die 코루틴
            StartCoroutine(Die(DiePartName));
        }
    }


    //사라지는거 딜레이시간 
    public float dieDelayTime = 2;
    private IEnumerator Die(string DiePart)
    {

        //좀비의 체력이 다달았을때 
        //Die 애니메이션 출력 
        cc.enabled = false;
        F_enemy.enabled = false;
        for(int i =0;i<zombieColiders.Length;i++)
        {
            zombieColiders[i].enabled = false;
        }
        //여기다가 죽음 애니메이션 추가 
        anim.SetTrigger(DiePart);
        //돈 추가
        if (timeOverdead == false)
        {
            MoneyManager.Instance.MONEY += deadMoney;
        }
        //킬포인트 추가 
        GameManager.Instance._KILLPOINT--;

        // 1. 일정시간 기다렸다가
        yield return new WaitForSeconds(dieDelayTime);
        enemyMaxHP = firstMaxHP;
        enemyDamage = firstDamage;
        F_enemy.speed = firstSpeed;
        DetectName();
    }
    void DetectName()
    {
        if (gameObject.name.Contains("NormalZombie"))
        {
            EnemyPoolingManager.Instance.InsertQueue(gameObject, EnemyPoolingManager.Instance.ZombiesQueue[0]);
        }
        if (gameObject.name.Contains("SpeedyZombie"))
        {
            EnemyPoolingManager.Instance.InsertQueue(gameObject, EnemyPoolingManager.Instance.ZombiesQueue[1]);
        }
        if (gameObject.name.Contains("TankZombie"))
        {
            EnemyPoolingManager.Instance.InsertQueue(gameObject, EnemyPoolingManager.Instance.ZombiesQueue[2]);
        }
        if (gameObject.name.Contains("AttackerZombie"))
        {
            EnemyPoolingManager.Instance.InsertQueue(gameObject, EnemyPoolingManager.Instance.ZombiesQueue[3]);
        }


    }

    public void TimeOver()
    {
        timeOverdead = true;
        StartCoroutine(Die("BodyDie"));
        //좀비의 자식으로 있는 불타는 이펙트의 setActive를 true로 
        zombieDieEffect.SetActive(true);
        //좀비의 Material의 color를 탄 갈색느낌이 나게 바꾸어준다.
        for (int i = 0; i < zombieMaterials.Length; i++)
        {
            zombieMaterials[i].color =new Color(130 / 255f, 127 / 255f, 127 / 255f);
        }
    }
    //5초 뒤에 성공 ui가 뜨게 하고 싶다. 
    private IEnumerator endingUITrue()
    {
        yield return new WaitForSeconds(5f);
        
    }
}