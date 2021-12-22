using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    //public GameObject continuepoint;

    [Header("�X�R�A")] public int score;
    [Header("���ݎc�@")] public int heartnum;
    [Header("�f�t�H�c�@")] public int defheart;
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
    /// �c�@���₷
    /// </summary>
    public void addHeartnum()
    {
        if(heartnum < 3)
        {
            ++heartnum;
        }
    }
    /// <summary>
    /// �c�@1���炷
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

    //���X�^�[�g
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
