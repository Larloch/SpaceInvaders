using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Scripts
{
    /// <summary>
    ///     ServiceLocator that can be used to avoid the abuse of the Singleton anti-pattern.
    ///     This is particularly useful for the unit tests.
    /// </summary>
    public class ServiceLocator : MonoBehaviour
    {
        /// <summary>
        ///     Currently registered services.
        /// </summary>
        private static readonly Dictionary<string, IGameService> services = new Dictionary<string, IGameService>();

        /// <summary>
        ///     Gets the service instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service to lookup.</typeparam>
        /// <returns>The service instance.</returns>
        public static T Get<T>() where T : IGameService
        {
            string key = typeof(T).Name;
            Assert.IsTrue(services.ContainsKey(key), $"{key} not registered.");

            return (T)services[key];
        }

        /// <summary>
        ///     Registers the service with the current service locator.
        ///     If the service is already registered, it is updated with the new instance.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="service">Service instance.</param>
        public static void Register<T>(T service) where T : IGameService
        {
            string key = typeof(T).Name;
            if (!services.ContainsKey(key))
            {
                services.Add(key, service);
            }
            else
            {
                services[key] = service;
            }
        }

        /// <summary>
        ///     Unregisters the service from the current service locator.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        public static void Unregister<T>() where T : IGameService
        {
            string key = typeof(T).Name;
            Assert.IsTrue(services.ContainsKey(key), $"Attempted to unregister service of type {key} which is not registered.");
            services.Remove(key);
        }
    }
}