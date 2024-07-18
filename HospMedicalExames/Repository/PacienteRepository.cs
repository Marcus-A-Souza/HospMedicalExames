
using HospMedicalExames.Models;
using HospMedicalExames.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


namespace HospMedicalExames.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly BancContext _bancContext;
        private readonly ILogger<PacienteModel> _logger;

        public PacienteRepository(BancContext bancContext, ILogger<PacienteModel> logger)
        {
            _bancContext = bancContext;
            _logger = logger;
        }
        public bool CPFUnico(string CPF)
        {
            return !_bancContext.Pacientes.Any(p => p.CPF == CPF);
        }
        public List<PacienteModel> BuscarTodos()
        {
            return _bancContext.Pacientes.ToList();
        }
        public PacienteModel ListarPorId(int id)
        {
#pragma warning disable CS8603 // Possível retorno de referência nula.
            return _bancContext.Pacientes.FirstOrDefault(x => x.Id == id);
#pragma warning restore CS8603 // Possível retorno de referência nula.
        }
        public PacienteModel BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Possível retorno de referência nula.
            return _bancContext.Pacientes.FirstOrDefault(p => p.Id == id);
#pragma warning restore CS8603 // Possível retorno de referência nula.
        }

        public List<PacienteModel> BuscarPorNomeCpf(string filtro)
        {
            try
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    return new List<PacienteModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar o paciente por nome ou CPF: {ex.Message}");
                throw;
            }
            return _bancContext.Pacientes
                .Where(p => EF.Functions.Like(p.Nome, $"%{filtro}%") || EF.Functions.Like(p.CPF, $"%{filtro}%")).ToList();
        }
        public PacienteModel Adicionar(PacienteModel paciente)
        {
            _bancContext.Pacientes.Add(paciente);
            _bancContext.SaveChanges();
            return paciente;
        }
        public PacienteModel Atualizar(PacienteModel paciente)
        {
            PacienteModel pacienteDB = ListarPorId(paciente.Id);
            if (pacienteDB == null) throw new Exception("Houve um erro na atualização do paciente");
            pacienteDB.Nome = paciente.Nome;
            pacienteDB.CPF = paciente.CPF;
            pacienteDB.DataDeNascimento = paciente.DataDeNascimento;
            pacienteDB.Sexo = paciente.Sexo;
            pacienteDB.Telefone = paciente.Telefone;
            pacienteDB.Email = paciente.Email;

            _bancContext.Pacientes.Update(pacienteDB);
            _bancContext.SaveChanges();
            return pacienteDB;
        }

        public bool Apagar(int id)
        {
            PacienteModel pacienteDB = ListarPorId(id);
            if (pacienteDB == null) throw new Exception("Houve um erro na deleção do paciente");

            _bancContext.Pacientes.Remove(pacienteDB);
            _bancContext.SaveChanges();

            return true;
        }
    }
}
