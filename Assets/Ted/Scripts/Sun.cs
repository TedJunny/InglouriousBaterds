using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 600초 동안 90도 회전하도록 제어하고 싶다.
// 필요속성 : 1초에 이동할 회전각도
public class Sun : MonoBehaviour
{
    [SerializeField] Text realTimeText;
    public Font m_Font;
    float sunRotSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 rotationPoint = new Vector3(200f, 0f, 600f);
        //transform.RotateAround(rotationPoint, Vector3.forward, sunRotSpeed * Time.deltaTime);
        //transform.LookAt(rotationPoint);

    }

    private void FixedUpdate()
    {
        sunRotSpeed = 0.15f;
        transform.Rotate(Vector3.right * sunRotSpeed * Time.deltaTime);

        RealTimer();

    }

    private void RealTimer()
    {
        float realTime = Time.realtimeSinceStartup * 36;
        int seconds = (int)(realTime % 60);
        int minutes = (int)(realTime / 60) % 60;
        int hours = (int)(realTime / 3600) % 24;

        string timerString = string.Format("{0:0} : {1:00} : {2:00}", hours, minutes, seconds);

        realTimeText.font = m_Font;
        // realTimeText.fontSize
        realTimeText.text = timerString;
    }
}
