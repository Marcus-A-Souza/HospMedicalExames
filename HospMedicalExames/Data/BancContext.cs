using HospMedicalExames.Models;
using Microsoft.EntityFrameworkCore;

namespace HospMedicalExames.Data
{
    public class BancContext : DbContext
    {
        public BancContext(DbContextOptions<BancContext> options) : base(options)
        {

        }
        public DbSet<PacienteModel> Pacientes { get; set; }
        public DbSet<TipoExameModel> TipoExames { get; set; }
        public DbSet<ExameModel> Exames { get; set; }
        public DbSet<ConsultaModel> Consulta { get; set; }
      
        
    }
}




