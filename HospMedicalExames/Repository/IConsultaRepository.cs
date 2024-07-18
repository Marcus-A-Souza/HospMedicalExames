using HospMedicalExames.Models;

namespace HospMedicalExames.Repository
{
    public interface IConsultaRepository
    {
        ConsultaModel ListarPorId(int id);
        List<ConsultaModel> BuscarTodos();
        List<ConsultaModel> BuscarPorPacienteId(int pacienteId);

        ConsultaModel Adicionar(ConsultaModel consulta);
        ConsultaModel Atualizar(ConsultaModel consulta);

        bool DataHoraConflitante(ConsultaModel consulta);

        bool Apagar(int id);

    }
}
