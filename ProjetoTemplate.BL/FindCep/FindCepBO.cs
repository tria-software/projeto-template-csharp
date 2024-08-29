using Newtonsoft.Json;
using ProjetoTemplate.Domain.DTO.Address;
using System.Net;

namespace ProjetoTemplate.BL.FindCep
{
    public class FindCepBO : IFindCepBO
    {
        public async Task<AddressZipCodeDTO> Find(string zipcode)
        {
            try
            {
                if (string.IsNullOrEmpty(zipcode))
                {
                    return null;
                }

                // Remover caracteres de formatação
                zipcode = zipcode.Replace(".", "").Replace("-", "");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://viacep.com.br/ws/" + zipcode + "/json/");
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<AddressZipCodeDTO>(responseString);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // A melhor prática é logar a exceção e não apenas lançar
                throw;
            }
        }
    }
}
