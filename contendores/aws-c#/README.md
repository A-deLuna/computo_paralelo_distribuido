# Pasos para hacer deploy en aws

1. Tener el comando aws installado
  Es fácil de instalar de https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html

1. Configurar AWS localmente
  ```
  aws configure
  key id
  secret key
  region us-west-1
  text
  ```

  Si no tienen su key id o secret key les puedo generar uno, sólo avisenme.

  Para verificar que todo funcionó correctamente corran `aws s3 ls` en la linea de comandos.
  Deberían de ver algo como: 
  ```
  $ aws s3 ls
  2020-10-12 15:09:09 aws-sam-cli-managed-default-samclisourcebucket-o0djc4x9m3ls
  2021-11-07 21:06:18 computo-paralelo-distribuido-cetys-2021
  2020-10-16 12:22:22 saas-identity-with-cognito-iden-destinationbucket-7qwc2aa47lg
  2020-10-16 12:21:43 saas-identity-with-cognito-identit-artifactbucket-14bdz4coc6988
  2020-10-14 09:25:21 serverless-test-dev-serverlessdeploymentbucket-323ggzltibv2
  ```

1. Configurar docker para conectarse al repositorio de imágenes de AWS
  Vamos a subir las imágenes que creamos localmente a AWS. Para poder hacer esto
  tenemos que configurar docker para darle permisos de subir cosas a AWS.
  ```
  aws ecr get-login-password --region us-west-1 | docker login --username AWS --password-stdin 147063123581.dkr.ecr.us-west-1.amazonaws.com
  ```

1. Seleccionar 

1. Build y push de la imagen a AWS
  Antes de empezar computadoras necesitamos subir nuestra imagen más reciente a AWS
  ```
  docker compose build
  docker compose push
  ```

1. Configurar docker para empezar servicios en AWS
  ```
  docker context create ecs aws
  ```

1. Cambiarse al contexto do docker que acabamos de crear
  En el paso anterior creamos un contexto de docker nuevo que se encarga de interpretar
  el Dockerfile.

  NOTA: `docker compose build` deja de funcionar en modo aws. Tampoco podemos empezar servicios localmente.
  Si queremos compilar o probar cambios localmente debemos cambiar al contexto default con `docker context use default`.

1. Re-nombrar la carpeta donde está su código a un nombre único por equipo
  Docker creará los recursos en aws usando el nombre de la carpeta como el nombre del proyecto. Para evitar
  conflictos donde un equipo accidentalmente modifica recursos de otro, por favor asegúrense que el nombre de la
  carpeta donde se encuentra su archivo `docker-compose.yaml` tiene un nombre único que se refiera a su equipo.

# Crear los servicios

Para iniciar debemos correr `docker compose up`. Tardará algunos minutos configurando todos los recursos en AWS.

```
docker compose up
WARNING services.build: unsupported attribute
WARNING services.build: unsupported attribute
[+] Running 20/20
 ⠿ aws-c                        CreateComplete         305.1s
 ⠿ LeaderTaskExecutionRole      CreateComplete          14.0s
 ⠿ LeaderTCP8080TargetGroup     CreateComplete           1.0s
 ⠿ WorkerTaskExecutionRole      CreateComplete          14.0s
 ⠿ LeaderTaskRole               CreateComplete          14.0s
 ⠿ CloudMap                     CreateComplete          47.1s
 ⠿ DefaultNetwork               CreateComplete           6.0s
 ⠿ LogGroup                     CreateComplete           2.3s
 ⠿ WorkerTaskRole               CreateComplete          14.0s
 ⠿ Cluster                      CreateComplete           7.0s
 ⠿ LoadBalancer                 CreateComplete         153.1s
 ⠿ Default8080Ingress           CreateComplete           1.0s
 ⠿ DefaultNetworkIngress        CreateComplete           1.0s
 ⠿ WorkerTaskDefinition         CreateComplete           3.0s
 ⠿ LeaderTaskDefinition         CreateComplete           3.0s
 ⠿ LeaderServiceDiscoveryEntry  CreateComplete           4.0s
 ⠿ WorkerServiceDiscoveryEntry  CreateComplete           2.0s
 ⠿ LeaderTCP8080Listener        CreateComplete           4.0s
 ⠿ LeaderService                CreateComplete          67.0s
 ⠿ WorkerService                CreateComplete          67.0s

```

# Encontrar el ip a donde mandar requests
El comando anterior configuró muchas cosas en AWS para que nuestro
servicio esté disponible. Para comunicarnos con él servicio necesitamos
identificar la dirección a la que tenemos que mandar requests.

Podemos usar el comando `docker compose ps` y nos debe dar un resultado así:
```
➜  ~/clase/contendores/aws-c# git:(main) ✗ docker compose ps
NAME                                          SERVICE  STATUS   PORTS
task/aws-c/1b17c254bd7c4745bea5d1eedcdae9b0   worker   Running
task/aws-c/22c1d529615041028d6fad987e62fcf6   worker   Running
task/aws-c/40e8186413a8402e8183a5a0f0bd92b5   leader   Running  aws-c-LoadB-ZZC08GFR08FQ-0cb9b82a9a81dd31.elb.us-west-1.amazonaws.com:8080->8080/tcp
task/aws-c/66622e94d43b45e59d11d53d642b45a5   worker   Running
task/aws-c/acb7acd2d023412f9ef1587c825387ff   worker   Running
```

El resultado de sus comandos será diferente para cada equipo.

Podemos ver que nos da la dirección donde está escuchando el leader. Si copiamos lo que salga en PORTS y recordamos que tenemos que agregar `/start` al final del url podemos abrir el link `aws-c-LoadB-ZZC08GFR08FQ-0cb9b82a9a81dd31.elb.us-west-1.amazonaws.com:8080/start`.

# Inspeccionar logs de ejecución
Si queremos monitorear los mensajes que imprime nuesto programa podemos correr `docker compose logs`.

También es posible ver los logs en el navegador a través de una [página de AWS](https://us-west-1.console.aws.amazon.com/cloudwatch/home?region=us-west-1#logsV2:log-groups).

# Troubleshooting
