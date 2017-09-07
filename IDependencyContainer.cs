using System.Collections.Generic;

namespace Elfisk.Commons
{
  public interface IDependencyContainer
  {
    T Resolve<T>();

    IEnumerable<T> ResolveAll<T>();
  }
}
