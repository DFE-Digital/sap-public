using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SAPPub.Infrastructure.Repositories.Generic
{
    public class DapperRepository<T> : IGenericCRUDRepository<T> where T : class
    {
        private readonly IDbConnection _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DapperRepository<T>> _logger;

        public DapperRepository(IDbConnection connection, IHttpContextAccessor httpContextAccessor, ILogger<DapperRepository<T>> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException();
        }

        public string GetUserIP()
        {
            var remoteIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            if (!string.IsNullOrEmpty(remoteIp))
            {
                return remoteIp;
            }
            return string.Empty;
        }

        public bool Create(T entity)
        {
            var rowsEffected = 0;
            try
            {
                var parameters = new DynamicParameters(entity);
                parameters.Add("@AuditIPAddress", GetUserIP());

                rowsEffected = _connection.Execute(
                     DapperHelpers.GetCreate(entity.GetType()),
                     parameters,
                     commandType: CommandType.Text
                     );
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create! - " + ex.Message, ex);
            }

            return rowsEffected > 0;
        }

        public IEnumerable<T>? ReadAll()
        {
            try
            {
                return _connection.Query<T>(
                    DapperHelpers.GetReadMultiple(typeof(T)),
                    commandType: CommandType.Text
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to readall! - " + ex.Message, ex);
            }

            return default;
        }

        public IEnumerable<T>? Query(string query, T entity)
        {
            try
            {
                return _connection.Query<T>(
                    query,
                    commandType: CommandType.Text
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to readall! - " + ex.Message, ex);
            }

            return default;
        }

        public T? Read(Guid Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);

                return _connection.Query<T>(
                        DapperHelpers.GetReadSingle(typeof(T)),
                        param,
                        commandType: CommandType.Text
                        ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to read! - " + ex.Message, ex);
            }
            return default;
        }

        public T? Read(string Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);

                return _connection.Query<T>(
                        DapperHelpers.GetReadSingle(typeof(T)),
                        param,
                        commandType: CommandType.Text
                        ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to read! - " + ex.Message, ex);
            }
            return default;
        }

        public bool Update(T entity)
        {
            var rowsEffected = 0;
            try
            {
                var parameters = new DynamicParameters(entity);
                parameters.Add("@AuditIPAddress", GetUserIP());

                rowsEffected = _connection.Execute(
                                     DapperHelpers.GetUpdate(entity.GetType()),
                                     parameters,
                                     commandType: CommandType.Text
                                     );
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update! - " + ex.Message, ex);
            }

            return rowsEffected > 0;
        }
    }
}
