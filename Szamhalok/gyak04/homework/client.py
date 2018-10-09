import socket

server_address = ('localhost', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

client.connect(server_address)

command = ""

while command != "EXIT":
    command = raw_input(">").upper()
    client.sendall(command)

    if command.upper() == "EXIT":
		print "Kapcsolat megszakitva"