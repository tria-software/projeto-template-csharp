using ProjetoTemplate.Domain.DTO.Address;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.FindCep
{
    public interface IFindCepBO
    {
        Task<AddressZipCodeDTO> Find(string zipcode);
    }
}
