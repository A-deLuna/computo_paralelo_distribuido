#include <thread>
#include <iostream>
#include <chrono>

void loop() {
  using namespace std::chrono_literals;
  while (true) {
    std::cout << "hola" << std::endl;
    std::this_thread::sleep_for(100ms);
  }
}

int main() {
  using namespace std::chrono_literals;
  std::thread t(loop);
  t.detach();
  std::this_thread::sleep_for(2s);
}
