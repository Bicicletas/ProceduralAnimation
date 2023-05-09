using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject continueButton;

    private void Start()
    {
        if (PlayerPrefs.HasKey("x"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void Play(string scene)
    {
        PlayerPrefs.DeleteAll();
        print("PlayerPrefs Delated");
        SceneManager.LoadScene(scene);
    }

    public void Continue(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
