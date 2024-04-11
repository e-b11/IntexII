using IntexII.Models;

public class EditUserViewModel
{
    public ApplicationUser User { get; set; }
    public List<string> Roles { get; set; }
    public List<string> SelectedRoles { get; set; }
}
