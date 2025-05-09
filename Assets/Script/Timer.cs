using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Timer 
{
    protected float initialTime;
    protected float Time { get; set; }
    
    public bool IsRunning { get; protected set; }
    
    public float Progress => Time / initialTime;
    
    public UnityAction OnTimerStart = delegate { };
    public UnityAction OnTimerStop = delegate { };

    public Timer(float time)
    {
        initialTime = time;
        Time = 0;
        IsRunning = false;
    }

    public void Start()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            OnTimerStart?.Invoke();
        } 
    }
    
    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            OnTimerStop?.Invoke();
        } 
    }

    public void Resume() => IsRunning = true;
    public void Pause() => IsRunning = false;
    
    public abstract void Tick(float deltaTime);
}

public class CountDownTimer : Timer
{
    public CountDownTimer(float time) : base(time)
    {
        
    }

    public override void Tick(float deltaTime)
    {
        if (IsRunning && Time > 0)
        {
            Time -= deltaTime;
        }

        if (IsRunning && Time <= 0)
        {
            Stop();
        }
    }
    
    public bool IsFinished => Time <= 0;
    
    public void Reset() => Time = initialTime;

    public void Reset(float time)
    {
        initialTime = time;
        Reset();
    }
}

public class StopWatchTimer : Timer
{
    public StopWatchTimer(float value) : base(0){}

    public override void Tick(float deltaTime)
    {
        if (IsRunning)
        {
            Time += deltaTime;
        }
    }
    
    public void Reset() => Time = 0;
    public float GetTime() => Time;
}
