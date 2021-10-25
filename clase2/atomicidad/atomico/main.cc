#include <iostream>
#include <chrono>
#include <thread>
#include <atomic>

std::atomic<int> count = 0;

int add_one() {
  count++;
}
int main() {
  for (int i = 0; i < 1000; i ++) {
    std::thread a(add_one);
    a.detach();
  }
  using namespace std::chrono_literals;
  std::this_thread::sleep_for(2s);

  std::cout << count << "\n";
}
