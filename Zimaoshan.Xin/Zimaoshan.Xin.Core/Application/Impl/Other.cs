using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Core.Application.Impl;

[Component(lifetimeScope: ServiceLifetime.Transient)]
public class Other : IOther
{
    public string Get()
    {
        return "调，都能调";
    }
}
