using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TesteApiSerie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private static readonly Dictionary<string, List<string>> seriesDatabase = new()
        {
            { "action", new List<string> { "Fallout", "GOT" } },
            { "comedy", new List<string> { "The Office", "B99" } },
            { "drama", new List<string> { "Anne com E", "Dark" } }
        };

        // Método para obter recomendações de séries por gênero
        [HttpGet("{genre}")]
        public IActionResult GetRecommendations(string genre)
        {
            if (seriesDatabase.TryGetValue(genre.ToLower(), out var recommendations))
            {
                return Ok(recommendations);
            }
            return NotFound("Gênero não encontrado.");
        }

        // Método para obter uma série específica
        [HttpGet("{genre}/{name}")]
        public IActionResult GetSeries(string genre, string name)
        {
            if (seriesDatabase.TryGetValue(genre.ToLower(), out var recommendations))
            {
                var series = recommendations.FirstOrDefault(s => s.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (series != null)
                {
                    return Ok(series);
                }
            }
            return NotFound("Série não encontrada.");
        }

        // Método para atualizar o nome de uma série existente
        [HttpPut("{genre}/{oldName}")]
        public IActionResult UpdateSeries(string genre, string oldName, [FromBody] string newName)
        {
            if (seriesDatabase.TryGetValue(genre.ToLower(), out var recommendations))
            {
                var index = recommendations.FindIndex(s => s.Equals(oldName, StringComparison.OrdinalIgnoreCase));
                if (index != -1)
                {
                    recommendations[index] = newName; // Atualiza o nome da série
                    return Ok("Série atualizada com sucesso.");
                }
            }
            return NotFound("Série não encontrada.");
        }

        // Método para deletar uma série
        [HttpDelete("{genre}/{name}")]
        public IActionResult DeleteSeries(string genre, string name)
        {
            if (seriesDatabase.TryGetValue(genre.ToLower(), out var recommendations))
            {
                var index = recommendations.FindIndex(s => s.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (index != -1)
                {
                    recommendations.RemoveAt(index); // Remove a série
                    return Ok("Série deletada com sucesso.");
                }
            }
            return NotFound("Série não encontrada.");
        }
    }
}
