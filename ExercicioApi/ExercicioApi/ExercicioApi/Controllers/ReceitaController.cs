using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ExercicioApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceitaController : ControllerBase
    {
        private static List<Receita> receitas = new List<Receita>
        {
            new Receita { ReceitaId = 1, NomeReceita = "Salada de Frango", TipoReceita = "Fit", Ingredientes = "Frango e Alface" },
            new Receita { ReceitaId = 2, NomeReceita = "Bolo de Chocolate", TipoReceita = "Normal", Ingredientes = "Farinha e Chocolate" },
            new Receita { ReceitaId = 3, NomeReceita = "Smoothie Verde", TipoReceita = "Fit", Ingredientes = "Couve e Limão" },
            new Receita { ReceitaId = 4, NomeReceita = "Pizza de Calabresa", TipoReceita = "Normal", Ingredientes = "Massa e Calabresa" },
            new Receita { ReceitaId = 5, NomeReceita = "Suco de Whatsapp", TipoReceita = "Whatsapp", Ingredientes = "Whatsapp e Smartphone"},
            new Receita { ReceitaId = 6, NomeReceita = "Salada de Whatsapp", TipoReceita = "Whatsapp", Ingredientes = "Whatsapp e Frutas Whatsapp"}
        };

        private readonly ILogger<ReceitaController> _logger;

        public ReceitaController(ILogger<ReceitaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{imc}", Name = "GetReceita")]
        public IActionResult Get(double imc)
        {
            Random rand = new Random();

            if (imc < 25)
            {
                var receitasNormal = receitas.Where(w => w.TipoReceita == "Normal").ToList();
                Receita receitaAleatoria = receitasNormal[rand.Next(receitasNormal.Count)];
                return new JsonResult(receitaAleatoria);
            }
            else if (imc < 50)
            {
                var receitasFit = receitas.Where(w => w.TipoReceita == "Fit").ToList();
                Receita receitaAleatoria = receitasFit[rand.Next(receitasFit.Count)];
                return new JsonResult(receitaAleatoria);
            } 
            else
            {
                var receitasWhatsapp = receitas.Where(w => w.TipoReceita == "Whatsapp").ToList();
                Receita receitaAleatoria = receitasWhatsapp[rand.Next(receitasWhatsapp.Count)];
                return new JsonResult(receitaAleatoria);
            }
        }

        [HttpGet]

        public IActionResult ListarTodas()
        {
            return Ok(receitas);
        }

        [HttpPost]

        public IActionResult Post([FromBody] Receita novaReiceita)
        {
            if (novaReiceita == null)
            {
                return BadRequest("Dados Inválidos");
            }

            int novoId = receitas.Max(r => r.ReceitaId) + 1;
            novaReiceita.ReceitaId = novoId;
            receitas.Add(novaReiceita);

            return Ok(novaReiceita);
        }

        [HttpPut("{id}")]

        public IActionResult Put(int id, [FromBody] Receita receitaAtualizada)
        {
            if (receitaAtualizada == null)
            {
                return BadRequest("Dados Inválidos");
            }

            var receitaExistente = receitas.FirstOrDefault(r => r.ReceitaId == id);
            if (receitaExistente == null)
            {
                return NotFound("Receita não encontrada");
            }

            receitaExistente.NomeReceita = receitaAtualizada.NomeReceita;
            receitaExistente.TipoReceita = receitaAtualizada.TipoReceita;
            receitaExistente.Ingredientes = receitaAtualizada.Ingredientes;

            return Ok(receitaExistente);
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            var receita = receitas.FirstOrDefault(r => r.ReceitaId == id);
            if (receita == null)
            {
                return NotFound("Receita não encontrada");
            }

            receitas.Remove(receita);
            return NoContent();
        }
    }
}
