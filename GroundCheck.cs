using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("エフェクト床判定")] public bool checkPlatformGround = true;
    private string groundTag = "Ground";
    private string platformTag = "GroundPlatform";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    public bool IsGround()
    {
        if(isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if(isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == groundTag)
        {
            isGroundEnter = true;           
        }
        else if(checkPlatformGround && collision.tag == platformTag)
        {
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true; 
        }
        else if (checkPlatformGround && collision.tag == platformTag)
        {
            isGroundStay = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundExit = true;  
        }
        else if (checkPlatformGround && collision.tag == platformTag)
        {
            isGroundExit = true;
        }
    }
}