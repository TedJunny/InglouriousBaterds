using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    [SerializeField] AudioClip[] damageSound;
    [SerializeField] AudioClip[] severeDamageSound;
    [SerializeField] AudioClip[] dyingSound;
    [SerializeField] AudioSource audio;
    [SerializeField] Image bloodScreen;
    [SerializeField] CharacterController cc;
    [SerializeField] float dieYpos;
    [SerializeField] GameObject endingUI;

    // 바리게이트의 hp
    public static PlayerHP Instance;
    private void Awake()
    {
        Instance = this;
    }

    //바리게이트의 최대HP
    public float MaxHP = 100;
    private float currentHP;
    //HP Text
    public Slider HP_Bar;

    public Text hpText;

    public float _PLAYERHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;

            HP_Bar.value = (float)currentHP / (float)MaxHP;
            // 추가 
            currentHP = (int)currentHP;
            hpText.text = currentHP.ToString() + " / " + MaxHP.ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHP = MaxHP;
        HP_Bar.value = currentHP;
        audio = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();
        endingUI.SetActive(false);
        hpText.text = currentHP.ToString() + " / " + MaxHP.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerDamage(float zombieAttackPower)
    {
        if (currentHP >= 50)
        {
            _PLAYERHP -= zombieAttackPower;
            RandomDamageSound();
            StartCoroutine(ShowBloodScreen());
        }
        else if (currentHP <= 50 && currentHP > 30)
        {
            _PLAYERHP -= zombieAttackPower;
            RandomDamageSound();
            StartCoroutine(SevereDamageScreen());
        }
        else if (currentHP <= 30 && currentHP > 0)
        {
            _PLAYERHP -= zombieAttackPower;
            RandomSevereDamageSound();
            bloodScreen.color = new Color(0.4f, 0.08f, 0.08f, 0.85f);
        }
        else if (currentHP <= 0)
        {
            StartCoroutine(PlayerDie());
            _PLAYERHP = 0;
            cc.enabled = false;
            bloodScreen.color = new Color(0.4f, 0.08f, 0.08f, 0.85f);
            return;
        }
    }

    private void RandomDamageSound()
    {
        int random = Random.Range(0, damageSound.Length);

        audio.PlayOneShot(damageSound[random]);
    }

    private void RandomSevereDamageSound()
    {
        int random = Random.Range(0, severeDamageSound.Length);

        audio.PlayOneShot(severeDamageSound[random]);
        print("엄청 아픈 사운드");
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(0.4f, 0.08f, 0.08f, Random.Range(0.3f, 0.4f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }

    IEnumerator SevereDamageScreen()
    {
        bloodScreen.color = new Color(0.4f, 0.08f, 0.08f, Mathf.Lerp(0.65f, 0.8f, Time.deltaTime * 100));
        yield return new WaitForSeconds(1.0f);
        bloodScreen.color = new Color(0.4f, 0.08f, 0.08f, Mathf.Lerp(0.8f, 0.65f, Time.deltaTime * 100));
    }

    IEnumerator PlayerDie()
    {
        int random = Random.Range(0, dyingSound.Length);
        audio.clip = dyingSound[random];
        audio.Play();
        print("죽는 사운드");

        while (transform.position.y > dieYpos)
        {
            transform.position += Vector3.down * 2.5f * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);
        endingUI.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0;
    }
}
