using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;
    private ObjectSpawner() { }

    [Header("Map settings")]
    private int objectsInMap;
    public int ObjectsInMap
    {
        get => objectsInMap;
        set
        {
            objectsInMap = value;
            if (objectsInMap < 0)
            {
                objectsInMap = 0;
            }
            if (objectsInMap > maxObjectsInMap)
            {
                objectsInMap = maxObjectsInMap;
            }
        }
    }

    [Min(0), SerializeField] private int maxObjectsInMap;
    public int MaxObjectsInMap { get => maxObjectsInMap; private set { } }

    [Space(15), SerializeField] private GameObject[] objectsPrefab;
    [Tooltip("The chance of spawning an object from the SpawnObjects array. Specify values for each element")]
    [SerializeField] private float[] spawnWeights;
    [Space(15), SerializeField] private Transform firstBorder;
    [SerializeField] private Transform secondBorder;

    [Header("Terrain Settings")]
    [SerializeField] private Terrain terrain;
    [Tooltip("Recomended base value = 0.05f")]
    [Range(0, 1), SerializeField] private float offsetObjects = 0.05f;

    #region Unity Methods: Awake, Start, etc...
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
    private void Start()
    {
        SpawnObjects();
        StartCoroutine(PermanentSpawnObjects());
    }
    private void OnDestroy() => StopAllCoroutines();
    #endregion
    private void SpawnObjects()
    {
        int randomIndex;
        float randomX;
        float randomZ;
        float terrainHeight;
        GameObject obj;
        while (ObjectsInMap < MaxObjectsInMap)
        {
            //Old spawn logic
            //randomX = Mathf.Lerp(firstBorder.position.x, secondBorder.position.x, Random.value);
            //randomZ = Mathf.Lerp(firstBorder.position.z, secondBorder.position.z, Random.value);

            //randomIndex = GetWeightedRandomIndex();
            //obj = Instantiate(objectsPrefab[randomIndex], new Vector3(randomX, firstBorder.position.y, randomZ), Quaternion.identity);

            //obj.transform.SetParent(transform);
            //ObjectsInMap++;

            randomX = Mathf.Lerp(firstBorder.position.x, secondBorder.position.x, Random.value);
            randomZ = Mathf.Lerp(firstBorder.position.z, secondBorder.position.z, Random.value);

            terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ));

            randomIndex = GetWeightedRandomIndex();
            obj = Instantiate(objectsPrefab[randomIndex], new Vector3(randomX, terrainHeight+offsetObjects, randomZ), Quaternion.identity);

            obj.transform.SetParent(transform);
            ObjectsInMap++;
        }
    }
    private IEnumerator PermanentSpawnObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            yield return new WaitUntil(() => objectsInMap < MaxObjectsInMap / 2);
            SpawnObjects();
        }
    }
    private int GetWeightedRandomIndex()
    {
        float totalWeight = 0f;
        foreach (float weight in spawnWeights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.value * totalWeight;

        for (int i = 0; i < spawnWeights.Length; i++)
        {
            if (randomValue < spawnWeights[i])
            {
                return i;
            }

            randomValue -= spawnWeights[i];
        }

        return spawnWeights.Length - 1;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((firstBorder.position + secondBorder.position) / 2f,
                        new Vector3(Mathf.Abs(firstBorder.position.x - secondBorder.position.x),
                                    Mathf.Abs(firstBorder.position.y - secondBorder.position.y),
                                    Mathf.Abs(firstBorder.position.z - secondBorder.position.z)));
    }
#endif
}
