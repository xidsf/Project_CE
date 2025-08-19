using UnityEngine;

public class Warrior : Player
{
    [SerializeField] int continuousAttackCount = 0;
    [SerializeField] bool isContinuousAttackEnabled = false;
    [SerializeField] ParticleSystem NormalAttackParticle;
    float initParticleRotation;

    protected override void Awake()
    {
        PlayerStat = new PlayerStat(3f, 2f, 5f, 5f);
        initParticleRotation = NormalAttackParticle.main.startRotation.constant;
        base.Awake();
    }

    protected override void Attack(float damage)
    {
        var ctx = GetCurrentPlayerContext(PlayerContextType.None);
        var attackVector = (ctx.MousePosition - transform.position).normalized;
        var maxAttackAnglePos = attackVector * PlayerStat.AttackRange.GetFinalValue() * 0.5f ;
        SetParticleConfig(ctx.TargetingRotation, PlayerStat.AttackRange.GetFinalValue());
        Instantiate(NormalAttackParticle, transform.position + maxAttackAnglePos, ctx.TargetingRotation);
    }

    protected override void CritAttack(float damage)
    {
        Attack(damage);
        
    }

    public override PlayerContext GetCurrentPlayerContext(PlayerContextType contextType)
    {
        var ctx = base.GetCurrentPlayerContext(contextType);

        return new WarriorContext(ctx.Caster, ctx.MousePosition, ctx.TargetingRotation, contextType, false);
    }

    protected override void SendAttackEvent(PlayerContext context)
    {
        var warrCtx = context as WarriorContext;
        if (warrCtx == null)
        {
            Logger.LogError("WarriorContext is null in SendAttackEvent.");
            return;
        }

        bool CanContinuousAttack = continuousAttackCount > 0;

        if (isContinuousAttackEnabled || CanContinuousAttack) //연속 공격일 때
        {
            if (!isContinuousAttackEnabled && CanContinuousAttack) //첫 연속공격일 때
            {
                continuousAttackCount--;
                PlayerStat.AttackSpeed.FixStat(500);
                isContinuousAttackEnabled = true;
            }
            else if(isContinuousAttackEnabled && CanContinuousAttack) //연속공격 중일 때
            {
                continuousAttackCount--;
            }
            else if (isContinuousAttackEnabled && !CanContinuousAttack) //마지막 연속공격일 때
            {
                PlayerStat.AttackSpeed.UnfixStat();
                isContinuousAttackEnabled = false;
            }
            warrCtx.IsContinuousAttack = true;
            base.SendAttackEvent(warrCtx);
            return;
            
        }
        else
        {
            warrCtx.IsContinuousAttack = false;
            warrCtx.ContextType = PlayerContextType.NormalAttack;
            base.SendAttackEvent(warrCtx);
        }
        
    }


    private void SetParticleConfig(Quaternion angle, float size)
    {
        if (NormalAttackParticle == null) return;

        var main = NormalAttackParticle.main;
        Vector3 euler = angle.eulerAngles;

        float zRotationRad = euler.z * Mathf.Deg2Rad;
        main.startRotation = initParticleRotation + zRotationRad;

        main.startSize = size;
    }

    public void IncrementContinuousAttackCount(int count)
    {
        continuousAttackCount += count;
    }
}