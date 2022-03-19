using System;
using System.Collections.Generic;

namespace Bundles
{
	/// <summary>
	/// Service container used by a <see cref="Bundler"/> and its child nodes
	/// </summary>
	public class BundleServiceContainer : IServiceProvider
	{
		private Dictionary<Type, object> _services = new Dictionary<Type, object>();

		/// <summary>
		/// Adds a service to the container
		/// </summary>
		/// <param name="provider"></param>
		public void AddService(object provider)
		{
			Type type = provider.GetType();
			_services.Add(type, provider);
		}

		/// <summary>
		/// Adds a service to the container
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void AddService<T>() where T : new()
		{
			var provider = new T();
			AddService(provider);
		}

		/// <summary>
		/// Retrieves a service from the container
		/// </summary>
		/// <param name="type"></param>
		/// <returns>Service of provided type if present; otherwise <c>null</c></returns>
		public object GetService(Type type)
		{
			if (_services.TryGetValue(type, out object service))
			{
				return service;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Retrieves a service from the container
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>Service of provided type if present; otherwise <c>null</c></returns>
		public T GetService<T>()
		{
			_services.TryGetValue(typeof(T), out object service);
			if (service is T tService)
			{
				return tService;
			}
			else
			{
				return default(T);
			}
		}

		/// <summary>
		/// Removes service of given type from the container
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void RemoveService<T>()
		{
			if (_services.ContainsKey(typeof(T)))
			{
				_services.Remove(typeof(T));
			}
		}
	}
}