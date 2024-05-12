using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void startGame(){
        SceneManager.LoadSceneAsync(1);
    }

    private void Start(){
    }

    


}
