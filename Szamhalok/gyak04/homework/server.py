#################################################################
###Imports
#################################################################

import socket
import select
import os

#################################################################
###Internal functions
#################################################################

def listDirectory():
	OutJSON = {}
	OutJSON['root'] = {}
	OutJSON['root']['dirs'] = {}
	OutJSON['root']['files'] = []
	directory = os.listdir(server_directory)
	for file in directory:
		DirectoryCollector(server_files + "\\" + file, OutJSON, ["root"], file)
	return OutJSON

def DirectoryCollector(path, OutJSON2, prevFolders, currPath):
	if os.path.isdir(path):
		jsonMaker(OutJSON2, prevFolders, currPath, False)
		dir = os.listdir(path)
		prevFolders.append(currPath)
		for file in dir:
			DirectoryCollector(path + "\\" + file, OutJSON2, copy.deepcopy(prevFolders), file)
	else:
		jsonMaker(OutJSON2, prevFolders, currPath, True)

#################################################################
###Script starts here
#################################################################

server_address = ("localhost", 10000)
server = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
server_directory = "C:\Users\Bence\felev5\Szamhalok\gyak04\homework\serverContent"

server.bind(server_address)

server.listen(1)

input = [server]

while input:
    timeout = 3
    read,write,excep = select.select(input, input, input, timeout)
	
    if not (read or write or excep):
		print "Server TimeOut"
		continue

    for r in read:
        if r is server:
            client, client_address = r.accept()
            input.append(client)
            print "Uj Kliens csatlakozott", client_address

        else:
            try:
                data = r.recv(200)
                if data == "EXIT":
                    r.close()
                    input.remove(r)
                    print "Kliens kilepett"