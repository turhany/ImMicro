using System.ComponentModel.DataAnnotations;
using ImMicro.Resources.App;

namespace ImMicro.Model.User
{
    public enum UserType
    {
        [Display(Name = nameof(Literals.UserType_Root), ResourceType = typeof(Literals))]
        Root = 1
    }
}