# -*- coding: utf-8 -*-
"""
Created on Wed Oct 25 18:57:38 2017

@author: Solammy
"""
import collections 
import re

dirname = "C:\\Users\\Solammy\\Desktop\\School\\Fall 2017\\Machine Learning\\Project\\txtFiles\\"
playerYearGameData = dict(dict(dict(dict())))
playerYearGameSeasonData = dict(dict(dict(collections.defaultdict(int))))
gameData = dict(collections.defaultdict(int))
gameFile = open("gameTraining.txt", 'r')
eventFile = open("eventTrainingSlim.txt", 'r')


def evalEvent(eventToken):
    x = list(eventToken)
    
    SO = 0
    S = 0
    D = 0
    T = 0
    HR = 0
    AB = 0
    W = 0
    if "SH" not in eventToken and "SF" not in eventToken:
        if x[0] in ["W","K","S","D","T","H","E", "F", "I"]:
            if x[0] == "W":
                W+=1
            elif x[0] == "I":
                W+=1
            elif x[0] == "K":
                SO+=1
                AB+=1
            elif x[0] == "S":
                S+=1
                AB+=1
            elif x[0] == "D":
                D+=1
                AB+=1
            elif x[0] == "T":
                T+=1
                AB+=1
            elif x[0] == "H":
                if len(x) > 1:
                    if x[1] != "P":
                        HR+=1
                        AB+=1
                    else:
                        HR+=1
                        AB+=1
            elif x[0] == "E":
                AB+=1
            elif x[0] == "F":
                AB+=1
            else:
                AB+=1
        else:
            AB+=1
        
    return SO, S, D, T, HR, AB, W

def getFiveGameStats(x, playerName, season):
    hits = 0
    atBats = 0
    SO = 0
    Singles = 0
    Doub = 0
    Trip = 0
    HR = 0
    Walks = 0
    avg = 0.000
    game1 = {}
    game2 = {}
    game3 = {}
    game4 = {}
    game5 = {}
    if x[0] != "none": game1 = playerYearGameData[playerName][season][x[0]]
    if x[1] != "none": game2 = playerYearGameData[playerName][season][x[1]]
    if x[2] != "none": game3 = playerYearGameData[playerName][season][x[2]]
    if x[3] != "none": game4 = playerYearGameData[playerName][season][x[3]]
    if x[4] != "none": game5 = playerYearGameData[playerName][season][x[4]]
    
    games = [game1, game2, game3, game4, game5]
    for game in games:
        if len(game) > 0:
            hits += game['gameHits']
            atBats += game['gameAB']
            SO += game['gameSO']
            Singles += game['gameSingles']
            Doub += game['gameDoub']
            Trip += game['gameTrip']
            HR += game['gameHR']
            Walks += game['gameWalk']
    
    if atBats != 0:
        avg = float(hits)/atBats
   
    return hits, atBats, SO, Singles, Doub, Trip, HR, Walks, avg
