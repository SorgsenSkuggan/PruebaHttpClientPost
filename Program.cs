/*
 (1) Git -> Abrir uno

(2) Codigo Peticion POST
curl -X 'POST' \
  'https://petstore.swagger.io/v2/pet' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 0,
  "name": "doggie"
}'

(3) Commit

Limpieza
Gestión de los recursos finitos
Velocidad

Enviar a jorge.hontoria@tipesoft.com
 */
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

static class HttpResponseMessageExtensions{
    internal static void WriteRequestToConsole(this HttpResponseMessage respuesta){
        if (respuesta is null) {
            return;
        }

        var request = respuesta.RequestMessage;
        Console.Write($"{request?.Method}");
        Console.Write($"{request?.RequestUri}");
        Console.WriteLine($"HTTP/{request?.Version}");
    }
}

public class HttpClientPost{
    public class Pet{
        public int? id { get; set; }
        public string? name { get; set; }

        public Pet(int? id, string? name){
            this.id = id;
            this.name = name;
        }
    }

    private static HttpClient clientePet = new(){
        BaseAddress = new Uri("https://petstore.swagger.io/v2/pet/"),
    };

    static async Task PostAsJsonAsync(HttpClient httpClient) {
        Pet pet1 = new Pet(0, "doggie");
        using HttpResponseMessage respuesta = await httpClient.PostAsJsonAsync(
            "addPet", pet1); //Instancia de Pet como JSON para la solicitud POST a la dirección del cliente.
            
        try {
            //Si la llamada es correcta continúa, sino devuelve excepción capturada en bloque catch.
            respuesta.EnsureSuccessStatusCode().WriteRequestToConsole();
            
            var pet = await respuesta.Content.ReadFromJsonAsync<Pet>(); //Deserialización
            Console.WriteLine($"{pet}\n");
        }catch (HttpRequestException e) {
            Console.WriteLine(e.Message +": "+ respuesta.StatusCode);
        }
    }

    static async Task Main() {
        await PostAsJsonAsync(clientePet);
    }
}