using ImageHosting.Storage.Exceptions;
using ImageHosting.Storage.Generic;
using NSubstitute.ExceptionExtensions;

namespace ImageHosting.Storage.UnitTests.Services;

public class RollbackCommandsTests
{
    [Theory, AutoNSubstituteData]
    public async Task Execute_all_successful([Frozen] IReadOnlyList<IRollbackCommand> commands,
        RollbackCommands rollbackCommands)
    {
        rollbackCommands.AddRange(commands);

        await rollbackCommands.ExecuteAsync();

        await commands[0].Received().ExecuteAsync();
        await commands[1].Received().ExecuteAsync();
        await commands[2].Received().ExecuteAsync();
        await commands[0].DidNotReceiveWithAnyArgs().RollbackAsync();
        await commands[1].DidNotReceiveWithAnyArgs().RollbackAsync();
        await commands[2].DidNotReceiveWithAnyArgs().RollbackAsync();
    }

    [Theory, AutoNSubstituteData]
    public async Task Execute_error_at_last_rollback([Frozen] IReadOnlyList<IRollbackCommand> commands,
        RollbackCommands rollbackCommands)
    {
        rollbackCommands.AddRange(commands);
        commands[2].ExecuteAsync().ThrowsAsyncForAnyArgs<Exception>();

        var act = () => rollbackCommands.ExecuteAsync();

        await act.Should().ThrowAsync<RollbackCommandsException>();
        await commands[0].Received().ExecuteAsync();
        await commands[1].Received().ExecuteAsync();
        await commands[2].Received().ExecuteAsync();
        await commands[0].Received().RollbackAsync();
        await commands[1].Received().RollbackAsync();
        await commands[2].DidNotReceiveWithAnyArgs().RollbackAsync();
    }

    [Theory, AutoNSubstituteData]
    public async Task Execute_error_at_first_rollback([Frozen] IReadOnlyList<IRollbackCommand> commands,
        RollbackCommands rollbackCommands)
    {
        rollbackCommands.AddRange(commands);
        commands[0].ExecuteAsync().ThrowsAsyncForAnyArgs<Exception>();

        var act = () => rollbackCommands.ExecuteAsync();

        await act.Should().ThrowAsync<RollbackCommandsException>();
        await commands[0].Received().ExecuteAsync();
        await commands[1].DidNotReceiveWithAnyArgs().ExecuteAsync();
        await commands[2].DidNotReceiveWithAnyArgs().ExecuteAsync();
        await commands[0].DidNotReceiveWithAnyArgs().RollbackAsync();
        await commands[1].DidNotReceiveWithAnyArgs().RollbackAsync();
        await commands[2].DidNotReceiveWithAnyArgs().RollbackAsync();
    }
}
