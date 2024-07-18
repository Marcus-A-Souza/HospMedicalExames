using HospMedicalExames.Models;
using HospMedicalExames.Repository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospMedicalExames.Controllers
{
    public class ExameController : Controller
    {
        private readonly IExameRepository _exameRepository;
        private readonly ITipoExameRepository _tipoExameRepository;

        public ExameController(IExameRepository exameRepository, ITipoExameRepository tipoExameRepository)
        {
            _exameRepository = exameRepository;
            _tipoExameRepository = tipoExameRepository;
        }

        public IActionResult Index()
        {
            List<ExameModel> exames = _exameRepository.BuscarTodos();
            return View(exames);
        }
        
        public IActionResult Criar()
        {
            List<TipoExameModel> tipoExames = _tipoExameRepository.BuscarTodosExames();
            ViewBag.tipoExame = tipoExames ?? new List<TipoExameModel>();
            return View();
        }

        public IActionResult Editar(int id)
        {
            ExameModel exame = _exameRepository.ListarPorId(id);
            List<TipoExameModel> tipoExames = _tipoExameRepository.BuscarTodosExames();
            ViewBag.tipoExame = tipoExames;
            return View(exame);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            ExameModel exame = _exameRepository.ListarPorId(id);
            return View(exame);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _exameRepository.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Exame apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Não foi possível apagar o exame";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possível apagar o exame. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(ExameModel exame)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _exameRepository.Adicionar(exame);
                    TempData["MensagemSucesso"] = "Exame cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                List<TipoExameModel> tipoExames = _tipoExameRepository.BuscarTodosExames();
                ViewBag.tipoExame = tipoExames ?? new List<TipoExameModel>();
                return View(exame);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possível cadastrar o exame. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(ExameModel exame)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _exameRepository.Atualizar(exame);
                    TempData["MensagemSucesso"] = "Exame atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View("Editar", exame);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possível atualizar o exame. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
