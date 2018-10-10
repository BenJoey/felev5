import socket
from random import randint

server_address = ("localhost", 10000)
server = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)

server.bind(server_address)
server.settimeout(1.0)

MyNumber = randint(1,101)
IsEnd = False

while True:
    try:
        data, cli_addr = server.recvfrom(20)
        if not IsEnd:
            if int(data)==MyNumber:
                IsEnd=True
                server.sendto("Kitalaltad", cli_addr)
            else:
                ToRet = "<" if int(data)<MyNumber else ">"
                server.sendto(ToRet, cli_addr)
        else:
            server.sendto("Mas megoldotta", cli_addr)
    except socket.error, msg:
        print "TimeOut"