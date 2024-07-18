
using HospMedicalExames.Data;
using HospMedicalExames.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace HospMedicalExames.Repository
{
    public class ExameRepository : IExameRepository
    {
        private readonly BancContext _bancContext;

        public ExameRepository(BancContext bancoContext)
        {
            _bancContext = bancoContext;
        }
        public List<ExameModel> BuscarTodos()
        {
            return _bancContext.Exames.ToList();
        }
        public ExameModel ListarPorId(int id)
        {
#pragma warning disable CS8603 // Possível retorno de referência nula.
            return _bancContext.Exames.FirstOrDefault(e => e.Id == id);
#pragma warning restore CS8603 // Possível retorno de referência nula.
        }
        public List<ExameModel> BuscarPorTipoExame(int tipoExameId)
        {
            return _bancContext.Exames.Where(e => e.TipoExameId == tipoExameId).ToList();
        }
        public List<ExameModel> BuscarPorNome(string nome)
        {
            return _bancContext.Exames.Where(e => e.NomeExame.Contains(nome)).ToList();
        }

        public ExameModel Adicionar(ExameModel exame)
        {

            _bancContext.Exames.Add(exame);
            _bancContext.SaveChanges();
            return exame;
        }
        public ExameModel Atualizar(ExameModel exame)
        {
            ExameModel exameDB = ListarPorId(exame.Id);
            if (exameDB == null) throw new Exception("Houve um erro na atualização do exame");

            exameDB.TipoExameId = exame.TipoExameId;
            exameDB.NomeExame = exame.NomeExame;
            exameDB.Observacoes = exame.Observacoes;

            _bancContext.Exames.Update(exameDB);
            _bancContext.SaveChanges();

            return exameDB;
        }

        public bool Apagar(int id)
        {
            ExameModel exameDB = ListarPorId(id);
            if (exameDB == null) throw new Exception("Houve um erro ao deletar o exame");

            _bancContext.Exames.Remove(exameDB);
            _bancContext.SaveChanges();

            return true;
        }
    }
}

