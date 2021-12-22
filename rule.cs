using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rule : MonoBehaviour
{
    private bool firstpush = false;

    public void rulebutton()
    {
        if(!firstpush)
        {
            SceneManager.LoadScene("rule");
            firstpush = true;
        }
    }
    public void titlebutton()
    {
            SceneManager.LoadScene("title");
    }
}
