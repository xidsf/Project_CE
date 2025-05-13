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
            //���� ������ �Ͼ ��
        }
    }


    public float GetCurrentHealth()
    {
        // ���� ������ ����� ���⼭ ����
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        // ���� ������ ����� ���⼭ ����
        return baseMaxHealth;
    }

    public float getCurrentHealthPercent()
    {
        return currentHealth / baseMaxHealth;
    }

    public float GetCurrentMoveSpeed()
    {
        // ���� ������ ����� ���⼭ ����
        return baseMoveSpeed;
    }

}
