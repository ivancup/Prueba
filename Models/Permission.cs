
namespace Proyecto.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class Permission
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [MaxLength(200)]
        [Required]
        [DisplayName("Descripción")]
        public string Description { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}