namespace RightRule.System.Common;

public abstract class Operator : IDisposable
{
    private bool _disposed = false;
    
    public abstract List<string> Identifiers { get; }

    public bool IsCustom { get; }

    public string Name => this.GetType().Name;

    protected Operator()
    {
        IsCustom = true;
    }

    internal Operator(bool isCustom)
    {
        IsCustom = isCustom;
    }

    public async Task<OperatorResult> ProcessAsync(object Left, object Right, object? OptionalData, bool Diagnostics)
    {
        try
        {
            return await DoOperatorProcessAsync(Left, Right, OptionalData, Diagnostics).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occured in {this.GetType().Name}: {ex.Message}");
        }
    }

    protected abstract Task<OperatorResult> DoOperatorProcessAsync(object Left, object Right, object? OptionalData, bool Diagnostics);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool Disposing) 
    {
        if (_disposed) return;

        // To Do Clean up unmanaged resources

        _disposed = true;
    }
}
