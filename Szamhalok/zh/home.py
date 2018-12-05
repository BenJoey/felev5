import socket
import sys
from map import getHousePort

if len(sys.argv) < 2:
    print "House is requested to start"
    sys.exit(1)

house_port = getHousePort(sys.argv[1])

if house_port == 404:
    print "This house does not exist"
    sys.exit(1)

server = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server.settimeout(1.0)

server.bind(("localhost", house_port))

while True:
    try:
        data, cli_addr = server.recvfrom(100)
        print "Santa arrived and he brought a ", data
        server.sendto("Hi Santa! Thx for the gift", cli_addr)
    except socket.error, msg:
        print "Time Out"
