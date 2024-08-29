using System;
using System.Collections.Generic;

namespace ProjetoTemplate.Domain.DTO.Profile
{
    public class ProfileDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public bool AccessAllModules { get; set; }
        public List<string> Modules { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
