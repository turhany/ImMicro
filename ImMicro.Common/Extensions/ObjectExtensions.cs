using System;
using System.Linq;
using System.Reflection;
// ReSharper disable MemberCanBePrivate.Global

namespace ImMicro.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Get object property info
        /// </summary>
        /// <param name="instance">Object</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>PropertyInfo</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception>
        public static PropertyInfo GetPropertyInfo(this object instance, string propertyName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var type = instance.GetType();

            var propertyInfo = type
                .GetProperties(BindingFlags.NonPublic |
                               BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.Static).FirstOrDefault(l => l.Name == propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentOutOfRangeException(propertyName);
            }

            return propertyInfo;
        }

        /// <summary>
        /// Set object property value
        /// </summary>
        /// <param name="instance">Object</param>
        /// <param name="propertyName">Property Name</param>
        /// <param name="value">value</param>
        /// <typeparam name="T">T</typeparam>
        /// <returns>T</returns>
        public static T SetPropertyValue<T>(this object instance, string propertyName, T value)
        {
            var propertyInfo = instance.GetPropertyInfo(propertyName);

            propertyInfo.SetValue(instance, value);

            return value;
        }
    }
}