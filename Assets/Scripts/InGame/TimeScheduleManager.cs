using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public enum TimeScaleType { Scaled, Unscaled }

public class TimeScheduleManager : Singleton<TimeScheduleManager>
{
    private class Timer
    {
        public double nextTickTime;
        public double tickRate; //0이면 실행 후 cancelled됨
        public TimeScaleType timeScaleType;
        public Action onTick;
        public bool isCancelled;
    }

    private class Cancellation : IDisposable
    {
        private Action _cancel;
        public Cancellation(Action cancel) => _cancel = cancel;
        public void Dispose()
        {
            _cancel?.Invoke();
            _cancel = null;
        }
    }

    private readonly List<Timer> timers = new();

    private void Update()
    {
        double scaledTime = GetNowTime(TimeScaleType.Scaled);
        double unscaledTime = GetNowTime(TimeScaleType.Unscaled);

        for (int i = 0; i < timers.Count; i++)
        {
            var timer = timers[i];

            if (timer.isCancelled) continue;

            double currentTime = timer.timeScaleType == TimeScaleType.Scaled ? scaledTime : unscaledTime;


            if (currentTime >= timer.nextTickTime)
            {
                if (timer.tickRate <= 0d)
                {
                    timer.onTick?.Invoke();
                    timer.isCancelled = true;
                    continue;
                }

                int missedTickCount = Math.Max(1, (int)Math.Ceiling((currentTime - timer.nextTickTime) / timer.tickRate));
                for (int j = 0; j < missedTickCount; j++)
                {
                    timer.onTick?.Invoke();
                }
                timer.nextTickTime += missedTickCount * timer.tickRate;
            }
            else continue;
        }

        if (Time.frameCount % 60 == 0)
        {
            timers.RemoveAll(x => x.isCancelled);
        }
    }

    private static double GetNowTime(TimeScaleType ts)
    {
        return (ts == TimeScaleType.Scaled) ? Time.timeAsDouble : Time.unscaledTimeAsDouble;
    }

    public IDisposable StartRepeatingTimerCondition(double tickRate, TimeScaleType timerType, Action onTick, bool fireImmediately = false)
    {
        if (tickRate <= 0)
        {
            Logger.LogError("Tick rate must be greater than 0 for repeating timers.");
            return null;
        }

        var nowTime = GetNowTime(timerType);
        var newTimer = new Timer
        {
            nextTickTime = nowTime + (fireImmediately ? 0f : tickRate),
            tickRate = tickRate,
            timeScaleType = timerType,
            onTick = onTick,
            isCancelled = false
        };
        timers.Add(newTimer);
        return new Cancellation(() => newTimer.isCancelled = true);
    }

    public IDisposable StartOnceTimerCondition(double delaySecond, TimeScaleType timerType, Action onTick)
    {
        var nowTime = GetNowTime(timerType);
        Timer newOnceTimer = null;
        newOnceTimer = new Timer
        {
            nextTickTime = nowTime + delaySecond,
            tickRate = 0,
            timeScaleType = timerType,
            onTick = () =>
            {
                onTick?.Invoke();
                newOnceTimer.isCancelled = true;
            },
            isCancelled = false
        };
        timers.Add(newOnceTimer);
        return new Cancellation(() => newOnceTimer.isCancelled = true);
    }

    public void RestartAllTimers()
    {
        double scaledTime = GetNowTime(TimeScaleType.Scaled);
        double unscaledTime = GetNowTime(TimeScaleType.Unscaled);

        foreach (var timer in timers)
        {
            if(timer.isCancelled) continue;
            double time = (timer.timeScaleType == TimeScaleType.Scaled) ? scaledTime : unscaledTime;
            timer.nextTickTime = time + timer.tickRate;
        }
    }


}