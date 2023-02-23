using Zimaoshan.Xin.Cache.Foundation.Annotations;
using Zimaoshan.Xin.Cache.Foundation.DependencyInjection;

namespace Zimaoshan.Xin.Core.Application.Impl;

[Component(lifetimeScope: ServiceLifetime.Transient)]
public class Other : IOther 
{
    public string Get()
    {
        return "调，都能调";
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