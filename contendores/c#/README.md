# Ejercicio: comunicación entre lider y trabajadores

## Objectivo
Tener un sistema de docker compose local que tenga lider y trabajadores

### Flujo general
Los trabajadores se registran con el lider al comenzar su ejecución.
El lider guarda los ips de cada trabajador para poderse comunicar con ellos en
el futuro.

El lider expone una ruta que inicia el procesamiento del archivo. El lider
distribuye trabajo a todos los trabajadores. Espera los resultados de todos.
Une los resultados de todos para producir una sola respuesta.
Responde al usuario con la respuesta.


### Endpoints del Lider
* /register
  * GET
  * Llamado por los trabajadores para dar a conocer su dirección ip.
  * El lider debe guardar las direcciones ip de todos los trabajadores
    que se registren.

* /start
  * GET
  * Llamado por el usuario del sistema
  * Expuesto publicamente
  * El lider debe comunicarse con todos los trabajadores registrados
  * Les debe decir cuál es el rango del archivo que deben procesar
  * Junta las repuestas de todos los trabajadores
  * responde al usuario con la respuesta al problema

### Endpoint del worker
* /work
  * POST
  * Llamado por el lider
  * Recibe parametros que le permiten saber qué posición del archivo le toca
    leer
  * Responde al lider con información de frecuencia de cada letra en el rango
    asignado


## Tips de C#

### ¿Cómo expongo rutas?
Sólo es necesario agregar métodos a la clase `MyController` de cada sistema.
Estos métodos necesitan estar anotados con `[Route("ruta")]`.
```c#
    [ApiController]
    public class MyController : ControllerBase
    {
        [HttpGet]
        [Route("register")]
        // Acepta requests en ip:8080/register
        public async Task<String> Register() {
        }
    }

```

### ¿Cómo defino si es POST o GET?
Sólo necesitas usar la anotación `[HttpGet]` o `[HttpPost]`.
```c#
        [HttpGet]
        [Route("get")]
        public async Task<String> Get() {
        }

        [HttpPost]
        [Route("post")]
        public async Task<String> Post(ReqBody rb) {
        }

        public struct ReqBody {
          public Id { get; set; }
        }
```

### ¿Cómo respondo con datos desde una ruta?

De forma automática asp .net serializa y deserializa objetos en formato JSON.
```c#
        [HttpGet]
        [Route("get")]
        public async Task<ReqBody> Get() {
          var rb = new ReqBody();
          rb.Id = 100;
          return rb;
        }

        public struct ReqBody {
          public Id { get; set; }
        }
```

Si mandamos un GET, el servidor responderá con `{"Id": 100}`

Se pueden pasar arreglos, diccionarios, etc.

### ¿Cómo recibo datos en una ruta?
Podemos mandar un post con `Content-Type: application/json` y cuerpo `{"Id": 1234}`.
C# va a convertirlo a un objecto fácil de parsear para nosotros.
```c#
        [HttpPost]
        [Route("post")]
        public async Task<String> Post(ReqBody rb) {
            Console.WriteLine(rb.Id);
        }

        public struct ReqBody {
          public Id { get; set; }
        }
```

### ¿Cómo mando un GET desde el servicio?
Hay varias formas de hacerlo pero usando `GetJsonAsync` automáticamente parsea
JSON a un objeto de c#.

```c#
    public struct ReqBody {
      public Id { get; set; }
    }

    [HttpGet]
    public async Task<int> Get() {
        using (HttpClient client = new HttpClient())
        {
            ReqBody rb = await client.GetJsonAsync<ReqBody>(new Uri(url));
            return rb.Id;
        }
    }
```

### ¿Cómo mando un POST desde el servicio?
También existen varias formas pero `PostAsJsonAsync` automáticamente convierte
un objecto de c# a JSON.
```c#
    public struct ReqBody {
      public Id { get; set; }
    }

    [HttpGet]
    public async Task<String> Get() {
        using (HttpClient client = new HttpClient())
        {
            ReqBody rb = new ReqBody();
            rb.Id = 100;
            await client.PostAsJsonAsync(new Uri("http://google.com/blah"), rb);
        }
        return "ok";
    }
```

Si la respuesda del servicio es también JSON la puedes parsear a un objecto de c# con:

```c#
    var response = await client.PostAsJsonAsync(uri, wr);
    ReqBody rb = await response.Content.ReadFromJsonAsync<ReqBody>();
```


### ¿Cómo obtengo la dirección IP de quien llama una ruta?

```c#
    [HttpGet]
    public async Task<String> Get() {
        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
        return ip;
    }
```

Algo a notar es que docker va a regresar las ips en formato ipv6. Se verán algo así: 
```
::ffff:172.18.0.3
```

### ¿Cómo mando requests a una direccion ipv6?
Para mandar requests a una direccion de ipv6 es necesario rodearla con `[]`. Por ejemplo:
```
http:://[::ffff:172.18.0.3]:8080/work
```

### ¿Cómo persisto datos en el controlador?
Cada request usa una nueva instancia del controlador en cada request. Para poder
persistir datos entre requests se puede usar una variable estática en la clase.
Esta será la misma en todos los requests.
```c#
    public class MyController : ControllerBase
    {
        public static int count = 0;

        [HttpGet]
        [Route("add")]
        // /register
        public async Task<int> Add()
        {
          MyController.count++;
          return MyController.count;
        }
   }
```


### ¿Cómo espero la respuesta de varios requests de forma simultanea?
Podemos usar `Task.WhenAll` para esperar el resultado de varios Tasks simultaneos.

```c#
      List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();
      foreach (var ip in ips) {
          using (var client = new HttpClient())
          {
            tasks.Add(client.PostAsJsonAsync(new Uri(ip), message));
          } 
      }
      var results = await Task.WhenAll(tasks);
      foreach (var response in results) {
        var resp = await response.Content.ReadFromJsonAsync<ReqBody>();
        Console.WriteLine("client response: " resp.Id);
      }
```

