import json
import time

#Internal functions
def isValinObjArr(arrtosearch, key, val):
	for i in arrtosearch:
		if i[key] == val:
			return True
	return False

def isRouteAvailable(linksArr, Route, demand):
	for i in xrange(0, len(Route)-1):
		Curr = [Route[i], Route[i+1]].sort()
		for i in linksArr:
			if i["points"] == Curr and i["capacity"] < demand:
				return False
	return True

def SetRouteOccup(linksArr, Route, demand):
	for i in xrange(0, len(Route)-1):
		Curr = [Route[i], Route[i+1]].sort()
		for i in linksArr:
			if i["points"] == Curr:
				i["capacity"] -= demand

def FreeRoute(linksArr, Route, cap):
	for i in xrange(0, len(Route)-1):
		Curr = [Route[i], Route[i+1]].sort()
		for i in linksArr:
			if i["points"] == Curr:
				i["capacity"] += cap

def FindRoute(demandObj, linksArr, RoutesArr):
	for i in RoutesArr:
		if i[0] == demandObj["end-points"][0] and i[len(i)-1] == demandObj["end-points"][1] and isRouteAvailable(linksArr, i, demandObj["demand"]):
			return i

#Script starts here
with open("cs1.json", "r") as read_file:
	data = json.load(read_file)

for i in data["links"]:
	i["points"] = i["points"].sort()

simDur = data["simulation"]["duration"]
for i in xrange(0,simDur):
	Te = isValinObjArr(data["simulation"]["demands"], "end-time", i)


#if "start-time" in data["simulation"]["demands"][3]:
#for current in data["simulation"]["demands"]:
#time.sleep(1)
#Test = data["possible-circuits"][3]
#T=isRouteAvailable(data["links"], Test)