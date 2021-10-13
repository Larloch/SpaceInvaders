using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts
{
    /// <summary>
    ///     ServiceLocator that can be used to avoid the abuse of the Singleton anti-pattern.
    ///     This is particularly useful for the unit tests.
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        ///     Currently registered services.
        /// </summary>
        private static readonly Dictionary<System.Type, IGameService> services = new Dictionary<System.Type, IGameService>();

        /// <summary>
        ///     Gets the service instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service to lookup.</typeparam>
        /// <returns>The service instance.</returns>
        public static T Get<T>() where T : IGameService
        {
            System.Type key = typeof(T);
            Assert.IsTrue(services.ContainsKey(key), $"{key} not registered.");

            return (T)services[key];
        }
        
        public static bool IsRegistered<T>() where T : IGameService
        {
            return services.ContainsKey(typeof(T));
        }

        /// <summary>
        ///     Registers the service with the current service locator.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="service">Service instance.</param>
        public static void Register<T>(T service) where T : IGameService
        {
            System.Type key = typeof(T);
            if (!services.ContainsKey(key))
            {
                services.Add(key, service);
            }
        }

        /// <summary>
        ///     Unregisters the service from the current service locator.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        public static void Unregister<T>() where T : IGameService
        {
            System.Type key = typeof(T);
            Assert.IsTrue(services.ContainsKey(key), $"Attempted to unregister service of type {key} which is not registered.");
            services.Remove(key);
        }
    }
}