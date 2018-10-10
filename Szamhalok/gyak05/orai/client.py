import socket

server_address = ('localhost', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
data=''

while data != "Kitalaltad" and data != "Mas megoldotta":
    inputgiven = raw_input("$ ")
    client.sendto(inputgiven, server_address)
    data, serv_addr = client.recvfrom(20)
    print data

client.close()