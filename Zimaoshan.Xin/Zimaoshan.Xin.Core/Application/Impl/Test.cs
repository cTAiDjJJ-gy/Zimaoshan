using Zimaoshan.Xin.Cache.Foundation.Annotations;
using Zimaoshan.Xin.Cache.Foundation.DependencyInjection;

namespace Zimaoshan.Xin.Core.Application.Impl;

public class Test : ITest, ITransientDependency
{
    public string GetNowTime()
    {
        return $"Hello! Not time is: {DateTime.Now}. GetHashCode: {GetHashCode()}";
    }
}

[Component(service: typeof(ITest2), lifetimeScope: ServiceLifetime.Scoped)]
public class Test2 : ITest2
{
    public string GetNowTime()
    {
        return $"Hello! Test2 Not time is: {DateTime.Now}. GetHashCode: {GetHashCode()}";
    }
}

[Component(service: typeof(ITest2), lifetimeScope: ServiceLifetime.Transient)]
public class Test3 : ITest2
{
    public string GetNowTime()
    {
        return $"Hello! Test3 Not time is: {DateTime.Now}. GetHashCode: {GetHashCode()}";
    }
}