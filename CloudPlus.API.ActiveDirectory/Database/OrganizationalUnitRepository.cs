using System;
using System.Data.SQLite;

namespace CloudPlus.Api.ActiveDirectory.Database
{
    public class OrganizationalUnitRepository : IOrganizationalUnitRepository
    {
        private readonly IDatabaseManager _dbManager;

        public OrganizationalUnitRepository(IDatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        public int GenerateNewOuId()
        {
            int newOu;

            using (var connection = _dbManager.GetConnection())
            {
                connection.Open();

                var getLastOuQuery = "SELECT OrganizationalUnitId FROM OrganizationalUnitIds  ORDER BY OrganizationalUnitId DESC LIMIT 1";
                
                using (var getLastOuCommand = new SQLiteCommand(getLastOuQuery, connection))
                {
                    var lastOu = Convert.ToInt32(getLastOuCommand.ExecuteScalar());

                    newOu = lastOu + 1;

                    var insertNewOuQuery = $"INSERT INTO OrganizationalUnitIds (OrganizationalUnitId) VALUES({newOu})";

                    using (var insert = new SQLiteCommand(insertNewOuQuery, connection))
                    {
                        insert.ExecuteNonQuery();
                    }
                }
            }

            return newOu;
        }
    }
}