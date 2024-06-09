using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverLogic : MonoBehaviour
{
    public GameObject exitScene;    //因為在一開始時他不是開的，我們沒辦法用程式找到他，一定要在inspector中拉進去
    
    public void backToMainMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    public void gameOver(){     //記得沒有辦法找到Scene
        exitScene.SetActive(true);
    }
}

