
using HospMedicalExames.Models;
using HospMedicalExames.Data;

namespace HospMedicalExames.Repository
{
    public class TipoExameRepository : ITipoExameRepository
    {
        private readonly BancContext _bancContext;

        public TipoExameRepository(BancContext bancContext)
        {
            _bancContext = bancContext;
        }

        public List<TipoExameModel> BuscarTodosExames()
        {
            return _bancContext.TipoExames.ToList();
        }

        public TipoExameModel ListarPorId(int id)
        {
#pragma warning disable CS8603 // Possível retorno de referência nula.
            return _bancContext.TipoExames.FirstOrDefault(x => x.Id == id);
#pragma warning restore CS8603 // Possível retorno de referência nula.
        }

        public TipoExameModel Adicionar(TipoExameModel tipoExame)
        {
            _bancContext.TipoExames.Add(tipoExame);
            _bancContext.SaveChanges();
            return tipoExame;

        }
        public TipoExameModel Atualizar(TipoExameModel tipoExame)
        {
            TipoExameModel tipoExameDB = ListarPorId(tipoExame.Id);
            if (tipoExameDB == null) throw new Exception("Houve um erro na atualização do exame");

            tipoExameDB.Nome = tipoExame.Nome;
            tipoExameDB.Descricao = tipoExame.Descricao;

            _bancContext.TipoExames.Update(tipoExameDB);
            _bancContext.SaveChanges();

            return tipoExameDB;

        }

        public bool Apagar(int id)
        {
            TipoExameModel tipoExameDB = ListarPorId(id);
            if (tipoExameDB == null) throw new Exception("Houve um erro ao deletar o tipo deexame");

            _bancContext.TipoExames.Remove(tipoExameDB);
            _bancContext.SaveChanges();

            return true;
        }




    }
}
