using Zimaoshan.Xin.Cache.Foundation.Annotations;
using Zimaoshan.Xin.Cache.Foundation.DependencyInjection;

namespace Zimaoshan.Xin.Core.Application.Impl;

public class Test : ITest, ITransientDependency
{
    private readonly IOther _other;
    public Test(IOther other)
    {
        this._other = other;
    }

    public string GetNowTime()
    {
        return $"Hello! Not time is: {DateTime.Now}. GetHashCode: {GetHashCode()} -> Call Me: {_other.Get()}";
    }
}

[Component(service: typeof(ITest2), lifetimeScope: ServiceLifetime.Scoped)]
public class Test2 : ITest2
{
    public IOther? Other { get; set; }
    public string GetNowTime()
    {
        return $"Hello! Test2 Not time is: {DateTime.Now}. GetHashCode: {GetHashCode()} -> Call Me: {Other?.Get()}";
    }
}

[Component(service: typeof(ITest3), lifetimeScope: ServiceLifetime.Transient)]
public class Test3 : ITest3
{
    public IOther<Other>? GenericOther { get; set; }
    public string GetNowTime()
    {
        return $"Hello! Test3 Not time is: {DateTime.Now}. GetHashCode: {GetHashCode()} -> Call Me: {GenericOther?.Get().Get() ?? throw new ArgumentNullException(nameof(GenericOther))}";
    }
}