using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using ImageHosting.Storage.Exceptions;

namespace ImageHosting.Storage.Generic;

public class RollbackCommands
{
    private readonly List<IRollbackCommand> _commands = [];

    public void Add(IRollbackCommand rollbackCommand)
    {
        _commands.Add(rollbackCommand);
    }

    public void AddRange(IEnumerable<IRollbackCommand> rollbackCommands)
    {
        foreach (var rollbackCommand in rollbackCommands)
        {
            Add(rollbackCommand);
        }
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Guard.HasSizeGreaterThan(_commands, 1);

        var i = 0;
        try
        {
            while (i < _commands.Count)
            {
                await _commands[i].ExecuteAsync(cancellationToken);
                i++;
            }
        }
        catch (Exception e)
        {
            var commandName = _commands[i].GetType().Name;

            i--;
            while (i >= 0)
            {
                await _commands[i].RollbackAsync();
                i--;
            }

            throw new RollbackCommandsException($"A rollback occurred due to an exception in {commandName}.", e);
        }
    }
}