using CliView;
using FluentAssertions;
using TaskExtensions;
using static System.TimeSpan;

namespace CliViewTests;

public class SmokeTests
{
    [Fact]
    public async Task SmokeTest()
    {
        // Arrange
        using var process = ApplicationTestRunner.StartApplication(typeof(Program));
        
        // Act
        var (read, waitFor) = ApplicationTestRunner.InitOutputBuffer(process);
        
        // Assert
        await waitFor("Enter key to quit", FromSeconds(10));
        
        read()
            .Should()
            .Be("""
                Starting ...

                Turns: 1
                Grid
                0 1 0
                1 0 0
                0 0 0

                Turns: 2
                Grid
                0 0 0
                0 0 0
                0 0 0

                Enter key to quit.
                """);
        
        process
            .HasExited
            .Should().BeFalse("Should hang until ENTER");

        await process
            .Enter()
            .Then(x => x.WaitForExitAsync())
            .Then(x => x.HasExited.Should().BeTrue("Should exist on ENTER"));
            
        read()
            .Should().Be("Thanks for playing. Goodbye.");
    }
}