# -*- coding: utf-8 -*-
import json
import time

#################################################################
###Internal functions
#################################################################
def isValinDemArr(demandsArr, key, val):
	Ret = []
	for i in xrange(0, len(demandsArr)):
		if demandsArr[i][key] == val:
			Ret.append(i)
	return Ret

def isRouteAvailable(linksArr, Route, demand):
	for i in xrange(0, len(Route)-1):
		Curr = sorted([Route[i], Route[i+1]])
		for j in linksArr:
			if j["points"] == Curr and j["capacity"] < demand:
				return False
	return True

def ModifyRoute(linksArr, Route, demand, ModVal):
	for i in xrange(0, len(Route)-1):
		Curr = sorted([Route[i], Route[i+1]])
		for j in linksArr:
			if j["points"] == Curr:
				j["capacity"] += (ModVal * demand)

def FindRoute(demandObj, linksArr, RoutesArr):
	for i in RoutesArr:
		if i[0] == demandObj["end-points"][0] and i[len(i)-1] == demandObj["end-points"][1] and isRouteAvailable(linksArr, i, demandObj["demand"]):
			return i
	return "Fail"

def Simulate(dataObj, demandInd):
	for i in demandInd:
		demandObj = data["simulation"]["demands"][i]
		Route = FindRoute(demandObj, dataObj["links"], dataObj["possible-circuits"])
		if Route != "Fail":
			ModifyRoute(dataObj["links"], Route, demandObj["demand"], -1)
		dataObj["simulation"]["demands"][i]["route-taken"] = Route
		Succ = False if Route == "Fail" else True
		PrintResult(dataObj["simulation"]["demands"][i], Succ, u"foglalás", dataObj)

def CloseProcess(dataObj, demandInd):
	for i in demandInd:
		if dataObj["simulation"]["demands"][i]["route-taken"] != "Fail":
			demandObj = data["simulation"]["demands"][i]
			ModifyRoute(dataObj["links"], demandObj["route-taken"], demandObj["demand"], 1)
			dataObj["simulation"]["demands"][i]["route-taken"] = ""
			PrintResult(dataObj["simulation"]["demands"][i], True, u"felszabadítás", dataObj)

def PrintResult(demandObj, isSuccesful, DemType, dataObj):
	dataObj["PC"] += 1
	ToPrint = str(dataObj["PC"]) + u".igény " + DemType + ":"
	ToPrint += str(demandObj["end-points"][0]) + "<->" + str(demandObj["end-points"][1]) + " st:"
	if DemType == u"foglalás":
		ToPrint += str(demandObj["start-time"]) + ("-sikeres" if isSuccesful else "-sikertelen")
	else:
		ToPrint += str(demandObj["end-time"])
	print ToPrint

#################################################################
###Script starts here
#################################################################
with open("cs1.json", "r") as read_file:
	data = json.load(read_file)

for i in data["links"]:
	i["points"] = sorted(i["points"])

for i in data["simulation"]["demands"]:
	i["route-taken"] = ""

simDur = data["simulation"]["duration"]
data["PC"] = 0
for i in xrange(1, simDur + 1):
	DemInd = isValinDemArr(data["simulation"]["demands"], "start-time", i)
	if DemInd != []:
		Simulate(data, DemInd)
	DemInd2 = isValinDemArr(data["simulation"]["demands"], "end-time", i)
	if DemInd2 != []:
		CloseProcess(data, DemInd2)
	#time.sleep(1)
print data["links"]