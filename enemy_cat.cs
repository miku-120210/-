using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_cat : MonoBehaviour
{
    #region//�C���X�y�N�^�[�p
    [Header("���Z�X�R�A")] public int myscore;
    [Header("�ړ����x")] public float speed;
    [Header("�d��")] public float gravity;
    [Header("��ʊO�ł��s������")] public bool nonVisibleAct;
    [Header("�ڐG����")] public enemy_sidecheck checkCollision;
    [Header("dead SE")] public AudioClip deadSE;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private SpriteRenderer sr = null;
    private Rigidbody2D rb = null;
    private bool rightTleftF = false;
    private Animator anime = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool isDead = false;
    #endregion

    //Start
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        //����łȂ��Ȃ�
        if (!oc.playerStepOn)
        {

            if (sr.isVisible || nonVisibleAct)
            {
                if (checkCollision.isOn || checkCollision.isTurn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            else
            {
                rb.Sleep();
            }
        }
        //���񂾂�
        else
        {
            if (!isDead)
            {
                //Score
                if (GManager.instance != null)
                {
                    GManager.instance.score += myscore;
                    GManager.instance.PlaySE(deadSE);
                }
                anime.Play("enemy_down");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 3));
            }
        }

    }
}

    