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
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        botStats = GetComponent<EnemyStats>();
    }
    private void Start() => RandomMove();
    private void FixedUpdate()
    {
        BotAI();
        CheckDistanceAndReset();
    }
    private void BotAI()
    {
        if(currentObject == null && IsBusy())
        {
            return;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, FOV, layer);
        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                currentObject = collider.transform;
            }
            if(currentObject != null)
            {
                if(currentObject.TryGetComponent(out CharacterStats _))
                {
                    CharacterLogic();
                    continue;
                }
                else
                {
                    OtherBotsLogic();
                    continue;
                }
            }
        }
        //Collider[] colliders = Physics.OverlapSphere(transform.position, FOV);

        //foreach (Collider collider in colliders)
        //{
        //    print("Checking collider");
        //    if (collider.gameObject != gameObject && collider.CompareTag("target"))
        //    {
        //        print("Collider is a target");
        //        currentObject = collider.transform;

        //        if (currentObject != null)
        //        {
        //            print("Current object is not null");
        //            if (currentObject.TryGetComponent(out CharacterStats _))
        //            {
        //                print("CharacterStats found");
        //                CharacterLogic();
        //            }
        //            else
        //            {
        //                print("No CharacterStats, running OtherBotsLogic");
        //                OtherBotsLogic();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        currentObject = null;
        //    }
        //}
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
    private void CharacterLogic()
    {
        busy = true;
        var charater = currentObject.GetComponent<CharacterStats>();
        if (charater.Mass >= botStats.Mass && !IsBusy())
        {
            print("RunAway");
            RunAway(currentObject);
        }
        else if (charater.Mass < botStats.Mass && !IsBusy())
        {
            print("MoveToEnemy");
            MoveToEnemy();
        }
    }
    private void OtherBotsLogic()
    {
        busy = true;
        var otherBot = currentObject.GetComponent<EnemyStats>();
        if (otherBot.Mass >= botStats.Mass && !IsBusy())
        {
            print("RunAway");
            RunAway(currentObject);
        }
        else if ( otherBot.Mass < botStats.Mass && !IsBusy())
        {
            print("MoveToEnemy");
            MoveToEnemy();
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
        busy = true;
        while (Vector3.Distance(transform.position, target.position) >= FOV)
        {
            Vector3 runDirection = transform.position - target.position;
            runDirection.Normalize();

            rb.AddForce(speed * runDirection);
        }
        yield return new WaitForSeconds(1f);
        print("TEST");
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
