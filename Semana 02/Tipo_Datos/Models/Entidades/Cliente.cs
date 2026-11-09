namespace Tipo_Datos.Models.Entidades
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Tipo_Datos.Models.Entidades.Base;

    [Table("Clientes")]
    public class Cliente : BaseModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Cedula_RUC { get; set; }

        public void ActualizarDatos(Cliente datos)
        {
            Nombres = datos.Nombres;
            Email = datos.Email;
            Telefono = datos.Telefono;
            Direccion = datos.Direccion;
            Cedula_RUC = datos.Cedula_RUC;
            isDelete = datos.isDelete;
            Update_At = DateTime.Now;
        }
    }
}
