using System.Diagnostics;

namespace lathoub;

public abstract class RequestResponsePump<T, U> 
{
    #region Events

    private event EventHandler<U> ReceivedResponse;

    private event EventHandler? Aasdf;

    private void RaiseResponse(U response)
    {
        var handler = ReceivedResponse;
        handler?.Invoke(this, response);
    }

    private void RaiseTimeOutError()
    {
        var handler = Aasdf;
        handler?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    /// <summary>
    /// Only Async Send method
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public virtual Task<U?>? SendAsync(T req)
    {
        var promise = new TaskCompletionSource<U>();

        void handler(object? sender, U e)
        {
            ReceivedResponse -= handler;
            promise.TrySetResult(e);
        }
        ReceivedResponse += handler;

        Send(req);

        var completed = Task.WhenAny(promise.Task, Task.Delay(2000));
        if (promise.Task.IsFaulted)
        {
            promise.SetCanceled();

            RaiseTimeOutError();

            return default;
        }

        return promise.Task;
    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract void Send(T req);

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Received(U res)
    {
        Debug.WriteLine($"Response: {res}");

        RaiseResponse(res);
    }

}
