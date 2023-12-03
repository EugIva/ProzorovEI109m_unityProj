using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Min(0), SerializeField] private float speed;
    [Min(0), SerializeField] private float FOV;
    private Rigidbody rb;
    public bool busy;
    private void Awake() => rb = GetComponent<Rigidbody>();
    private void Start() => RandomMove();
    private void FixedUpdate() => BotAI();
    private void BotAI()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, FOV);

        foreach (Collider collider in colliders)
        {
            CharacterStats enemyComponent = collider.GetComponent<CharacterStats>();

            if (enemyComponent != null)
            {
                if (enemyComponent.Mass >= rb.mass)
                {
                    print("RunAway");
                    RunAway();
                }
                else if(enemyComponent.Mass < rb.mass)
                {
                    print("MoveToEnemy");
                    MoveToEnemy();
                }
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
            float X = Random.Range(-1f, 1f);
            float Y = Random.Range(-1f, 1f);

            Vector3 moveInput = new(X, 0, Y);

            Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveInput;

            rb.AddForce(speed * moveDirection);
            yield return new WaitForSeconds(2f);
            rb.velocity = Vector2.zero;
        }
    }
    private void RunAway()
    {

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
