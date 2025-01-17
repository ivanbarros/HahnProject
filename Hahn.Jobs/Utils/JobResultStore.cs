// Utils/JobResultStore.cs
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Hahn.Jobs.Utils;

public static class JobResultStore
{
    // Key: JobId, Value: TaskCompletionSource<object>
    private static ConcurrentDictionary<string, TaskCompletionSource<object>> _store = new ConcurrentDictionary<string, TaskCompletionSource<object>>();

    /// <summary>
    /// Registers a new job and returns its unique JobId.
    /// </summary>
    public static string RegisterJob()
    {
        var jobId = Guid.NewGuid().ToString();
        var tcs = new TaskCompletionSource<object>();
        _store[jobId] = tcs;
        return jobId;
    }

    /// <summary>
    /// Awaits the result of a job with the specified JobId.
    /// </summary>
    public static async Task<T> GetJobResultAsync<T>(string jobId, int timeoutSeconds = 30)
    {
        if (_store.TryGetValue(jobId, out var tcsObj))
        {
            var task = tcsObj.Task;
            if (await Task.WhenAny(task, Task.Delay(timeoutSeconds * 1000)) == task)
            {
                if (task.Result is T result)
                {
                    return result;
                }
            }
        }

        return default(T);
    }

    /// <summary>
    /// Sets the result for a completed job.
    /// </summary>
    public static void SetJobResult<T>(string jobId, T result)
    {
        if (_store.TryGetValue(jobId, out var tcsObj))
        {
            tcsObj.SetResult(result);
            _store.TryRemove(jobId, out _);
        }
    }
}