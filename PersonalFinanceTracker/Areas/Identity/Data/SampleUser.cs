using Microsoft.AspNetCore.Identity;

namespace PersonalFinanceTracker.Areas.Identity.Data;

public class SampleUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}