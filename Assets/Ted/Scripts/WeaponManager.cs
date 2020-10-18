using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 무기 게임 오브젝트를 활성화 비활성화 시키는 방법을 통해서 무기를 교체한다.
// 현재 무기에 맞는 이미지를 UI를 통해서 보여준다.
public class WeaponManager : MonoBehaviour
{
    [SerializeField] float switchDelay = 1f; // 무기 변경 후 다시 변경할 때까지의 시간
    [SerializeField] GameObject[] weapon;
    [SerializeField] GameObject[] weaponImage;
    [SerializeField] GameObject reloadUI;

    private int index = 0; // 현재 무기의 index 번호
    public bool isSwitching = false; // 딜레이에 필요한 bool 변수 

    void Start()
    {
        // 현재 무기를 제외한 다른 무기를 비활성화 시킨다.
        InitializeWeapon();
    }
       
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
        {
            index++;
            if (index >= weapon.Length)
                index = 0;
            StartCoroutine(SwitchDelay(index));
            reloadUI.SetActive(false);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            index--;
            if (index < 0)
                index = weapon.Length - 1;
            StartCoroutine(SwitchDelay(index));
            reloadUI.SetActive(false);
        }

        for (int i = 49; i < 58; i++)
        {
            if (Input.GetKeyDown((KeyCode)i) && !isSwitching && weapon.Length > i - 49 && index != i - 49)
            {
                index = i - 49;
                StartCoroutine(SwitchDelay(index));
                reloadUI.SetActive(false);
            }
        }
    }

    private void InitializeWeapon()
    {
        for (int i = 0; i < weapon.Length; i++)
        {
            weapon[i].SetActive(false);
            weaponImage[i].SetActive(false);
        }
        weapon[0].SetActive(true);
        weaponImage[0].SetActive(true);
        index = 0;
    }

    private IEnumerator SwitchDelay (int newIndex)
    {
        isSwitching = true;
        SwitchWeapons(newIndex);
        yield return new WaitForSeconds(switchDelay);
        isSwitching = false;
    }

    private void SwitchWeapons(int newIndex)
    {
        for (int i = 0; i < weapon.Length; i++)
        {
            weapon[i].SetActive(false);
            weaponImage[i].SetActive(false);
        }
        weapon[newIndex].SetActive(true);
        weaponImage[newIndex].SetActive(true);
    }
}
