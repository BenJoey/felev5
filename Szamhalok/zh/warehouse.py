import socket
import select
import time
import random
import struct

server_addr = ("localhost", 10000)
server = socket.socket(socket.AF_INET,socket.SOCK_STREAM)

server.bind(server_addr)

server.listen(1)

input = [server]

presents = { ##itt csak azert fura par nev hogy pont 10 karakter legyen :)
        "ps4console" : "location1",  ##location1 port: 11000
        "hugelaptop" : "location1",
        "VR_Headset" : "location2",  ##location2 port: 12000
        "GTX2080_VC" : "location2",
        "bodypillow" : "location3"   ##location3 port: 13000
        }
#presents 10 chars
#locations 9 chars

while input:
    timeout = 1
    read,write,excep = select.select(input, input, input, timeout)
    if not (read or write or excep):
        print "Time out"
        continue

    for s in read:
        if s is server:
            cli, cli_addr = s.accept()
            input.append(cli)
            print "Client connected:", cli_addr
        else:
            try:
                data = s.recv(20)
                print "Santa arrived, waiting for a gift!"
                NumOfPresents = random.randint(1,3)
                s.sendall(str(NumOfPresents))
                for i in range(0,NumOfPresents):
                    randKey = random.choice(presents.keys())
                    curr_present = (randKey, presents[randKey])
                    packer = struct.Struct('10s 9s')
                    compressed = packer.pack(*curr_present)
                    cli.sendall(compressed)
                    time.sleep(1)
                s.close()
                input.remove(s)
                print "Santa has received the gifts and he's on the way to deliver"
            except:
                s.close()
                input.remove(s)
                print "Santa_Client has died during the procces :("
