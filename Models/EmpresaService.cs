using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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

            var segmento = JObject.Parse(item.Segmento.ToString());
            var segmentoId = (string)segmento["$oid"];
            var segmentoResponse = await _httpClient.GetAsync($"https://nocodebackend.azurewebsites.net/api/v1/dataset/6426ead776204041752bec02/611edbd7fd5915f2ae005dc2/{segmentoId}?apiKey=6426e9ef76204041752bebff");
            Console.WriteLine(segmentoResponse);
            segmentoResponse.EnsureSuccessStatusCode();
            var segmentoJson = await segmentoResponse.Content.ReadAsStringAsync();
            dynamic segmentoData = JsonConvert.DeserializeObject(segmentoJson);
            var nomeSegmento = segmentoData.Nome.ToString();

            empresas.Add(new Empresa
            {
                Nome = item.Nome,
                Site = item.Site,
                Segmento = nomeSegmento
            });
            Console.WriteLine($"Empresa adicionada: {item.Nome} - {item.Site} - {nomeSegmento}");

        }
        return empresas;
    }

}
