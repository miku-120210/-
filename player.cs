using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    #region//�C���X�y�N�^�[�p
    [Header("�ڒn����")] public GroundCheck ground;
    [Header("�d��")] public float gravity;
    [Header("�W�����v���x")] public float jumpSpeed;
    [Header("�W�����v����")] public float jumpHeight;
    [Header("�W�����v����")] public float jumpLimitTime;
    [Header("�V�䔻��")] public GroundCheck head;
    [Header("���݂����荂���̊���")] public float stepOnRate;
    [Header("�S�[������")] public GameObject goalstr;
    public GameObject continuepoint;
    public GameObject result;
    //SE
    [Header("�W�����v��")] public AudioClip jumpSE;
    [Header("�_���[�W��")] public AudioClip damageSE;
    [Header("�G�|����")] public AudioClip taosuSE;
    [Header("�Q�[���I�[�o�[��")] public AudioClip gameoverSE;
    [Header("�S�[����")] public AudioClip goalSE;
    public GameObject bgm;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Animator anime = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isJump = false;
    private float jumpPos = 0.0f; //��ׂ鍂��
    private bool isHead = false;
    private float jumpTime = 0.0f;
    private string enemyTag = "Enemy";
    private bool isDown = false;
    private CapsuleCollider2D capcol = null;
    private bool isOtherJump = false;
    private float otherJumpHeight = 0.0f;
    private bool nondownanime = false;
    private string deadtag = "deadarea";
    private string hittag = "hitarea";
    private string goaltag = "goal";
    #endregion

    #region//Start
    void Start()
    {
        anime = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
    }
    #endregion

    #region//Update
    void FixedUpdate()
    {
        if(!isDown && !GManager.instance.isGameover)
        {
            //�ݒu����
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //�c���x
            float ySpeed = GetYSpeed();

            //�A�j���[�V����
            SetAnimation();
            
            //�ړ����x
            rb.velocity = new Vector2(4.4f, ySpeed);


        }
        else
        {
            rb.velocity = new Vector2(0, -gravity);
        }
    }
    /// <summary>
    /// Y�����ŕK�v�Ȍv�Z�����A���x��Ԃ�
    /// </summary>
    /// <returns>Y���̑���</returns>
    private float GetYSpeed()
    {
        float ySpeed = -gravity;
        float verticalKey = Input.GetAxis("Vertical");
        
        //����Â����W�����v
        if(isOtherJump)
        {
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y; //���ݍ�������ׂ鍂����艺��
            bool canTime = jumpLimitTime > jumpTime; //�W�����v���Ԃ̒���

            if (canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        //�n�ʂɂ����ꍇ
        else if (isGround)
        {
            if (verticalKey > 0)
            {
                if (!isJump)
                {
                    GManager.instance.PlaySE(jumpSE);
                }
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //�W�����v�ʒu�L�^
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        //�W�����v���Ă���ꍇ
        else if (isJump)
        {
            bool pushUpKey = verticalKey > 0; //�{�^����������
            bool canHeight = jumpPos + jumpHeight > transform.position.y; //���ݍ�������ׂ鍂����艺��
            bool canTime = jumpLimitTime > jumpTime; //�W�����v���Ԃ̒���

            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }
        return ySpeed;
    }
    /// <summary>
    /// �A�j���[�V�����ݒ�
    /// </summary>
    private void SetAnimation()
    {
        anime.SetBool("jump", isJump || isOtherJump);
        anime.SetBool("ground", isGround);
    }

    #region//�G����
    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == enemyTag)
             { 
                //���݂�����ɂȂ鍂���i�L���������̐ݒ�%�j
                float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

                //���݂�����̈ʒu�i���悩��ݒ�%�j
                float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

                foreach(ContactPoint2D p in collision.contacts)
                {
                    if (p.point.y < judgePos)
                    {
                        //����Â����璵�˂�
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if (o != null)
                        {
                            otherJumpHeight = o.boundHeight;
                            o.playerStepOn = true;
                            jumpPos = transform.position.y;
                            isOtherJump = true;
                            isJump = false;
                            jumpTime = 0.0f;
                        }
                        else
                        {
                            Debug.Log("objectcollision���ĂȂ�");
                        }
                    }
                    else
                    {
                        damage(true);
                        break;
                    }
                }
             }
                
        }
    #endregion

    /// <summary>
    /// �R���e�B�j���[�ҋ@��
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWating()
    {
        if (GManager.instance.isGameover)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nondownanime;
        }
    }
    /// <summary>
    /// �_�E�����[�V�����I���������ibool�j
    /// </summary>
    private bool IsDownAnimEnd()
    {
        if (isDown && anime != null)
        {
            AnimatorStateInfo currentState = anime.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("down"))
            {
                if(currentState.normalizedTime >= 0.5)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// �R���e�B�j���[����
    /// </summary>
    public void Continueplayer()
    {
        isDown = false;
        anime.Play("player");
        isJump = false;
        isOtherJump = false;
        nondownanime = false;
    }

    private void damage(bool downanime)
    {
        if (isDown)
        {
            return;
        }
        else
        {
            if(downanime)
            {
                anime.Play("down");
                //���C���[��ύX����
                gameObject.layer = LayerMask.NameToLayer("player");
                //2�b���G
                Invoke("layerChange", 1.7f);
            }
            else
            {
                nondownanime = true;
                this.transform.position = continuepoint.transform.position;
            }
            isDown = true;
            GManager.instance.PlaySE(damageSE);
            GManager.instance.subHeartnum();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == hittag)
        {
            damage(true);
        }
        else if(collision.tag == deadtag)
        {
            damage(false);
        }
           

        else if(collision.tag == goaltag)
        {
            anime.Play("player_smile");
            isDown = true;
            goalstr.SetActive(true);
            GManager.instance.PlaySE(goalSE);
            bgm.SetActive(false);
            Invoke("playresult", 4f);
        }
    }
    private void layerChange()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    } 
    private void playresult()
    {
        result.SetActive(true);
    }


}
#endregion