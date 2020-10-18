using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;//이름
    public GameObject go_prefab; // 실제 설치될 프리팹
    public GameObject go_PreiviewPrefab; //미리보기 프리팹

}

public class CraftManual : MonoBehaviour
{
    // 상태 변수 : 매뉴얼이 활성화 된 상태 
    private bool isActivated = false;
    //미리보기 프리팹이 활성화된 상태 
    private bool isPreviewActivated = false;

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_barricade;//바리게이트 탭

    private GameObject go_Preview; //미리보기 프리팹을 담을 변수 
    private GameObject go_Prefab; //생성할 프리팹을 담을 변수 

    //플레이어 눈앞에 생기면 좋겠다, 플레이어가 움직이면 미리보기 프리팹도 따라왔으면 좋겠다. 
    [SerializeField]
    private Transform tf_Player;// 플레이어 위치 

    //Raycast 필요 변수 선언
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range; //사정거리
    //ray 쏘는 포지션 
    [SerializeField]
    Transform ShootPosition;
    int LastPosX, LastPosY, LastPosZ;
    //구매 가능 
    private bool OnBuy = true;
    public int B1Price = 100;
    public int B2Price = 150;

    //오브젝트가 바닥위로 올라오게끔 y값을 올려주는 값(각 오브젝트 마다 값이 달라야함)
    private float height;
    Vector3 mousePos;

    public void SlotClick(int _slotNumber)
    {

        if (_slotNumber == 0)
        {
            BuyBarricade(B1Price);
            height = 0.5f;
        }
        if (_slotNumber == 1)
        {
            BuyBarricade(B2Price);
            height = 1f;
        }
        if (OnBuy)
        {
            //Craft의 n번 째 아이템 버튼을 누르면 n번째 아이템이 생성
            go_Preview = Instantiate(craft_barricade[_slotNumber].go_PreiviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
            // 변수에 Craft의 n번 째 아이템 변수를 담음 
            go_Prefab = craft_barricade[_slotNumber].go_prefab;// 변수를 담음 
            isPreviewActivated = true;
            go_BaseUI.SetActive(false);
        }

    }
    void BuyBarricade(int BnPrice)
    {
        if (MoneyManager.Instance.MONEY < BnPrice)
        {
            OnBuy = false;
        }
        else
        {

            MoneyManager.Instance.MONEY -= BnPrice;
        }
    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        // 미리보기 프리팹이 생성되고도 중복 생성 되는 것을 막자
        // 미리 보기 활성화 상태가 true일 때만 Tab으로 UI창을 띄울수 있도록함
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }
        if (isPreviewActivated)
        {
            //미리보기 프리팹의 포지션을 계속 바꿀수 있게 
            PreviewPositionUpdate();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }
    // 마우스 왼쪽 클릭을 하면 미리보기 프리팹을 없애고 원래 생성할 오브젝트를 생성 
    private void Build()
    {   // 미리보기 프리팹이 켜져있고 미리보기 프리팹에서 건축 가능하다고 판단하면
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            //hitinfo의 위치에 바리게이트 프리팹 생성
            Instantiate(go_Prefab, go_Preview.transform.position, Quaternion.identity);
            //미리보기 프리팹을 파괴시키고
            Destroy(go_Preview);

            isActivated = false;
            isPreviewActivated = false;
            //
            go_Preview = null;
            go_Prefab = null;
        }
    }
    //미리보기 프리팹을 생성 할 때 플레이어의 앞에서 생성하게 함
    private void PreviewPositionUpdate()
    {


        if (Physics.Raycast(ShootPosition.position, ShootPosition.forward, out hitInfo, range, layerMask))
        {
            //레이가 닿은 곳의 x좌표와 y ,z 좌표를 얻는다.
            int PosX = (int)Mathf.Round(hitInfo.point.x);
            int PosY = (int)Mathf.Round(hitInfo.point.y);
            int posZ = (int)Mathf.Round(hitInfo.point.z);

            //  Debug.Log("X:" + PosX + " Z : " + posZ);
            // '||' :둘중 하나가 참이면 참으로 ,둘다 거짓이어야 거짓
            if (PosX != LastPosX || PosY != LastPosY || posZ != LastPosZ)
            {
                //현재 위치한 좌표 보관 
                LastPosX = PosX;
                LastPosY = PosY;
                LastPosZ = posZ;
                //
                go_Preview.transform.position = new Vector3(PosX, PosY + height, posZ);
            }
        }

    }
    //esc를 눌렀을 때 취소하면 일어나는일 
    public void Cancel()
    {
        if (isPreviewActivated)
        {
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
            go_BaseUI.SetActive(false);
        }
    }
    //상점 컨트롤러
    private void Window()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }
    // 상점 열기 
    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }
    // 상점 닫기 
    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }

}

