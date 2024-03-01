using CopartnerUser.DataAccessLayerADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CopartnerUser.ServiceLayer.Services.Interfaces;
using CopartnerUser.Common.Models;
using Microsoft.Extensions.Logging;

namespace CopartnerUser.ServiceLayer.UserService
{
    public class UserService : IUserService
    {
        private readonly DBManager _dbManager;
        private readonly ILogger _logger;

        public UserService(DBManager dbManager, ILogger<UserService> logger)
        {
            _dbManager = dbManager;
            _logger = logger;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Getting all users.");

            var commandText = "spGetUsers";
            var commandType = CommandType.StoredProcedure;
            var parameters = new IDbDataParameter[0]; // No parameters needed for this query

            var dataTable = _dbManager.GetDataTable(commandText, commandType, parameters);

            var users = new List<User>();
            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new User
                {
                    Id = Convert.ToInt32(row["Id"]),
                    FirstName = Convert.ToString(row["FirstName"]),
                    LastName = Convert.ToString(row["LastName"])
                });
            }

            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"Getting user by Id: {id}.");

            var commandText = "spGetUserById";
            var commandType = CommandType.StoredProcedure;
            var parameters = new IDbDataParameter[]
            {
        _dbManager.CreateParameter("Id", id, DbType.Int32)
            };

            var dataTable = _dbManager.GetDataTable(commandText, commandType, parameters);

            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new User
                {
                    Id = Convert.ToInt32(row["Id"]),
                    FirstName = Convert.ToString(row["FirstName"]),
                    LastName = Convert.ToString(row["LastName"])
                };
            }

            return null; // User not found
        }

        public async Task<int> AddUserAsync(User user)
        {
            _logger.LogInformation($"Adding user: {user.FirstName} {user.LastName}.");

            var commandText = "spAddUser";
            var commandType = CommandType.StoredProcedure;
            var parameters = new IDbDataParameter[]
            {
                _dbManager.CreateParameter("FirstName", user.FirstName, DbType.String),
                _dbManager.CreateParameter("LastName", user.LastName, DbType.String)
            };

            var lastId = _dbManager.GetScalarValue(commandText, commandType, parameters);
            return Convert.ToInt32(lastId);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _logger.LogInformation($"Updating user: {user.Id}.");

            var commandText = "spUpdateUser";
            var commandType = CommandType.StoredProcedure;
            var parameters = new IDbDataParameter[]
            {
                _dbManager.CreateParameter("Id", user.Id, DbType.Int32),
                _dbManager.CreateParameter("FirstName", user.FirstName, DbType.String),
                _dbManager.CreateParameter("LastName", user.LastName, DbType.String)
            };

            // Execute the stored procedure
            var rowsAffected = _dbManager.ExecuteNonQuery(commandText, commandType, parameters);

            // Return true if at least one row was affected, indicating the update was successful
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            _logger.LogInformation($"Deleting user with Id: {id}.");

            var commandText = "spDeleteUser";
            var commandType = CommandType.StoredProcedure;
            var parameters = new IDbDataParameter[]
            {
                _dbManager.CreateParameter("Id", id, DbType.Int32)
            };

            var rowsAffected = _dbManager.ExecuteNonQuery(commandText, commandType, parameters);
            return rowsAffected > 0;
        }
    }
}
