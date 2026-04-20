namespace ScriptProviderService.Interfaces;

internal interface IScriptWarmupState
{
    bool IsReady { get; }
    void SetReady();
}