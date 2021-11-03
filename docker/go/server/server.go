package main

import (
	"fmt"
	"io"
	"net/http"
)

func main() {
	fmt.Println("empezando")
	http.HandleFunc("/", func(rw http.ResponseWriter, req *http.Request) {
		fmt.Println("Recibi un request")
		io.WriteString(rw, "Hola")
	})
	http.ListenAndServe(":80", nil)
}
