using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject g;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (g.activeSelf)
            {
                g.SetActive(false);
            }
            else
            {
                g.SetActive(true);
            }
        }
    }
}
