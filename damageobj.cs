using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageobj : MonoBehaviour
{
    //public GameObject collisionobj;

    private string playerTag = "Player";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == playerTag)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Invoke("colback", 3f);
        }
    }

    private void colback()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
