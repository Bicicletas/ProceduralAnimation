using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject continueButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.SetActive(false);
        }
    }

    public void Play(string scene)
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync(scene);
    }

    public void Continue(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
