using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnCloseClicked()
    {
        Application.Quit();
    }
}
