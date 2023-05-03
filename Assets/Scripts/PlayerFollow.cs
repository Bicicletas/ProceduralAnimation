using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] GameObject player;

    [Header("Position \n")]
    [SerializeField] private bool Xpos;
    [SerializeField] private bool Ypos;
    [SerializeField] private bool Zpos;

    [Header("Rotation \n")]
    [SerializeField] private bool Xrot;
    [SerializeField] private bool Yrot;
    [SerializeField] private bool Zrot;

    private void Update()
    {
        transform.position = new Vector3(Xpos ? player.transform.position.x : transform.position.x, 
            Ypos ? player.transform.position.y : transform.position.y, 
            Zpos ? player.transform.position.z : transform.position.z);

        transform.rotation = Quaternion.Euler(new Vector3(Xrot ? player.transform.eulerAngles.x : transform.eulerAngles.x,
            Yrot ? player.transform.eulerAngles.y : transform.eulerAngles.y,
            Zrot ? player.transform.eulerAngles.z : transform.eulerAngles.z));
    }
}
