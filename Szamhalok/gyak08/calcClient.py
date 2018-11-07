import socket
import sys

server_addr = ('localhost', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

client.connect(server_addr)

values = sys.argv[1] + sys.argv[2] + sys.argv[3]
client.sendall(values)

data = client.recv(20)
print data

client.close()