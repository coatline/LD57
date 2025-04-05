using UnityEngine;

// https://github.com/CodeSmile-0000011110110111/de.codesmile.time/blob/main/Runtime/IntervalTimer.cs
[System.Serializable]
public struct IntervalTimer
{
    [Tooltip("Time in seconds until elapsed.")]
    public float Interval;
    [HideInInspector]
    public float Timer;

    /// <summary>
    ///     State of the internal Timer towards the goal (0f or negative).
    /// </summary>
    /// <remarks>
    ///     The internal timer runs from Interval down towards 0, and might even go negative. Benefits:
    ///     - Timer is unaffected by changes to Interval until restarted.
    ///     - Timer value speaks for itself: value '1.4' reads as '1.4 seconds until elapsed'
    ///     - Timer value 0 or less means the timer is elapsed.
    /// </remarks>

    /// <summary>
    ///     Returns true if the timer has elapsed, or started as elapsed.
    /// </summary>
    public bool IsElapsed => Timer <= 0f;
    public bool IsStopped => Timer > Interval;
    public bool IsRunning => Timer <= Interval;
    public float PercentageComplete => Interval == 0 ? 1 : (1 - (Timer / Interval));

    public IntervalTimer(float interval, bool started = false)
    {
        Interval = interval;

        if (started)
            Timer = Interval;
        else
            Timer = float.MaxValue;
    }


    /// <summary>
    ///     Starts (restarts) the timer.
    /// </summary>
    /// <exception cref="ArgumentException">If Interval is negative (editor only)</exception>
    public void Start()
    {
#if UNITY_EDITOR
        if (Interval < 0f) throw new System.ArgumentException($"Interval is negative: {Interval}");
#endif
        Timer = Interval;
    }

    public void StartIfNotAlready()
    {
        if (IsRunning == false)
            Start();
    }

    public void StartWithInterval(float interval)
    {
        Interval = interval;
        Start();
    }

    public void Stop() => Timer = float.MaxValue;

    /// <summary>
    ///     Starts the timer in elapsed state. Sets Timer to 0f.
    /// </summary>
    /// <remarks>
    ///     Use this where you need a timer to fire on its first use rather than waiting out the first
    ///     Interval. For example a weapon should fire right after the reload timer/counter has elapsed
    ///     but subsequently should fire only at the Interval rate.
    /// </remarks>
    public void GoOff() => Timer = 0f;

    /// <summary>
    ///     Decrements the timer by the amount of delta time.
    /// </summary>
    /// <exception cref="ArgumentException">If deltaTime is negative (editor only).</exception>
    /// <returns>True if the timer is elapsed, false otherwise.</returns>
    public bool Decrement(float deltaTime)
    {
#if UNITY_EDITOR
        if (deltaTime < 0f) throw new System.ArgumentException($"DeltaTime is negative: {deltaTime}");
        if (IsStopped) throw new System.InvalidOperationException("Timer is stopped: Call Start() before Decrement()");
#endif

        Timer -= deltaTime;
        return IsElapsed;
    }

    public bool DecrementIfRunning(float deltaTime) => IsRunning && Decrement(deltaTime);
    public void Print() => Debug.Log($"Timer: {Timer}, Interval: {Interval}, Percentage: {PercentageComplete * 100}%");
}