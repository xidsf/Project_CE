using SuperMaxim.Messaging;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


//필요한 내장 컴포넌트들
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public abstract class Player : MonoBehaviour
{
    public Rigidbody2D PlayerRigid { get; protected set; }

    public PlayerStat PlayerStat { get; protected set; }
    public PlayerState PlayerActionState { get; protected set; } = PlayerState.IDLE;

    public event Action<PlayerContext> OnPlayerAttack;
    public event Action<PlayerContext> OnPlayerCriticalAttack;

    protected PlayerMovement playerMovement;
    protected PlayerAnimationHandler playerAnimationHandler;
    protected PlayerInput playerInput;

    private Coroutine reEnableCo;
    private Camera mainCam;

    private float accumulatedAttackTime = 0f;

    #region INPUT_EVENTS

    private InputAction m_MoveAction;
    private InputAction m_ItemASkillAction;
    private InputAction m_ItemBSkillAction;
    private InputAction m_PauseAction;
    private InputAction m_ExLifeAction;

    public event Action<InputAction.CallbackContext> OnMove;
    public event Action<InputAction.CallbackContext> OnMoveCanceled;
    public event Action<InputAction.CallbackContext> OnItemASkillUsed;
    public event Action<InputAction.CallbackContext> OnItemBSkillUsed;
    public event Action<InputAction.CallbackContext> OnPause;
    public event Action<InputAction.CallbackContext> OnExLifeUsed;

    #endregion

    protected virtual void Awake()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerInput = GetComponent<PlayerInput>();

        m_MoveAction = playerInput.actions["Move"];
        m_ItemASkillAction = playerInput.actions["SkillA"];
        m_ItemBSkillAction = playerInput.actions["SkillB"];
        m_PauseAction = playerInput.actions["Pause"];
        m_ExLifeAction = playerInput.actions["ExLife"];

        if (m_MoveAction == null || m_ItemASkillAction == null || m_ItemBSkillAction == null || m_PauseAction == null || m_ExLifeAction == null)
        {
            Logger.LogError("InputAction is Null");
            return;
        }

        m_MoveAction.performed += ctx => OnMove?.Invoke(ctx);
        m_MoveAction.canceled += ctx => OnMoveCanceled?.Invoke(ctx);
        m_ItemASkillAction.performed += ctx => OnItemASkillUsed?.Invoke(ctx);
        m_ItemBSkillAction.performed += ctx => OnItemBSkillUsed?.Invoke(ctx);
        m_PauseAction.performed += ctx => OnPause?.Invoke(ctx);
        m_ExLifeAction.performed += ctx => OnExLifeUsed?.Invoke(ctx);

        mainCam = Camera.main;
    }

    protected virtual void Update()
    {
        if(!InGameManager.Instance.isPaused)
        {
            TryAttack();
        }
    }


    protected virtual void TryAttack()
    {
        var playerAttackSpeed = PlayerStat.AttackSpeed.GetFinalValue();
        var attackInterval = PlayerStat.ATTACK_SPEED_MULTIPLIER / playerAttackSpeed;

        accumulatedAttackTime += Time.deltaTime;
        if (accumulatedAttackTime >= attackInterval)
        {
            float calcCrit = UnityEngine.Random.Range(0, 1000) / 10;
            float damage;
            var ctx = GetCurrentPlayerContext(PlayerContextType.None);

            if (calcCrit < PlayerStat.CritChance.GetFinalValue())
            {
                damage = PlayerStat.AttackDamage.GetFinalValue() * PlayerStat.CritDamage.GetFinalValue();
                ctx.ContextType = PlayerContextType.CriticalNormalAttack;
                CritAttack(damage);
            }
            else
            {
                damage = PlayerStat.AttackDamage.GetFinalValue();
                ctx.ContextType = PlayerContextType.NormalAttack;
                Attack(damage);
            }
            SendAttackEvent(ctx);
            accumulatedAttackTime = 0f;
        }
    }

    public virtual PlayerContext GetCurrentPlayerContext(PlayerContextType emitType)
    {
        Vector2 screen = Mouse.current.position.ReadValue();

        var screenPos = new Vector3(screen.x, screen.y, -mainCam.transform.position.z);
        var mouseWorldPos = mainCam.ScreenToWorldPoint(screenPos);
        mouseWorldPos.z = 0f;

        var playerTransform = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 dir = (mouseWorldPos - playerTransform).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotate = Quaternion.Euler(0, 0, angle);

        return new PlayerContext(this, mouseWorldPos, rotate, emitType);
    }

    protected abstract void Attack(float damage);

    protected virtual void CritAttack(float damage)
    {
        Attack(damage);
    }

    protected virtual void SendAttackEvent(PlayerContext context)
    {
        OnPlayerAttack?.Invoke(context);
        if (context.ContextType == PlayerContextType.CriticalNormalAttack)
        {
            OnPlayerCriticalAttack?.Invoke(context);
        }
    }

    protected virtual void HandleMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        if (playerMovement != null)
        {
            playerMovement.ChangeMoveDirection(inputValue);
        }
        if(playerAnimationHandler != null)
        {
            playerAnimationHandler.PlayerState = PlayerState.MOVE;
        }
    }

    protected virtual void CancelMove(InputAction.CallbackContext ctx)
    {
        if (playerMovement != null)
        {
            playerMovement.CancelMove();
        }
        if (playerAnimationHandler != null)
        {
            playerAnimationHandler.PlayerState = PlayerState.IDLE;
        }
    }

    //단순 일시정지 함수. Player말고 GameManager에서 관리하는게 더 좋음
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

    private void OnEnable()
    {
        OnMove += HandleMove;
        OnMoveCanceled += CancelMove;
    }

    private void OnDisable()
    {
        OnMove -= HandleMove;
        OnMoveCanceled -= CancelMove;
    }

    /// <summary>
    /// 일정 시간 입력 비활성 (타임스케일 무시)
    /// </summary>
    public void DisableUserInputInTime(float seconds)
    {
        if (reEnableCo != null) StopCoroutine(reEnableCo);
        SetEnabled(false);
        reEnableCo = StartCoroutine(ReEnableAfterRealtime(seconds));
    }

    public void SetEnabled(bool enabled)
    {
        if (playerInput != null)
            playerInput.enabled = enabled;
    }

    private IEnumerator ReEnableAfterRealtime(float seconds)
    {
        var end = Time.realtimeSinceStartup + Mathf.Max(0f, seconds);
        while (Time.realtimeSinceStartup < end)
            yield return null;

        SetEnabled(true);
        reEnableCo = null;
    }

    // 액션맵 전환(쓸지는 몰...루?)
    public void SwitchActionMap(string mapName)
    {
        if (string.IsNullOrEmpty(mapName) || playerInput == null) return;
        playerInput.SwitchCurrentActionMap(mapName);
    }

    [SerializeField] private Color _attackRangeColor = new Color(0.2f, 0.6f, 1f, 0.8f);

    private void OnDrawGizmos()
    {
        if (PlayerStat == null || PlayerStat.AttackRange == null) return;

        float radius = Mathf.Max(0f, PlayerStat.AttackRange.GetFinalValue());
        Vector3 center = transform.position;

#if UNITY_EDITOR
        // 씬뷰에 얇은 원(디스크)을 그립니다. (기본: XY 평면)
        Handles.color = _attackRangeColor;
        Handles.DrawWireDisc(center, Vector3.forward, radius);   // 3D라면 Vector3.up
        // 중심점 표시(선택)
        Handles.DrawSolidDisc(center, Vector3.forward, Mathf.Max(0.02f, radius * 0.02f));
#else
        // 에디터 외 실행 환경에서는 와이어 스피어로 대체 표시
        Gizmos.color = _attackRangeColor;
        Gizmos.DrawWireSphere(center, radius);
#endif
    }
}
