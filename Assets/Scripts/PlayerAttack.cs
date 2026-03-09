using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float cooldownAttack = 0.4f;

    [Header("Ray Origin")]
    [SerializeField] private Transform attackSource;
    [SerializeField] private LayerMask hitMask = ~0;

    private float nextAttackTime;

    private void Awake()
    {
        if (attackSource == null && Camera.main != null)
            attackSource = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + cooldownAttack;

        if (attackSource == null)
        {
            Debug.LogWarning("PlayerAttack: attackSource is null (assign MainCamera).");
            return;
        }

        Ray ray = new Ray(attackSource.position, attackSource.forward);
        Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.yellow, 0.5f);

        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, hitMask))
        {
            EnemyHealth health = hit.collider.GetComponentInParent<EnemyHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log($"Hit {hit.collider.name} for {damage} damage");
            }
            else
            {
                Debug.Log($"Hit {hit.collider.name} but no EnemyHealth found");
            }
        }
        else
        {
            Debug.Log("Attack missed");
        }
    }
}
