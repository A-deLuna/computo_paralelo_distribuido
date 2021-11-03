package main

import (
	"fmt"
	"io"
	"log"
	"net/http"
	"strings"
	"sync"
	"sync/atomic"
	"time"
)

var cnt int32 = 0
var wg sync.WaitGroup
var m sync.Mutex
var ips = []string{}

func main() {
	fmt.Println("Empezando master")
	go startServer(":8080", internalHandler)
	go readyHandler()
	log.Fatal(startServer(":80", externalHandler))
}

func readyHandler() {
	for {
		for _, ip := range ips {
			url := fmt.Sprintf("http://%s:8080", ip)
			_, err := http.Get(url)
			if err != nil {
				fmt.Println(err)
			}
		}
		time.Sleep(5 * time.Second)
	}
}

func externalHandler(rw http.ResponseWriter, req *http.Request) {
	fmt.Println("Recibi externo")
	msg := fmt.Sprintf("Pings = %d\n", atomic.LoadInt32(&cnt))
	io.WriteString(rw, msg)
}

var workers = []int{}

func internalHandler(rw http.ResponseWriter, req *http.Request) {
	atomic.AddInt32(&cnt, 1)
	fmt.Printf("Recibi ping remoaddr=%q, forwarded-host=%q\n",
		req.RemoteAddr, req.Header.Get("X-Forwarded-For"))

	ip := strings.Split(req.RemoteAddr, ":")[0]
	m.Lock()
	ips = append(ips, ip)
	m.Unlock()

}

func startServer(addr string, handler http.HandlerFunc) error {
	mux := http.NewServeMux()
	mux.HandleFunc("/", handler)
	return http.ListenAndServe(addr, mux)
}
