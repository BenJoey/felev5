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

while command.upper() != "EXIT":
    command = raw_input("$ ")

    if (command[0:2].upper() == "DL" and len(command)>2):
		if "\\" in command:
			file = command.split("\\")[-1]
		else:
			file = ''
			for word in command.split(" ")[1:]:
				file = file + word + " "
			if (not command[-1][-1] == " "):
				file = file.strip()
		client.sendall(("GS"+ command[2:]))
		FileSize = client.recv(1000)
	  	client.sendall(command)
		try:
			data = client.recv(int(FileSize))
		except:
			data = FileSize
			dump = client.recv(20)
		if (data == 'Empty'):
			print "Hibas fajlnev vagy nem letezo fajl"
		elif (data == 'Directory'):
			print "A megadott utvonal egy konyvtar"
		else:
			f = open(file, "w")
			f.write(data)
			f.close()
			print "Fajl sikeresen mentve a Kliens mappajaba!"
			
    else:
		client.sendall(command)
		data = client.recv(100000)
		print data