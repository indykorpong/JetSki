using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverText;
    public bool IsGameEnd = false;

    void Update()
    {
        if(IsGameEnd)
        {
            GameOverText.SetActive(true);
            if (Input.anyKeyDown)
            {
                SceneManager.LoadSceneAsync("Mainmenu");
            }
        }
    }

}
