using System.Collections;
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
    public int ExLifeSkill { get; protected set; }
    private readonly float exLifeSkillCooldown = 5f;
    private float currentExLifeSkillCooldown = 0f;

    public PlayerStat PlayerStat { get; protected set; }
    public NormalAttack NormalAttack { get; protected set; }

    public PlayerInput PlayerInput { get; protected set; }
    public Rigidbody2D PlayerRigid { get; protected set; }
    public Animator Animator { get; protected set; }

    public PlayerInputHandler PlayerInputHandler { get; protected set; }
    public PlayerMovement PlayerMovement { get; protected set; }
    public PlayerAnimationHandler PlayerAnimationHandler { get; protected set; }

    bool isEventsSubscribed = false;

    protected virtual void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        PlayerRigid = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();

        PlayerInputHandler = GetComponent<PlayerInputHandler>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAnimationHandler = GetComponent<PlayerAnimationHandler>();
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
            if(link && !isEventsSubscribed)
            {
                EventSubscriberComponent.SubscribeEvent();
                isEventsSubscribed = true;
            }
            else if(!link && isEventsSubscribed)
            {
                EventSubscriberComponent.UnsubscribeEvent();
                isEventsSubscribed = false;
            }
        }
    }

    protected void TryUseExLifeSkill(InputAction.CallbackContext ctx)
    {
        if(ExLifeSkill > 0 && currentExLifeSkillCooldown <= 0)
        {
            ExLifeSkill -= 1;
            //UseExLifeSkill();
            StartCoroutine(ExLifeSkillCooldownCoroutine());
        }
    }

    //protected abstract void UseExLifeSkill();

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
        ExLifeSkill++;
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
