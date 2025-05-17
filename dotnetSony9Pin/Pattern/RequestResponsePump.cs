namespace Pynch.Tools;

public abstract class RequestResponsePump<TRequest, TResponse>
{
    private readonly SemaphoreSlim _mutex = new(1, 1);

    private TaskCompletionSource<TResponse>? _currentTcs;

    public async Task<TResponse> SendAsync(TRequest request, int timeoutMs = 500, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken);
        try
        {
            if (_currentTcs != null)
                throw new InvalidOperationException("Only one request can be active at a time.");

            _currentTcs = new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

            Send(request); // fire request
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(timeoutMs);

            await using var _ = timeoutCts.Token.Register(() => _currentTcs.TrySetCanceled());

            return await _currentTcs.Task;
        }
        catch (TaskCanceledException)
        {
            throw new TimeoutException("The response was not received in time.");
        }
        finally
        {
            _currentTcs = null;
            _mutex.Release();
        }
    }

    protected abstract void Send(TRequest request);

    /// <summary>
    /// Call this when the response arrives asynchronously (e.g., from event, queue, etc.)
    /// </summary>
    protected void Received(TResponse response)
    {
        _currentTcs?.TrySetResult(response);
    }
}
