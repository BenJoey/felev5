import sys

inp = open("input.txt","r")

[maxTest, aktTest] = inp.readline().split(',')
[maxHf, aktHf] = inp.readline().split(',')
maxZH = float(inp.readline())
Result = {2:-1, 3:-1, 4:-1, 5:-1}
TestP = (float(aktTest)/float(maxTest))*0.25
HfP = (float(aktHf)/float(maxHf))*0.35
for i in xrange(0,int(maxZH)+1):
	ZhP = (i/maxZH)*0.4
	Sum = TestP+HfP+ZhP
	if Sum>=0.5 and Result[2]==-1:
		Result[2]=i
	if Sum>=0.6 and Result[3]==-1:
		Result[3]=i
	if Sum>=0.75 and Result[4]==-1:
		Result[4]=i
	if Sum>=0.85 and Result[5]==-1:
		Result[5]=i
print "Eddigi:", (HfP+TestP)
for i in Result.keys():
	if Result[i]==-1:
		print i, ": Remenytelen"
	else:
		print i, ":", Result[i]