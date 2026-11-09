using Microsoft.EntityFrameworkCore;
using System.Data;
using Tipo_Datos.Data;
using Tipo_Datos.Models.Entidades;

namespace Tipo_Datos.Servicios
{
    public class ClientesServicio
    {
        private readonly DatosDbContext _dbContext;

        public ClientesServicio(DatosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Cliente>> Listar()
        {
            return _dbContext.Clientes.AsNoTracking().ToListAsync();
        }

        public Task<Cliente> Buscar(int id)
        {
            return _dbContext.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Crear(Cliente cliente)
        {
            cliente.Id = 0;
            cliente.Create_At = DateTime.Now;
            cliente.Update_At = cliente.Create_At;

            _dbContext.Clientes.Add(cliente);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Actualizar(Cliente datos)
        {
            var cliente = await _dbContext.Clientes.FindAsync(datos.Id);
            if (cliente == null) return false;

            cliente.ActualizarDatos(datos);
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<ResultadoEliminarCliente> Eliminar(int id)
        {
            // Mantiene la comprobación de ventas y el borrado en la misma transacción.
            await using var transaccion = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente == null) return ResultadoEliminarCliente.NoEncontrado;

            if (!cliente.isDelete || await _dbContext.Ventas.AnyAsync(v => v.ClientesModelId == id))
                return ResultadoEliminarCliente.NoPermitido;

            _dbContext.Clientes.Remove(cliente);
            await _dbContext.SaveChangesAsync();
            await transaccion.CommitAsync();
            return ResultadoEliminarCliente.Eliminado;
        }
    }

    public enum ResultadoEliminarCliente
    {
        Eliminado,
        NoEncontrado,
        NoPermitido
    }
}
