using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Settings : ScriptableObject
{
    [Min(0)]
    public int numOfObject = 1;
    public LayerMask whatIsGround;
    [Range(0, 80)]
    public float minObjectSpawnThreshold = 10;
    [Range(0, 80)]
    public float maxObjectSpawnThreshold = 30;
    public float objectSpawnOffsetY = 0.1f;
    [Min(1)]
    public float minScale = 1;
    [Min(1)]
    public float maxScale = 2;
    public GameObject[] objects;

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
