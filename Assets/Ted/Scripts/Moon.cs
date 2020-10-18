using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    float rotSpeed;
    float gameplayTime;
    Light moon;
    // Start is called before the first frame update
    void Start()
    {
        moon = GetComponent<Light>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameplayTime += Time.deltaTime;
        if (gameplayTime > 400f)
        {
            float speed = 0.75f / 200f;
            moon.intensity += speed * Time.deltaTime;
        }
    
    }
}
