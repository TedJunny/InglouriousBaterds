using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    [SerializeField] float transitionSpeed = 20f;
    [SerializeField] float bobSpeed = 4.8f;
    [SerializeField] float bobAmount = 0.05f;
    [SerializeField] PlayerMove controller;
    [SerializeField] Vector3 restPosition;

    private float timer = Mathf.PI / 2;
    Vector3 camPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HeadBobEffect();
    }

    void HeadBobEffect()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)  // 플레이어가 움직이고 있다면
        {
            if (Input.GetKey(KeyCode.LeftShift)) // Sprint시 Headbob효과 증대
            {
                timer += Time.deltaTime * bobSpeed * 1.2f;
                transform.localPosition = new Vector3(Mathf.Cos(timer) * bobAmount * 1.3f, restPosition.y + Mathf.Abs((Mathf.Sin(timer)) * bobAmount * 1.3f), restPosition.z);
            }
            else
            {
                timer += Time.deltaTime * bobSpeed;
                transform.localPosition = new Vector3(Mathf.Cos(timer) * bobAmount, restPosition.y + Mathf.Abs((Mathf.Sin(timer)) * bobAmount), restPosition.z);
            }
        }
        else // 플레이어가 움직이고 있지 않다면
        {
            timer = Mathf.PI / 2;
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, restPosition.x, transitionSpeed * Time.deltaTime), Mathf.Lerp(transform.localPosition.y, restPosition.y, transitionSpeed * Time.deltaTime),
                Mathf.Lerp(transform.localPosition.z, restPosition.z, transitionSpeed * Time.deltaTime));

        }

        if (timer > Mathf.PI * 2)
            timer = 0;
    }
}
