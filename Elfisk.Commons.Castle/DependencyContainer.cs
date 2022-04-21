using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Elfisk.Commons.Castle
{
  public class DependencyContainer : IDependencyContainer
  {
    private IWindsorContainer Container;


    public DependencyContainer(IWindsorContainer innerContainer)
    {
      Container = innerContainer;
    }


    #region IDependencyContainer Members

    public IDependencyContainer AddComponent<I, T>()
      where I : class
      where T : class, I
    {
      return AddComponent(typeof(I), typeof(T));
    }

    public IDependencyContainer AddComponent<I, T>(string key)
      where I : class
      where T : class, I
    {
      return AddComponent(key, typeof(I), typeof(T));
    }

    public IDependencyContainer AddComponent<I, T>(string key, DependencyLifeStyle style)
      where I : class
      where T : class, I
    {
      return AddComponent(key, typeof(I), typeof(T), style);
    }

    public IDependencyContainer AddComponent(Type serviceType, Type classType)
    {
      Container.Register(Component.For(serviceType).ImplementedBy(classType));
      return this;
    }

    public IDependencyContainer AddComponent(string key, Type serviceType, Type classType)
    {
      Container.Register(Component.For(serviceType).Named(key).ImplementedBy(classType));
      return this;
    }

    public IDependencyContainer AddComponent(string key, Type serviceType, Type classType, DependencyLifeStyle style)
    {
      ComponentRegistration<object> registration = Component.For(serviceType).Named(key).ImplementedBy(classType);

      if (style == DependencyLifeStyle.Transient)
        registration = registration.LifeStyle.Transient;

      Container.Register(registration);
      return this;
    }

    public bool AddComponentIfNotRegistered(Type serviceType, Type classType)
    {
      try
      {
        AddComponent(serviceType, classType);
        return true;
      }
      catch (global::Castle.MicroKernel.ComponentRegistrationException)
      {
        return false;
      }
    }

    public bool AddComponentIfNotRegistered(string key, Type serviceType, Type classType)
    {
      try
      {
        AddComponent(key, serviceType, classType);
        return true;
      }
      catch (global::Castle.MicroKernel.ComponentRegistrationException)
      {
        return false;
      }
    }


    public bool AddComponentIfNotRegistered(string key, Type serviceType, Type classType, DependencyLifeStyle style)
    {
      try
      {
        AddComponent(key, serviceType, classType, style);
        return true;
      }
      catch (global::Castle.MicroKernel.ComponentRegistrationException)
      {
        return false;
      }
    }

    public IDependencyContainer AddDefaultComponent<I, T>()
      where I : class
      where T : class, I
    {
      Container.Register(Component.For<I>().ImplementedBy<T>().IsDefault());
      return this;
    }

    public IDependencyContainer AddDefaultComponent(string key, Type serviceType, Type classType)
    {
      Container.Register(Component.For(serviceType).Named(key).ImplementedBy(classType).IsDefault());
      return this;
    }

    public IDependencyContainer AddInstance<I>(I instance)
      where I : class
    {
      Container.Register(Component.For<I>().Instance(instance));
      return this;
    }

    public object Resolve(Type t)
    {
      return Container.Resolve(t);
    }

    public object Resolve(Type t, IDictionary<string,object> arguments)
    {
      return Container.Resolve(t, Arguments.FromNamed(arguments));
    }

    public object Resolve(Type t, object argumentsAsAnonymousType)
    {
      return Container.Resolve(t, Arguments.FromProperties(argumentsAsAnonymousType));
    }

    public T Resolve<T>()
    {
      return Container.Resolve<T>();
    }

    public T Resolve<T>(IDictionary<string, object> arguments)
    {
      return Container.Resolve<T>(Arguments.FromNamed(arguments));
    }

    public T Resolve<T>(object argumentsAsAnonymousType)
    {
      return Container.Resolve<T>(Arguments.FromProperties(argumentsAsAnonymousType));
    }

    public T[] ResolveAll<T>()
    {
      return Container.ResolveAll<T>();
    }

    public T[] ResolveAll<T>(IDictionary<string, object> arguments)
    {
      return Container.ResolveAll<T>(Arguments.FromNamed(arguments));
    }

    public T[] ResolveAll<T>(object argumentsAsAnonymousType)
    {
      return Container.ResolveAll<T>(Arguments.FromProperties(argumentsAsAnonymousType));
    }

    public T Create<T>()
    {
      if (!Container.Kernel.HasComponent(typeof(T)))
      {
        AddComponent(typeof(T).ToString(), typeof(T), typeof(T), DependencyLifeStyle.Transient);
      }

      return Resolve<T>();
    }

    public T Create<T>(object args)
    {
      if (!Container.Kernel.HasComponent(typeof(T)))
      {
        AddComponent(typeof(T).ToString(), typeof(T), typeof(T), DependencyLifeStyle.Transient);
      }

      return Resolve<T>(args);
    }


    public interface IFactory<T, T1>
    {
      T Create(T1 p1);
    }


    public interface IFactory<T, T1, T2>
    {
      T Create(T1 p1, T2 p2);
    }


    public T Create<T, T1>(T1 p1) where T : IConstructor<T1>
    {
      if (!Container.Kernel.HasComponent(typeof(T)))
      {
        Container.Kernel.AddFacility<TypedFactoryFacility>();

        AddComponent(typeof(T).ToString(), typeof(T), typeof(T), DependencyLifeStyle.Transient);

        Container.Kernel.Register(Component.For<IFactory<T, T1>>().AsFactory());
      }

      return Container.Resolve<IFactory<T, T1>>().Create(p1);
    }


    public T Create<T, T1, T2>(T1 p1, T2 p2) where T : IConstructor<T1, T2>
    {
      if (!Container.Kernel.HasComponent(typeof(T)))
      {
        Container.Kernel.AddFacility<TypedFactoryFacility>();

        AddComponent(typeof(T).ToString(), typeof(T), typeof(T), DependencyLifeStyle.Transient);

        Container.Kernel.Register(Component.For<IFactory<T, T1, T2>>().AsFactory());
      }

      return Container.Resolve<IFactory<T, T1, T2>>().Create(p1, p2);
    }

    #endregion
  }
}
