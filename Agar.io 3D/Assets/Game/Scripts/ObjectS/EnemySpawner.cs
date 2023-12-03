using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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
    [Space(15)]public GameObject[] EnemyPrefab;

    [Space(15), SerializeField] private Transform firstBorder;
    [SerializeField] private Transform secondBorder;

    #region Unity Methods: Awake, Start, etc...
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
                obj.transform.SetParent(transform);
            }
            EnemiesInMap++;
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
