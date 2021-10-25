package main

import (
	"fmt"
	"time"
)

func main() {
	go loop()
	time.Sleep(3 * time.Second)
}

func loop() {
	for {
		fmt.Println("Hola")
		time.Sleep(100 * time.Millisecond)
	}
}
