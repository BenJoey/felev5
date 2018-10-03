import socket
import sys

server_addr = ('pc6w05', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

client.connect(server_addr)
#client.sendall("helo szerver")

data = client.recv(20)
print "server uzeni: ", data

client.close()