def gameStats(tempDict, labelText, playerName, year, gameID):
    ##season
    if playerName in playerYearGameData:
        if year in playerYearGameData[playerName]:
            if gameID in playerYearGameData[playerName][year]:
                tempDict[labelText+"SeasonSO"] = playerYearGameData[playerName][year][gameID]["seasonSO"]
                tempDict[labelText+"SeasonSingles"] = playerYearGameData[playerName][year][gameID]["seasonSingles"]
                tempDict[labelText+"SeasonDoub"] = playerYearGameData[playerName][year][gameID]["seasonDoub"]
                tempDict[labelText+"SeasonTrip"] = playerYearGameData[playerName][year][gameID]["seasonTrip"]
                tempDict[labelText+"SeasonHR"] = playerYearGameData[playerName][year][gameID]["seasonHR"]
                tempDict[labelText+"SeasonHits"] = playerYearGameData[playerName][year][gameID]["seasonHits"]
                tempDict[labelText+"SeasonAB"] = playerYearGameData[playerName][year][gameID]["seasonAB"]
                tempDict[labelText+"SeasonWalk"] = playerYearGameData[playerName][year][gameID]["seasonWalk"]
                tempDict[labelText+"SeasonAVG"] = playerYearGameData[playerName][year][gameID]["seasonAVG"]
                ##5game
                tempDict[labelText+"fiveGameSO"] = playerYearGameData[playerName][year][gameID]["fiveGameSO"]
                tempDict[labelText+"fiveGameSingles"] = playerYearGameData[playerName][year][gameID]["fiveGameSingles"]
                tempDict[labelText+"fiveGameDoub"] = playerYearGameData[playerName][year][gameID]["fiveGameDoub"]
                tempDict[labelText+"fiveGameTrip"] = playerYearGameData[playerName][year][gameID]["fiveGameTrip"]
                tempDict[labelText+"fiveGameHR"] = playerYearGameData[playerName][year][gameID]["fiveGameHR"]
                tempDict[labelText+"fiveGameHits"] = playerYearGameData[playerName][year][gameID]["fiveGameHits"]
                tempDict[labelText+"fiveGameAB"] = playerYearGameData[playerName][year][gameID]["fiveGameAB"]
                tempDict[labelText+"fiveGameWalk"] = playerYearGameData[playerName][year][gameID]["fiveGameWalk"]
                tempDict[labelText+"fiveGameAVG"] = playerYearGameData[playerName][year][gameID]["fiveGameAVG"]
                #single game
                tempDict[labelText+"gameSO"] = playerYearGameData[playerName][year][gameID]["gameSO"]
                tempDict[labelText+"gameSingles"] = playerYearGameData[playerName][year][gameID]["gameSingles"]
                tempDict[labelText+"gameDoub"] = playerYearGameData[playerName][year][gameID]["gameDoub"]
                tempDict[labelText+"gameTrip"] = playerYearGameData[playerName][year][gameID]["gameTrip"]
                tempDict[labelText+"gameHR"] = playerYearGameData[playerName][year][gameID]["gameHR"]
                tempDict[labelText+"gameHits"] = playerYearGameData[playerName][year][gameID]["gameHits"]
                tempDict[labelText+"gameAB"] = playerYearGameData[playerName][year][gameID]["gameAB"]
                tempDict[labelText+"gameWalk"] = playerYearGameData[playerName][year][gameID]["gameWalk"]
                
                
            else:
                tempDict[labelText+"SeasonSO"] = 0
                tempDict[labelText+"SeasonSingles"] = 0
                tempDict[labelText+"SeasonDoub"] = 0
                tempDict[labelText+"SeasonTrip"] = 0
                tempDict[labelText+"SeasonHR"] = 0
                tempDict[labelText+"SeasonHits"] = 0
                tempDict[labelText+"SeasonAB"] = 0
                tempDict[labelText+"SeasonWalk"] = 0
                tempDict[labelText+"SeasonAVG"] = 0.000
                ##5game
                tempDict[labelText+"fiveGameSO"] = 0
                tempDict[labelText+"fiveGameSingles"] = 0
                tempDict[labelText+"fiveGameDoub"] = 0
                tempDict[labelText+"fiveGameTrip"] = 0
                tempDict[labelText+"fiveGameHR"] = 0
                tempDict[labelText+"fiveGameHits"] = 0
                tempDict[labelText+"fiveGameAB"] = 0
                tempDict[labelText+"fiveGameWalk"] = 0
                tempDict[labelText+"fiveGameAVG"] = 0.000
                #single game
                tempDict[labelText+"gameSO"] = 0
                tempDict[labelText+"gameSingles"] = 0
                tempDict[labelText+"gameDoub"] = 0
                tempDict[labelText+"gameTrip"] = 0
                tempDict[labelText+"gameHR"] = 0
                tempDict[labelText+"gameHits"] = 0
                tempDict[labelText+"gameAB"] = 0
                tempDict[labelText+"gameWalk"] = 0
                
####Get game data from file, store totals for each game

