using System;
using System.Collections.Generic;

namespace Elfisk.Commons
{
  public interface IDependencyResolver
  {
    object Resolve(Type t);
    object Resolve(Type t, IDictionary<string, object> arguments);
    object Resolve(Type t, object argumentsAsAnonymousType);

    T Resolve<T>();
    T Resolve<T>(IDictionary<string, object> arguments);
    T Resolve<T>(object argumentsAsAnonymousType);

    T[] ResolveAll<T>();
    T[] ResolveAll<T>(IDictionary<string, object> arguments);
    T[] ResolveAll<T>(object argumentsAsAnonymousType);

    /// <summary>
    /// Create transient instance of passed type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Create<T>();

    /// <summary>
    /// Create transient instance of passed type with named arguments.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args">Anonymous class with properties for named parameters.</param>
    /// <returns></returns>
    T Create<T>(object args);

    /// <summary>
    /// Create transient instance of passed type with type safe constructor parameters. 
    /// Constructor parameter must be named "p1".
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1">Input type 1</typeparam>
    /// <param name="p1">Input 1</param>
    /// <returns></returns>
    T Create<T, T1>(T1 p1) where T : IConstructor<T1>;

    /// <summary>
    /// Create transient instance of passed type with type safe constructor parameters.
    /// Constructor parameters must be named "p1" and "p2".
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1">Input type 1</typeparam>
    /// <typeparam name="T2">Input type 2</typeparam>
    /// <param name="p1">Input 1</param>
    /// <param name="p2">Input 2</param>
    /// <returns></returns>
    T Create<T, T1, T2>(T1 p1, T2 p2) where T : IConstructor<T1, T2>;
  }

  public interface IConstructor<T1>
  {
    // Nothing
  }

  public interface IConstructor<T1, T2>
  {
    // Nothing
  }

  public enum DependencyLifeStyle { Singleton = 0, Transient = 1 }


  public interface IDependencyContainer : IDependencyResolver
  {
    IDependencyContainer AddComponent<I, T>()
      where I : class
      where T : class, I;
    IDependencyContainer AddComponent<I, T>(string key)
      where I : class
      where T : class, I;
    IDependencyContainer AddComponent<I, T>(string key, DependencyLifeStyle style)
      where I : class
      where T : class, I;
    IDependencyContainer AddComponent(Type serviceType, Type classType);
    bool AddComponentIfNotRegistered(Type serviceType, Type classType);
    IDependencyContainer AddComponent(string key, Type serviceType, Type classType);
    bool AddComponentIfNotRegistered(string key, Type serviceType, Type classType);
    IDependencyContainer AddComponent(string key, Type serviceType, Type classType, DependencyLifeStyle style);
    bool AddComponentIfNotRegistered(string key, Type serviceType, Type classType, DependencyLifeStyle style);
    IDependencyContainer AddDefaultComponent<I, T>()
      where I : class
      where T : class, I;
    IDependencyContainer AddDefaultComponent(string key, Type serviceType, Type classType);
    IDependencyContainer AddInstance<I>(I instance)
      where I : class;
  }
}
