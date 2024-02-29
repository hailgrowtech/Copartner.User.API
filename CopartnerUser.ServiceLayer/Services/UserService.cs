using CopartnerUser.DataAccessLayerADO.Models;
using CopartnerUser.DataAccessLayerADO;
using CopartnerUser.ServiceLayer.ExpertService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CopartnerUser.ServiceLayer.UserService
{
    public class UserService : IUserService
    {
        private readonly DBManager _dbManager;

        public UserService(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var commandText = "SELECT * FROM Users";
            var commandType = CommandType.Text;
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
            var commandText = "SELECT * FROM Users WHERE Id = @Id";
            var commandType = CommandType.Text;
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
            var commandText = "INSERT INTO Users (FirstName, LastName) VALUES (@FirstName, @LastName); SELECT SCOPE_IDENTITY();";
            var commandType = CommandType.Text;
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
            var commandText = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
            var commandType = CommandType.Text;
            var parameters = new IDbDataParameter[]
            {
                _dbManager.CreateParameter("FirstName", user.FirstName, DbType.String),
                _dbManager.CreateParameter("LastName", user.LastName, DbType.String),
                _dbManager.CreateParameter("Id", user.Id, DbType.Int32)
            };

            var rowsAffected = _dbManager.ExecuteNonQuery(commandText, commandType, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var commandText = "DELETE FROM Users WHERE Id = @Id";
            var commandType = CommandType.Text;
            var parameters = new IDbDataParameter[]
            {
                _dbManager.CreateParameter("Id", id, DbType.Int32)
            };

            var rowsAffected = _dbManager.ExecuteNonQuery(commandText, commandType, parameters);
            return rowsAffected > 0;
        }
    }
}
