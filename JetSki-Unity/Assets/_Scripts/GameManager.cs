using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static Text scoreText;
    private static int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
}
