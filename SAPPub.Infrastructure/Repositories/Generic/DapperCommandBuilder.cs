using Dapper;
using System.Data;

namespace SAPPub.Infrastructure.Repositories.Generic;

public class DapperCommandBuilder
{
    private string? _sql;
    private object? _parameters;
    private CommandType _commandType = CommandType.Text;
    private CommandFlags _commandFlags = CommandFlags.Buffered | CommandFlags.NoCache;

    public DapperCommandBuilder WithCommandText(string sql)
    {
        _sql = sql;
        return this;
    }

    public DapperCommandBuilder WithParameters(object parameters)
    {
        _parameters = parameters;
        return this;
    }

    public CommandDefinition Create(CancellationToken ct)
    {
        if (_sql is null)
            throw new InvalidOperationException("Command text must be set before creating a CommandDefinition.");
        return new CommandDefinition(commandText: _sql,
                    parameters: _parameters,
                    transaction: null,
                    commandTimeout: null,
                    commandType: _commandType,
                    flags: _commandFlags,
                    cancellationToken: ct);
    }
}
