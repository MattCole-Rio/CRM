using Crm.Domain.CRM;
using FluentAssertions;

namespace Crm.Tests.Unit.Domain;

public sealed class ContactTests
{
    [Fact]
    public void Contact_requires_first_and_last_name()
    {
        Action act = () => new Contact(Guid.NewGuid(), "", "Smith");
        act.Should().Throw<ArgumentException>();

        Action act2 = () => new Contact(Guid.NewGuid(), "John", "");
        act2.Should().Throw<ArgumentException>();
    }
}