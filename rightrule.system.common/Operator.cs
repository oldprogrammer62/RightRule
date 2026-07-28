namespace RightRule.System.Common;

public abstract class Operator
{
    private bool _disposed = false;
    
    public abstract List<string> Identifiers { get; }

    public Task<bool> ProcessAsync(object left, object right)
    {
        return Task.Run<bool>(async () =>
        {
            try
            {
                return await DoOperatorProcessAsync(left, right).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occured in {this.GetType().Name}: {ex.Message}");
            }
        });
    }

    protected abstract Task<bool> DoOperatorProcessAsync(object left, object right);

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
