# -*- coding: utf-8 -*-
import json
import time

#################################################################
###Internal functions
#################################################################
def isValinDemArr(key, val):
	Ret = []
	for i in xrange(0, len(data["simulation"]["demands"])):
		if data["simulation"]["demands"][i][key] == val:
			Ret.append(i)
	return Ret

def isRouteAvailable(Route, demand):
	for i in xrange(0, len(Route)-1):
		Curr = sorted([Route[i], Route[i+1]])
		for j in data["links"]:
			if j["points"] == Curr and j["capacity"] < demand:
				return False
	return True

def ModifyRoute(Route, demand, ModVal):
	for i in xrange(0, len(Route)-1):
		Curr = sorted([Route[i], Route[i+1]])
		for j in data["links"]:
			if j["points"] == Curr:
				j["capacity"] += (ModVal * demand)

def FindRoute(demandObj):
	for i in data["possible-circuits"]:
		if i[0] == demandObj["end-points"][0] and i[len(i)-1] == demandObj["end-points"][1] and isRouteAvailable(i, demandObj["demand"]):
			return i
	return "Fail"

def Simulate(demandInd):
	for i in demandInd:
		demandObj = data["simulation"]["demands"][i]
		Route = FindRoute(demandObj)
		if Route != "Fail":
			ModifyRoute(Route, demandObj["demand"], -1)
		data["simulation"]["demands"][i]["route-taken"] = Route
		Succ = False if Route == "Fail" else True
		PrintResult(data["simulation"]["demands"][i], Succ, u"foglalás")

def CloseProcess(demandInd):
	for i in demandInd:
		if data["simulation"]["demands"][i]["route-taken"] != "Fail":
			demandObj = data["simulation"]["demands"][i]
			ModifyRoute(demandObj["route-taken"], demandObj["demand"], 1)
			data["simulation"]["demands"][i]["route-taken"] = ""
			PrintResult(data["simulation"]["demands"][i], True, u"felszabadítás")

def PrintResult(demandObj, isSuccesful, DemType):
	data["PC"] += 1
	ToPrint = str(data["PC"]) + u".igény " + DemType + ":" + str(demandObj["end-points"][0]) + "<->" + str(demandObj["end-points"][1]) + " st:"
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

simDur = data["simulation"]["duration"]
data["PC"] = 0
for i in xrange(1, simDur + 1):
	DemInd = isValinDemArr("start-time", i)
	if DemInd != []:
		Simulate(DemInd)
	DemInd2 = isValinDemArr("end-time", i)
	if DemInd2 != []:
		CloseProcess(DemInd2)
	time.sleep(1)