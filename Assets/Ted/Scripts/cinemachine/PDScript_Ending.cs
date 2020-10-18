using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PDScript_Ending : MonoBehaviour
{
    [SerializeField] GameObject sceneCharacter;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject endingUI;
    [SerializeField] Camera weaponCam;
    public PlayableDirector endingPD;
    private Camera endingSceneCam;
    private float time;

    void Start()
    {
        sceneCharacter.SetActive(false);
        endingSceneCam = GetComponent<Camera>();
        endingSceneCam.enabled = false;
        endingSceneCam.GetComponent<CinemachineBrain>().enabled = false;
        endingPD.Pause();
        endingUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 605f)
        {
            // CinemachineBrain이 들어있는 카메라를 활성화 시킨다
            endingSceneCam.GetComponent<CinemachineBrain>().enabled = true;
            endingSceneCam.enabled = true;
            weaponCam.enabled = false;
            // 캔버스를 비활성화
            canvas.SetActive(false);
            // 씬에 등장할 캐릭터들을 활성화 시킨다
            sceneCharacter.SetActive(true);
            // 씨네마신 엔딩 PD 재생 시킨다.
            endingPD.Play();

            if (endingPD.time >= endingPD.duration)
            {
                endingUI.SetActive(true);

                Time.timeScale = 0;
            }

        }
    }
}
