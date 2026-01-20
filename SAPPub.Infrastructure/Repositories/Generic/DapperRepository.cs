using Dapper;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.Generic
{
    public class DapperRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<DapperRepository<T>> _logger;

        public DapperRepository(IDbConnection connection, ILogger<DapperRepository<T>> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException();
        }

        public IEnumerable<T>? ReadAll()
        {
            try
            {
                var result=  _connection.Query<T>(
                    DapperHelpers.GetQuery(typeof(T)),
                    commandType: CommandType.Text
                    );


                _logger.LogError($"Read all! from {_connection.Database} - result: {result.Count()}");

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to readall! from {_connection.Database} - {ex.Message}", ex);
            }

            return default;
        }
    }
}
