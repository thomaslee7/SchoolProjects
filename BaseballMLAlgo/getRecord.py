# -*- coding: utf-8 -*-
"""
Created on Sat Nov 18 14:39:15 2017

@author: Solammy
"""

from load_file import fileLoader

fl = fileLoader()

gameData = fl.loadFile("gameTrainingWSeasonand5Game1.txt")

gD = fl.loadFile("gameTrainingWSeasonand5Game2.txt")

gameData.update(gD)

recordData = dict(dict(dict()))

for each in gameData:
    team = gameData[each]["HomeTeam"]
    year = gameData[each]["year"]
    if team in recordData and year in recordData[team]:
        recordData[team][year]["Wins"] = 0
        recordData[team][year]["Losses"] = 0
        recordData[team][year]["Ties"]= 0
    else:
        if team not in recordData:
            recordData[team] = {}
        if year not in recordData[team]:
            recordData[team][year] = {}
        recordData[team][year]["Wins"] = 0
        recordData[team][year]["Losses"] = 0
        recordData[team][year]["Ties"]= 0
    
for each in gameData:
    Hteam = gameData[each]["HomeTeam"]
    Vteam = gameData[each]["VisTeam"]
    year = gameData[each]["year"]
    gameID = gameData[each]["GameID"]
    visRecord = str(recordData[Vteam][year]["Wins"])+","+str(recordData[Vteam][year]["Losses"]) +","+ str(recordData[Vteam][year]["Ties"])
    homeRecord = str(recordData[Hteam][year]["Wins"])+","+str(recordData[Hteam][year]["Losses"]) +","+ str(recordData[Hteam][year]["Ties"])
    gameData[each]["VisRecord"] = visRecord
    gameData[each]["HomeRecord"] = homeRecord
    
    if gameData[each]["VisFinalScore"] > gameData[each]["HomeFinalScore"]:
        recordData[Vteam][year]["Wins"] +=1
        recordData[Hteam][year]["Losses"] +=1
    elif gameData[each]["VisFinalScore"] < gameData[each]["HomeFinalScore"]:
        recordData[Hteam][year]["Wins"] +=1
        recordData[Vteam][year]["Losses"] +=1
    else:
        recordData[Hteam][year]["Ties"] +=1
        recordData[Vteam][year]["Ties"] +=1

outfile = open("gameTrainingRecord.txt", 'w')

for each in gameData:
    outputline = list(gameData[each].values())
    s = ','.join(str(e) for e in outputline)
    outfile.write(s)
    outfile.write('\n')
outfile.close()
    