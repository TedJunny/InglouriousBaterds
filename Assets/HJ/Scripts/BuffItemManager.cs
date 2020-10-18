using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffItemManager : MonoBehaviour
{
    #region "체력아이템 부분"
    //체력 아이템 가격
    public int heartItemPrice = 30;
    //회복하는 양
    public int curePlayerHP = 10;
    //MaxHP의 늘어나는 양 
    public int maxHpPlus = 20;

    #endregion
    #region "총알 구매 아이템 부분"
    // 현재 Player가 가지고 있는 총 총알 수 
    //private int playerLeftBullet;
    //총알 가격 
    public int BulletPrice = 30;
    //증가하는 총알의 양 
    public int BulletUP = 30;
    #endregion
    #region "공격력 업 아이템 부분" 
    //플레이어의 공격력 

    //아이템가격
    public int AttackUpPrice = 100;
    //증가하는 양 
    public int AttackUpValue = 10;
    #endregion
    //구매 가능 상태 
    bool OnBuy = true;
    // 버프상점 창이 활성화 유무 
    [HideInInspector]
    public bool isBuffShopActive = false;
    //버프상점 창
    public GameObject buffShop;
    //상점 효과음 
    private AudioSource shopSoundManager;
    public AudioClip shopSound;

    public static BuffItemManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        shopSoundManager = GetComponent<AudioSource>();

    }
    // Update is called once per frame
    void Update()
    {
        //Tab을 누르면 윈도우창을 열수 있게
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Window();
        }
    }
    // Tab키를 누르면 버프창이 켜지고 꺼진다. 
    private void Window()
    {
        if (!isBuffShopActive)
        {
            isBuffShopActive = true;
            buffShop.SetActive(true);
        }
        else
        {
            isBuffShopActive = false;
            buffShop.SetActive(false);
        }
    }
    //총알 구입 버튼 
    //버튼을 클릭하면 금액이 차감되고 플레이어의 MAX탄창이 증가 하게 하고 싶다.
    //필요요 소 - 아이템 가격, 증가하는 양 , 플레이어의 최대 총알 
    public void AKBulletUpClick()
    {
        
        if (MoneyManager.Instance.MONEY < BulletPrice)
        {
            OnBuy = false;
        }
        else
        {
            OnBuy = true;
            //사운드 실행 
            shopSoundManager.PlayOneShot(shopSound);
            MoneyManager.Instance.MONEY -= BulletPrice;

        }
        if (OnBuy)
        {
            Weapon_AK.Instance.BulletsLeft += BulletUP;

        }

    }

    public void GlockBulletUpClick()
    {
        
        if (MoneyManager.Instance.MONEY < BulletPrice)
        {
            OnBuy = false;
        }
        else
        {
            OnBuy = true;
            //사운드 실행 
            shopSoundManager.PlayOneShot(shopSound);
            MoneyManager.Instance.MONEY -= BulletPrice;

        }
        if (OnBuy)
        {
            Weapon_Glock.Instance.BulletsLeft += BulletUP;

        }
    }

    //HP 아이템 버튼을 클릭하면 금액이 차감되고 플레이어의 HP가 회복 되게 하고싶다.
    //필요요소- 아이템 가격, 회복하는 양 , 
    public void HeartItemClick()
    {
        
        if (MoneyManager.Instance.MONEY < heartItemPrice)
        {
            OnBuy = false;
        }
        else
        {
            OnBuy = true;
            //사운드 실행 
            shopSoundManager.PlayOneShot(shopSound);
            //금액 지불 s
            MoneyManager.Instance.MONEY -= heartItemPrice;


        }
        if (OnBuy)
        {

            if (PlayerHP.Instance._PLAYERHP >= PlayerHP.Instance.MaxHP)
            {
                PlayerHP.Instance.MaxHP += maxHpPlus;
                PlayerHP.Instance._PLAYERHP = PlayerHP.Instance.MaxHP;
            }
            else
            {
                //HP회복 
                PlayerHP.Instance._PLAYERHP += curePlayerHP;
                PlayerHP.Instance.MaxHP += maxHpPlus;
                if (PlayerHP.Instance._PLAYERHP >= PlayerHP.Instance.MaxHP)
                {
                 
                    PlayerHP.Instance._PLAYERHP = PlayerHP.Instance.MaxHP;
                }
            }


        }
    }

    //플레이어 공격력 증가 
    //공격력 업 아이템을 클릭하면 금액이 차감되고 플레이어의 공격력이올라가게 하고싶다.
    //필요요소 - 아이템 가격, 올라가는 공격력, 
    public void AttackUPClick()
    {
        
        if (MoneyManager.Instance.MONEY < AttackUpPrice)
        {
            OnBuy = false;
        }
        else
        {
            OnBuy = true;
            //사운드 실행 
            shopSoundManager.PlayOneShot(shopSound);
            MoneyManager.Instance.MONEY -= AttackUpPrice;
        }
        if (OnBuy)
        {
            //이부분에 플레이어의 공격력을 받아오기 
            Weapon_AK.Instance.WeaponDamage += AttackUpValue;
            Weapon_Glock.Instance.WeaponDamage += AttackUpValue;

        }
    }
}