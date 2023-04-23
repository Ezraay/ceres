namespace Shared.States;

public class UserState
{
    private void NotifyStateChanged() => OnChange?.Invoke();
    public event Action? OnChange;

    private string? userName;
    public string UserName
    {
        get => userName ?? string.Empty;
        set
        {
            userName = value;
            NotifyStateChanged();
        }
    }

    private bool readyToPlay;
    public bool ReadyToPlay
    {
        get => readyToPlay;
        set
        {
            readyToPlay = value;
            NotifyStateChanged();
        }
    }
}
