import socket
import sys
import struct

server_addr = ('localhost', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

client.connect(server_addr)

values = (int(sys.argv[1]), sys.argv[2], int(sys.argv[3]))
packer = struct.Struct('I 1s I')
packed_data = packer.pack(*values)
client.sendall(packed_data)

data = client.recv(20)
print data

client.close()