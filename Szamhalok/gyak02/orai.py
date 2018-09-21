# -*- coding: utf-8 -*-
import json
import time

##################################
###Internal functions
##################################
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

def SetRouteOccup(linksArr, Route, demand):
	for i in xrange(0, len(Route)-1):
		Curr = sorted([Route[i], Route[i+1]])
		for j in linksArr:
			if j["points"] == Curr:
				j["capacity"] -= demand
	return linksArr

def FreeRoute(linksArr, Route, cap):
	for i in xrange(0, len(Route)-1):
		Curr = sorted([Route[i], Route[i+1]])
		for j in linksArr:
			if j["points"] == Curr:
				j["capacity"] += cap
	return linksArr

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
			dataObj["links"] = SetRouteOccup(dataObj["links"], Route, demandObj["demand"])
		dataObj["simulation"]["demands"][i]["route-taken"] = Route
		Succ = False if Route == "Fail" else True
		dataObj["PC"] += 1
		PrintResult(data["simulation"]["demands"][i], Succ, u"foglalás", dataObj)
	return dataObj

def CloseProcess(dataObj, demandInd):
	for i in demandInd:
		if data["simulation"]["demands"][i]["route-taken"] != "Fail":
			demandObj = data["simulation"]["demands"][i]
			FreeRoute(dataObj["links"], demandObj["route-taken"], demandObj["demand"])
			dataObj["simulation"]["demands"][i]["route-taken"] = ""
			dataObj["PC"] += 1
			PrintResult(dataObj["simulation"]["demands"][i], True, u"felszabadítás", dataObj)
	return dataObj

def PrintResult(demandObj, isSuccesful, DemType, dataObj):
	ToPrint = []
	ToPrint.append(str(dataObj["PC"]) + u".igény " + DemType)
	ToPrint.append(str(demandObj["end-points"][0]) + "<->" + str(demandObj["end-points"][1]) + " st")
	if DemType == u"foglalás":
		Part = str(demandObj["start-time"]) + ("-sikeres" if isSuccesful else "-sikertelen")
	else:
		Part = str(demandObj["end-time"])
	ToPrint.append(Part)
	print ':'.join(ToPrint)

##################################
###Script starts here
##################################
with open("cs1.json", "r") as read_file:
	data = json.load(read_file)

for i in data["links"]:
	i["points"] = sorted(i["points"])

for i in data["simulation"]["demands"]:
	i["route-taken"] = ""

#print data["simulation"]["demands"][2]["start-time"]
simDur = data["simulation"]["duration"]
data["PC"] = 0
for i in xrange(1, simDur+1):
	DemInd = isValinDemArr(data["simulation"]["demands"], "start-time", i)
	if DemInd != []:
		data = Simulate(data, DemInd)
		#Succ = False if Ret == "Fail" else True
		#PrintCount += 1
		#PrintResult(data["simulation"]["demands"][DemInd], Succ, u"foglalás", PrintCount)
	DemInd2 = isValinDemArr(data["simulation"]["demands"], "end-time", i)
	if DemInd2 != []:
		data = CloseProcess(data, DemInd2)
		#PrintCount += 1
		#PrintResult(data["simulation"]["demands"][DemInd2], Succ, u"felszabadítás", PrintCount)
print data["links"]

#if "start-time" in data["simulation"]["demands"][3]:
#for current in data["simulation"]["demands"]:
#time.sleep(1)
#Test = data["possible-circuits"][3]
#T=isRouteAvailable(data["links"], Test)
#print ([u'A', u'S01'] == ["A", "S01"])