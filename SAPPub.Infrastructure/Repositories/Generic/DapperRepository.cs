using Dapper;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Mapping.ValueCodes; 
using SAPPub.Infrastructure.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SAPPub.Infrastructure.Repositories.Generic
{
    public class DapperRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<DapperRepository<T>> _logger;
        private readonly ICodedValueMapper _codedValueMapper;

        public DapperRepository(
            IDbConnection connection,
            ILogger<DapperRepository<T>> logger,
            ICodedValueMapper codedValueMapper) 
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _codedValueMapper = codedValueMapper ?? throw new ArgumentNullException(nameof(codedValueMapper));
        }

        public T? Read(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return default;

            return ReadSingle(new { Id = id });
        }


        public IEnumerable<T> ReadAll()
        {
            try
            {
                var sql = DapperHelpers.GetReadMultiple(typeof(T));
                if (string.IsNullOrWhiteSpace(sql))
                    throw new NotSupportedException($"No ReadMultiple query for {typeof(T).Name}");

                var items = _connection.Query<T>(sql).ToList();

                _codedValueMapper.Apply(items); // <-- map *_Coded -> numeric + _Reason

                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed ReadAll for {Type}", typeof(T).Name);
                return Enumerable.Empty<T>();
            }
        }

        public T? ReadSingle(object parameters)
        {
            try
            {
                var sql = DapperHelpers.GetReadSingle(typeof(T));
                if (string.IsNullOrWhiteSpace(sql))
                    throw new NotSupportedException($"No ReadSingle query for {typeof(T).Name}");

                var item = _connection.QuerySingleOrDefault<T>(sql, parameters);

                if (item is not null)
                    _codedValueMapper.Apply(item);

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed ReadSingle for {Type} params={Params}", typeof(T).Name, parameters);
                return default;
            }
        }

    }
}
