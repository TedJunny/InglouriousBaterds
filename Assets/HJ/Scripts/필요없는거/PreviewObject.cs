using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    // 충돌한 오브젝트의 컬라이더 
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround; //지상 레이어는 무시하게
    private const int IGNORE_RAYCAST_LAYER = 2;//IGNORE_RAYCAST_LAYER도 무시하게 
    [SerializeField]
    private int Barricade;//바리게이트의 레이어 번호 찾기 

    [SerializeField]
    private Material green;//설치 가능할 때 색상
    [SerializeField]
    private Material red;// 설치 불가능 할 때 색상 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
        {
            
            SetColor(red);//빨강 
        }
        else
        {
            SetColor(green);//초록
        }
    }
    /*
     // 자식이 여러개인 오브젝트 일때 
    private void SetColor(Material mat)
    {   // 자기 자신 안에 있는(자식) 의 트랜스폼을 가져와서 반복문을 돌린다. 
        foreach (Transform tf_Child in this.transform)
        {   // 기존 자식의 트랜스폼에서  Renderer 컴포넌트를 받아오고 그 컴포넌트의 materials.Length를 가져온다.
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];
            //가져온 머테리얼들에 넣어줄 색깔을 선정해주고 
            for (int i = 0; i <newMaterials.Length ; i++)
            {
                newMaterials[i] = mat;
            }
            // 색을 바꾸어준다. 
            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }
    */
    //자식이 없는 단일 오브젝트 일때 색 바꾸기
    private void SetColor(Material mat)
    {
        var newMaterials = new Material[this.GetComponent<Renderer>().materials.Length];

        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = mat;
        }
        this.GetComponent<Renderer>().materials = newMaterials;
    }

    //오브젝트가 충돌 했을 때 colliderList에 추가
    private void OnTriggerEnter(Collider other)
    {
        //예외 : other의 게임오브젝트의 레이어가  지상 레이어가 아니고 IGNORE_RAYCAST_LAYER가 아닐때(&& = 둘다 참일 때 참,하나라도 거짓이면 거짓)
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER && other.gameObject.layer != Barricade)
        {
            
            colliderList.Add(other);
            
        }
    }
    //오브젝트가 빠져나올때 colliderList에서 제거
    private void OnTriggerExit(Collider other)
    {   
        
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER && other.gameObject.layer != Barricade)
        {
            colliderList.Remove(other);
            
        }
    }
    //색상이 빨간색일 때 (미리보기 프리팹이 다른 콜라이더에 닿았을 때) 오브젝트 생성을 못하게끔 막음
    public bool isBuildable()
    {
        //olliderList.Count가 0개 일 때만 true를 받아오게함 
        return colliderList.Count == 0;
    }

}
