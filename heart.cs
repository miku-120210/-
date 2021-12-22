using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heart : MonoBehaviour
{
    private Text HPtext = null;
    private int oldheart = 0;
    void Start()
    {
        HPtext = GetComponent<Text>();
        if(GManager.instance != null)
        {
            HPtext.text = "Å~ " + GManager.instance.heartnum;
        }
        else
        {
            Debug.Log("No GameManager");
            Destroy(this);
        }
    }

    void Update()
    {
        if(oldheart != GManager.instance.heartnum)
        {
            HPtext.text = "Å~ " + GManager.instance.heartnum;
            oldheart = GManager.instance.heartnum;
        }
    }
}
