import socket
import select
import sys
import struct

prevResults = {}

multi_group = ("224.3.29.71", 11000)
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.settimeout(1.0)
ttl = struct.pack('b', 1)
sock.setsockopt(socket.IPPROTO_IP, socket.IP_MULTICAST_TTL, ttl)

server_address = ('localhost', 10000)
server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind(server_address)
server.listen(1)
server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR,1)
inputs = [server]

while inputs:
    timeout = 1
    readable, writeable, exc = select.select(inputs, inputs, inputs, timeout)
    
    if not (readable or writeable or exc):
        print "Timeout"
    
    for s in readable:
        if s is server:
            client, client_addr = s.accept()
            print "Uj kliens", client_addr
            inputs.append(client)
        else:
            try:
                data = s.recv(1024)
                if not data:
                    print "Kliens kilepett"
                    s.close()
                    inputs.remove(s)
                elif data in prevResults:
                    s.sendall(prevResults[data])
                else:
                    sock.sendto(data, multi_group)
                    data2, addr = sock.recvfrom(1024)
                    prevResults[data]=data2
                    s.sendall(data2)
            except socket.error, msg:
                    s.close()
                    inputs.remove(s)
                    print "Kliens hibaval kilepett"
sock.close()