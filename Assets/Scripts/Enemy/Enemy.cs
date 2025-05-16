using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    EnemyStat enemyStat;
    Rigidbody2D rigid;
    Animator anim;

    bool isAttack = false;
    string attackString = "Attack";

    private void Awake()
    {
        enemyStat = new EnemyStat(20f, 1f);
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        StartCoroutine(WalkCoroutine());
    }

    private void OnEnable()
    {
        enemyStat.onDeathEvent += GiveExpGold;
        enemyStat.onDeathEvent += OnDeath;
        DamagableCollisionCache.Register(this);
    }

    private void OnDisable()
    {
        enemyStat.onDeathEvent -= GiveExpGold;
        enemyStat.onDeathEvent -= OnDeath;
        DamagableCollisionCache.UnRegister(this);
    }

    private void ApplyVelocity(float speed)
    {
        rigid.linearVelocityX = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttack = true;
            anim.SetTrigger(attackString);
        }
    }

    IEnumerator WalkCoroutine()
    {
        while(true)
        {
            if (isAttack)
            {
                ApplyVelocity(0);
                yield return new WaitForSeconds(1.5f);
                isAttack = false;
            }
            else
            {
                ApplyVelocity(-enemyStat.GetCurrentMoveSpeed());
                yield return null;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        enemyStat.DamageHP(damage);
        Debug.Log("damaged: " + damage);
    }

    public void GiveExpGold()
    {
        // 경험치와 골드 지급
        //
    }

    private void OnDeath()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        StopAllCoroutines();
        rigid.linearVelocityX = 0;
        anim.SetTrigger("Death");
        Destroy(gameObject, 1f);
    }
}
