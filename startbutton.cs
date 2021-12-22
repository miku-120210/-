using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startbutton : MonoBehaviour
{
    private bool firstPush = false;

    public void PressStart()
    {
    
        Debug.Log("start");

        if (!firstPush) 
        {
            Debug.Log("go to next");
            SceneManager.LoadScene("player");
            firstPush = true;
        }
     }
}
