using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScript : MonoBehaviour
{
   public Button ResetButton;

    // Update is called once per frame
    public void Reset(){
        PlayerPrefs.SetInt("HighScore", 0);
    }
}
