package main

import (
	"fmt"
	"time"
)

var cnt int = 0

func main() {

	for i := 0; i < 1000; i++ {
		go addOne()
	}
	time.Sleep(2 * time.Second)
	fmt.Printf("total: %d\n", cnt)
}

func addOne() {
	cnt++
}
