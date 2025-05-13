using UnityEngine;

public class EnemyStat
{
    private float currentHealth;
    private float baseMaxHealth;
    private float baseMoveSpeed;
    public bool isSuperArmor = false;

    public EnemyStat(float health, float speed)
    {
        baseMaxHealth = health;
        currentHealth = baseMaxHealth;
        baseMoveSpeed = speed;
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //뭔가 죽으면 일어날 일
        }
    }


    public float GetCurrentHealth()
    {
        // 버프 적용이 생기면 여기서 적용
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        // 버프 적용이 생기면 여기서 적용
        return baseMaxHealth;
    }

    public float getCurrentHealthPercent()
    {
        return currentHealth / baseMaxHealth;
    }

    public float GetCurrentMoveSpeed()
    {
        // 버프 적용이 생기면 여기서 적용
        return baseMoveSpeed;
    }

}
