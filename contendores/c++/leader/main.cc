#include <simple-web-server/client_http.hpp>
#include <simple-web-server/server_http.hpp>
#include <string>
#include <iostream>


using HttpServer = SimpleWeb::Server<SimpleWeb::HTTP>;
using HttpClient = SimpleWeb::Client<SimpleWeb::HTTP>;


int main() {
  HttpServer server;
  server.config.port = 8080;

  // Workers llaman register para darse a conocer.
  server.resource["^/register$"]["GET"] = [](std::shared_ptr<HttpServer::Response> response,
      std::shared_ptr<HttpServer::Request> request) {
    std::string ip = "[" + request->remote_endpoint().address().to_string() + "]";
    std::cout << "recibi register del ip: " << ip << std::endl;
    response->write("ok");
  };

  // Start comienza la computacion del trabajo en paralelo
  server.resource["^/start$"]["GET"] = [](std::shared_ptr<HttpServer::Response> response,
      std::shared_ptr<HttpServer::Request> request) {
    std::cout << "recibi start" << std::endl;
    // TODO mandar requests a los clientes 
    response->write("ok");
  };

  std::promise<unsigned short> server_port;
  std::thread server_thread([&server, &server_port]() {
    // Start server
    server.start([&server_port](unsigned short port) {
      server_port.set_value(port);
    });
  });
  std::cout << "Server listening on port " << server_port.get_future().get() << std::endl
       << std::endl;

  server_thread.join();

  return 0;
}
