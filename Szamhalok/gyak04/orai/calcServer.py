import socket
import select
import struct

server_address = ('localhost', 10000)
server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind(server_address)
server.listen(1)
input = [server]

while input:
    timeout = 1
    readable, writeable, exc = select.select(input, input, input, timeout)
    
    if not (readable or writeable or exc):
        print "Timeout"
    
    for s in readable:
        if s is server:
            client, client_addr = s.accept()
            print "Uj kliens", client_addr
            input.append(client)
        else:
            try:
                data = s.recv(20)
                if data:
                    unpacker = struct.Struct('I 1s I')
                    unpacked_data = unpacker.unpack(data)
                    Op = unpacked_data[1]
                    ToRet = str(unpacked_data[0]) + Op + str(unpacked_data[2]) + "="
                    if Op == '+':
                        ToRet += str(unpacked_data[0] + unpacked_data[2])
                    if Op == '-':
                        ToRet += str(unpacked_data[0] - unpacked_data[2])
                    if Op == '*':
                        ToRet += str(unpacked_data[0] * unpacked_data[2])
                    if Op == '/':
                        if unpacked_data[2] == 0:
                            ToRet = "Error:Osztas nullaval"
                        else:
                            ToRet += str(unpacked_data[0] / unpacked_data[2])
                    s.sendall(ToRet)
                else:
                    s.close()
                    input.remove(s)
                    print "Kliens kilepett"
            except socket.error, msg:
                    s.close()
                    input.remove(s)
                    print "Kliens hibaval kilepett"