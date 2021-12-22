using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text score = null;
    private int oldscore = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
        if(GManager.instance != null)
        {
            score.text = "Score " + GManager.instance.score;
        }
        else
        {
            Debug.Log("No GameManager");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(oldscore != GManager.instance.score)
        {
            score.text = "Score " + GManager.instance.score;
            oldscore = GManager.instance.score;
        }
    }
}
