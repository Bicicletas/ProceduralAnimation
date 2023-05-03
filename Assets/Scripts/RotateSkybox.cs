using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] Material skybox;
    [SerializeField] Material vFog;
    [SerializeField] Light sun;
    [SerializeField] float speed = 1;

    [Range(0, 1)]
    [SerializeField] float vFogAlpha = 0.5f;
    float rotationSpeed;

    private void Update()
    {
        skybox.SetColor("_Tint", sun.color);
        skybox.SetFloat("_Exposure", sun.intensity > 0.5f ? sun.intensity : 0.5f);
        vFog.SetColor("Color_69AD1AD", new Color(sun.color.r, sun.color.g, sun.color.b, vFogAlpha));
        RenderSettings.fogColor = sun.color;

        rotationSpeed += Time.deltaTime * speed;

        if(rotationSpeed >= 360)
        {
            rotationSpeed = 0;
        }
        skybox.SetFloat("_Rotation", Mathf.Clamp(rotationSpeed, 0, 360));
    }
}
