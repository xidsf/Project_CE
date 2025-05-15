using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Player : MonoBehaviour
{
    private int lookDir = 1; // 1: right, -1: left Move함수에서의 float연산 방지를 위한 변수

    public Stat MoveSpeed { get; protected set; }
    public Stat AttackRange { get; protected set; }
    public Stat AttackDamage { get; protected set; }
    public Stat AttackSpeed { get; protected set; }
    public Stat CritChance { get; protected set; }
    public Stat CritDamage { get; protected set; }

    readonly float baseAttackSpeed = 10f;
    private float attackCooldown;
    private float currentAttackCooldown = 0;

    private int exLifeSkill = 2;
    public int ExLifeSkill
    {
        get { return exLifeSkill; }
    }
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

        AttackSpeed.OnStatChanged += CalculateAttackCooldown;
        CalculateAttackCooldown();
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

        playerRigid.linearVelocityX = dir * MoveSpeed.GetFinalValue();

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

    protected void CalculateAttackCooldown()
    {
        attackCooldown = baseAttackSpeed / AttackSpeed.GetFinalValue();
        if(currentAttackCooldown > 0)
        {
            AdjustCurrentAttackCooldown();
        }
    }

    protected void AdjustCurrentAttackCooldown()
    {
        float remainRatio = currentAttackCooldown / attackCooldown;
        currentAttackCooldown = attackCooldown * remainRatio;
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

    public void AddExLifeSkill()
    {
        exLifeSkill++;
    }

    //단순 일시정지 함수. Player말고 UI로 옮겨야함
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
