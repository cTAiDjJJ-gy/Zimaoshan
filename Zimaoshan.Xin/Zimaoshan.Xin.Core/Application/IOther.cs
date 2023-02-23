namespace Zimaoshan.Xin.Core.Application;

public interface IOther
{
    string Get();
}

public interface IOther<T> where T : IOther
{
    T Get();
}