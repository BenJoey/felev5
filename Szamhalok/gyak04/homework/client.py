#################################################################
###Imports
#################################################################

import socket
import ast
import json

#################################################################
###Script starts here
#################################################################

server_address = ('localhost', 10000)
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

client.connect(server_address)

command = ""

while command != "EXIT":
  command = raw_input("$ ").upper()
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

  else:
    print "Nem letezo parancs"