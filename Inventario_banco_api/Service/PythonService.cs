namespace Inventario_banco_api.Service;
using System.Net.Http.Json;

public class PythonService
{
    private readonly HttpClient _http;

    public PythonService(HttpClient http)
    {
        _http = http;
    }

    // Análisis
    public async Task<object> ObtenerAnalisis()
    {
        var response = await _http.GetFromJsonAsync<object>(
            "http://127.0.0.1:8000/api/analisis"
        );
        return response;
    }

    // Exportar PDF — devuelve los bytes directamente
    public async Task<byte[]> ExportarPdf()
    {
        var response = await _http.GetAsync(
            "http://127.0.0.1:8000/api/exportar-pdf"
        );
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<byte[]> ExportarExcel()
    {
        var response = await _http.GetAsync(
            "http://127.0.0.1:8000/api/exportar-excel"
        );
        return await response.Content.ReadAsByteArrayAsync();
    }
}