using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Player : MonoBehaviour
{
    private int lookDir = 1; // 1: right, -1: left Move�Լ������� float���� ������ ���� ����

    protected float moveSpeed = 5f;
    protected float attackRange;
    protected float attackDamage;
    readonly float baseAttackSpeed = 40f;
    private float attackSpeed;
    public float AttackSpeed
    {
        get => attackSpeed;
        protected set
        {
            attackSpeed = Mathf.Max(0.1f, value);

            attackCooldown = CalculateAttackCooldown();
            float prevAttackSpeedPercent = currentAttackCooldown / attackCooldown;
            currentAttackCooldown = attackCooldown * prevAttackSpeedPercent;
        }
    }
    private float attackCooldown;
    private float currentAttackCooldown = 0;

    private int exLifeSkill = 2;
    private float exLifeSkillCooldown = 5f;
    private float currentExLifeSkillCooldown = 0f;


    private PlayerInputHandler inputHandler;
    private Rigidbody2D playerRigid;
    private Animator anim;

    private string moveFrontString = "isMoveFront";
    private string moveBackString = "isMoveBack";

    protected virtual void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        playerRigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void OnEnable()
    {
        inputHandler.OnMove += Move;
        inputHandler.OnMoveCanceled += CancelMove;
        inputHandler.OnLook += Look;
        inputHandler.OnExLifeUsed += TryUseExLifeSkill;
        inputHandler.OnPause += Pause;
    }

    protected virtual void OnDisable()
    {
        inputHandler.OnMove -= Move;
        inputHandler.OnMoveCanceled -= CancelMove;
        inputHandler.OnLook -= Look;
        inputHandler.OnExLifeUsed -= TryUseExLifeSkill;
        inputHandler.OnPause -= Pause;
    }

    protected virtual void Start()
    {
        StartCoroutine(AttackCoroutine());
    }

    protected void Move(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        int dir;

        if (inputValue.x > 0) dir = 1;
        else dir = -1;

        playerRigid.linearVelocityX = dir * moveSpeed;

        ChangeAnim();
    }

    protected void CancelMove(InputAction.CallbackContext ctx)
    {
        playerRigid.linearVelocityX = 0;
        anim.SetBool(moveFrontString, false);
        anim.SetBool(moveBackString, false);
    }

    protected void Look(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        lookDir = inputValue.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(lookDir, 1, 1);
        ChangeAnim();
    }

    protected void ChangeAnim()
    {
        if(playerRigid.linearVelocityX == 0)
        {
            anim.SetBool(moveFrontString, false);
            anim.SetBool(moveBackString, false);
        }
        else if (lookDir * playerRigid.linearVelocityX > 0)
        {
            anim.SetBool(moveFrontString, true);
            anim.SetBool(moveBackString, false);
        }
        else
        {
            anim.SetBool(moveFrontString, false);
            anim.SetBool(moveBackString, true);
        }
    }

    protected float CalculateAttackCooldown()
    {
        return baseAttackSpeed / AttackSpeed;
    }

    protected IEnumerator AttackCoroutine()
    {
        while(true)
        {
            if(currentAttackCooldown <= 0)
            {
                Attack();
                currentAttackCooldown = attackCooldown;
            }
            currentAttackCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    protected abstract void Attack();

    protected void TryUseExLifeSkill(InputAction.CallbackContext ctx)
    {
        if(exLifeSkill > 0 && currentExLifeSkillCooldown <= 0)
        {
            exLifeSkill -= 1;
            UseExLifeSkill();
            StartCoroutine(ExLifeSkillCooldownCoroutine());
        }
    }

    protected abstract void UseExLifeSkill();

    IEnumerator ExLifeSkillCooldownCoroutine()
    {
        currentExLifeSkillCooldown = exLifeSkillCooldown;
        while (currentExLifeSkillCooldown > 0)
        {
            currentExLifeSkillCooldown -= Time.deltaTime;
            yield return null;
        }
        currentExLifeSkillCooldown = 0f;
    }

    //�ܼ� �Ͻ����� �Լ�. Player���� UI�� �Űܾ���
    private void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Time.timeScale = 0f;
        }
        else if (ctx.canceled)
        {
            Time.timeScale = 1f;
        }
    }
}
