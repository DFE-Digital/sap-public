using Dapper;
using Npgsql;

namespace SAPPub.Infrastructure.Repositories;

public class DBHealthCheck(NpgsqlDataSource dataSource)
{

    public async Task<bool> IsDatabaseAvailableAsync()
    {
        try
        {
            using var connection = await dataSource.OpenConnectionAsync().ConfigureAwait(false);
            //var sql = "select 1";
            //var cmd = new CommandDefinition(
            //        commandText: sql,
            //        parameters: null,
            //        transaction: null,
            //        commandTimeout: null,
            //        commandType: CommandType.Text,
            //        flags: CommandFlags.Buffered | CommandFlags.NoCache
            //        );

            //var items = (await connection.QueryAsync<T>(cmd).ConfigureAwait(false)).ToList();
            await connection.ExecuteScalarAsync<int>("SELECT 1");

            return true;
        }
        catch
        {
            return false;
        }
    }
}
