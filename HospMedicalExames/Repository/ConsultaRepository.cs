using HospMedicalExames.Data;
using HospMedicalExames.Models;

namespace HospMedicalExames.Repository
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly BancContext _bancContext;
        private readonly ILogger<ConsultaRepository> _logger;


        public ConsultaRepository(BancContext bancoContext, ILogger<ConsultaRepository> logger)
        {
            _bancContext = bancoContext;
            _logger = logger;
        }

        public List<ConsultaModel> BuscarTodos()
        {
            return _bancContext.Consulta.ToList();
        }
        public ConsultaModel ListarPorId(int id)
        {
            try
            {
#pragma warning disable CS8603 // Possível retorno de referência nula.
                return _bancContext.Consulta.FirstOrDefault(c => c.Id == id);
#pragma warning restore CS8603 // Possível retorno de referência nula.
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao listar consulta por ID: {ex.Message}");
                throw;
            }
        }
        public List<ConsultaModel> BuscarPorPacienteId(int pacienteId)
        {
            return _bancContext.Consulta
                .Where(c => c.PacienteId == pacienteId)
                .ToList();
        }

        public ConsultaModel Adicionar(ConsultaModel consulta)
        {
            if (DataHoraConflitante(consulta))
            {
                throw new Exception("Conflito de horário. Já existe uma consulta agendada neste horário");

            }
            _bancContext.Consulta.Add(consulta);
            _bancContext.SaveChanges();
            return consulta;
        }

        public bool Apagar(int id)
        {
            ConsultaModel consultaDB = ListarPorId(id);
            if (consultaDB == null) throw new Exception("Houve um erro ao excluir a consulta");

            _bancContext.Consulta.Remove(consultaDB);
            _bancContext.SaveChanges();
            return true;
        }

        public ConsultaModel Atualizar(ConsultaModel consulta)
        {
            ConsultaModel consultaDB = ListarPorId(consulta.Id);
            if (consultaDB == null) throw new Exception("Houve um erro ao atualizar a consulta");

            consultaDB.ExameId = consulta.ExameId;
            consultaDB.PacienteId = consulta.PacienteId;
            consultaDB.DataHora = consulta.DataHora;
            consulta.Protocolo = consulta.Protocolo;

            _bancContext.Consulta.Update(consultaDB);
            _bancContext.SaveChanges();
            return consultaDB;

        }


        public bool DataHoraConflitante(ConsultaModel novaConsulta)
        {
            try
            {
                return (_bancContext.Consulta.Any(c =>
                c.DataHora == novaConsulta.DataHora
                && c.ExameId == novaConsulta.ExameId
                && c.Id != novaConsulta.Id));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao verificar conflito de data e hora: {ex.Message}");
                throw;
            }
        }
    }
}
