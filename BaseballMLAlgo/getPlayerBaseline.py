# -*- coding: utf-8 -*-
"""
Created on Fri Nov 24 13:51:41 2017

@author: Thomas Lee

"""

from load_file import fileLoader

fl = fileLoader()

gameData = fl.loadsubFile("eventTrainingWSeasonand5Game.txt", 79,99,0,14,15,16)
         
       
count = 0
correct = 0
SOacc = 0
SOABSmeanError = 0.0
SORecallCorrect = 0
SORecallTotal = 0
Hitsacc = 0
HitsABSmeanError = 0.0
HitsRecallCorrect = 0
HitsRecallTotal = 0
ABacc = 0
ABABSmeanError = 0.0
ABRecallCorrect = 0
ABRecallTotal = 0
Singlesacc = 0
SinglesABSmeanError = 0.0
SinglesRecallCorrect = 0
SinglesRecallTotal = 0
Doubacc = 0
DoubABSmeanError = 0.0
DoubRecallCorrect = 0
DoubRecallTotal = 0
Tripacc = 0
TripABSmeanError = 0.0
TripRecallCorrect = 0
TripRecallTotal = 0
HRacc = 0
HRABSmeanError = 0.0
HRRecallCorrect = 0
HRRecallTotal = 0
Walkacc = 0
WalkABSmeanError = 0.0
WalkRecallCorrect = 0
WalkRecallTotal = 0

for each in gameData:
    
   year = gameData[each]["year"]
   if year == 15 or year == 16:
       predictedSO = int(float(gameData[each]["fiveGameSO"]))
       predictedHits = int(float(gameData[each]["fiveGameHits"]))
       predictedAB = int(float(gameData[each]["fiveGameAB"]))
       predictedSingles = int(float(gameData[each]["fiveGameSingles"]))
       predictedDoub = int(float(gameData[each]["fiveGameDoub"]))
       predictedTrip = int(float(gameData[each]["fiveGameTrip"]))
       predictedWalk = int(float(gameData[each]["fiveGameWalk"]))
       predictedHR = int(float(gameData[each]["fiveGameHR"]))
       
       gameSO = int(float(gameData[each]["gameSO"]))
       gameHits = int(float(gameData[each]["gameHits"]))
       gameAB = int(float(gameData[each]["gameAB"]))
       gameSingles = int(float(gameData[each]["gameSingles"]))
       gameDoub = int(float(gameData[each]["gameDoub"]))
       gameTrip = int(float(gameData[each]["gameTrip"]))
       gameWalk = int(float(gameData[each]["gameWalk"]))
       gameHR = int(float(gameData[each]["gameHR"]))
       
       if gameSO > 0:
           SORecallTotal += 1
       difSO = abs(predictedSO - gameSO)
       SOABSmeanError += difSO
       if difSO == 0.0:
           SOacc += 1
           if predictedSO > 0:
               SORecallCorrect += 1       
       
       if gameHits > 0:
           HitsRecallTotal += 1
       difHits = abs(predictedHits - gameHits)
       HitsABSmeanError += difHits
       if difHits == 0:
           Hitsacc += 1
           if predictedHits > 0:
               HitsRecallCorrect += 1
       
       if gameAB > 0:
           ABRecallTotal += 1
       difAB = abs(predictedAB - gameAB)
       ABABSmeanError += difAB
       if difAB == 0:
           ABacc += 1
           if predictedAB > 0:
               ABRecallCorrect += 1
       
       if gameSingles > 0:
           SinglesRecallTotal += 1
       difSingles = abs(predictedSingles - gameSingles)
       SinglesABSmeanError += difSingles
       if difSingles == 0:
           Singlesacc += 1
           if predictedSingles > 0:
               SinglesRecallCorrect += 1       
       
       if gameDoub > 0:
           DoubRecallTotal += 1
       difDoub = abs(predictedDoub - gameDoub)
       DoubABSmeanError += difDoub
       if difDoub == 0:
           Doubacc += 1
           if predictedDoub > 0:
               DoubRecallCorrect += 1
       
       if gameTrip > 0:
           TripRecallTotal += 1
       difTrip = abs(predictedTrip - gameTrip)
       TripABSmeanError += difTrip
       if difTrip == 0:
           Tripacc += 1
           if predictedTrip > 0:
               TripRecallCorrect += 1       
       
       if gameHR > 0:
           HRRecallTotal += 1
       difHR = abs(predictedHR - gameHR)
       HRABSmeanError += difHR
       if difHR == 0:
           HRacc += 1
           if predictedHR > 0:
               HRRecallCorrect += 1
       
       if gameWalk > 0:
           WalkRecallTotal += 1
       difWalk = abs(predictedWalk - gameWalk)
       WalkABSmeanError += difWalk
       if difWalk == 0:
           Walkacc += 1
           if predictedWalk > 0:
               WalkRecallCorrect += 1
          
       count+=1
   
   
