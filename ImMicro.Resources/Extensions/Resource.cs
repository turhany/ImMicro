using ImMicro.Resources.Service;
#pragma warning disable CS8602

namespace ImMicro.Resources.Extensions
{
    public class Resource
    {
        public static string Created(string entity)
        {
            return GetString(ServiceResponseMessage.RESOURCE_CREATED, entity.ToLowerInvariant());
        }

        public static string Created(string entity, string name)
        {
            return GetString(ServiceResponseMessage.RESOURCE_CREATED_WITH_NAME, entity.ToLowerInvariant(),
                $"\"{name}\"");
        }

        public static string NotFound(string entity)
        {
            return GetString(ServiceResponseMessage.RESOURCE_NOT_FOUND, entity);
        }

        public static string Updated(string entity, string name)
        {
            return GetString(ServiceResponseMessage.RESOURCE_UPDATED_WITH_NAME, entity, $"\"{name}\"");
        }

        public static string Duplicate(string entity)
        {
            return GetString(ServiceResponseMessage.DUPLICATE_DATA, entity);
        }

        public static string Retrieved()
        {
            return GetString(ServiceResponseMessage.RESOURCE_RETRIEVED);
        }
        
        public static string Deleted(string entity, string name)
        {
            return GetString(ServiceResponseMessage.RESOURCE_DELETED_WITH_NAME, entity, $"\"{name}\"");
        }

        #region Private Methods

        private static string GetString(string value, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToString().Trim();
            }

            return string.Format(value, args);
        }

        #endregion
    }
}