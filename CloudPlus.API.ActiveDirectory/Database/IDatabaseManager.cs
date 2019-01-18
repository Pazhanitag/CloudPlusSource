using System.Data.SQLite;

namespace CloudPlus.Api.ActiveDirectory.Database
{
    public interface IDatabaseManager
    {
        SQLiteConnection GetConnection();
        void InitializeDatabase();
    }
}