namespace ImMicro.Common.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Object has property
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>bool</returns>
        public static bool HasProperty(this object obj, string propertyName) => obj.GetType().GetProperty(propertyName) != null;
    }
}