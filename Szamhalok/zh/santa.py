import socket
import time
import sys
import struct
from map import getHousePort

wareh_loc = ("localhost", 10000)

warehouse = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
warehouse.connect(wareh_loc)

warehouse.sendall("Gimme me gifts!")

PresentNum = warehouse.recv(20)
print "Santa has recived " + PresentNum + " gifts"

presents = {}
for i in xrange(0, int(PresentNum)):
    curr_pres = warehouse.recv(100)
    unpacker = struct.Struct('10s 9s')
    decompr_gift = unpacker.unpack(curr_pres)
    print "Received gift: " + decompr_gift[0] + " to be delivered to: " + decompr_gift[1]
    presents[decompr_gift[0].strip()] = decompr_gift[1]
    time.sleep(1)

warehouse.close()

for gift, loc in presents.items():
    portToSend = getHousePort(loc)
    home = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    home_addr = ("localhost", portToSend)
    home.sendto(gift, home_addr)
    data, addr = home.recvfrom(100)
    print "House says: ", data
    home.close()
