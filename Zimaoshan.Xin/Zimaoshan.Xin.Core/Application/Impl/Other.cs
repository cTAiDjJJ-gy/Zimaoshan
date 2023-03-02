using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Core.Application.Impl;

[Component(lifetimeScope: ServiceLifetime.Transient)]
public class Other : IOther 
{
    public string Get()
    {
        return "调，都能调";
    }

    public string GetByUserID(string userID)
    {
        return $"Now: {DateTime.UtcNow} => ID({userID}): 你是谁，你好帅!";
    }
}

[Component(lifetimeScope: ServiceLifetime.Transient)]
public class GenericOther<T> : IOther<T> where T : IOther
{
    public IOther? Call { get; set; }

    public T Get()
    {
        return (T?)Call ?? throw new ArgumentNullException(nameof(Call));
    }
}