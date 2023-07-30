using System.Reflection;
using Election2023.Domain.Common;
using Election2023.Application.Interfaces.Services;

namespace UnitTests;

public class ArchTests
{
    [Fact]
    public void DomainHasNoDependency()
    {
        var assembly = Assembly.GetAssembly(typeof(IEntity));
        var cons = assembly?.GetReferencedAssemblies().Any(asm => asm.Name!.Contains("Election"));
        Assert.False(cons);
    }

    [Fact]
    public void ApplicationOnlyDependsOnDomainOrShared()
    {
        //Arrange
        var assembly = Assembly.GetAssembly(typeof(IDateTimeService));
        var deps = assembly?.GetReferencedAssemblies()
            .Where(asm => asm.Name!.Contains("Election"))
            .Select(asm => asm.Name);

        //Assert
        Assert.NotNull(deps);
        Assert.InRange(deps.Count(), 1, 2);
        deps.ToList().ForEach(n => Assert.Matches(@"(?<=Election2023.)Domain|Shared", n));
    }
}
