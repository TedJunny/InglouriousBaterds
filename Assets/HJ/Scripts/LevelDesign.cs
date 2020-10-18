using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesign : MonoBehaviour
{
    public static LevelDesign Instance;
    private void Awake()
    {
        if(Instance ==null)
        {
            Instance = this;
        }
    }
    //에너미 생성 반복 빈도
    public int[] repeatEnemySpawn;
    //한번에 생성하는 에너미 양
    public int[] enemySpawn;
    //에너미 생성사이 딜레이 
    public int[] waveDelayTime;
    //레벨에 따라 곱해줄 에너미의 공격력
    public float[] addEnemyAttack;
    //레벨에 따라 곱해줄 에너미의 체력
    public float[] addEnemyHP;
    //레벨에 따라 곱해줄 에너미의 스피드 
    public float[] addEnemySpeed;

    
}
