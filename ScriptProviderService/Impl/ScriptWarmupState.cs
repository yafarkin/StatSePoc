using ScriptProviderService.Interfaces;

namespace ScriptProviderService.Impl;

internal sealed class ScriptWarmupState : IScriptWarmupState
{
    private int _ready;
    
    public bool IsReady => Volatile.Read(ref _ready) == 1;
    
    public void SetReady()
    {
        Interlocked.Exchange(ref _ready, 1);
    }
}