using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Transform currentObject;

    [Min(0), SerializeField] internal float FOV;
    [SerializeField] private LayerMask layer;
    private Rigidbody rb;
    private EnemyStats botStats;

    private bool changedSpeed;
    public bool busy;
    public bool foundEnemy;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        botStats = GetComponent<EnemyStats>();
    }
    private void Start() => RandomMove();
    private void FixedUpdate()
    {
        CreatePhysicsCircle();
        if (foundEnemy)
        {
            EvaluateEnemy();
            return;
        }
        LootObjects();
    }
    private void EvaluateEnemy()
    {
        if (currentObject != null)
        {
            float currentObjectMass = GetObjectMass(currentObject);

            float distance = Vector3.Distance(transform.position, currentObject.transform.position);
            Vector3 direction = currentObject.transform.position - transform.position;
            direction.Normalize();

            //Attack object
            if (currentObjectMass <= botStats.Mass)
            {
                rb.velocity = direction * botStats.Speed;

                if (distance >= FOV)
                {
                    MissingObject();
                }
            }
            //Run away from object
            else
            {
                if (!changedSpeed)
                {
                    changedSpeed = true;
                    botStats.ChangeSpeed(0.9f);
                }
                Vector3 awayDirection = -direction.normalized;
                rb.velocity = new Vector3(awayDirection.x, 0, awayDirection.z) * botStats.Speed;

                if (distance >= FOV * 2)
                {
                    MissingObject();
                    botStats.ChangeSpeed(1, true);
                    changedSpeed = false;
                }
            }
        }
    }

    #region logic
    private void LootObjects()
    {
        if (currentObject != null)
        {
            float distance = Vector3.Distance(transform.position, currentObject.transform.position);
            //None physic movement
            //float step = speed * Time.fixedDeltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, currentObject.transform.position, step);

            //Physic movement
            Vector3 direction = currentObject.transform.position - transform.position;
            direction.Normalize();

            rb.velocity = direction * botStats.Speed;
            if (distance >= FOV)
            {
                MissingObject();
            }
        }
    }
    private void CreatePhysicsCircle()
    {
        if(currentObject == null && IsBusy() || currentObject == null && foundEnemy)
        {
            MissingObject();
            return;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, FOV, layer);
        foreach (var collider in colliders)
        {
            //Found Enemy
            if(collider.gameObject != gameObject)
            {
                //Priority number 1
                //Found Other bot
                collider.TryGetComponent(out EnemyStats bot);
                if (bot != null)
                {
                    currentObject = collider.transform;
                    busy = true;
                    foundEnemy = true;
                    return;
                }
                //Priority number 2
                //Found Character
                collider.TryGetComponent(out CharacterStats character);
                if (character != null)
                {
                    currentObject = collider.transform;
                    busy = true;
                    foundEnemy = true;
                    return;
                }
                //Priority number 3
                //Found items
                if (!foundEnemy && currentObject == null)
                {
                    currentObject = collider.transform;
                    busy = true;
                    return;
                }
            }
        }
    }
    private void RandomMove() => StartCoroutine(RandomMoveStart());
    private IEnumerator RandomMoveStart()
    {
        while (true)
        {
            if (!Timer.isPause)
            {
                yield return new WaitUntil(() => !IsBusy());
                yield return new WaitForSecondsRealtime(0.5f);
                if (IsBusy())
                {
                    continue;
                }
                rb.velocity = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * botStats.Speed;
                yield return new WaitForSecondsRealtime(3f);
            }
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
    #endregion

    #region other
    public bool IsBusy()
    {
        if (busy)
        {
            return true;
        }
        return false;
    }
    private float GetObjectMass(Transform currentObject)
    {
        if (currentObject.TryGetComponent(out CharacterStats character))
        {
            return character.Mass;
        }
        else if (currentObject.TryGetComponent(out EnemyStats enemy))
        {
            return enemy.Mass;
        }
        return 0;
    }
    private void MissingObject()
    {
        currentObject = null;
        foundEnemy = false;
        busy = false;
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOV);
    }
#endif
}
