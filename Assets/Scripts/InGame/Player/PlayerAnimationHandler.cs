using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private SPUM_Prefabs m_Prefabs;
    private Dictionary<PlayerState, int> m_IndexPair = new();
    private PlayerMovement m_PlayerMovement;

    public PlayerState PlayerState { get; set; } = PlayerState.IDLE;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();

        if (m_Prefabs == null)
        {
            m_Prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
            if (!m_Prefabs.allListsHaveItemsExist())
            {
                m_Prefabs.PopulateAnimationLists();
            }
        }
        m_Prefabs.OverrideControllerInit();
        foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
        {
            m_IndexPair[state] = 0;
        }
    }

    private void LateUpdate()
    {
        PlayStateAnimation();
    }

    public void PlayStateAnimation()
    {
        m_Prefabs.PlayAnimation(PlayerState, m_IndexPair[PlayerState]);
        transform.localScale = m_PlayerMovement.LookDirection == PlayerLookDirection.Right ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

}
