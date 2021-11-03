# syntax=docker/dockerfile:1
FROM golang:1.17-alpine
WORKDIR /app
COPY . .
RUN go build main.go
ENTRYPOINT ./main
