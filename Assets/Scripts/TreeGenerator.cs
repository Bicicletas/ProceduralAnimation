using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    [SerializeField] float spawnDelay = 0.8f;

    [System.Serializable]
    public class Settings
    {
        public string name;
        public float sizeOffset;
        public int numOfTrees;
        public float rayOriginY = 60;
        public LayerMask whatIsGround;
        public float minTreeSpawnThreshold;
        public float maxTreeSpawnThreshold;
        public float treeSpawnOffsetY = 0.1f;
        public GameObject tree;
        [HideInInspector] public List<Vector3> positions = new List<Vector3>();

        public Vector2 RandomPos(Vector3 parentPos)
        {
            float x = Random.Range(parentPos.x - sizeOffset, parentPos.x + sizeOffset);
            float z = Random.Range(parentPos.z - sizeOffset, parentPos.z + sizeOffset);

            return new Vector2(x, z);
        }

        public float RandomRotation()
        {
            float angle = Random.Range(0, 360);

            return angle;
        }
    }

    [SerializeField] Settings[] settings;

    private void Start()
    {
        Invoke(nameof(GenerateTrees), spawnDelay);
    }

    void GenerateTrees()
    {
        /*
        for (int i = 0; i < numOfTrees; i++)
        {

            foreach(Vector3 pos in positions)
            {
                if(Mathf.Round(positions[i].magnitude) + spawnSeparationThreshold == Mathf.Round(pos.magnitude) + spawnSeparationThreshold)
                {
                    positions.Remove(positions[i]);

                    break;
                }
            }
        }
        */
        foreach(Settings s in settings)
        {
            for (int i = 0; i < s.numOfTrees; i++)
            {
                s.positions.Add(new Vector3(s.RandomPos(transform.parent.position).x, s.rayOriginY, s.RandomPos(transform.parent.position).y));

                RaycastHit hit;

                if (Physics.Raycast(s.positions[i], Vector3.down, out hit, 100f, s.whatIsGround))
                {
                    if (hit.point.y > s.minTreeSpawnThreshold && hit.point.y < s.maxTreeSpawnThreshold)
                    {
                        GameObject tree = s.tree;

                        GameObject instance = Instantiate(tree, new Vector3(hit.point.x, hit.point.y - s.treeSpawnOffsetY, hit.point.z), Quaternion.Euler(tree.transform.eulerAngles.x, tree.transform.eulerAngles.y + s.RandomRotation(), tree.transform.eulerAngles.z));

                        instance.transform.parent = hit.transform;
                    }
                }
            }
        }
    }

    
}
