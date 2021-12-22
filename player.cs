using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    #region//インスペクター用
    [Header("接地判定")] public GroundCheck ground;
    [Header("重力")] public float gravity;
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプ高さ")] public float jumpHeight;
    [Header("ジャンプ長さ")] public float jumpLimitTime;
    [Header("天井判定")] public GroundCheck head;
    [Header("踏みつけ判定高さの割合")] public float stepOnRate;
    [Header("ゴール文字")] public GameObject goalstr;
    public GameObject continuepoint;
    public GameObject result;
    //SE
    [Header("ジャンプ音")] public AudioClip jumpSE;
    [Header("ダメージ音")] public AudioClip damageSE;
    [Header("敵倒す音")] public AudioClip taosuSE;
    [Header("ゲームオーバー音")] public AudioClip gameoverSE;
    [Header("ゴール音")] public AudioClip goalSE;
    public GameObject bgm;
    #endregion

    #region//プライベート変数
    private Animator anime = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isJump = false;
    private float jumpPos = 0.0f; //飛べる高さ
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
            //設置判定
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //縦速度
            float ySpeed = GetYSpeed();

            //アニメーション
            SetAnimation();
            
            //移動速度
            rb.velocity = new Vector2(4.4f, ySpeed);


        }
        else
        {
            rb.velocity = new Vector2(0, -gravity);
        }
    }
    /// <summary>
    /// Y成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed()
    {
        float ySpeed = -gravity;
        float verticalKey = Input.GetAxis("Vertical");
        
        //踏んづけたジャンプ
        if(isOtherJump)
        {
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y; //現在高さが飛べる高さより下か
            bool canTime = jumpLimitTime > jumpTime; //ジャンプ時間の長さ

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
        //地面にいた場合
        else if (isGround)
        {
            if (verticalKey > 0)
            {
                if (!isJump)
                {
                    GManager.instance.PlaySE(jumpSE);
                }
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプ位置記録
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        //ジャンプしている場合
        else if (isJump)
        {
            bool pushUpKey = verticalKey > 0; //ボタン押したか
            bool canHeight = jumpPos + jumpHeight > transform.position.y; //現在高さが飛べる高さより下か
            bool canTime = jumpLimitTime > jumpTime; //ジャンプ時間の長さ

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
    /// アニメーション設定
    /// </summary>
    private void SetAnimation()
    {
        anime.SetBool("jump", isJump || isOtherJump);
        anime.SetBool("ground", isGround);
    }

    #region//敵判定
    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == enemyTag)
             { 
                //踏みつけ判定になる高さ（キャラ高さの設定%）
                float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

                //踏みつけ判定の位置（足先から設定%）
                float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

                foreach(ContactPoint2D p in collision.contacts)
                {
                    if (p.point.y < judgePos)
                    {
                        //踏んづけたら跳ねる
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
                            Debug.Log("objectcollisionついてない");
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
    /// コンティニュー待機か
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
    /// ダウンモーション終了したか（bool）
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
    /// コンティニューする
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
                //レイヤーを変更する
                gameObject.layer = LayerMask.NameToLayer("player");
                //2秒無敵
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