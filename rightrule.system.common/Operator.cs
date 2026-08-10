namespace RightRule.System.Common;

public abstract class Operator
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

    public async Task<EvaluationResult> ProcessAsync(object left, object right, object? OptionalData)
    {
        try
        {
            return await DoOperatorProcessAsync(left, right, OptionalData).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occured in {this.GetType().Name}: {ex.Message}");
        }
    }

    protected abstract Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OptionalData);

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
