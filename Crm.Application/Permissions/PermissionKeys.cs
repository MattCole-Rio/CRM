namespace Crm.Application.Permissions;

public static class PermissionKeys
{
    public static class Rbac
    {
        public const string Manage = "RBAC.Manage";
    }

    public static class Crm
    {
        public const string BusinessView = "CRM.Business.View";
        public const string BusinessCreate = "CRM.Business.Create";
        public const string BusinessEdit = "CRM.Business.Edit";

        public const string ContactView = "CRM.Contact.View";
        public const string ContactCreate = "CRM.Contact.Create";
        public const string ContactEdit = "CRM.Contact.Edit";
    }
}