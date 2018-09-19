import json
import time

with open("cs1.json", "r") as read_file:
	data = json.load(read_file)
	print( data["possible-circuits"])

simDur = data["simulation"]["duration"]
for i in xrange(0,simDur):
	


#if "start-time" in data["simulation"]["demands"][3]:
#for current in data["simulation"]["demands"]:
#time.sleep(1)