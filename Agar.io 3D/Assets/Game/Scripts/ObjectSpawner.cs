using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Map settings")]
    [Min(0), SerializeField] private int objectsInMap;
    public int ObjectsInMap
    {
        get => objectsInMap;
        private set
        {
            objectsInMap = value;
            if(objectsInMap > maxObjectsInMap)
            {
                objectsInMap = maxObjectsInMap;
            }
        }
    }
    [Min(0), SerializeField] private int maxObjectsInMap;
    public int MaxObjectsInMap { get => maxObjectsInMap; private set { } }

    [Space(15), SerializeField] private GameObject[] spawnObjects;
    [Tooltip("The chance of spawning an object from the SpawnObjects array. Specify values for each element")]
    [SerializeField] private float[] spawnWeights;
    [Space(15), SerializeField] private Transform firstBorder;
    [SerializeField] private Transform secondBorder;

    [Header("Other")]
    [Tooltip("All the objects that appear will be inside the map")]
    [SerializeField] private Transform map;

    #region Unity Methods: Awake, Start, etc...
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
        GameObject obj;
        while (ObjectsInMap < MaxObjectsInMap)
        {
            randomX = Mathf.Lerp(firstBorder.position.x, secondBorder.position.x, Random.value);
            randomZ = Mathf.Lerp(firstBorder.position.z, secondBorder.position.z, Random.value);

            randomIndex = GetWeightedRandomIndex();
            obj = Instantiate(spawnObjects[randomIndex], new Vector3(randomX, 0, randomZ), Quaternion.identity);

            obj.transform.SetParent(map);
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
}
