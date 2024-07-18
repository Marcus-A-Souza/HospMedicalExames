using HospMedicalExames.Models;


namespace HospMedicalExames.Repository
{
    public interface ITipoExameRepository
    {
        TipoExameModel ListarPorId(int id);
        List<TipoExameModel> BuscarTodosExames();
        TipoExameModel Atualizar(TipoExameModel tipoExame);
        TipoExameModel Adicionar(TipoExameModel tipoExame);

        bool Apagar(int id);

    }
}