SOPerfAccuracy = (float(SOacc)/count)*100
SORecall = (float(SORecallCorrect)/SORecallTotal)*100
SOMeanError = (float(SOABSmeanError)/count)
HitsAccuracy = (float(Hitsacc)/count)*100
HitsRecall = (float(HitsRecallCorrect)/HitsRecallTotal)*100
HitsMeanError = (float(HitsABSmeanError)/count)
ABAccuracy = (float(ABacc)/count)*100
ABRecall = (float(ABRecallCorrect)/ABRecallTotal)*100
ABMeanError = (float(ABABSmeanError)/count)
SinglesAccuracy = (float(Singlesacc)/count)*100
SinglesRecall = (float(SinglesRecallCorrect)/SinglesRecallTotal)*100
SinglesMeanError = (float(SinglesABSmeanError)/count)
DoubleAccuracy = (float(Doubacc)/count)*100
DoubleRecall = (float(DoubRecallCorrect)/DoubRecallTotal)*100
DoubleMeanError = (float(DoubABSmeanError)/count)
TripAccuracy = (float(Tripacc)/count)*100
TripRecall = (float(TripRecallCorrect)/TripRecallTotal)*100
TripMeanError = (float(TripABSmeanError)/count)
HRAccuracy = (float(HRacc)/count)*100
HRRecall = (float(HRRecallCorrect)/HRRecallTotal)*100
HRMeanError = (float(HRABSmeanError)/count)
WalkAccuracy = (float(Walkacc)/count)*100   
WalkRecall = (float(WalkRecallCorrect)/WalkRecallTotal)*100
WalkMeanError = (float(WalkABSmeanError)/count)
       


print(str(SOPerfAccuracy)+"% SO Accuracy")
print(str(SORecall)+"% SO Non-Zero Recall")
print(str(SOMeanError)+ "SO mean error")
print(str(HitsAccuracy)+"% Hits Accuracy")
print(str(HitsRecall) + "% Hits Non-Zero Recall")
print(str(HitsMeanError)+ " Hits mean error")
print(str(ABAccuracy)+"% AB Accuracy")
print(str(ABRecall) + "% AB Non-Zero Recall")
print(str(ABMeanError)+" AB mean error")
print(str(SinglesAccuracy)+"% Singles Accuracy")
print(str(SinglesRecall)+"% Singles Non-Zero Recall")
print(str(SinglesMeanError)+" Singles mean error")
print(str(DoubleAccuracy)+"% Doubles Accuracy")
print(str(DoubleRecall)+"% Double Non-Zero Recall")
print(str(DoubleMeanError)+" Double mean error")
print(str(TripAccuracy)+"% Triples Accuracy")
print(str(TripRecall)+"% Triples Non-Zero Recall")
print(str(TripMeanError)+" Trip mean error")
print(str(HRAccuracy)+"% HR Accuracy")
print(str(HRRecall)+"% HR Non-Zero Recall")
print(str(HRMeanError)+" HR mean error")
print(str(WalkAccuracy)+"% Walk Accuracy")
print(str(WalkRecall)+"% Walk Non-Zero Recall")
print(str(WalkMeanError)+" Walk mean error")

