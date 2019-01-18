using CloudPlus.Api.ActiveDirectory.Database;
using Owin;

namespace CloudPlus.Api.ActiveDirectory.Extensions
{
    public static class DatabaseConfig
    {
        public static void InitializeDatabase(this IAppBuilder app, IDatabaseManager dbManager)
        {
            dbManager.InitializeDatabase();
        }
    }
}