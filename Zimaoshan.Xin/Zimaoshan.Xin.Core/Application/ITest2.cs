using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Core.Application;

[WithCache]
public interface ITest2
{
    [Cache("Test2")]
    string GetNowTime();
}

[WithCache]
public interface ITest3
{
    [Cache("Test3")]
    string GetNowTime();
}
