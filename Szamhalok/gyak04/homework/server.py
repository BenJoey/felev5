#################################################################
###Imports
#################################################################

import socket
import select
import os
import json
import ast
import copy
import sys
import datetime

#################################################################
###Internal functions
#################################################################

def CreateJSON_OBJ(JSON_INOUT, prevFolders, currPath, isFile):
	for prev in prevFolders:
		if (prev == prevFolders[0] and prev == prevFolders[-1] and isFile):
			JSON_INOUT['root']['files'].append(currPath)
		elif (prev == prevFolders[-1] and isFile):
			prevFolder[prev]['files'].append(currPath)
		
		elif(prev == prevFolders[0] and prev == prevFolders[-1] and not isFile):
			JSON_INOUT['root']['dirs'][currPath] = {}
			JSON_INOUT['root']['dirs'][currPath]['dirs'] = {}
			JSON_INOUT['root']['dirs'][currPath]['files'] = []
		elif (prev == prevFolders[-1] and not isFile):
			prevFolder[prev]['dirs'][currPath] = {}
			prevFolder[prev]['dirs'][currPath]['dirs'] = {}
			prevFolder[prev]['dirs'][currPath]['files'] = []
			
		elif prev == prevFolders[0]:
			prevFolder = JSON_INOUT[prev]['dirs']
		else:
			prevFolder = prevFolder[prev]['dirs']

def listDirectory():
	OutJSON = {}
	OutJSON['root'] = {}
	OutJSON['root']['dirs'] = {}
	OutJSON['root']['files'] = []
	directory = os.listdir(server_directory)
	for file in directory:
		DirectoryCollector(server_directory + "\\" + file, OutJSON, ["root"], file)
	return OutJSON

def DirectoryCollector(path, OutJSON2, prevFolders, currPath):
	if os.path.isdir(path):
		CreateJSON_OBJ(OutJSON2, prevFolders, currPath, False)
		dir = os.listdir(path)
		prevFolders.append(currPath)
		for file in dir:
			DirectoryCollector(path + "\\" + file, OutJSON2, copy.deepcopy(prevFolders), file)
	else:
		CreateJSON_OBJ(OutJSON2, prevFolders, currPath, True)

def SearchDirectory(path_to_dir, currPath, search, finds):
	if os.path.isdir(path_to_dir):
		direct = os.listdir(path_to_dir)
		for file in direct:
			SearchDirectory(path_to_dir + "\\" + file, file, search, finds)
	elif (currPath == search):
		finds.append(path_to_dir)

#################################################################
###Script starts here
#################################################################

server_address = ("localhost", 10000)
server = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
server_directory = "serverContent"

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

                if data.upper() == "EXIT":
                    r.sendall("Kapcsolat megszakitva")
                    r.close()
                    input.remove(r)
                    print "Kliens kilepett"
                elif data.upper() == "DIR":
					Dir_json_obj = str(listDirectory())
					ToRet = "File szerkezet:\n" + str(json.dumps(ast.literal_eval(Dir_json_obj), indent=4))
					r.sendall(ToRet)
				
                elif (data[0:4].upper() == "FIND" and len(data) > 5):
					directory_to_search = os.listdir(server_directory)
					search = ''
					finds = []
					for idontknow in data.split(" ")[1:]:
						search = search + idontknow + " "
					if (not data[-1][-1] == " "):
						search = search.strip()
					for file in directory_to_search:
						SearchDirectory(server_directory + "\\" + file, file, search, finds)
					if not finds:
						r.sendall("Hibas fajlnev vagy nem letezo fajl")
					else:
						finds = list(map( lambda x: x.replace(server_directory + '\\', ''), finds))
						ToRet = "A file(ok) eleresi helye(i):\n" + str(finds).replace('\\\\', '\\')
						r.sendall(ToRet)
                elif (data[0:7].upper() == "MODTIME" and len(data)>7):
					file = ''
					for word in data.split(" ")[1:]:
						file = file + word + " "
					if (not data[-1][-1] == " "):
						file = file.strip()
					path = server_directory + "\\" + file
					if (not os.path.exists(path)):
						r.sendall("Hibas fajlnev vagy nem letezo fajl")
					elif (os.path.isdir(path)):
						r.sendall("A megadott utvonal egy konyvtar")
					else:
						time = os.path.getmtime(path)
						ToRet = "A file modositasi datuma:" + str(datetime.datetime.fromtimestamp(float(time)))
						r.sendall(ToRet)
                elif ((data[0:2].upper() in ["DL","GS"]) and len(data)>2):
					file = ''
					for word in data.split(" ")[1:]:
						file = file + word + " "
					if (not data[-1][-1] == " "):
						file = file.strip()
					path = server_directory + "\\" + file
					if (not os.path.exists(path)):
						r.sendall("Empty")
					elif (os.path.isdir(path)):
						r.sendall("Directory")
					else:
						f = open(path)
						if os.path.getsize(path) > 0:
							backString = f.read()
							if data[0:2].upper() == "DL":
								r.sendall(backString)
							else:
								r.sendall(str(sys.getsizeof(backString)))
						else:
							r.sendall("1")
                else:
					r.sendall("Hibas parancs")
            except:
				r.close()
				input.remove(r)
				print "A kliens eroszakosan megszakadt"