using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Need add new enemies + their skins
    [Header("Map settings")]
    [Min(0), SerializeField] private int enemiesInMap;
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
    [Space(15)]public GameObject[] EnemyPrefab;

    [Space(15), SerializeField] private Transform firstBorder;
    [SerializeField] private Transform secondBorder;

    [Header("Other")]
    [SerializeField] private Transform enemiesObj;

    #region Unity Methods: Awake, Start, etc...
    private void Awake() => DontDestroyOnLoad(gameObject);

    private void Start() => SpawnObjects();
    private void OnDestroy() => StopAllCoroutines();
    #endregion

    private void SpawnObjects()
    {
        float randomX;
        float randomZ;
        GameObject obj;
        while(EnemiesInMap < MaxEnemiesInMap)
        {
            randomX = Mathf.Lerp(firstBorder.position.x, secondBorder.position.x, Random.value);
            randomZ = Mathf.Lerp(firstBorder.position.z, secondBorder.position.z, Random.value);
            for (int i = 0; i < EnemyPrefab.Length; i++)
            {
                obj = Instantiate(EnemyPrefab[i], new Vector3(randomX, firstBorder.position.y, randomZ), Quaternion.identity);
                obj.transform.SetParent(enemiesObj);
            }
            EnemiesInMap++;
        }
    }
}
