using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class PDScript : MonoBehaviour
{
    public PlayableDirector pd;
    [SerializeField] GameObject sceneChar;
    [SerializeField] GameObject canvas;
    [SerializeField] AudioSource weapon;
    [SerializeField] Camera weaponCam;
    Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        sceneChar.SetActive(true);
        canvas.SetActive(false);
        weapon.enabled = false;
        weaponCam.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (pd.time >= pd.duration)
        {
            if (cam.GetComponent<CinemachineBrain>())
            {
                cam.GetComponent<CinemachineBrain>().enabled = false;
                cam.enabled = false;
            }

            weaponCam.enabled = true;
            sceneChar.SetActive(false);
            canvas.SetActive(true);
            weapon.enabled = true;
            Destroy(gameObject, 3.0f);

        }
    }
}
