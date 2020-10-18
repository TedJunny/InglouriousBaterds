using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 마우스 입력에 따라 물체를 회전시키고 싶다.
// 필요속성 : 회전속도
// 마우스 Y 를 이동시킬때(X축회전) 회전의 제약을 걸고싶다.
// 필요속성 : 제약 범위 -60 ~ +60
public class CamRotate : MonoBehaviour
{
    // 필요속성
    public float rotSpeed = 200;
    // 회전된 각도에 대한 정보를 직접 속성 값을 통해서 엔진에 제공
    float mx;
    float my;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자의 마우스 입력에 따라 물체를 회전시키고 싶다.
        // 1. 사용자의 입력에따라
        if (!BuffItemManager.Instance.isBuffShopActive)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            // 2. 방향이 필요
            // 3. 회전시키고 싶다.
            mx += h * rotSpeed * Time.deltaTime;
            my += v * rotSpeed * Time.deltaTime;
            // 카메라의 상하 움직임에 대하여 제약을 걸고 싶다.
            my = Mathf.Clamp(my, -80, 60);

            transform.eulerAngles = new Vector3(-my, mx, 0);

        }

    }
}
