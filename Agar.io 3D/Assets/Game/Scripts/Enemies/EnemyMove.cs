using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Transform currentObject;

    [Space(15), Min(0), SerializeField] private float speed;
    [Min(0), SerializeField] private float FOV;
    [SerializeField] private LayerMask layer;
    private Rigidbody rb;
    private EnemyStats botStats;

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
        //Move to object
        if (currentObject != null)
        {
            float currentObjectMass = GetObjectMass(currentObject);

            float distance = Vector3.Distance(transform.position, currentObject.transform.position);
            float step = speed * Time.fixedDeltaTime;

            if (currentObjectMass < botStats.Mass)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentObject.transform.position, step);

                if (distance >= FOV)
                {
                    MissingObject();
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards 
                    (transform.position, transform.position - 
                    (currentObject.transform.position - transform.position), step);

                if(distance >= FOV * 2)
                {
                    MissingObject();
                }
            }
        }
    }
    private void LootObjects()
    {
        if (currentObject != null)
        {
            float distance = Vector3.Distance(transform.position, currentObject.transform.position);
            var pos = Vector3.Lerp(transform.position, currentObject.transform.position, Time.fixedDeltaTime * speed / distance);
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }
    private void MissingObject()
    {
        currentObject = null;
        foundEnemy = false;
        busy = false;
    }
    private float GetObjectMass(Transform currentObject)
    {
        if(currentObject.TryGetComponent(out CharacterStats character))
        {
            print(character.Mass);
            return character.Mass;
        }
        else if(currentObject.TryGetComponent(out EnemyStats enemy))
        {
            print(enemy.Mass);
            return enemy.Mass;
        }
        return 0;
    }
    private void CreatePhysicsCircle()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, FOV, layer);
        foreach (var collider in colliders)
        {
            //Find Enemy
            if (collider.gameObject != gameObject)
            {
                currentObject = collider.transform;
                busy = true;
                foundEnemy = true;
                return;
            }
            //Find items
            if(!foundEnemy && currentObject == null && collider.gameObject != gameObject)
            {
                currentObject = collider.transform;
                busy = true;
                return;
            }
        }
    }
    private void CheckDistanceAndReset()
    {
        if (currentObject != null)
        {
            float distanceToCurrentObject = Vector3.Distance(transform.position, currentObject.position);

            if (distanceToCurrentObject > FOV)
            {
                currentObject = null;
                busy = false;
            }
        }
    }
    private void RandomMove() => StartCoroutine(RandomMoveStart());
    private IEnumerator RandomMoveStart()
    {
        while (true)
        {
            yield return new WaitUntil(() => !IsBusy());
            yield return new WaitForSeconds(0.5f);
            if (IsBusy())
            {
                continue;
            }
            rb.velocity = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * speed;
            yield return new WaitForSeconds(3f);
        }
    }

    private void RunAway(Transform target) => StartCoroutine(RunAwayStart(target));
    private IEnumerator RunAwayStart(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) >= FOV)
        {
            Vector3 runDirection = transform.position - target.position;
            runDirection.Normalize();

            rb.AddForce(speed * runDirection);
        }
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector3.zero;
        busy = false;
    }
    private void MoveToEnemy()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOV);
    }
#endif
    public bool IsBusy()
    {
        if (busy)
        {
            return true;
        }
        return false;
    }
}
