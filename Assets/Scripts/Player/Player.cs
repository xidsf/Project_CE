using UnityEngine;
using UnityEngine.InputSystem;

//필요한 내장 컴포넌트들
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
// 필요한 컴포넌트들
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
        PlayerInputHandler.OnPause += Pause;
    }

    protected virtual void OnDisable()
    {
        SubscribeEvent(false);
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

    //단순 일시정지 함수. Player말고 GameManager에서 관리하는게 더 좋을 것 같음
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
