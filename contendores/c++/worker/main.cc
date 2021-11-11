#include <simple-web-server/client_http.hpp>
#include <simple-web-server/server_http.hpp>
#include <string>
#include <iostream>


using HttpServer = SimpleWeb::Server<SimpleWeb::HTTP>;
using HttpClient = SimpleWeb::Client<SimpleWeb::HTTP>;



int main() {
  HttpServer server;
  server.config.port = 8080;
  
  // Manda request al leader para que guarde nuestro IP.
  HttpClient client("leader:8080");
  auto req = client.request("GET", "/register");
  std::cout << "Respuesta de leader/register:" << req->content.string() << std::endl;

  // Work empieza a computar
  server.resource["^/work$"]["GET"] = [](std::shared_ptr<HttpServer::Response> response,
      std::shared_ptr<HttpServer::Request> request) {
    std::cout << "receiving work" << std::endl;
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
