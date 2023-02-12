using Zimaoshan.Xin.Cache.Foundation.DependencyInjection;

namespace Zimaoshan.Xin.Core.Application.Impl;

public class Test : ITest, ITransientDependency
{
    public string GetNowTime()
    {
        return $"Hello! Not time is: {DateTime.Now}. GetHashCode: {GetHashCode()}";
    }
}