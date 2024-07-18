using HospMedicalExames.Models;
using HospMedicalExames.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HospMedicalExames.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteRepository _pacienteRepository;
        public PacienteController(IPacienteRepository PacienteRepository)
        {
            _pacienteRepository = PacienteRepository;
        }
        public IActionResult Index()
        {
            List<PacienteModel> paciente = _pacienteRepository.BuscarTodos();
            return View(paciente);
        }
        public IActionResult Criar()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            PacienteModel paciente = _pacienteRepository.ListarPorId(id);
            return View(paciente);
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            PacienteModel paciente = _pacienteRepository.ListarPorId(id);
            return View(paciente);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _pacienteRepository.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Paciente apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Não conseguimos apagar o paciente";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos apagar o paciente, detalhes do erro {erro.Message}";
                return RedirectToAction("Index");

            }
        }
        [HttpPost]
        public IActionResult Criar(PacienteModel paciente)
        {
            try
            {
                if (!_pacienteRepository.CPFUnico(paciente.CPF))
                {
                    ModelState.AddModelError("CPF", "já está cadastrado.");
                }
                if (ModelState.IsValid)
                {
                    _pacienteRepository.Adicionar(paciente);
                    TempData["MensagemSucesso"] = "Paciente cadastrado com sucesso";
                    return RedirectToAction("Index");
                }
                return View(paciente);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Paciente não cadastrado, tente novamente. Detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Alterar(PacienteModel paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _pacienteRepository.Atualizar(paciente);
                    TempData["MensagemSucesso"] = "Paciente alterado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", paciente);

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Não conseguimos atualizar os dados do paciente, tente novamente. Detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
