using Microsoft.AspNetCore.Mvc;
using Tipo_Datos.Models.Entidades;
using Tipo_Datos.Servicios;

namespace Tipo_Datos.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClientesServicio _clientes;

        public ClientesController(ClientesServicio clientes)
        {
            _clientes = clientes;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _clientes.Listar());
        }

        public IActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo([Bind("Nombres,Email,Telefono,Direccion,Cedula_RUC,isDelete")] Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            await _clientes.Crear(cliente);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _clientes.Buscar(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromRoute] int id, [Bind("Id,Nombres,Email,Telefono,Direccion,Cedula_RUC,isDelete")] Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();
            if (!ModelState.IsValid) return View(cliente);
            if (!await _clientes.Actualizar(cliente)) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await _clientes.Buscar(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmacionEliminar([FromRoute] int id)
        {
            var resultado = await _clientes.Eliminar(id);
            if (resultado == ResultadoEliminarCliente.NoEncontrado) return NotFound();

            if (resultado == ResultadoEliminarCliente.NoPermitido)
                TempData["ErrorCliente"] = "El cliente tiene ventas registradas o no está habilitado para eliminar.";

            return RedirectToAction(nameof(Index));
        }
    }
}
