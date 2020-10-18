using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRecoil : MonoBehaviour
{   // 반동 관련 변수 선언
    [SerializeField] public float rotationSpeed = 6;
    [SerializeField] public float returnSpeed = 25;
    [SerializeField] public Vector3 recoilRotation = new Vector3(2f, 2f, 2f);
    [SerializeField] public Vector3 recoilRotationAimin = new Vector3(0.5f, 0.5f, 1.5f);

    public bool aiming;
    private Vector3 currentRotaiton;
    private Vector3 rot;
    private Weapon_AK _AK;
    private Weapon_Glock _Glock;

    private void Start()
    {
        _AK = GetComponentInChildren<Weapon_AK>();
        _Glock = GetComponentInChildren<Weapon_Glock>();
    }


    private void FixedUpdate()
    {
        currentRotaiton = Vector3.Lerp(currentRotaiton, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotaiton, rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(rot);
    }

    public void Recoil()
    {
        if (_AK.isAiming || _Glock.isAiming)
        {
            currentRotaiton += new Vector3(-recoilRotationAimin.x, Random.Range(-recoilRotationAimin.y, recoilRotationAimin.y), Random.Range(-recoilRotationAimin.z, recoilRotationAimin.z));
           
        }
        else
        {
            currentRotaiton += new Vector3(-recoilRotation.x, Random.Range(recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));

        }
    }


}
