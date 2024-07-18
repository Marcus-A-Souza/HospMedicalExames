using HospMedicalExames.Models;
using HospMedicalExames.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospMedicalExames.Controllers
{
    public class ConsultaController : Controller
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ITipoExameRepository _tipoExameRepository;
        private readonly IExameRepository _exameRepository;
        public ConsultaController
        (
           IConsultaRepository consultaRepository,
           IPacienteRepository pacienteRepository,
           ITipoExameRepository tipoExameRepository,
           IExameRepository exameRepository
        )
        {
            _consultaRepository = consultaRepository;
            _pacienteRepository = pacienteRepository;
            _tipoExameRepository = tipoExameRepository;
            _exameRepository = exameRepository;
        }
        public IActionResult Index(string filtro)
        {
            try
            {
                List<ConsultaModel> consultas;
                if (!string.IsNullOrEmpty(filtro))
                {
                    var pacientes = _pacienteRepository.BuscarPorNomeCpf(filtro);

                    consultas = new List<ConsultaModel>();

                    foreach (var paciente in pacientes)
                    {
                        var consultasDoPaciente = _consultaRepository.BuscarPorPacienteId(paciente.Id);
                        consultas.AddRange(consultasDoPaciente);
                    }
                }
                else
                {
                    consultas = _consultaRepository.BuscarTodos();
                }

                return View(consultas);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { mensagemErro = ex.Message });
            }
        }
        public IActionResult Criar(int tipoExameId)
        {
            var Pacientes = _pacienteRepository.BuscarTodos();
            var tipoExame = _tipoExameRepository.BuscarTodosExames();
            var Exame = _exameRepository.BuscarPorTipoExame(tipoExameId);

            DateTime dataHoraAtual = DateTime.Now;
            string protocolo = $"{dataHoraAtual:ddMMyyyy-HHmm}";
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            string filtro = HttpContext.Request.Query["filtro"];
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.

            ViewBag.Pacientes = new SelectList(Pacientes ?? new List<PacienteModel>(), "Id", "Nome");
            ViewBag.TipoExame = new SelectList(tipoExame ?? new List<TipoExameModel>(), "Id", "Nome");
            ViewBag.Exames = new SelectList(Exame ?? new List<ExameModel>(), "Id", "Nome");
            ViewBag.protocolo = protocolo;
            ViewBag.Filtro = filtro;

            return View();
        }
        [HttpPost]
        public JsonResult ObterExamesPorTipoExame(int tipoExameId)
        {
            var exames = _exameRepository.ListarPorId(tipoExameId);
            return Json(exames);
        }

        [HttpPost]
        public IActionResult Criar(ConsultaModel consulta)
        {
            var pacientes = _pacienteRepository.BuscarTodos();
            var tipoExame = _tipoExameRepository.BuscarTodosExames();
            var exame = _exameRepository.BuscarTodos();

#pragma warning disable CS0168 // A variável foi declarada, mas nunca foi usada
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Pacientes = new SelectList(pacientes ?? new List<PacienteModel>(), "Id", "Nome", consulta?.PacienteId);
                    ViewBag.TipoExame = new SelectList(tipoExame ?? new List<TipoExameModel>(), "Id", "Nome", consulta?.TipoExameId);
                    ViewBag.Exame = new SelectList(exame ?? new List<ExameModel>(), "Id", "Nome", consulta?.ExameId);
                    return View(consulta);
                }
                var paciente = _pacienteRepository.BuscarPorId(consulta.PacienteId);
                if (paciente == null)
                {
                    TempData["MensagemErro"] = "Paciente não encontrado. Por favor, cadastre o paciente antes de agendar a consulta";
                    return RedirectToAction("CadastroPaciente", "Paciente", new { pacienteId = consulta.PacienteId });
                }
                if (!_consultaRepository.DataHoraConflitante(consulta))
                {
                    consulta.Protocolo = Guid.NewGuid().ToString();
                    _consultaRepository.Adicionar(consulta);
                    TempData["MensagemSucesso"] = "Consulta agendada com sucesso";

                    return RedirectToAction("Index", consulta);
                }
                else
                {
                    ModelState.AddModelError("DataHora", "Já existe uma consulta agendada para este horário");
                    ViewBag.Pacientes = new SelectList(pacientes ?? new List<PacienteModel>(), "Id", "Nome", consulta?.PacienteId);
                    ViewBag.TipoExame = new SelectList(tipoExame ?? new List<TipoExameModel>(), "Id", "Nome", consulta?.TipoExameId);
                    ViewBag.Exame = new SelectList(exame ?? new List<ExameModel>(), "Id", "Nome", consulta?.ExameId);

                    return View(consulta);
                }

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = "Consulta não agendada, tente novamente";
                return RedirectToAction("Criar");
            }
#pragma warning restore CS0168 // A variável foi declarada, mas nunca foi usada
        }
    }
}

