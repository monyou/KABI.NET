using System.ComponentModel;

namespace KABINET_Domain.Enums
{
    public enum UserRoles
    {
        [Description("Admin")]
        Admin = 1,
        [Description("Member")]
        Member = 2,
        [Description("Blocked")]
        Blocked = 3
    }
}
