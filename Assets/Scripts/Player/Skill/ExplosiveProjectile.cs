using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExplosiveProjectile : MonoBehaviour
{
    float attackAreaRadius;
    float attackDamage;

    public void Initialize(float attackAreaRadius, float attackDamage)
    {
        this.attackAreaRadius = attackAreaRadius;
        this.attackDamage = attackDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, attackAreaRadius);
            foreach(var enemys in collisions)
            {
                if(DamagableCollisionCache.TryGetDamageable(enemys, out IDamageable damagable))
                {
                    damagable.TakeDamage(attackDamage);
                }
            }

            Destroy(gameObject);
        }
    }
}
