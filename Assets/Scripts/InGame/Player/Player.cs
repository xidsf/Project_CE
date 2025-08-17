using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

//필요한 내장 컴포넌트들
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public abstract class Player : MonoBehaviour
{
    public Rigidbody2D PlayerRigid { get; protected set; }
    public PlayerStat PlayerStat { get; protected set; }
    public PlayerState PlayerActionState { get; protected set; } = PlayerState.IDLE;

    protected PlayerMovement m_PlayerMovement;
    protected PlayerAnimationHandler m_PlayerAnimationHandler;
    protected PlayerInput m_PlayerInput;

    private Coroutine m_ReEnableCo;

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

    //event를 담기 위한 Rapper
    private Action<InputAction.CallbackContext> m_MoveActionHandler;
    private Action<InputAction.CallbackContext> m_MoveCancledActionHandler;
    private Action<InputAction.CallbackContext> m_ItemASkillActionHandler;
    private Action<InputAction.CallbackContext> m_ItemBSkillActionHandler;
    private Action<InputAction.CallbackContext> m_PauseActionHandler;
    private Action<InputAction.CallbackContext> m_ExLifeActionHandler;
    #endregion

    protected virtual void Awake()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        m_PlayerInput = GetComponent<PlayerInput>();

        m_MoveAction = m_PlayerInput.actions["Move"];
        m_ItemASkillAction = m_PlayerInput.actions["SkillA"];
        m_ItemBSkillAction = m_PlayerInput.actions["SkillB"];
        m_PauseAction = m_PlayerInput.actions["Pause"];
        m_ExLifeAction = m_PlayerInput.actions["ExLife"];

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
    }

    protected abstract void ExecuteNormalAttack();

    protected virtual void HandleMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        if (m_PlayerMovement != null)
        {
            m_PlayerMovement.ChangeMoveDirection(inputValue);
        }
        if(m_PlayerAnimationHandler != null)
        {
            m_PlayerAnimationHandler.PlayerState = PlayerState.MOVE;
        }
    }

    protected virtual void CancelMove(InputAction.CallbackContext ctx)
    {
        if (m_PlayerMovement != null)
        {
            m_PlayerMovement.CancelMove();
        }
        if (m_PlayerAnimationHandler != null)
        {
            m_PlayerAnimationHandler.PlayerState = PlayerState.IDLE;
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
        if (m_ReEnableCo != null) StopCoroutine(m_ReEnableCo);
        SetEnabled(false);
        m_ReEnableCo = StartCoroutine(ReEnableAfterRealtime(seconds));
    }

    public void SetEnabled(bool enabled)
    {
        if (m_PlayerInput != null)
            m_PlayerInput.enabled = enabled;
    }

    private IEnumerator ReEnableAfterRealtime(float seconds)
    {
        var end = Time.realtimeSinceStartup + Mathf.Max(0f, seconds);
        while (Time.realtimeSinceStartup < end)
            yield return null;

        SetEnabled(true);
        m_ReEnableCo = null;
    }

    // 액션맵 전환(쓸지는 몰...루?)
    public void SwitchActionMap(string mapName)
    {
        if (string.IsNullOrEmpty(mapName) || m_PlayerInput == null) return;
        m_PlayerInput.SwitchCurrentActionMap(mapName);
    }
}
