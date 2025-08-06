using UnityEngine;

[CreateAssetMenu(menuName = "SkillEffects/MeleeNormalAttack")]
public class MeleeNormalAttack : SkillEffectSO
{
    private float particleSize = 1;

    public override void Activate(Player player, StatusContext context)
    {
        Transform playerTransform = context.Attacker.transform;
        Vector2 attackDirection = context.Attacker.PlayerMovement.GetLookDirection();
        float attackDistance = context.Attacker.PlayerStat.AttackRange.GetFinalValue();

        Vector3 spawnParticlePosition = playerTransform.position + new Vector3(attackDistance * attackDirection.x * 0.5f, 0, 0);
        SpawnAttackParticle(spawnParticlePosition, attackDistance);

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

    private void SpawnAttackParticle(Vector3 spawnPosition, float attackDistance)
    {
        if(attackParticle != null)
        {
            if(particleSize * 2 != attackDistance)
            {
                ParticleSystem.MainModule main = attackParticle.GetComponent<ParticleSystem>().main;
                main.startSize = attackDistance * 0.5f;
            }

            GameObject instantiatedParticle = Instantiate(attackParticle, spawnPosition, Quaternion.identity);
            Destroy(instantiatedParticle, 0.1f);
        }
    }
}
