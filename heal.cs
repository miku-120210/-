using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heal : MonoBehaviour
{
    public int heartnum;
    public playerItem playercheck;
    [Header("jiroSE")] public AudioClip jiroSE;

    private Text HPtext = null;
    void Update()
    {
        //HPtext = GetComponent<Text>();
        if (playercheck.isOn)
        {
            if(GManager.instance != null)
            {
                GManager.instance.addHeartnum();
                GManager.instance.PlaySE(jiroSE);
                Destroy(this.gameObject);
            }
        }
    }
}
