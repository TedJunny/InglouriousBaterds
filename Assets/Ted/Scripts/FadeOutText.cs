using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FadeOutText : MonoBehaviour
{
    public PlayableDirector director;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (director.time >= director.duration)
        {
            Destroy(gameObject, 3.0f);
        }
    }
}