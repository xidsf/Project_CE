using UnityEngine;

[CreateAssetMenu(menuName = "SkillEffects/MeleeNormalAttack")]
public class MeleeNormalAttack : SkillEffectSO
{
    public override void Activate(StatusContext context)
    {
        Transform playerTransform = context.Attacker.transform;
        Vector2 attackDirection = context.Attacker.PlayerMovement.GetLookDirection();
        float attackDistance = context.Attacker.PlayerStat.AttackRange.GetFinalValue();

        Vector3 spawnParticlePosition = playerTransform.position + new Vector3(attackDistance * attackDirection.x * 0.5f, 0, 0);
        SpawnParticle(spawnParticlePosition, attackDistance);

        RaycastHit2D[] hit = Physics2D.RaycastAll(playerTransform.position, attackDirection, attackDistance, LayerMask.GetMask(damageableString));

        foreach (RaycastHit2D hitInfo in hit)
        {
            if (hitInfo.collider != null)
            {
                if (DamagableCollisionCache.TryGet(hitInfo.collider, out IDamageable damagable))
                {
                    damagable.TakeDamage(context.Attacker.PlayerStat.AttackDamage.GetFinalValue());
                }
            }
        }
    }

    private void SpawnParticle(Vector3 spawnPosition, float attackDistance)
    {
        GameObject instantiatedParticle = Instantiate(attackParticle, spawnPosition, Quaternion.identity);
        Destroy(instantiatedParticle, 0.1f);
    }

}
