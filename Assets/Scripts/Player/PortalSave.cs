using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSave : MonoBehaviour
{
    private GameObject saveButton;
    private PauseMenu pauseMenu;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();

        saveButton = pauseMenu.saveButton;
    }

    private void OnTriggerEnter(Collider other)
    {
        pauseMenu.Pause();
        saveButton.SetActive(true);
    }
}
