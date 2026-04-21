namespace Common.Interfaces;

public interface IWarmupState
{
    bool IsReady { get; }
    void SetReady();
}