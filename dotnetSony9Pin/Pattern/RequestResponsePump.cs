using System.Diagnostics;

namespace Pynch.Tools;

public abstract class RequestResponsePump<TRequest, TResponse>
{
    #region Events

    private event EventHandler<TResponse>? ReceivedResponse;

    private void RaiseResponse(TResponse response)
    {
        var handler = ReceivedResponse;
        handler?.Invoke(this, response);
    }

    #endregion

    private readonly Lock _lock = new();

    /// <summary>
    /// Only Async Send method
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public virtual Task<TResponse> SendAsync(TRequest req)
    {
        lock (_lock)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            var promise = new TaskCompletionSource<TResponse>();

            ReceivedResponse += Handler;

            Send(req);

            var completed = Task.WhenAny(promise.Task, Task.Delay(500));
            if (completed.Result != promise.Task)
            {
                promise.SetCanceled();

                throw new TimeoutException("Timeout");
            }

            stopwatch.Stop();

            return promise.Task;

            void Handler(object? sender, TResponse e)
            {
                ReceivedResponse -= Handler;
                promise.TrySetResult(e);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract void Send(TRequest req);

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Received(TResponse res)
    {
        RaiseResponse(res);
    }

}