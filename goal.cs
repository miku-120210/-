using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    public playerItem trigger;
    private Animator anime = null;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger.isOn)
        {
            anime.Play("player_smile");
        }
        
    }
}
