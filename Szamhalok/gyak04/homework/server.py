import socket
import select

server_adress = ("localhost", 10000)
server = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
server_files = "E:\Python\hd\serverFiles"

server.bind(server_adress)

server.listen(1)

input = [server]