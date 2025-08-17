using System;
using System.Collections;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public Player Player { get; private set; }

    IDisposable repeatingExample;

    protected override void Init()
    {
        m_IsDestroyOnLoad = false;
        base.Init();

        Player = FindAnyObjectByType<Player>();
        if (Player == null)
        {
            Logger.LogError("Player not found in the scene. Please ensure a Player object is present.");
        }

        repeatingExample = TimeScheduleManager.Instance.StartRepeatingTimerCondition(2f, TimeScaleType.Scaled, () =>
        {
            Logger.Log("test");
        });
        StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(10f);
        repeatingExample.Dispose();
    }
}
