using Newtonsoft.Json;

public class EmpresaService
{
    private readonly HttpClient _httpClient;

    public EmpresaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Empresa>> GetEmpresas()
    {
        var response = await _httpClient.GetAsync("https://nocodebackend.azurewebsites.net/api/v1/dataset/6426ead776204041752bec02/611ed902fd5915f2ae005dbb?apiKey=6426e9ef76204041752bebff");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(json);
        var empresas = new List<Empresa>();
        foreach (var item in data.data)
        {
            empresas.Add(new Empresa
            {
                Nome = item.Nome,
                Site = item.Site,
                Segmento = item.Segmento.ToString()

            });
        }
        return empresas;
    }
}