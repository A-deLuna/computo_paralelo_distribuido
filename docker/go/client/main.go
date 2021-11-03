package main

import (
	"fmt"
	"net/http"
)

func main() {
	for {
		resp, _ := http.Get("http://server:80")
		fmt.Printf("recibi=%s\n", resp.Status)
	}
}
