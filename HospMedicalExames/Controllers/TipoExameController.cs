using HospMedicalExames.Models;
using HospMedicalExames.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HospMedicalExames.Controllers
{
    public class TipoExameController : Controller
    {
        private readonly ITipoExameRepository _tipoExameRepository;

        public TipoExameController(ITipoExameRepository tipoExameRepository)
        {
            _tipoExameRepository = tipoExameRepository;
        }

        public IActionResult Index()
        {
            List<TipoExameModel> tipoExame = _tipoExameRepository.BuscarTodosExames();
            return View(tipoExame);
        }
        public IActionResult Criar()
        {

            return View();
        }
        public IActionResult Editar(int id)
        {
            TipoExameModel tipoExame = _tipoExameRepository.ListarPorId(id);
            return View(tipoExame);
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            TipoExameModel tipoExame = _tipoExameRepository.ListarPorId(id);
            return View(tipoExame);
        }
        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _tipoExameRepository.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Tipo de exame excluido com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = " Houve um erro ao apagar o exame";
                }
                return RedirectToAction("Index");

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos apagar o tipo de exame, detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Criar(TipoExameModel NomeTipoExame)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _tipoExameRepository.Adicionar(NomeTipoExame);
                    TempData["MensagemSucesso"] = "tipo de exame cadastrado com sucesso";
                    return RedirectToAction("Index");
                }
                return View(NomeTipoExame);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Exame não cadastrado, tente novamente. Detalhe do erro {erro.Message}";
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public IActionResult Alterar(TipoExameModel tipoExame)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _tipoExameRepository.Atualizar(tipoExame);
                    TempData["MensagemSucesso"] = "Tipo de exame alterado com sucesso";
                    return RedirectToAction("Index");

                }
                return View("Editar", tipoExame);

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar o exame, tente novamente. Detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");

            }
        }
    }
}
