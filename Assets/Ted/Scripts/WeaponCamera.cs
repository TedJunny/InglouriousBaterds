using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WeaponCamera : MonoBehaviour
{
    PlayableDirector pdDirector;
    PlayableDirector pdDirectorEnding;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        pdDirector = GameObject.Find("PD_Opening").GetComponent<PlayableDirector>();
        //pdDirectorEnding = GameObject.Find("Closing").GetComponent<PlayableDirector>();
        cam = GetComponent<Camera>();
        cam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pdDirector.time >= pdDirector.duration)
        {
            cam.enabled = true;
            //print("웨폰캠 끄기");
            //if (EndingSceneisPlaying())
            //{
            //    cam.enabled = false;
            //}
        }

    }

    private bool EndingSceneisPlaying()
    {
        if (pdDirectorEnding.time < pdDirectorEnding.duration)
        {
            return true;
        }

        else if (pdDirectorEnding.time >= pdDirectorEnding.duration)
        {
            return false;
        }
        else return false;
    }
}
