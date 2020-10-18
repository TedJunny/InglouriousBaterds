using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라의 이동방향의 반대 방향으로 무기를 흔드는 효과를 부여하고 싶다.
public class WeaponSway : MonoBehaviour
{
    [SerializeField] float swayAmount;
    [SerializeField] float maxAmount;
    [SerializeField] float smoothAmount;

    private Vector3 initialPosition;

    private void Start()
    {
        // 무기의 위치는 로컬 좌표를 사용하기 때문에 transform.localPosition을 사용한다
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BuffItemManager.Instance.isBuffShopActive)
        {
            SwayMotion();
        }
    }

    void SwayMotion()
    {
        // 마우스의 입력을 받는다 (
        float movementX = -Input.GetAxis("Mouse X") * swayAmount;
        float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
        float rotationX = -Input.GetAxis("Mouse Y") * swayAmount;
        float rotationY = -Input.GetAxis("Mouse X") * swayAmount * 2f;
        // 최대 최소값을 설정한다.
        Mathf.Clamp(movementX, -maxAmount, maxAmount);
        Mathf.Clamp(movementY, -maxAmount, maxAmount);
        Mathf.Clamp(rotationX, -maxAmount, maxAmount);
        Mathf.Clamp(rotationY, -maxAmount, maxAmount);
        // 마우스 입력값에 최대 최소를 설정한 수치를 위치와 회전 정보로 설정한다
        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        Quaternion finalRotation = new Quaternion(rotationX, rotationY, 0, 1);
        // 그 위치로 부드럽게 이동하게 만든다.
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation, Time.deltaTime * smoothAmount);
    }
}
