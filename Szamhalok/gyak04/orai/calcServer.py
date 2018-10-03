import socket

server_address = ('localhost', 10000)
server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind(server_adress)

server.listen(1)