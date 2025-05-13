using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStat enemyStat;
    Rigidbody2D rigid;
    Animator anim;

    string attackString = "Attack";

    private void Awake()
    {
        enemyStat = new EnemyStat(20f, 1f);
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ApplyVelocity(-enemyStat.GetCurrentMoveSpeed());
    }

    private void ApplyVelocity(float speed)
    {
        rigid.linearVelocityX = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(TestCoroutine());
            anim.SetTrigger(attackString);
        }
    }

    IEnumerator TestCoroutine()
    {
        ApplyVelocity(0);
        yield return new WaitForSeconds(2f);
        ApplyVelocity(-enemyStat.GetCurrentMoveSpeed());
    }
}
