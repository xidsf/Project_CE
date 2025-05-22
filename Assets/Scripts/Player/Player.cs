using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//�ʿ��� ���� ������Ʈ��
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
// �ʿ��� ������Ʈ��
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationHandler))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private ExLifeSkillSO exLifeSkill;
    public ExLifeSkillSO ExLifeSkillSO
    {
        get => exLifeSkill;
    }

    public PlayerStat PlayerStat { get; protected set; }
    public NormalAttack NormalAttack { get; protected set; }

    public PlayerInput PlayerInput { get; protected set; }
    public Rigidbody2D PlayerRigid { get; protected set; }
    public Animator Animator { get; protected set; }

    public PlayerInputHandler PlayerInputHandler { get; protected set; }
    public PlayerMovement PlayerMovement { get; protected set; }
    public PlayerAnimationHandler PlayerAnimationHandler { get; protected set; }
    public ExLifeSkillHandler ExLifeSkillHandler { get; protected set; }

    public event Action<float> onExLifeSkillUsed;

    protected virtual void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        PlayerRigid = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();

        PlayerInputHandler = GetComponent<PlayerInputHandler>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        ExLifeSkillHandler = GetComponent<ExLifeSkillHandler>();
        NormalAttack = GetComponent<NormalAttack>();

        AssembleComponent();
    }

    protected virtual void OnEnable()
    {
        SubscribeEvent(true);
        PlayerInputHandler.OnExLifeUsed += TryUseExLifeSkill;
        PlayerInputHandler.OnPause += Pause;
    }

    protected virtual void OnDisable()
    {
        SubscribeEvent(false);
        PlayerInputHandler.OnExLifeUsed -= TryUseExLifeSkill;
        PlayerInputHandler.OnPause -= Pause;
    }

    private void AssembleComponent()
    {
        foreach (var initializableComponent in GetComponents<IInitializable>())
        {
            initializableComponent.Initialize(this);
        }
    }

    private void SubscribeEvent(bool link)
    {
        foreach (var EventSubscriberComponent in GetComponents<IEventSubscriber>())
        {
            if(link)
                EventSubscriberComponent.SubscribeEvent();
            else
                EventSubscriberComponent.UnsubscribeEvent();
        }
    }

    private void TryUseExLifeSkill(InputAction.CallbackContext ctx)
    {
        if(ExLifeSkillHandler.TryUseExLiseSkill())
        {
            float exLifeSkillTime = exLifeSkill.sumOfCastingTime;
            exLifeSkillTime += PlayerAnimationHandler.ExLifeSkillLength + PlayerAnimationHandler.ParryingAnimLength;
            onExLifeSkillUsed?.Invoke(exLifeSkillTime);
        }
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
