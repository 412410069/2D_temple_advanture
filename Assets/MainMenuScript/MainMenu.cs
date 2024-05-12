using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void startGame(){
        SceneManager.LoadSceneAsync(1);
    }

    public void quitGame(){
        Application.Quit(); //build之後才會有用
        EditorApplication.isPlaying = false;
    }

    private void Start(){
    }

    


}
