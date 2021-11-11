FROM debian:latest
RUN apt-get update &&  apt-get install -y \
    curl \
    cmake \
    g++ \
    git \
    libboost-filesystem-dev \
    libboost-thread-dev \
    libssl-dev \
    && rm -rf /var/lib/apt/lists/*
RUN git clone https://gitlab.com/eidheim/Simple-Web-Server.git \
    && cd Simple-Web-Server \
    && cmake -H. -Bbuild \
    && cmake --build build \
    && cmake --install build
WORKDIR /app
COPY main.cc .
RUN g++ main.cc -lpthread
ENTRYPOINT ./a.out
