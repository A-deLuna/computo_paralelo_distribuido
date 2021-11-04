Para buildear y taggear la imagen
```
docker compose build
```

para subir la imagen a ecr
```
docker compose push
```

para levantar servicios
```
docker compose up
```

para apagar servicios
```
docker compose down
```

para hacer log in a ecr para subir imagenes
```
aws ecr get-login-password --region us-west-1 | docker login --username AWS --password-stdin 147063123581.dkr.ecr.us-west-1.amazonaws.com
```

para conectar docker compose con ecs
```
docker context create ecs aws
```

para cambiar de contexto
```
docker context use aws
docker context use default
```
