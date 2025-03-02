using System.Data.SqlTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API127.Models;

public class ApplicationUser  : IdentityUser
{
    public string Name { get; set; }
}     