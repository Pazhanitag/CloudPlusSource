using System.Data.SQLite;
using System.IO;
using System.Transactions;
using System.Web;
using CloudPlus.Resources;

namespace CloudPlus.Api.ActiveDirectory.Database
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly string _dbPath;
        private readonly IConfigurationManager _configurationManager;
        public DatabaseManager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _dbPath = $"{HttpContext.Current.Server.MapPath("~")}\\{_configurationManager.GetByKey("AD.OrganizationalUnitIdsDbPath")}";
        }

        public void InitializeDatabase()
        {
            if (DatabaseExists())
                return;

            CreateDatabase();
            CreateTables();
        }


        public SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            return connection;
        }

        private void CreateTables()
        {
            using (var transaction = new TransactionScope())
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    const string createIndex = "CREATE UNIQUE INDEX 'OrganizationalUnitIdIndex' ON 'OrganizationalUnitIds' ('OrganizationalUnitId');";

                    const string createTable = "CREATE TABLE 'OrganizationalUnitIds' ('OrganizationalUnitId' INTEGER PRIMARY KEY NOT NULL UNIQUE);";

                    using (var createTableCommand = new SQLiteCommand(createTable, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }
                    
                    using (var createIndexCommand = new SQLiteCommand(createIndex, connection))
                    {
                        createIndexCommand.ExecuteNonQuery();
                    }

                    var insertInitialValue =
                        $"INSERT INTO OrganizationalUnitIds (OrganizationalUnitId) VALUES({_configurationManager.GetByKey("AD.OrganizationalUnitInitialId")})";
                    
                    transaction.Complete();

                    using (var insertInitialValueCommand = new SQLiteCommand(insertInitialValue, connection))
                    {
                        insertInitialValueCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private bool DatabaseExists()
        {
            return File.Exists(_dbPath);
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(_dbPath);
        }
    }
}