using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    private EnemySpawner() { }
    //Need add new enemies + their skins
    [Header("Map settings")]
    private int enemiesInMap;
    public int EnemiesInMap
    {
        get => enemiesInMap;
        set
        {
            enemiesInMap = value;
            if(enemiesInMap < 0)
            {
                enemiesInMap = 0;
            }
            if(enemiesInMap > maxEnemiesInMap)
            {
                enemiesInMap = maxEnemiesInMap;
            }
        }
    }

    [Min(0), SerializeField] private int maxEnemiesInMap;
    public int MaxEnemiesInMap { get => maxEnemiesInMap; private set { } }

    [Range(0, 30), SerializeField] private int spawnRate;

    [Space(15)]public GameObject[] EnemyPrefab;

    [Space(15), SerializeField] private Transform firstBorder;
    [SerializeField] private Transform secondBorder;

    [Header("Terrain Settings")]
    [SerializeField] private Terrain terrain;
    [Tooltip("Recomended base value = 0.1f")]
    [Range(0, 1), SerializeField] private float offsetEnemies = 0.1f;

    #region Unity Methods: Awake, Start, etc...
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        float randomX;
        float randomZ;
        float terrainHeight;
        GameObject obj;
        while(EnemiesInMap < MaxEnemiesInMap)
        {
            randomX = Mathf.Lerp(firstBorder.position.x, secondBorder.position.x, Random.value);
            randomZ = Mathf.Lerp(firstBorder.position.z, secondBorder.position.z, Random.value);

            terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ));

            for (int i = 0; i < EnemyPrefab.Length; i++)
            {
                obj = Instantiate(EnemyPrefab[i], new Vector3(randomX, terrainHeight + offsetEnemies, randomZ), Quaternion.identity);
                obj.transform.SetParent(transform);
                EnemiesInMap++;
            }
        }
    }
    private IEnumerator PermanentSpawnObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            yield return new WaitUntil(() => EnemiesInMap < MaxEnemiesInMap);
            yield return new WaitForSeconds(spawnRate);
            SpawnObjects();
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((firstBorder.position + secondBorder.position) / 2f,
                        new Vector3(Mathf.Abs(firstBorder.position.x - secondBorder.position.x),
                                    Mathf.Abs(firstBorder.position.y - secondBorder.position.y),
                                    Mathf.Abs(firstBorder.position.z - secondBorder.position.z)));
    }
#endif
}
