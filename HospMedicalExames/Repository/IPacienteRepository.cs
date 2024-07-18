using HospMedicalExames.Models;


namespace HospMedicalExames.Repository
{
    public interface IPacienteRepository
    {
        PacienteModel ListarPorId(int id);
        List<PacienteModel> BuscarTodos();
        PacienteModel BuscarPorId(int id);

        List<PacienteModel> BuscarPorNomeCpf(string filtro);
        PacienteModel Adicionar(PacienteModel paciente);
        PacienteModel Atualizar(PacienteModel paciente);
        bool Apagar(int id);
        bool CPFUnico(string CPF);
    }
}
