using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    //public GameObject continuepoint;

    [Header("スコア")] public int score;
    [Header("現在残機")] public int heartnum;
    [Header("デフォ残機")] public int defheart;
    [HideInInspector] public bool isGameover;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 残機増やす
    /// </summary>
    public void addHeartnum()
    {
        if(heartnum < 3)
        {
            ++heartnum;
        }
    }
    /// <summary>
    /// 残機1減らす
    /// </summary>
    public void subHeartnum()
    {
        if(heartnum > 0)
        {
            --heartnum;
        }
        else
        {
            isGameover = true;
        }
    }

    //リスタート
    public void retrygame()
    {
        SceneManager.LoadScene("player");
        isGameover = false;
        heartnum = defheart;
        score = 0;
    }

    public void PlaySE(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("Not AudioSource");
        }
    }
}
