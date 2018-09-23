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

def WriteJSON(OutArr, type):
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
    pingTime = datetime.now()
    print "Pings finished in:", datetime.now() - startTime
    TraceResult = p.map(runCommand, traceCommands)
    print "Traces finished in:", datetime.now() - pingTime
    WriteJSON(PingResult, "pings")
    WriteJSON(TraceResult, "traces")
    print "Full Runtime:", datetime.now() - startTime

#for i in xrange(0,len(data)):
    #p = multiprocessing.Process(target=runCommand, args=(' ' + data[i][1],))
    #jo.append(p)
    #p.start()
#P = runCommand('ping -n 5 ' + data[1][1])
#print P
#print 'tracert ' + data[1][1]
#subprocess.call(['tracert', 'google.com'], shell=True)
#p = multiprocessing.Pool(multiprocessing.cpu_count())