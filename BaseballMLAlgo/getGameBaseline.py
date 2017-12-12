# -*- coding: utf-8 -*-
"""
Created on Sat Nov 18 22:50:47 2017

@author: Thomas Lee
"""

from load_file import fileLoader

fl = fileLoader()

gameData = fl.loadFile("gameTrainingRecord.txt",79,99,0,14,15,16)


def predictScore(teamName, year):
    return int(float(runData[teamName][year]["Runs"])/runData[teamName][year]["Games"])

runData = dict(dict(dict()))
for each in gameData: 
    team = gameData[each]["HomeTeam"]
    year = gameData[each]["year"]
    if team in runData and year in runData[team]:
       runData[team][year]["Runs"] = 0
       runData[team][year]["Games"] = 1        
    else:
        if team not in runData:
            runData[team] = {}
        if year not in runData[team]:
            runData[team][year] = {}
        runData[team][year]["Runs"] = 0
        runData[team][year]["Games"] = 1
          
       
count = 0
totalError = 0
correct = 0
visPerfectPredicted = 0
visWithinOnePredicted = 0
visWithinTwoPredicted = 0
visGreaterThanTwoPredicted = 0
homePerfectPredicted = 0
homeWithinOnePredicted = 0
homeWithinTwoPredicted = 0
homeGreaterThanTwoPredicted = 0
for each in gameData:    
    year = gameData[each]["year"]
    #predict winner
    if year == "15" or year == "16":
        homeTeam = gameData[each]["HomeTeam"]
        homeTeamScore = int(float(gameData[each]["HomeFinalScore"]))
        hWins = int(float(gameData[each]["HomeWins"]))
        hLosses = int(float(gameData[each]["HomeLosses"]))
        hTies = int(float(gameData[each]["HomeTies"]))
        visTeam = gameData[each]["VisTeam"]
        visTeamScore = int(float(gameData[each]["VisFinalScore"]))
        vWins = int(float(gameData[each]["VisWins"]))
        vLosses = int(float(gameData[each]["VisLosses"]))
        vTies = int(float(gameData[each]["VisTies"]))
        actualWinner = ""
        predictedWinner = homeTeam
        if homeTeamScore > visTeamScore:
            actualWinner = homeTeam
        elif visTeamScore > homeTeamScore:
            actualWinner = visTeam
        else:
            actualWinner = "Tie"
        if predictedWinner == actualWinner:
            correct += 1
        count += 1
        
        # predict scores
        visPredictedScore = predictScore(visTeam, year)
        homePredictedScore = predictScore(homeTeam, year)
        runData[homeTeam][year]["Runs"] += homeTeamScore
        runData[homeTeam][year]["Games"] += 1
        runData[visTeam][year]["Runs"] += visTeamScore
        runData[visTeam][year]["Games"] += 1
        visDif = abs(visPredictedScore - visTeamScore)
        homeDif = abs(homePredictedScore - homeTeamScore)
        totalError += (visDif + homeDif)
       # visiting team
        if visDif == 0:
            visPerfectPredicted +=1
        elif visDif == 1:
            visWithinOnePredicted +=1
        elif visDif == 2:
            visWithinTwoPredicted +=1
        else:
            visGreaterThanTwoPredicted +=1
        #home team
        if homeDif == 0:
            homePerfectPredicted +=1
        elif homeDif == 1:
            homeWithinOnePredicted +=1
        elif homeDif == 2:
            homeWithinTwoPredicted +=1
        else:
            homeGreaterThanTwoPredicted +=1
        
    
        
winnerAccuracy = float(correct)/count
winnerAccuracy = winnerAccuracy*100
format(winnerAccuracy, '.2f')
print(str(winnerAccuracy)+"% Winner Prediction Accuracy")

meanABSError = (float(totalError)/(count*2))
print(str(meanABSError)+" Mean Absolute Error")

perfect = visPerfectPredicted + homePerfectPredicted
one = visWithinOnePredicted + homeWithinOnePredicted
two = visWithinTwoPredicted + homeWithinTwoPredicted
more = visGreaterThanTwoPredicted + homeGreaterThanTwoPredicted


PerfectAccuracy = (float(perfect)/(count*2))*100
OneAccuracy = (float(one)/(count*2))*100
TwoAccuracy = (float(two)/(count*2))*100
MoreAccuracy = (float(more)/(count*2))*100

print(str(PerfectAccuracy)+"% Perfect Visitor Score Accuracy")
print(str(OneAccuracy)+"% Within One Visitor Score Accuracy")
print(str(TwoAccuracy)+"% Within Two Visitor Score Accuracy")
print(str(MoreAccuracy)+"% More Than Two Visitor Score Accuracy")

