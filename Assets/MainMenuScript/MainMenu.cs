using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ExitScene;
    public void startGame(){
        SceneManager.LoadSceneAsync(1);
    }

    private void Start(){
        ExitScene = GameObject.Find("ExitScene");
    }

    public void gameOver(){     //記得沒有辦法找到Scene
        // ExitScene.SetActive(true);
    }


}
