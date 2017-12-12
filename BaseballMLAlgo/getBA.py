# -*- coding: utf-8 -*-
"""
Created on Wed Oct 25 17:45:50 2017

@author: Solammy
"""


dirname = "C:\\Users\\Solammy\\Desktop\\School\\Fall 2017\\Machine Learning\\Project\\txtFiles\\"
playerData = dict(dict(dict()))
outf = open("eventTrainingSlim.txt", 'w')
eventFile = open("eventTraining.txt", 'r')
for line in eventFile:
    tokens = line.split(",")
    eventCode = list(tokens[13])
    #print(eventCode[1])
    if eventCode[1] in ["W","K","S","D","T","H","E"] and eventCode[2] not in ["B", "H","P"]:
        s = ","
        outf.write(s.join(tokens))
    elif eventCode[1] in ["1","2","3","4","5","6","7","8","9"]:
        if "SH" not in tokens[13]:
            s = ","
            outf.write(s.join(tokens))
    
outf.close()
eventFile.close()
        
    
    