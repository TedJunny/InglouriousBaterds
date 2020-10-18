using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ted_Title : MonoBehaviour
{
    public string sceneName = "GameScene_Alphatype";

    public void  OnClickStart()
    {
        SceneManager.LoadScene(sceneName);  
        Debug.Log("시작");
    }

    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("종료");
    }
}
