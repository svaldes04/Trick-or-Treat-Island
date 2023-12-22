using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    int score = 0;
    public AudioClip pointSound;
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        string message = "SCORE: "+score.ToString();
        scoreText.text = message;
        instance = this;
    }

    public void addPoint()
    {
        score++;
        string message = "SCORE: " + score.ToString();
        scoreText.text = message;
        audioSource.PlayOneShot(pointSound, 2);
    }
}
