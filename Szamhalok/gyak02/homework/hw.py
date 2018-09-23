import subprocess
import multiprocessing
import csv
import json
from datetime import datetime

#################################################################
###Internal functions
#################################################################

def runCommand(cmd):  ## Runs the command given as parameter
    p = subprocess.Popen(cmd, shell=True, stdout=subprocess.PIPE)
    return p.communicate()[0]

def CutMiddle(Amount, InputArr):  ## Returns the first and last X amount of elements of the input array
    return (InputArr[:Amount] + InputArr[(len(InputArr)-Amount):])

def WriteJSON(OutArr, type): ## Writes the data into the json file
    Templ = {
        "date" : datetime.today().strftime('%Y%m%d'),
        "system" : "windows",
        type : []
    }
    Fname = "ping.json" if type == "pings" else "traceroute.json"
    for i in xrange(0, len(OutArr)):
        Templ[type].append({
            "target" : str(data[i][1]),
            "output" : OutArr[i]
        })
    with open(Fname, "w") as write_file:
        json.dump(Templ, write_file, indent=4)

#################################################################
###Script starts here
#################################################################

startTime = datetime.now()  ## Only use this to check the runtime of the script
with open('top-1m.csv', 'rb') as csvfile:
    data = CutMiddle(100, list(csv.reader(csvfile)))

pingCommands = []
traceCommands = []
for i in xrange(0, len(data)):  ## Collects the line commands in arrays
    pingCommands.append('ping -n 10 ' + data[i][1])
    traceCommands.append('tracert ' + data[i][1])

if __name__ == '__main__':
    multiprocessing.freeze_support()
    p = multiprocessing.Pool(25)
    PingResult = p.map(runCommand, pingCommands)
    TraceResult = p.map(runCommand, traceCommands)
    WriteJSON(PingResult, "pings")
    WriteJSON(TraceResult, "traces")
    print "Full Runtime:", datetime.now() - startTime