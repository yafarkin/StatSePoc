using Common.Interfaces;

namespace Common.Impl;

public abstract class WarmupState : IWarmupState
{
    private int _ready;
    
    public bool IsReady => Volatile.Read(ref _ready) == 1;
    
    public void SetReady()
    {
        Interlocked.Exchange(ref _ready, 1);
    }
}