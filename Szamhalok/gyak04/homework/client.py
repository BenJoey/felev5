#################################################################
###Imports
#################################################################

import socket

#################################################################
###Script starts here
#################################################################

server_address = ('localhost', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

client.connect(server_address)

command = ""

while command != "EXIT":
    command = raw_input("$ ")
    client.sendall(command)

    if command.upper() == "EXIT":
        print "Kapcsolat megszakitva"

    elif (command[0:2].upper() == "DL" and len(command)>2):
		if "\\" in command:
			file = command.split("\\")[-1]
		else:
			file = ''
			for word in command.split(" ")[1:]:
				file = file + word + " "
			if (not command[-1][-1] == " "):
				file = file.strip()
		print file
		data = client.recv(100000)
		if (data == 'Empty'):
			print "Hibas fajlnev vagy nem letezo fajl"
		elif (data == 'Directory'):
			print "A megadott utvonal egy konyvtar"
		else:
			f = open(file, "w")
			f.write(data)
			f.close()
			print "file sikeresen atmasolva a Kliens mappajaba!"
			
    else:
        data = client.recv(100000)
        print data