print("going through file")
for line in eventFile:
    lineSplit = line.split(",")
    tokens = []
    for token in lineSplit:
        token = re.sub(r'^"|"$', '', token)
        tokens.append(token)
    ##Copy tokens from line needed in output
    temp = dict()
    if len(tokens) < 16:
        raise ValueError('event tokens incorrect size')
    ###################################
    ## store per-game-data#####
    date = list(tokens[1])
    yr = date[:2]
    year = ''.join(str(e) for e in yr)
    SO, S, D, T, HR, AB, W = evalEvent(tokens[13])
     
    if tokens[9] in playerYearGameData and year in playerYearGameData[tokens[9]] and tokens[0] in playerYearGameData[tokens[9]][year]:
        ## game data ##
        playerYearGameData[tokens[9]][year][tokens[0]]["GameID"] = tokens[0]
        playerYearGameData[tokens[9]][year][tokens[0]]["Date"] = tokens[1]
        playerYearGameData[tokens[9]][year][tokens[0]]["DayOfWeek"] = tokens[2]
        playerYearGameData[tokens[9]][year][tokens[0]]["StartTime"] = tokens[3]
        playerYearGameData[tokens[9]][year][tokens[0]]["DayNight"] = tokens[4]
        playerYearGameData[tokens[9]][year][tokens[0]]["GameLoc"] = tokens[5]
        playerYearGameData[tokens[9]][year][tokens[0]]["VisStartPit"] = tokens[6]
        playerYearGameData[tokens[9]][year][tokens[0]]["HomeStartPit"] = tokens[7]
        playerYearGameData[tokens[9]][year][tokens[0]]["Temp"] = tokens[8]
        playerYearGameData[tokens[9]][year][tokens[0]]['batter'] = tokens[9]
        playerYearGameData[tokens[9]][year][tokens[0]]['batterHand'] =tokens[10]
        playerYearGameData[tokens[9]][year][tokens[0]]['defPos'] = tokens[14]
        playerYearGameData[tokens[9]][year][tokens[0]]['battingPos'] = tokens[15]
        ## player game data ##
        playerYearGameData[tokens[9]][year][tokens[0]]['gameSO'] += SO
        playerYearGameData[tokens[9]][year][tokens[0]]['gameSingles'] += S
        playerYearGameData[tokens[9]][year][tokens[0]]['gameDoub'] += D
        playerYearGameData[tokens[9]][year][tokens[0]]['gameTrip'] += T
        playerYearGameData[tokens[9]][year][tokens[0]]['gameHR'] += HR
        hits = S+D+T+HR
        playerYearGameData[tokens[9]][year][tokens[0]]['gameHits'] += hits
        playerYearGameData[tokens[9]][year][tokens[0]]['gameAB'] += AB
        playerYearGameData[tokens[9]][year][tokens[0]]['gameWalk'] += W
        
        
    else:#first game of season or first AB of game
        ## game data ##
        if tokens[9] not in playerYearGameData:
            playerYearGameData[tokens[9]] = {}
        if year not in playerYearGameData[tokens[9]]:
            playerYearGameData[tokens[9]][year] = {}
        if tokens[0] not in playerYearGameData[tokens[9]][year]:
            playerYearGameData[tokens[9]][year][tokens[0]] = {}
        playerYearGameData[tokens[9]][year][tokens[0]]["GameID"] = tokens[0]
        playerYearGameData[tokens[9]][year][tokens[0]]["Date"] = tokens[1]
        playerYearGameData[tokens[9]][year][tokens[0]]["DayOfWeek"] = tokens[2]
        playerYearGameData[tokens[9]][year][tokens[0]]["StartTime"] = tokens[3]
        playerYearGameData[tokens[9]][year][tokens[0]]["DayNight"] = tokens[4]
        playerYearGameData[tokens[9]][year][tokens[0]]["GameLoc"] = tokens[5]
        playerYearGameData[tokens[9]][year][tokens[0]]["VisStartPit"] = tokens[6]
        playerYearGameData[tokens[9]][year][tokens[0]]["HomeStartPit"] = tokens[7]
        playerYearGameData[tokens[9]][year][tokens[0]]["Temp"] = tokens[8]
        playerYearGameData[tokens[9]][year][tokens[0]]['batter'] = tokens[9]
        playerYearGameData[tokens[9]][year][tokens[0]]['batterHand'] =tokens[10]
        playerYearGameData[tokens[9]][year][tokens[0]]['defPos'] = tokens[14]
        playerYearGameData[tokens[9]][year][tokens[0]]['battingPos'] = tokens[15]
        ## player game data ##
        playerYearGameData[tokens[9]][year][tokens[0]]['gameSO'] = SO
        playerYearGameData[tokens[9]][year][tokens[0]]['gameSingles'] = S
        playerYearGameData[tokens[9]][year][tokens[0]]['gameDoub'] = D
        playerYearGameData[tokens[9]][year][tokens[0]]['gameTrip'] = T
        playerYearGameData[tokens[9]][year][tokens[0]]['gameHR'] = HR
        hits = S+D+T+HR
        playerYearGameData[tokens[9]][year][tokens[0]]['gameHits'] = hits
        playerYearGameData[tokens[9]][year][tokens[0]]['gameAB'] = AB
        playerYearGameData[tokens[9]][year][tokens[0]]['gameWalk'] = W
        
        
