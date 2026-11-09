using Microsoft.AspNetCore.Mvc;
using Tipo_Datos.Models.Entidades;
using Tipo_Datos.Servicios;

namespace Tipo_Datos.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesApiController : ControllerBase
    {
        private readonly ClientesServicio _clientes;

        public ClientesApiController(ClientesServicio clientes)
        {
            _clientes = clientes;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var listaClientes = await _clientes.Listar();
            listaClientes.Insert(0, new Cliente
            {
                Nombres = "Seleccione un cliente",
                Cedula_RUC = string.Empty,
                Direccion = string.Empty,
                Email = string.Empty,
                Telefono = string.Empty,
                Create_At = DateTime.Now,
                Update_At = DateTime.Now
            });

            return listaClientes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClientesModel(int id)
        {
            var cliente = await _clientes.Buscar(id);
            if (cliente == null) return NotFound();

            return cliente;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientesModel(int id, Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            if (!await _clientes.Actualizar(cliente)) return NotFound();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostClientesModel(Cliente cliente)
        {
            await _clientes.Crear(cliente);
            return CreatedAtAction(nameof(GetClientesModel), new { id = cliente.Id }, cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientesModel(int id)
        {
            var resultado = await _clientes.Eliminar(id);
            if (resultado == ResultadoEliminarCliente.NoEncontrado) return NotFound();
            if (resultado == ResultadoEliminarCliente.NoPermitido)
                return Conflict("El cliente tiene ventas registradas o no está habilitado para eliminar.");

            return NoContent();
        }
    }
}
