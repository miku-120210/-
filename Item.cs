using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int myscore;
    public playerItem playercheck;
    [Header("Coin SE")] public AudioClip coinSE;
    void Update()
    {
        if (playercheck.isOn)
        {
            if (GManager.instance != null)
            {
                GManager.instance.score += myscore;
                GManager.instance.PlaySE(coinSE);
                Destroy(this.gameObject);
            }
        }
    }
}
