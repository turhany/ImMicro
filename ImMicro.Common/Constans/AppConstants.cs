namespace ImMicro.Common.Constans
{
    public static class AppConstants
    {
        public const string ProductName = "ImMicro";
        public const string JsonContentType = "application/json";
        
        public const string PostgreSqlConnectionString = "PostgreSqlConnectionString";
        public const string RedisConnectionString = "RedisConnectionString";
        
        public const string RedisCacheInstanceName = "ImMicroCache";
        public const int DefaultCacheDuration = 10;
        
        public const string HashKey = "turhany";
         
        public const string RabbitMqSettingsOptionName = "RabbitMqSettings";
        public const string RedLockSettingsOptionName = "RedLockSettings";
        
        public const string ClaimTypesId = "Id";
        public const string SymmetricKey = "!#_ImMicro_Symmetric_Key_2022_!#";
        public const double AccessTokenExpireMinute = 6 * 60;//6 hour
        public const double RefreshTokenExpireMinute = 7 * 60; //7 hour
    }
}