using CopartnerUser.Common.Models;
using CopartnerUser.DataAccessLayerADO;
using CopartnerUser.ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopartnerUser.ServiceLayer.Services
{
    public class AvailabilityTypesService : IAvailabilityTypesService
    {
        private readonly DBManager _dbManager;

        public AvailabilityTypesService(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public Task<int> AddAvailabilityTypeAsync(AvailabilityTypes availabilityTypes)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAvailabilityTypeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AvailabilityTypes>> GetAllAvailabilityTypesAsync()
        {
            var commandText = "SELECT * FROM AvailabilityTypes";
            var commandType = CommandType.Text;
            var parameters = new IDbDataParameter[0]; // No parameters needed for this query

            var dataTable = _dbManager.GetDataTable(commandText, commandType, parameters);

            var availabilityTypes = new List<AvailabilityTypes>();
            foreach (DataRow row in dataTable.Rows)
            {
                availabilityTypes.Add(new AvailabilityTypes
                {
                    Id = Convert.ToInt32(row["Id"]),
                    AvailabilityType = Convert.ToString(row["AvailabilityType"]),
                });
            }

            return availabilityTypes;
        }

        public async Task<AvailabilityTypes> GetAvailabilityTypeByIdAsync(int id)
        {
            var commandText = "SELECT * FROM AvailabilityTypes WHERE Id = @Id";
            var commandType = CommandType.Text;
            var parameters = new IDbDataParameter[]
            {
                _dbManager.CreateParameter("Id", id, DbType.Int32)
            };

            var dataTable = _dbManager.GetDataTable(commandText, commandType, parameters);

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new AvailabilityTypes
                {
                    Id = Convert.ToInt32(row["Id"]),
                    AvailabilityType = Convert.ToString(row["AvailabilityType"]),
                };
            }

            return null; // AvailabilityTypes not found
        }

        public async Task<bool> UpdateAvailabilityTypeAsync(AvailabilityTypes availabilityTypes)
        {
            var commandText = "UPDATE Users SET AvailabilityType = @AvailabilityType WHERE Id = @Id";
            var commandType = CommandType.Text;
            var parameters = new IDbDataParameter[]
            {
                _dbManager.CreateParameter("AvailabilityType", availabilityTypes.AvailabilityType, DbType.String),
                _dbManager.CreateParameter("Id", availabilityTypes.Id, DbType.Int32)
            };

            var rowsAffected = _dbManager.ExecuteNonQuery(commandText, commandType, parameters);
            return rowsAffected > 0;
        }
    }
}
