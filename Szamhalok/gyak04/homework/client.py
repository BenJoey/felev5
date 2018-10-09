#################################################################
###Imports
#################################################################

import socket
import ast
import json
import datetime

#################################################################
###Script starts here
#################################################################

server_address = ('localhost', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

client.connect(server_address)

command = ""

while command != "EXIT":
    command = (raw_input("$ ")).upper()
    client.sendall(command)

    if command.upper() == "EXIT":
        print "Kapcsolat megszakitva"

    elif command.upper() == "DIR":
		data = client.recv(100000)
		print "File szerkezet: "
		print json.dumps(ast.literal_eval(data), indent=4)

    elif (command[0:4] == "FIND" and len(command) > 5):
		data = client.recv(1000)
		if (data == 'Empty'):
			print "Hibas fajlnev vagy nem letezo fajl"
		else:
			print "A file(ok) eleresi helye(i):"
			print data.replace('\\\\', '\\')

    elif (command[0:7] == "MODTIME" and len(command)>7):
		data = client.recv(100)
		if (data == 'Empty'):
			print "Hibas fajlnev vagy nem letezo fajl"
		elif (data == 'Directory'):
			print "A megadott utvonal egy konyvtar"
		else:
			print "A file modositasi datuma:", datetime.datetime.fromtimestamp(float(data))

    elif (command[0:2] == "DL" and len(command)>2):
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
			print "file sikeresen atmasolva a client.py mappajaba!"

    else:
        print "Nem letezo parancs"