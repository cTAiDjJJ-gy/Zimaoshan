using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Core.Application;

[WithCache]
public interface IOther
{
    [Cache("Other")]
    string Get();

    [Cache("Other_@userID", expiration: 300)]
    string GetByUserID(string userID);
}

public interface IOther<T> where T : IOther
{
    T Get();
}