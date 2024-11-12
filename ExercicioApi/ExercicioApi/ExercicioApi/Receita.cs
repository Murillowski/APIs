namespace ExercicioApi
{
    public class Receita
    {
        public int ReceitaId { get; set; }

        public string TipoReceita { get; set; } = string.Empty;

        public string NomeReceita { get; set; } = string.Empty;

        public string Ingredientes { get; set; } = string.Empty;
    }
}
