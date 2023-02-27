using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Core.Application;

public interface IOther
{
    [Cache("Other")]
    string Get();
}

public interface IOther<T> where T : IOther
{
    T Get();
}