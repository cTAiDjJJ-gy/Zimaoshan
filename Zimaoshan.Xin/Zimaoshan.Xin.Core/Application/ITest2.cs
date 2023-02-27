using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Core.Application;

public interface ITest2
{
    [Cache("Test2")]
    string GetNowTime();
}

public interface ITest3
{
    [Cache("Test3")]
    string GetNowTime();
}
