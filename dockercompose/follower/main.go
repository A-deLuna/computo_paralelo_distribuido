package main

import (
	"fmt"
	"log"
	"net/http"
)

func main() {
	fmt.Println("Empezando follower")
	_, err := http.Get("http://leader:8080")

	if err != nil {
		fmt.Println(err)
	}

	http.HandleFunc("/", func(rw http.ResponseWriter, req *http.Request) {
		fmt.Println("recibi mensaje en el follower")
	})

	log.Fatal(http.ListenAndServe(":8080", nil))
}
