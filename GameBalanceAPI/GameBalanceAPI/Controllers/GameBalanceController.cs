using GameBalanceAPI;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameBalanceController : ControllerBase
    {
        private static List<GameBalance> games = new List<GameBalance>
        {
            new GameBalance { GameId = 1, Name = "Battlefield 2042", Genre = "First-person shooter", SizeGB = 150 },
            new GameBalance { GameId = 2, Name = "Red Dead Redemption 2", Genre = "Action-adventure", SizeGB = 120 },
            new GameBalance { GameId = 3, Name = "Minecraft (Java Edition)", Genre = "Sandbox", SizeGB = 2 },
            new GameBalance { GameId = 4, Name = "Horizon Forbidden West", Genre = "Action RPG, Open-world", SizeGB = 100 },
            new GameBalance { GameId = 5, Name = "FIFA 23", Genre = "Sports, Simulation", SizeGB = 50 },
        };

        private readonly ILogger<GameBalanceController> _logger;

        public GameBalanceController(ILogger<GameBalanceController> logger)
        {
            _logger = logger;
        }

        // Endpoint para sugerir um jogo com base no espaço livre
        [HttpGet("Sugerir/{freeSpaceGB}", Name = "GetGameSuggestion")]
        public IActionResult GetGameSuggestion(double freeSpaceGB)
        {
            var suitableGames = games.Where(g => g.SizeGB <= freeSpaceGB).ToList();
            if (suitableGames.Count == 0)
            {
                return NotFound("Nenhum jogo disponível para o espaço informado.");
            }

            Random rand = new Random();
            GameBalance randomGame = suitableGames[rand.Next(suitableGames.Count)];

            return new JsonResult(randomGame);
        }

        // Endpoint para listar todos os jogos
        [HttpGet]
        public IActionResult ListAll()
        {
            return Ok(games);
        }

        // Endpoint para criar um novo jogo
        [HttpPost]
        public IActionResult Post([FromBody] GameBalance newGame)
        {
            if (newGame == null || string.IsNullOrEmpty(newGame.Name))
            {
                return BadRequest("Dados inválidos.");
            }

            // Define um novo GameId baseado no maior ID atual
            int newId = games.Max(g => g.GameId) + 1;
            newGame.GameId = newId;
            games.Add(newGame);

            return Ok(newGame);
        }

        // Endpoint para atualizar um jogo existente
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] GameBalance updatedGame)
        {
            if (updatedGame == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var existingGame = games.FirstOrDefault(g => g.GameId == id);
            if (existingGame == null)
            {
                return NotFound("Jogo não encontrado.");
            }

            // Atualiza os dados do jogo existente
            existingGame.Name = updatedGame.Name;
            existingGame.Genre = updatedGame.Genre;
            existingGame.SizeGB = updatedGame.SizeGB;

            return Ok(existingGame);
        }

        // Endpoint para deletar um jogo
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var game = games.FirstOrDefault(g => g.GameId == id);
            if (game == null)
            {
                return NotFound("Jogo não encontrado.");
            }

            games.Remove(game);
            return NoContent();  // Retorna 204 se a remoção for bem-sucedida
        }
    }
}