package main

import (
	"fmt"
	"sync/atomic"
	"time"
)

var cnt int64 = 0

func main() {

	for i := 0; i < 1000; i++ {
		go addOne()
	}
	time.Sleep(2 * time.Second)
	fmt.Printf("total: %d\n", cnt)
}

func addOne() {
	atomic.AddInt64(&cnt, 1)
}
