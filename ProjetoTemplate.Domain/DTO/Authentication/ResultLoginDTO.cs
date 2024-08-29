namespace ProjetoTemplate.Domain.DTO.Authentication
{
    public class ResultLoginDTO
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
