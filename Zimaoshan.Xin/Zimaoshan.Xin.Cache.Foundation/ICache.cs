namespace Zimaoshan.Xin.Cache.Foundation;

/// <summary>
/// ICache
/// </summary>
public interface ICache
{
    T? Get<T>(string key);

    void Remove(string key);

    void Set<T>(string key, T obj, TimeSpan? timeout = null);
}