#include <iostream>
#include <thread>
#include <chrono>
#include <semaphore>


std::counting_semaphore<3> sem(0);
void waitn(int i) {
  std::this_thread::sleep_for(std::chrono::seconds(i));
  std::cout << "finished after " << i << " seconds" << std::endl;
  sem.release();
}

int main() {
  std::thread t1(waitn, 1);
  t1.detach();
  std::thread t2(waitn, 2);
  t2.detach();
  std::thread t3(waitn, 3);
  t3.detach();

  for (int i = 0; i < 3; i++ ) {
    sem.acquire();
  }
  std::cout << "process end!\n";
  return 0;
}
