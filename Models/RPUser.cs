using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ResourcePlanning.Models
{
    public class RPUser : IdentityUser
    {

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Organization { get; set; }

        public bool IsDomainUser { get; set; }

        [NotMapped]
        public string UserCompany { get; set; }
    }
}
