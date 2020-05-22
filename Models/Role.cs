

namespace Proyecto.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class Role
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [MaxLength(200)]
        [Required]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Permisos")]
        public ICollection<Permission> Permissions { get; set; }

        public ICollection<User> Users { get; set; }

        public int[] SelectedValues { get; set; }
    }
}