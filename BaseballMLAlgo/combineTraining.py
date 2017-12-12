import os
dirname = "C:\\Users\\Solammy\\Desktop\\School\\Fall 2017\\Machine Learning\\Project\\txtFiles\\"

gameData = dict(dict())
eventData = dict(dict())

for filename in os.listdir(dirname):
    if "GAME" in filename:
        f = open(filename, 'r')
        for line in f:
            tokens = line.split(",")
            temp = dict()
            if len(tokens) == 49:
                temp["GameID"] = tokens[0]
                temp["Date"] = tokens[1]
                temp["DayOfWeek"] = tokens[2]
                temp["StartTime"] = tokens[3]
                temp["DayNight"] = tokens[4]
                temp["VisTeam"] = tokens[5]
                temp["HomeTeam"] = tokens[6]
                temp["GameLoc"] = tokens[7]
                temp["VisStartPit"] = tokens[8]
                temp["HomeStartPit"] = tokens[9]
                temp["Temp"] = tokens[10]
                temp["VisFinalScore"] = tokens[11]
                temp["HomeFinalScore"] = tokens[12]
                temp["VisBat1"] = tokens[13]
                temp["VisBat1Pos"] = tokens[14]
                temp["VisBat2"] = tokens[15]
                temp["VisBat2Pos"] = tokens[16]
                temp["VisBat3"] = tokens[17]
                temp["VisBat3Pos"] = tokens[18]
                temp["VisBat4"] = tokens[19]
                temp["VisBat4Pos"] = tokens[20]
                temp["VisBat5"] = tokens[21]
                temp["VisBat5Pos"] = tokens[22]
                temp["VisBat6"] = tokens[23]
                temp["VisBat6Pos"] = tokens[24]
                temp["VisBat7"] = tokens[25]
                temp["VisBat7Pos"] = tokens[26]
                temp["VisBat8"] = tokens[27]
                temp["VisBat8Pos"] = tokens[28]
                temp["VisBat9"] = tokens[29]
                temp["VisBat9Pos"] = tokens[30]
                temp["HomeBat1"] = tokens[31]
                temp["HomeBat1Pos"] = tokens[32]
                temp["HomeBat2"] = tokens[33]
                temp["HomeBat2Pos"] = tokens[34]
                temp["HomeBat3"] = tokens[35]
                temp["HomeBat3Pos"] = tokens[36]
                temp["HomeBat4"] = tokens[37]
                temp["HomeBat4Pos"] = tokens[38]
                temp["HomeBat5"] = tokens[39]
                temp["HomeBat5Pos"] = tokens[40]
                temp["HomeBat6"] = tokens[41]
                temp["HomeBat6Pos"] = tokens[42]
                temp["HomeBat7"] = tokens[43]
                temp["HomeBat7Pos"] = tokens[44]
                temp["HomeBat8"] = tokens[45]
                temp["HomeBat8Pos"] = tokens[46]
                temp["HomeBat9"] = tokens[47]
                temp["HomeBat9Pos"] = tokens[48]
                gameData.update({tokens[0]:temp})
           
        
count = 0
for filename in os.listdir(dirname):
    if "EVENT" in filename:
        f = open(filename, 'r')
        for line in f:
            tokens = line.split(",")
            temp = dict()
            if len(tokens) < 9:
                raise ValueError('event tokens incorrect size')
            if tokens[0] in gameData:
                temp["GameID"] = gameData[tokens[0]]["GameID"]
                temp["Date"] = gameData[tokens[0]]["Date"]
                temp["DayOfWeek"] = gameData[tokens[0]]["DayOfWeek"]
                temp["StartTime"] = gameData[tokens[0]]["StartTime"]
                temp["DayNight"] = gameData[tokens[0]]["DayNight"]
                temp["GameLoc"] = gameData[tokens[0]]["GameLoc"]
                temp["VisStartPit"] = gameData[tokens[0]]["VisStartPit"]
                temp["HomeStartPit"] = gameData[tokens[0]]["HomeStartPit"]
                temp["Temp"] = gameData[tokens[0]]["Temp"]
                temp['batter'] = tokens[1]
                temp['batterHand'] = tokens[2]
                temp['pitcher'] = tokens[3]
                temp['pitcherHand'] = tokens[4]
                temp['eventText'] = tokens[5]
                temp['defPos'] = tokens[6]
                temp['battingPos'] = tokens[7]
                temp['hitVal'] = tokens[8]
                eventData.update({count:temp})
                count+=1
                
f = open("eventTraining.txt", 'w')
for each in eventData:
    outputLine = []
    for each2 in eventData[each]:
        outputLine.append(eventData[each][each2])
    s = ","
    f.write(s.join(outputLine))
f.close()

q = open("gameTraining.txt", 'w')
for each in gameData:
    outputLine = []
    for each2 in gameData[each]:
        outputLine.append(gameData[each][each2])
    s = ","
    q.write(s.join(outputLine))
    
q.close()        

    
