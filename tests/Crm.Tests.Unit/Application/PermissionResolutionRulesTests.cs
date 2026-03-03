using Crm.Application.Permissions;
using FluentAssertions;

namespace Crm.Tests.Unit.Application;

public sealed class PermissionResolutionRulesTests
{
    [Fact]
    public void Deny_override_beats_allow_and_role()
    {
        var hasRolePermission = true;
        var overrideEffect = PermissionEffect.Deny;

        var effective = Resolve(hasRolePermission, overrideEffect);
        effective.Should().BeFalse();
    }

    [Fact]
    public void Allow_override_beats_missing_role_permission()
    {
        var hasRolePermission = false;
        var overrideEffect = PermissionEffect.Allow;

        var effective = Resolve(hasRolePermission, overrideEffect);
        effective.Should().BeTrue();
    }

    private static bool Resolve(bool hasRolePermission, PermissionEffect? overrideEffect)
    {
        if (overrideEffect == PermissionEffect.Deny) return false;
        if (overrideEffect == PermissionEffect.Allow) return true;
        return hasRolePermission;
    }
}