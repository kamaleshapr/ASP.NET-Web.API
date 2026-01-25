using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Data.Utils
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public  string? FirstName { get; set; }
        [Required]
        public  string? LastName { get; set; }

        public int? EmployeeId { get; set; }

    }
}
