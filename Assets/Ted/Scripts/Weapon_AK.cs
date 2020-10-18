using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Weapon_AK : MonoBehaviour
{
    #region Weapon specification
    [SerializeField] float shootRange = 100;
    [SerializeField] int bulletsPerMag = 30; // 탄창 당 총알 수
    [SerializeField] int bulletsLeft = 150; // 현재 Player가 가지고 있는 총 총알 수 (탄창 당 총알 수로 나누면 탄창의 수)
    [SerializeField] public int currentBullets; // 현재 탄창의 총알 수
    [SerializeField] float fireRate = 0.3f;
    [SerializeField] int weaponDamage = 43;
    [SerializeField] Vector3 aimPosition; // 정조준 모드에서의 위취
    [SerializeField] GameObject shopUI;// 버프상점 의 유무판정
    private Vector3 originalPosition;  // 기본 모드에서의 총의 위치 
    #endregion

    #region    Parameters
    private float fireTimer;
    private bool isRealoading;
    public bool isAiming;
    private bool isRunning;
    private bool shootInput;
    private bool isBuilding;
    public bool isShooting;
    #endregion

    #region    References
    [SerializeField] GameObject Crosshair;
    [SerializeField] public GameObject reloadTextUI;
    [SerializeField] Transform ShootPosition;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem bulletCasing;
    [SerializeField] Text bulletsText;
    [SerializeField] Text fireModeText;
    [SerializeField] WeaponManager wm;
    [SerializeField] GameObject noAmmoTextUI;
    private Animator anim;
    private CharacterController cc;
    private CamRecoil recoil;
    private PlayerMove pm;
    #endregion

    #region  Prefabs
    [SerializeField] GameObject hitParticles;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] GameObject bulletImpactNormal;
    #endregion

    #region Sounds
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioClip drawSound;
    #endregion

    #region Recoil
    //[SerializeField] Transform camRecoil;
    //[SerializeField] Vector3 recoilkickback;
    //[SerializeField] float recoilAmount;
    #endregion

    #region Grenade
    //[SerializeField] GameObject bombFactory;
    //[SerializeField] float throwPower = 15f;
    #endregion

    private float currentTime;

    public enum ShootMode { Auto, Semi }
    public ShootMode shootingMode;

    public int BulletsLeft
    {
        get
        {
            return bulletsLeft;
        }

        set
        {
            bulletsLeft = value;
            bulletsText.text = currentBullets + " / " + bulletsLeft;
        }
    }

    //weaponDamageUI
    public Text akmDamageText;
    public int WeaponDamage
    {
        get
        {
            return weaponDamage;
        }
        set
        {
            weaponDamage = value;
            akmDamageText.text = weaponDamage.ToString();
        }
    }

    public static Weapon_AK Instance;


    private void Awake()
    {
        // OnEnable 이전에 호출되는 함수. 라이프사이클에서 가장 먼저 호출된다
        anim = GetComponent<Animator>();
        bulletsText.text = currentBullets + " / " + bulletsLeft;
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        // 해당 게임오브젝트가 활성화 되었을 때 자동으로 호출되는 함수
        anim.CrossFadeInFixedTime("Draw", 0.01f);
        audioSource.clip = drawSound;
        audioSource.Play();
        bulletsText.text = currentBullets + " / " + bulletsLeft; // 현재 잔여 탄환 개수를 UI에 표기
    }

    void Start()
    {
        shootingMode = ShootMode.Auto;
        // 탄창의 탄약 수를 초기화
        currentBullets = bulletsPerMag;
        audioSource = GetComponent<AudioSource>();
        bulletsText.text = currentBullets + " / " + bulletsLeft; // 현재 잔여 탄환 개수를 UI에 표기
        fireModeText.text = shootingMode.ToString();
        reloadTextUI.SetActive(false);
        Crosshair.SetActive(false);
        isBuilding = false;
        noAmmoTextUI.SetActive(false);
        originalPosition = transform.localPosition;
        cc = GetComponentInParent<CharacterController>();
        wm = GetComponentInParent<WeaponManager>();
        recoil = GetComponentInParent<CamRecoil>();
        pm = GetComponentInParent<PlayerMove>();
        akmDamageText.text = weaponDamage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        fireModeText.text = shootingMode.ToString();
        currentTime += Time.deltaTime;

        switch (shootingMode) // 발사 모드 제어 스크립트 (열거형 형식)
        {
            case ShootMode.Auto: // 연사 모드 
                shootInput = Input.GetButton("Fire1"); // 총기 발사 신호 변수

                if (Input.GetKeyDown(KeyCode.B)) // 발사 모드 변경
                {
                    shootingMode = ShootMode.Semi;
                }
                break;

            case ShootMode.Semi: // 단발 모드
                shootInput = Input.GetButtonDown("Fire1"); // 총기 발사 신호 변수

                if (Input.GetKeyDown(KeyCode.B)) // 발사 모드 변경
                {
                    shootingMode = ShootMode.Auto;
                }
                break;
        }

        // 현재 애니메이터가 0번 레이어(base layer)의 어떤 상태에 있는지 정보 가져오기
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isRealoading = info.IsName("Reload"); // Reload 상태와 isReloading의 bool 정보를 같게 하겠다

        if (shootInput) // 총기 발사 신호가 들어오면 RifleShoot 메소드에서 총기 발사 수행
        {
            RifleShoot();
        }
        else Crosshair.SetActive(false);


        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (currentBullets < bulletsPerMag && bulletsLeft > 0 && !isAiming)
            {
                DoReload(); // 총기 재장전 메소드 실행
            }

        }

        if (fireTimer < fireRate) // 사격 간격 조절에 필요한 float 변수
            fireTimer += Time.deltaTime;

        if (bulletsLeft != 0)
        {
            noAmmoTextUI.SetActive(false);
        }


        AimDownSights();
        Run();
        //RecoilBack();
    }


    private void RifleShoot()
    {
        if (fireTimer < fireRate || currentBullets == 0 || isRealoading || isBuilding || isRunning || wm.isSwitching || shopUI.activeSelf)
        {
            if (currentBullets == 0)
            {
                reloadTextUI.SetActive(true);
                if (bulletsLeft == 0)
                {
                    noAmmoTextUI.SetActive(true);
                    reloadTextUI.SetActive(false);
                }

            }
            isShooting = false;
            // 연사 속도에 해당하는 시간이 지나지 않으면 총기 발사 되지 않도록 제어
            return;
        }
        //Recoil();
        noAmmoTextUI.SetActive(false);
        Crosshair.SetActive(true);

        // Raycast를 적용할 LayerMask에 대한 정보를 입력
        int layer = LayerMask.NameToLayer("Floor");
        layer = 1 << layer;
        RaycastHit hit;

        // 해당 레이어를 제외하고 RaycastHit 정보를 처리한다.
        if (Physics.Raycast(ShootPosition.transform.position, ShootPosition.transform.forward, out hit, shootRange, ~layer))
        {
            print(hit.collider.tag);
            // 부딪힌 녀석이 Following Zombie라면 Die()함수 호출
            FollowingEnemyAI es = hit.transform.GetComponent<FollowingEnemyAI>();
            FollowingEnemyAI rootEs = hit.transform.root.GetComponent<FollowingEnemyAI>();
           
            if (es) // 적의 몸통에 총알이 맞았을 때
            {
                es.OnNormalDamageProcess(weaponDamage, "BodyDamage", "BodyDie");
                // 적을 맞추면 붉은빛 탄흔 및 타격 이펙트를 실행한다.
                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(hitParticleEffect, 0.5f);
                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(bulletHole, 0.5f);

                // 적을 맞추면 크로스헤어의 색이 붉은색으로 바뀐다.
                Crosshair.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(1).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(2).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(3).GetComponent<Image>().color = Color.red;
            }
            //적이 오른쪽을 맞았을때 
            else if (hit.collider.tag == "EnemyRight")
            {
                rootEs.OnNormalDamageProcess(weaponDamage, "RightDamage", "RightDie");
                // 적을 맞추면 붉은빛 탄흔 및 타격 이펙트를 실행한다.
                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(hitParticleEffect, 0.5f);
                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(bulletHole, 0.5f);

                // 적을 맞추면 크로스헤어의 색이 붉은색으로 바뀐다.
                Crosshair.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(1).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(2).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(3).GetComponent<Image>().color = Color.red;
            }
            //적이 왼쪽을 맞았을때 
            else if (hit.collider.tag == "EnemyLeft")
            {
                rootEs.OnNormalDamageProcess(weaponDamage, "LeftDamage", "LeftDie");
                // 적을 맞추면 붉은빛 탄흔 및 타격 이펙트를 실행한다.
                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(hitParticleEffect, 0.5f);
                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(bulletHole, 0.5f);

                // 적을 맞추면 크로스헤어의 색이 붉은색으로 바뀐다.
                Crosshair.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(1).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(2).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(3).GetComponent<Image>().color = Color.red;
            }
            //적이 머리를 맞았을때 
            else if (hit.collider.tag == "EnemyHead")
            {
                rootEs.OnNormalDamageProcess(weaponDamage, "HeadDamage", "HeadDie");
                // 적을 맞추면 붉은빛 탄흔 및 타격 이펙트를 실행한다.
                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(hitParticleEffect, 0.5f);
                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(bulletHole, 0.5f);

                // 적을 맞추면 크로스헤어의 색이 붉은색으로 바뀐다.
                Crosshair.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(1).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(2).GetComponent<Image>().color = Color.red;
                Crosshair.transform.GetChild(3).GetComponent<Image>().color = Color.red;
            }

            else
            {
                GameObject bulletHoleNormal = Instantiate(bulletImpactNormal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(bulletHoleNormal, 0.5f);
                // 크로스헤어의 색을 defalut 값으로 돌려 놓는다.
                Crosshair.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                Crosshair.transform.GetChild(1).GetComponent<Image>().color = Color.white;
                Crosshair.transform.GetChild(2).GetComponent<Image>().color = Color.white;
                Crosshair.transform.GetChild(3).GetComponent<Image>().color = Color.white;
            }


        }

        else if (Physics.Raycast(ShootPosition.transform.position, ShootPosition.transform.forward, out hit, shootRange, layer))
        {
            GameObject bulletHoleNormal = Instantiate(bulletImpactNormal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            Destroy(bulletHoleNormal, 0.5f);
            // 크로스헤어의 색을 defalut 값으로 돌려 놓는다.
            Crosshair.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            Crosshair.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            Crosshair.transform.GetChild(2).GetComponent<Image>().color = Color.white;
            Crosshair.transform.GetChild(3).GetComponent<Image>().color = Color.white;
        }

        recoil.Recoil();
        currentBullets--; // 탄환의 개수를 감소시킨다.
        fireTimer = 0; // 연사 속도 계산 타이머를 리셋
        audioSource.PlayOneShot(shootSound); // 발사 소리 재생
        anim.CrossFadeInFixedTime("Fire", 0.01f);
        muzzleFlash.Play(); // 총구 섬광 이펙트
        bulletCasing.Play(); // 탄피 발사 이펙트
        bulletsText.text = currentBullets + " / " + bulletsLeft; // 현재 잔여 탄환 개수를 UI에 표기
        isShooting = true;

    }


    //private void GentThrow()
    //{
    //    // 마우스 좌측 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        GameObject bomb = Instantiate(bombFactory);
    //        bomb.transform.position = Camera.main.transform.position;

    //        // 수류탄 오브젝트의 Rigidbody 컴포넌트를 구한다.
    //        Rigidbody rb = bomb.GetComponent<Rigidbody>();
    //        // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가한다.
    //        rb.AddForce(ShootPosition.transform.forward * throwPower, ForceMode.Impulse);
    //    }
    //}

    private void DoReload()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (isRealoading) return; // 장전 중에 애니메이션이 다시 시작되는 것을 방지

        anim.CrossFadeInFixedTime("Reload", 0.01f);
        audioSource.PlayOneShot(reloadSound);

    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        //                                 IF              then    1st     else   2nd
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
        bulletsText.text = currentBullets + " / " + bulletsLeft;

    }

    private void AimDownSights()
    {
        if (Input.GetButton("Fire2") && !isRealoading && !isRunning)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * 8f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 38f, Time.deltaTime * 8f);
            isAiming = true;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 5f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60f, Time.deltaTime * 8f);
            isAiming = false;
        }
    }

    private void Run()
    {
        anim.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift) && !pm.isCrouching);
        isRunning = Input.GetKey(KeyCode.LeftShift) ? true : false;
    }


    //private void Recoil()
    //{
    //    Vector3 recoilVector = new Vector3(Random.Range(-recoilkickback.x, recoilkickback.x), recoilkickback.y, recoilkickback.z);
    //    Vector3 recoilCamVector = new Vector3(-recoilVector.y * 400f, recoilVector.x * 200f, 0);

    //    transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + recoilVector, recoilAmount / 2f);
    //    camRecoil.localRotation = Quaternion.Slerp(camRecoil.localRotation, Quaternion.Euler(camRecoil.localEulerAngles + recoilCamVector), recoilAmount);
    //}

    //private void RecoilBack()
    //{
    //    camRecoil.localRotation = Quaternion.Slerp(camRecoil.localRotation, Quaternion.identity, Time.deltaTime * 2f);
    //}
}
