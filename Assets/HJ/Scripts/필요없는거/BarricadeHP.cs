using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeHP : MonoBehaviour
{
    // 바리게이트의 hp
    private static BarricadeHP Instance;
    private void Awake()
    {
        Instance = this;
    }



    //바리게이트의 체력이 30% 이하 일때 번쩍거리게 하고싶다. 
    //필요 속성 
    //-현재 체력, 최대 체력 , 현재 Material, 바꿀 Material;

    //바리게이트의 최대HP
    public float MaxHP = 20;
    //바리게이트의 현재HP
    private float currentHP;
    //현재 Material
    [SerializeField]
    private Material currentMat;
    //바꿀 Material
    [SerializeField]
    private Material white;

    public float _BARRICADEHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
            if (currentHP < MaxHP * 0.3)
            {
                StartCoroutine("ChangeColor");
            }
            if (currentHP < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHP = MaxHP;


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyBarricade(float zombieAttackPower)
    {
        _BARRICADEHP -= zombieAttackPower;


    }

    IEnumerator ChangeColor()
    {
        for (int i = 0; i < 3; i++)
        {
            this.GetComponent<Renderer>().material = white;
            this.GetComponent<Renderer>().material = currentMat;
            yield return null;
        }
    }

}

