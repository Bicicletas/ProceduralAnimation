using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    MeshCollider[] terrainChunks;

    [SerializeField] GameObject waterChunk;

    private void Update()
    {
        terrainChunks = FindObjectsOfType<MeshCollider>();

        foreach (MeshCollider m in terrainChunks)
        {
            MeshFilter mf = m.gameObject.GetComponent<MeshFilter>();

            m.sharedMesh = mf.sharedMesh;

            if (m.transform.childCount == 0 && m.gameObject.activeSelf && m.gameObject.name == "Terrain Chunk")
            {

                GameObject instance = Instantiate(waterChunk, new Vector3(m.transform.position.x, waterChunk.transform.position.y, m.transform.position.z), waterChunk.transform.rotation);

                instance.transform.parent = m.transform;
            }
        }
    }
}
