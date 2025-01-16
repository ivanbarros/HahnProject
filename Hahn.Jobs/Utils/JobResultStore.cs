// Utils/JobResultStore.cs
using StackExchange.Redis;
using System.Text.Json;

namespace Hahn.Jobs.Utils;

public class JobResultStore
{
    private static ConnectionMultiplexer _redis;
    private static IDatabase _db;

    static JobResultStore()
    {
        _redis = ConnectionMultiplexer.Connect("localhost"); // Update with your Redis connection string
        _db = _redis.GetDatabase();
    }

    /// <summary>
    /// Registers a new job and returns its unique JobId.
    /// </summary>
    public static string RegisterJob()
    {
        var jobId = Guid.NewGuid().ToString();
        // Initialize the result key with a placeholder
        _db.StringSet(jobId, "");
        return jobId;
    }

    /// <summary>
    /// Awaits the result of a job with the specified JobId.
    /// </summary>
    public static async Task<T> GetJobResultAsync<T>(string jobId, int timeoutSeconds = 30)
    {
        var timeout = TimeSpan.FromSeconds(timeoutSeconds);
        var startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < timeout)
        {
            var result = await _db.StringGetAsync(jobId);
            if (!result.IsNullOrEmpty)
            {
                // Deserialize the result
                return JsonSerializer.Deserialize<T>(result);
            }

            await Task.Delay(500); // Poll every 500ms
        }

        return default(T);
    }

    /// <summary>
    /// Sets the result for a completed job.
    /// </summary>
    public static void SetJobResult<T>(string jobId, T result)
    {
        var serializedResult = JsonSerializer.Serialize(result);
        _db.StringSet(jobId, serializedResult);
    }
}