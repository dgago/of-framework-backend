using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

using Microsoft.Practices.Unity;

namespace of.web.http.dependencies
{
	public class ApiUnityDependencyResolver : IDependencyResolver
	{
		private readonly IUnityContainer _container;

		public ApiUnityDependencyResolver(IUnityContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}
			_container = container;
		}

		public void Dispose()
		{
			_container.Dispose();
		}

		public object GetService(Type serviceType)
		{
			try
			{
				return _container.Resolve(serviceType);
			}
			catch (ResolutionFailedException ex)
			{
				return null;
			}
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				return _container.ResolveAll(serviceType);
			}
			catch (ResolutionFailedException)
			{
				return new List<object>();
			}
		}

		public IDependencyScope BeginScope()
		{
			IUnityContainer child = _container.CreateChildContainer();
			return new ApiUnityDependencyResolver(child);
		}
	}
}