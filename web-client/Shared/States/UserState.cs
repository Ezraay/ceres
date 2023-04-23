namespace Shared.States;

public class UserState
{
    private string? userName;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public string UserName
    {
        get => userName ?? string.Empty;
        set
        {
            userName = value;
            NotifyStateChanged();
        }
    }
    public event Action? OnChange;
}
