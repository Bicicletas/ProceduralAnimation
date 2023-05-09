using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    public GameObject waterChunk;

    private void Start()
    {
        Invoke(nameof(MeshCollider), 1.1f);

        GameObject instance = Instantiate(waterChunk, new Vector3(transform.position.x, waterChunk.transform.position.y, transform.position.z), waterChunk.transform.rotation);

        instance.transform.parent = transform;
    }

    void MeshCollider()
    {
        MeshFilter mf = GetComponent<MeshFilter>();

        MeshCollider mc = GetComponent<MeshCollider>();

        int i = 0;

        if (mc.sharedMesh != mf.sharedMesh && i < 1)
        {
            mc.sharedMesh = mf.sharedMesh;
            i++;
        }
    }
}