eventFile.close()
##use game totals to get indexed season totals & 5 game stats
print("getting season totals and 5 games")
for playerName in playerYearGameData:
    if playerName not in playerYearGameSeasonData:
        playerYearGameSeasonData[playerName] = {}
    for season in playerYearGameData[playerName]:
        if season not in playerYearGameSeasonData[playerName]:
            playerYearGameSeasonData[playerName][season] = {}
        seasonSO = 0
        seasonSingles=0
        seasonDoub = 0
        seasonTrip = 0
        seasonHR=0
        seasonHits = 0
        seasonAB=0
        seasonWalk = 0
        seasonAVG = 0.000
        prevGameIDs = ["none","none","none","none","none"]
        for game in playerYearGameData[playerName][season]:                        
            if game not in playerYearGameSeasonData[playerName][season]:
                ##set keys for each game ###
                playerYearGameSeasonData[playerName][season][game] = {}
                playerYearGameSeasonData[playerName][season][game]['seasonSO'] = seasonSO
                playerYearGameSeasonData[playerName][season][game]['seasonSingles'] = seasonSingles
                playerYearGameSeasonData[playerName][season][game]['seasonDoub'] = seasonDoub 
                playerYearGameSeasonData[playerName][season][game]['seasonTrip'] = seasonTrip 
                playerYearGameSeasonData[playerName][season][game]['seasonHR'] = seasonHR 
                playerYearGameSeasonData[playerName][season][game]['seasonHits'] = seasonHits 
                playerYearGameSeasonData[playerName][season][game]['seasonAB'] = seasonAB 
                playerYearGameSeasonData[playerName][season][game]['seasonWalk'] = seasonWalk 
                playerYearGameSeasonData[playerName][season][game]['seasonAVG'] = seasonAVG
                playerYearGameSeasonData[playerName][season][game]['fiveGameHits'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameAB'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameSO'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameSingles'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameDoub'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameTrip'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameHR'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameWalk'] = 0
                playerYearGameSeasonData[playerName][season][game]['fiveGameAVG'] = 0.000
                #game ID of previous 5 games
                
            #running season total
            seasonSO += playerYearGameData[playerName][season][game]['gameSO'] 
            seasonSingles += playerYearGameData[playerName][season][game]['gameSingles']
            seasonDoub += playerYearGameData[playerName][season][game]['gameDoub'] 
            seasonTrip += playerYearGameData[playerName][season][game]['gameTrip'] 
            seasonHR += playerYearGameData[playerName][season][game]['gameHR'] 
            seasonHits += playerYearGameData[playerName][season][game]['gameHits'] 
            seasonAB += playerYearGameData[playerName][season][game]['gameAB'] 
            seasonWalk += playerYearGameData[playerName][season][game]['gameWalk'] 
            #set info to current season total
            playerYearGameSeasonData[playerName][season][game]['seasonSO'] += seasonSO
            playerYearGameSeasonData[playerName][season][game]['seasonSingles'] += seasonSingles
            playerYearGameSeasonData[playerName][season][game]['seasonDoub'] += seasonDoub 
            playerYearGameSeasonData[playerName][season][game]['seasonTrip'] += seasonTrip 
            playerYearGameSeasonData[playerName][season][game]['seasonHR'] += seasonHR 
            playerYearGameSeasonData[playerName][season][game]['seasonHits'] += seasonHits 
            playerYearGameSeasonData[playerName][season][game]['seasonAB'] += seasonAB 
            playerYearGameSeasonData[playerName][season][game]['seasonWalk'] += seasonWalk 
            if playerYearGameSeasonData[playerName][season][game]['seasonAB'] != 0:
                playerYearGameSeasonData[playerName][season][game]['seasonAVG'] = float(playerYearGameSeasonData[playerName][season][game]['seasonHits'])/playerYearGameSeasonData[playerName][season][game]['seasonAB']
            
            fivehits, fiveatBats, fiveSO, fiveSingles, fiveDoub, fiveTrip, fiveHR, fiveWalks, fiveAVG = getFiveGameStats(prevGameIDs, playerName, season)
            ##slide games to the right by one ##
            prevGameIDs[4] = prevGameIDs[3]
            prevGameIDs[3] = prevGameIDs[2]
            prevGameIDs[2] = prevGameIDs[1]
            prevGameIDs[1] = prevGameIDs[0]
            prevGameIDs[0] = game
            playerYearGameSeasonData[playerName][season][game]['fiveGameHits'] = fivehits
            playerYearGameSeasonData[playerName][season][game]['fiveGameAB'] = fiveatBats
            playerYearGameSeasonData[playerName][season][game]['fiveGameSO'] = fiveSO
            playerYearGameSeasonData[playerName][season][game]['fiveGameSingles'] = fiveSingles
            playerYearGameSeasonData[playerName][season][game]['fiveGameDoub'] = fiveDoub
            playerYearGameSeasonData[playerName][season][game]['fiveGameTrip'] = fiveTrip
            playerYearGameSeasonData[playerName][season][game]['fiveGameHR'] = fiveHR
            playerYearGameSeasonData[playerName][season][game]['fiveGameWalk'] = fiveWalks
            playerYearGameSeasonData[playerName][season][game]['fiveGameAVG'] = fiveAVG
            
print("sending season data to game data")
for player in playerYearGameSeasonData:
    for season in playerYearGameSeasonData[player]:
        for game in playerYearGameSeasonData[player][season]:
            for key, value in playerYearGameSeasonData[player][season][game].items():
                playerYearGameData[player][season][game][key] = value
                
for player in playerYearGameData:
    for season in playerYearGameData[player]:
        currentGame = 1
        for game in playerYearGameData[player][season]:
            playerYearGameData[player][season][game]['seasonSO'] = float(playerYearGameData[player][season][game]['seasonSO'])/currentGame
            playerYearGameData[player][season][game]['seasonSingles'] = float(playerYearGameData[player][season][game]['seasonSingles'])/currentGame
            playerYearGameData[player][season][game]['seasonDoub'] = float(playerYearGameData[player][season][game]['seasonDoub'])/currentGame 
            playerYearGameData[player][season][game]['seasonTrip'] = float(playerYearGameData[player][season][game]['seasonTrip'])/currentGame 
            playerYearGameData[player][season][game]['seasonHR'] = float(playerYearGameData[player][season][game]['seasonHR'])/currentGame
            playerYearGameData[player][season][game]['seasonHits'] = float(playerYearGameData[player][season][game]['seasonHits'])/currentGame
            playerYearGameData[player][season][game]['seasonAB'] = float(playerYearGameData[player][season][game]['seasonAB'])/currentGame 
            playerYearGameData[player][season][game]['seasonWalk'] = float(playerYearGameData[player][season][game]['seasonWalk'])/currentGame 
            playerYearGameData[player][season][game]['seasonAB'] = float(playerYearGameData[player][season][game]['seasonAB'])/currentGame
            currentGame += 1
            playerYearGameData[player][season][game]['fiveGameHits'] = float(playerYearGameData[player][season][game]['fiveGameHits'])/5
            playerYearGameData[player][season][game]['fiveGameAB'] = playerYearGameData[player][season][game]['fiveGameAB']/5
            playerYearGameData[player][season][game]['fiveGameSO'] = playerYearGameData[player][season][game]['fiveGameSO']/5
            playerYearGameData[player][season][game]['fiveGameSingles'] = playerYearGameData[player][season][game]['fiveGameSingles']/5
            playerYearGameData[player][season][game]['fiveGameDoub'] = playerYearGameData[player][season][game]['fiveGameDoub']/5
            playerYearGameData[player][season][game]['fiveGameTrip'] = playerYearGameData[player][season][game]['fiveGameTrip']/5
            playerYearGameData[player][season][game]['fiveGameHR'] = playerYearGameData[player][season][game]['fiveGameHR']/5
            playerYearGameData[player][season][game]['fiveGameWalk'] = playerYearGameData[player][season][game]['fiveGameWalk']/5
         
print("sending data to game training file")
for line in gameFile:
   lineSplit = line.split(",")
   tokens = []
   for token in lineSplit:
        token = re.sub(r'^"|"$', '', token)
        token.rstrip()
        tokens.append(token)
   date = list(tokens[1])
   yr = date[:2]
   year = ''.join(str(e) for e in yr)
   temp = collections.defaultdict(int)
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
   gameStats(temp, "VisBat1", temp["VisBat1"], year, tokens[0])
   temp["VisBat2"] = tokens[15]
   temp["VisBat2Pos"] = tokens[16]
   gameStats(temp, "VisBat2", temp["VisBat2"], year, tokens[0])
   temp["VisBat3"] = tokens[17]
   temp["VisBat3Pos"] = tokens[18]
   gameStats(temp, "VisBat3", temp["VisBat3"], year, tokens[0])
   temp["VisBat4"] = tokens[19]
   temp["VisBat4Pos"] = tokens[20]
   gameStats(temp, "VisBat4", temp["VisBat4"], year, tokens[0])
   temp["VisBat5"] = tokens[21]
   temp["VisBat5Pos"] = tokens[22]
   gameStats(temp, "VisBat5", temp["VisBat5"], year, tokens[0])
   temp["VisBat6"] = tokens[23]
   temp["VisBat6Pos"] = tokens[24]
   gameStats(temp, "VisBat6", temp["VisBat6"], year, tokens[0])
   temp["VisBat7"] = tokens[25]
   temp["VisBat7Pos"] = tokens[26]
   gameStats(temp, "VisBat7", temp["VisBat7"], year, tokens[0])
   temp["VisBat8"] = tokens[27]
   temp["VisBat8Pos"] = tokens[28]
   gameStats(temp, "VisBat8", temp["VisBat8"], year, tokens[0])
   temp["VisBat9"] = tokens[29]
   temp["VisBat9Pos"] = tokens[30]
   gameStats(temp, "VisBat9", temp["VisBat9"], year, tokens[0])
   temp["HomeBat1"] = tokens[31]
   temp["HomeBat1Pos"] = tokens[32]
   gameStats(temp, "HomeBat1", temp["HomeBat1"], year, tokens[0])
   temp["HomeBat2"] = tokens[33]
   temp["HomeBat2Pos"] = tokens[34]
   gameStats(temp, "HomeBat2", temp["HomeBat2"], year, tokens[0])
   temp["HomeBat3"] = tokens[35]
   temp["HomeBat3Pos"] = tokens[36]
   gameStats(temp, "HomeBat3", temp["HomeBat3"], year, tokens[0])
   temp["HomeBat4"] = tokens[37]
   temp["HomeBat4Pos"] = tokens[38]
   gameStats(temp, "HomeBat4", temp["HomeBat4"], year, tokens[0])
   temp["HomeBat5"] = tokens[39]
   temp["HomeBat5Pos"] = tokens[40]
   gameStats(temp, "HomeBat5", temp["HomeBat5"], year, tokens[0])
   temp["HomeBat6"] = tokens[41]
   temp["HomeBat6Pos"] = tokens[42]
   gameStats(temp, "HomeBat6", temp["HomeBat6"], year, tokens[0])
   temp["HomeBat7"] = tokens[43]
   temp["HomeBat7Pos"] = tokens[44]
   gameStats(temp, "HomeBat7", temp["HomeBat7"], year, tokens[0])
   temp["HomeBat8"] = tokens[45]
   temp["HomeBat8Pos"] = tokens[46]
   gameStats(temp, "HomeBat8", temp["HomeBat8"], year, tokens[0])
   temp["HomeBat9"] = tokens[47]
   temp["HomeBat9Pos"] = tokens[48]
   gameStats(temp, "HomeBat9", temp["HomeBat9"], year, tokens[0])
   gameData.update({tokens[0]:temp})
   
gameFile.close()
   

outEvent = open("eventTrainingWSeasonand5Game.txt", 'w')
print("outputting")
for player in playerYearGameData:
    for season in playerYearGameData[player]:
        for game in playerYearGameData[player][season]:
            outputline = list(playerYearGameData[player][season][game].values())
            for e in outputline:
                e = str(e)
                if e != '\n':
                    outEvent.write(e+",")
            outEvent.write('\n')

    
outEvent.close()

out1Game = open("gameTrainingWSeasonand5Game1.txt", 'w')
out2Game = open("gameTrainingWSeasonand5Game2.txt", 'w')
count =0
for game in gameData:
    outputline = list(gameData[game].values())
    if count % 2 == 0:
        for e in outputline:
            e = str(e)
            e.rstrip()
            out1Game.write(e+",")
        out1Game.write('\n')
    else:
        for e in outputline:
            e = str(e)
            e.rstrip()
            out2Game.write(e+",")
        out2Game.write('\n')
    count+=1
out1Game.close()
out2Game.close()
