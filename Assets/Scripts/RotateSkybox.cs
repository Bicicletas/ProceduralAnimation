using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] Material skybox;
    [SerializeField] Light sun;
    [SerializeField] float speed = 1;
    float rotationSpeed;

    private void Update()
    {
        skybox.SetColor("_Tint", sun.color);
        skybox.SetFloat("_Exposure", sun.intensity > 0.5f ? sun.intensity : 0.5f);
        RenderSettings.fogColor = sun.color;

        rotationSpeed += Time.deltaTime * speed;

        if(rotationSpeed >= 360)
        {
            rotationSpeed = 0;
        }
        skybox.SetFloat("_Rotation", Mathf.Clamp(rotationSpeed, 0, 360));
    }
}
