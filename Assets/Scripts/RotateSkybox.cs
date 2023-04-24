using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] Material skybox;
    [SerializeField] float speed = 1;
    float rotationSpeed;

    private void Update()
    {
        rotationSpeed += Time.deltaTime * speed;

        if(rotationSpeed >= 360)
        {
            rotationSpeed = 0;
        }
        skybox.SetFloat("_Rotation", Mathf.Clamp(rotationSpeed, 0, 360));
    }
}
