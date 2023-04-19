using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] float spawnDelay = 0.8f;
    [SerializeField] float sizeOffset = 50;
    [SerializeField] float rayOriginY = 80;


    [System.Serializable]
    public class Settings
    {
        public string objectName;
        public int numOfObject;
        public LayerMask whatIsGround;
        public float minObjectSpawnThreshold;
        public float maxObjectSpawnThreshold;
        public float objectSpawnOffsetY = 0.1f;
        public float minScale = 1;
        public float maxScale = 2;
        public GameObject[] objects;
        [HideInInspector] public List<Vector3> positions = new List<Vector3>();

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

    [SerializeField] Settings[] settings;

    private void Start()
    {
        Invoke(nameof(GenerateObjects), spawnDelay);
    }

    Vector2 RandomPos(Vector3 parentPos)
    {
        float x = Random.Range(parentPos.x - sizeOffset, parentPos.x + sizeOffset);
        float z = Random.Range(parentPos.z - sizeOffset, parentPos.z + sizeOffset);

        return new Vector2(x, z);
    }

    void GenerateObjects()
    {
        foreach(Settings s in settings)
        {
            for (int i = 0; i < s.numOfObject; i++)
            {
                s.positions.Add(new Vector3(RandomPos(transform.parent.position).x, rayOriginY, RandomPos(transform.parent.position).y));

                RaycastHit hit;

                if (Physics.Raycast(s.positions[i], Vector3.down, out hit, 100f, s.whatIsGround))
                {
                    if (hit.point.y > s.minObjectSpawnThreshold && hit.point.y < s.maxObjectSpawnThreshold)
                    {
                        GameObject @object = s.objects[s.RandomIndex()];

                        GameObject instance = Instantiate(@object, new Vector3(hit.point.x, hit.point.y - s.objectSpawnOffsetY, hit.point.z), Quaternion.Euler(@object.transform.eulerAngles.x, @object.transform.eulerAngles.y + s.RandomRotation(), @object.transform.eulerAngles.z));

                        instance.transform.parent = hit.transform;

                        float scale = s.RandomScale();

                        instance.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
        }
    }

    
}
