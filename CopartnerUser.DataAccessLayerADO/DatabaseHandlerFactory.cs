using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopartnerUser.DataAccessLayerADO
{
    public class DatabaseHandlerFactory
    {
        private string connectionString;
        private string providerName;

        public DatabaseHandlerFactory(IConfiguration configuration, string connectionStringName, string providerName)
        {
            connectionString = configuration.GetConnectionString(connectionStringName);
            this.providerName = providerName;
        }

        public IDatabaseHandler CreateDatabase()
        {
            IDatabaseHandler database = null;

            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    database = new SqlDataAccess(connectionString);
                    break;
                //case "system.data.oracleclient":
                //    database = new OracleDataAccess(connectionStringSettings.ConnectionString);
                //    break;
                //case "system.data.oleDb":
                //    database = new OledbDataAccess(connectionStringSettings.ConnectionString);
                //    break;
                //case "system.data.odbc":
                //    database = new OdbcDataAccess(connectionStringSettings.ConnectionString);
                //    break;
                default:
                    break;
            }

            return database;
        }

        public string GetProviderName()
        {
            return providerName;
        }
    }
}
