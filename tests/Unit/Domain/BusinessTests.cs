using Crm.Domain.CRM;
using FluentAssertions;

namespace Crm.Tests.Unit.Domain;

public sealed class BusinessTests
{
    [Fact]
    public void Business_requires_name()
    {
        var tenantId = Guid.NewGuid();
        Action act = () => new Business(tenantId, "");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Business_trims_name()
    {
        var b = new Business(Guid.NewGuid(), "  Acme  ");
        b.Name.Should().Be("Acme");
    }
}