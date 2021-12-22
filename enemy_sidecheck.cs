using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_sidecheck : MonoBehaviour
{
    [HideInInspector] public bool isOn = false;
    [HideInInspector] public bool isTurn = false;

    private string groundTag = "Ground";
    private string enemyTag = "Enemy";
    private string turnTag = "turn";

    #region//ê⁄êGîªíË
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == groundTag || collision.tag == enemyTag)
        {
            isOn = true;
        }
        else if(collision.tag == turnTag)
        {
            isTurn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == groundTag || collision.tag == enemyTag)
        {
            isOn = false;
        }
        else if(collision.tag == turnTag)
        {
            isTurn = false;
        }
    }
    #endregion
}
