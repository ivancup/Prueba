

namespace Proyecto.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Apellido")]
        public string Lastname { get; set; }

        [DisplayName("Teléfono")]
        public string Phone { get; set; }

        [DisplayName("Fecha nacimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateBirth { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Correo")]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Role> Roles { get; set; }

        public int[] SelectedRoles { get; set; }
    }
}