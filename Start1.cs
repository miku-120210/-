using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start1 : MonoBehaviour
{
    public GameObject playerobj;
    public GameObject continuepoint;
    public GameObject gameoverobj;
    [Header("Gameover SE")] public AudioClip gameoverSE;
    [Header("Retry SE")] public AudioClip retrySE;

    private player p;
    private bool dogameover = false;
    private bool retrygame = false;

    void Start()
    {
         
        if(playerobj != null && continuepoint != null)
        {
            gameoverobj.SetActive(false);
            playerobj.transform.position = continuepoint.transform.position;

            p = playerobj.GetComponent<player>();
            if (p == null)
            {
                Debug.Log("Not player");
            }
        }
        else
        {
            Debug.Log("empty");
        }
    }

    void Update()
    {
        //ゲームオーバーの処理
        if(GManager.instance.isGameover && !dogameover)
        {
            gameoverobj.SetActive(true);
            GManager.instance.PlaySE(gameoverSE);
            dogameover = true;
        }
        //プレイヤーがダメージ受けたときの処理
        else if(p != null && p.IsContinueWating() && !dogameover)
        {

            // playerobj.transform.position = continuepoint.transform.position;
            p.Continueplayer();
            
        }
    }
    public void retry()
    {
        GManager.instance.retrygame();
        GManager.instance.PlaySE(retrySE);
        /*playerobj.transform.position = continuepoint.transform.position;
        gameoverobj.SetActive(false);
        p.Continueplayer();*/
    }
}
