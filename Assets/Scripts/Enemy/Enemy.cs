using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
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
        enemyStat.onDeathEvent += GiveExpGold;
    }

    private void Update()
    {
        StartCoroutine(WalkCoroutine());
    }

    public void GiveExpGold()
    {
        // 경험치와 골드 지급
        //
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
}
