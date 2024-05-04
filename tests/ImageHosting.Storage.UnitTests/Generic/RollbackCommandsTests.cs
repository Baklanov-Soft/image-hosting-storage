using ImageHosting.Storage.Domain.Common;
using ImageHosting.Storage.Domain.Exceptions;
using NSubstitute.ExceptionExtensions;

namespace ImageHosting.Storage.UnitTests.Generic;

public class RollbackCommandsTests
{
    [Fact]
    public async Task Execute_all_successful()
    {
        var command1 = Substitute.For<IRollbackCommand>();
        var command2 = Substitute.For<IRollbackCommand>();
        var command3 = Substitute.For<IRollbackCommand>();
        var rollbackCommands = new RollbackCommands();
        rollbackCommands.Add(command1);
        rollbackCommands.Add(command2);
        rollbackCommands.Add(command3);

        await rollbackCommands.ExecuteAsync();

        await command1.Received().ExecuteAsync();
        await command2.Received().ExecuteAsync();
        await command3.Received().ExecuteAsync();
        await command1.DidNotReceiveWithAnyArgs().RollbackAsync();
        await command2.DidNotReceiveWithAnyArgs().RollbackAsync();
        await command3.DidNotReceiveWithAnyArgs().RollbackAsync();
    }

    [Fact]
    public async Task Execute_error_at_last_rollback()
    {
        var command1 = Substitute.For<IRollbackCommand>();
        var command2 = Substitute.For<IRollbackCommand>();
        var command3 = Substitute.For<IRollbackCommand>();
        var rollbackCommands = new RollbackCommands();
        rollbackCommands.Add(command1);
        rollbackCommands.Add(command2);
        rollbackCommands.Add(command3);
        command3.ExecuteAsync().ThrowsAsyncForAnyArgs<Exception>();

        var act = () => rollbackCommands.ExecuteAsync();

        await act.Should().ThrowAsync<RollbackCommandsException>();
        await command1.Received().ExecuteAsync();
        await command2.Received().ExecuteAsync();
        await command3.Received().ExecuteAsync();
        await command1.Received().RollbackAsync();
        await command2.Received().RollbackAsync();
        await command3.DidNotReceiveWithAnyArgs().RollbackAsync();
    }

    [Fact]
    public async Task Execute_error_at_first_rollback()
    {
        var command1 = Substitute.For<IRollbackCommand>();
        var command2 = Substitute.For<IRollbackCommand>();
        var command3 = Substitute.For<IRollbackCommand>();
        var rollbackCommands = new RollbackCommands();
        rollbackCommands.Add(command1);
        rollbackCommands.Add(command2);
        rollbackCommands.Add(command3);
        command1.ExecuteAsync().ThrowsAsyncForAnyArgs<Exception>();

        var act = () => rollbackCommands.ExecuteAsync();

        await act.Should().ThrowAsync<RollbackCommandsException>();
        await command1.Received().ExecuteAsync();
        await command2.DidNotReceiveWithAnyArgs().ExecuteAsync();
        await command3.DidNotReceiveWithAnyArgs().ExecuteAsync();
        await command1.DidNotReceiveWithAnyArgs().RollbackAsync();
        await command2.DidNotReceiveWithAnyArgs().RollbackAsync();
        await command3.DidNotReceiveWithAnyArgs().RollbackAsync();
    }
}
