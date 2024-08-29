namespace ProjetoTemplate.Domain.Models
{
    public class Profile : BaseModel
    {
        public string Description { get; set; }
        public bool AccessAllModules { get; set; }
        public bool IsAdmin { get; set; }
        public bool Status { get; set; }
        public List<ProfileAccess> ProfileAccess { get; set; }
    }
}
