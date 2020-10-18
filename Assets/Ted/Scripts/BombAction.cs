using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일정시간이 지나면 폭발하는 수류탄을 만들고 싶다.
// 필요 요소 : 폭발 지연시간, 일정시간
public class BombAction : MonoBehaviour
{
    [SerializeField] GameObject bombEffect;
    [SerializeField] float delayTime = 3.8f;


    float currentTime;
    float subcurrentTime;

    private void Start()
    {

    }


    private void Update()
    {
        currentTime += Time.deltaTime;
        // 2. 현재 시간이 폭발 지연시간보다 커지게 되면
        if (currentTime > delayTime)
        {
            currentTime = 0;
            // 3. 폭발음과 함께 폭발 효과가 구현된다.
            GameObject sm = GameObject.Find("SoundManager");
            SoundManager Audio = sm.GetComponent<SoundManager>();
            Audio.OnExplosionSound();

            GameObject vfx = Instantiate(bombEffect);
            vfx.transform.position = transform.position;
            print("폭발!");

            Destroy(gameObject);
            print("폭탄 없어짐!");
        }

    }
}





