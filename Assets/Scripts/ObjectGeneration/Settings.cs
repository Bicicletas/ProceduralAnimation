using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Settings : ScriptableObject
{
    [Header("What to Spawn")]
    [Space]
    public GameObject[] objects;

    [Space(20)]
    [Header("Where to Spawn")]
    public LayerMask whatToCollide;
    [Range(0, 80)]
    public float minObjectSpawnThreshold = 10;
    [Range(0, 80)]
    public float maxObjectSpawnThreshold = 30;
    public bool worldInstance = false;
    public float objectSpawnOffsetY = 0.1f;

    [Space(20)]
    [Header("How to Spawn")]
    [Space]
    public bool childInstanceScale = true;
    [Min(0.01f)]
    public float minScale = 1;
    [Min(0.01f)]
    public float maxScale = 2;

    [Space(20)]
    [Header("When to Spawn")]
    [Space]
    [Tooltip("Applied if random scale is greater than 0")]
    public int randomScale = 10;

    public bool RandomizedSpawnRate()
    {
        int index = Random.Range(0, randomScale);

        if (index == (randomScale - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public float RandomRotation()
    {
        float angle = Random.Range(0, 360);

        return angle;
    }

    public float RandomScale()
    {
        float index = Random.Range(minScale, maxScale);

        return index;
    }

    public int RandomIndex()
    {
        int index = Random.Range(0, objects.Length);

        return index;
    }
}
