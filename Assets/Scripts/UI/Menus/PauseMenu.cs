using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PasueMenu;

    [Header("Panels of the Pasue Menu")]
    [Space]
    [SerializeField] GameObject[] panels;

    [Header("UI that should be hiden while paused")]
    [Space]
    [SerializeField] GameObject[] otherUI;
    [SerializeField] GameObject inventoryMenu;
    [Space]
    public GameObject saveButton;

    [Header("UI Postproces Effect")]
    [Space]
    [SerializeField] GameObject pp;

    public static PauseMenu instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PasueMenu.activeSelf)
            {
                Resume();
            }
            else if (!inventoryMenu.activeSelf)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PasueMenu.SetActive(false);
        saveButton.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.canMove = true;
        pp.SetActive(false);
        UpdateOtherUI(true);
    }

    public void Pause()
    {
        PasueMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        PlayerController.canMove = false;
        pp.SetActive(true);
        UpdateOtherUI(false);
    }

    public void Quit(string scene)
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(scene);
    }

    public void SaveQuit(string scene)
    {
        Inventory.instance.GetItemAmount();
        PlayerController.instance.CurrentPos();
        SceneManager.LoadScene(scene);
    }

    void UpdateOtherUI(bool b)
    {
        foreach (GameObject g in otherUI)
        {
            g.SetActive(b);
        }
    }
}
