# -*- coding: utf-8 -*-
"""
Created on Thu Nov 16 16:51:05 2017

@author: Thomas Lee

The current set of this file runs phase 3: Use predicted player statistics to influence a 
game predicting neural network
"""

from load_file import fileLoader #custom file that must be in the same directory as learn.py
from featureExtract import featureExtract #custom file that must be in the same directory as learn.py
import numpy
import copy
import math
from sklearn.neural_network import MLPClassifier
from sklearn.metrics import mean_absolute_error
from sklearn.feature_extraction import DictVectorizer


fl = fileLoader()
fe = featureExtract()
gameDict = fl.loadFile("gameTrainingRecord.txt",79,99,0,14,15,16)
eventDict = fl.loadsubFile("eventTrainingWSeasonand5Game.txt", 79, 99, 0, 14, 15, 16)

#method that checks to see if the player (batterName)
#is batting at position (batPos) at the start of
#the game (gameID)
def checkPos(batterName, batPos, gameID):
    vis = "VisBat"+str(batPos)
    hom = "HomeBat"+str(batPos)
    
    if gameDict[gameID][vis] == batterName:
        return vis
    elif gameDict[gameID][hom] == batterName:
        return hom
 
#Calculates precision and 
#non-zero percision - displayed as "non-zero Recall"
#prints out the values  
def findPreAndRec(trueY, predY):
    recallTotal = 0
    perfectRecall = 0
    precisionTotal = 0
    perfect = 0
    for t, p in zip(trueY, predY):
        temp = abs(t - p)
        if temp == 0:
            perfect += 1
            if t > 0.0:
                perfectRecall += 1
        if t > 0.0:
            recallTotal += 1
        precisionTotal += 1
        
    print("Total precision: " + str((float(perfect)/precisionTotal)*100) + "%")
    print("Non-Zero recall: " + str((float(perfectRecall)/recallTotal)*100) + "%")

	#calculates the variance and standard deviation of
	#the true and predicted value sets
def findSignificance(trueY, predY):
    avgTrue = 0.0
    avgPred = 0.0
    for idx, value in enumerate(trueY):
        avgTrue += value
    avgTrue = float(avgTrue)/len(trueY)
    for idx, value in enumerate(predY):
        avgPred += value
    avgPred = float(avgPred)/len(predY)
    
    difTrue = 0.0
    for idx, each in enumerate(trueY):
        difTrue += math.pow((each - avgTrue), 2)
    varianceTrue = float(difTrue)/(len(trueY))
    SDTrue = math.sqrt(varianceTrue)
    difPred= 0.0
    for idx, each in enumerate(predY):
        difPred += math.pow((each - avgPred), 2)
    variancePred = float(difPred)/(len(predY))
    SDPred = math.sqrt(variancePred)
    
    """Sd = math.sqrt((math.pow(sqrtTrue, 2)/(len(trueY)) + math.pow(sqrtPred, 2)/(len(predY))))
    tScore = float(abs(avgTrue - avgPred))/Sd
    df = (len(trueY) + len(predY) - 2.0)"""
    
    print("Variance True: " + str(varianceTrue))
    print("SD True: " + str(SDTrue))
    print("Variance Pred: " + str(variancePred))
    print("SD Pred: " + str(SDPred))

#Trains and returns on neural network for one player stat
def playerStat(statDict, trStart, trEnd, valyrStart, valyrEnd, testyrStart, testyrEnd, trainVect, valVect, testVect, statName, hLayer1, hLayer2):
    train, trainY, val, valY, test, testY = fe.featurePlayerPrep(statDict, trStart, trEnd, valyrStart, valyrEnd, testyrStart, testyrEnd, statName)
	#transforms training, validation, and test X vector to floating-point vector 
	#translating nominal value to [1-0]vector representation
    trainX = trainVect.fit_transform(train).toarray()
    valX = valVect.fit_transform(val).toarray()
    testX = testVect.fit_transform(test)
    mlpR = MLPClassifier(hidden_layer_sizes=(hLayer1, hLayer2))
	#train the NN
    mlpR.fit(trainX, trainY)
	#predict validation Y
    predArray = mlpR.predict(valX)
    predArrayY = predArray.tolist()
	#compare predicted validation Y to actual validation Y
    print(statName + " val mean absolute error = "+ str(mean_absolute_error(valY, predArrayY)))
	#add validation to training
    numpy.concatenate((trainX, valX))
    numpy.concatenate((trainY, valY))
	#retrain w/ validation included
    mlpR.fit(trainX, trainY)
	#predict Test Y
    predArray = mlpR.predict(testX)
    predArrayY = predArray.tolist()
	#compare predicted test Y to actual test Y
    print(statName + " test mean absolute error = "+ str(mean_absolute_error(testY, predArrayY)))
    findSignificance(testY, predArrayY)
    findPreAndRec(testY, predArrayY)
	#return the NN for later use
    return mlpR

	#Calculate the win predictor accuracy
def winnerAcc(trueY, predY):
    games = 0
    correct = 0
    for t, p in zip(trueY,predY):
        if t == p:
            correct+=1
        games +=1
    
    print(str((float(correct)/games)*100) + "% winner correctly predicted")
    
#score,winner, and player stat dictionary to vector
vecTrainScore = DictVectorizer()
vecTrainWinner = DictVectorizer()
vecValScore = DictVectorizer()
vecValWinner = DictVectorizer()
vecTestScore = DictVectorizer()
vecTestWinner = DictVectorizer()

vecTrainSO = DictVectorizer()
vecValSO = DictVectorizer()
vecTestSO = DictVectorizer()
vecTrainSingles = DictVectorizer()
vecValSingles = DictVectorizer()
vecTestSingles = DictVectorizer()
vecTrainDoub = DictVectorizer()
vecValDoub = DictVectorizer()
vecTestDoub = DictVectorizer()
vecTrainTrip = DictVectorizer()
vecValTrip = DictVectorizer()
vecTestTrip = DictVectorizer()
vecTrainHR = DictVectorizer()
vecValHR = DictVectorizer()
vecTestHR = DictVectorizer()
vecTrainAB = DictVectorizer()
vecValAB = DictVectorizer()
vecTestAB = DictVectorizer()
vecTrainHits = DictVectorizer()
vecValHits = DictVectorizer()
vecTestHits = DictVectorizer()
vecTrainWalk = DictVectorizer()
vecValWalk = DictVectorizer()
vecTestWalk = DictVectorizer()

testYearEnd = 16
testYearStart = 15
valYearEnd = 14
valYearStart = 0
trYearEnd = 99
trYearStart = 79
################################
# Phase 1                      #
# Predicting player game stats #
################################
#copy eventDict before using to make sure no entries are manipulated
SODict = copy.deepcopy(eventDict)
print("SO")
##strikeout
SONN = playerStat(SODict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainSO, vecValSO, vecTestSO, "gameSO", 26, 10)
del SODict
SinglesDict = copy.deepcopy(eventDict)
##Singles
SinglesNN = playerStat(SinglesDict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainSingles, vecValSingles, vecTestSingles, "gameSingles", 26,26)
del SinglesDict
DoubDict = copy.deepcopy(eventDict)
##doubles
DoubNN = playerStat(DoubDict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainDoub, vecValDoub, vecTestDoub, "gameDoub", 26,10)
del DoubDict
print("Trip")
TripDict = copy.deepcopy(eventDict)
##Triples
TripNN = playerStat(TripDict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainTrip, vecValTrip, vecTestTrip, "gameTrip", 26,26)
del TripDict
HRDict = copy.deepcopy(eventDict)
##Homeruns
HRNN = playerStat(HRDict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainHR, vecValHR, vecTestHR, "gameHR", 52,26)
del HRDict
ABDict = copy.deepcopy(eventDict)
##At-Bats
ABNN = playerStat(ABDict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainAB, vecValAB, vecTestAB, "gameAB", 52,52)
del ABDict
HitsDict = copy.deepcopy(eventDict)
print("Hits")
##Hits
HitsNN = playerStat(HitsDict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainHits, vecValHits, vecTestHits, "gameHits", 52,10)
del HitsDict
WalkDict = copy.deepcopy(eventDict)
##Walks
WalkNN = playerStat(WalkDict, trYearStart, trYearEnd, valYearStart,valYearEnd,testYearStart,testYearEnd, vecTrainWalk, vecValWalk, vecTestWalk, "gameWalk", 52,26)
del WalkDict

########################################################
# This section is for phase 3                          #
# it should be commented out if you are not intending  #
# to use predicted player values to influence your     #
# game NN                                              #
########################################################
for each in eventDict:
    retXvals, retNonvals = fe.finalSeasonPrep(eventDict[each])#get important values for particular player for particular game
    exampleSO = vecTestSO.transform(retXvals).toarray()#transform into array for SO
    SOpred = SONN.predict(exampleSO)#predict number of strikouts he'll get in game
	# do the same for other stats
    exampleSingles = vecTestSingles.transform(retXvals).toarray()
    Singlespred = SinglesNN.predict(exampleSingles)
    exampleDoub = vecTestDoub.transform(retXvals).toarray()
    Doubpred = DoubNN.predict(exampleDoub)
    exampleTrip = vecTestTrip.transform(retXvals).toarray()
    Trippred = TripNN.predict(exampleTrip)
    exampleHR = vecTestHR.transform(retXvals).toarray()
    HRpred = HRNN.predict(exampleHR)
    exampleAB = vecTestAB.transform(retXvals).toarray()
    ABpred = ABNN.predict(exampleAB)
    exampleHits = vecTestHits.transform(retXvals).toarray()
    Hitspred = HitsNN.predict(exampleHits)
    exampleWalk = vecTestWalk.transform(retXvals).toarray()
    Walkpred = WalkNN.predict(exampleWalk)
    #find game and player/batter/batting position and put in predicted stats, etc
    gameID = str(retNonvals[0])
    batterName = str(retNonvals[1])
    batPos = retNonvals[3]
    if gameID in gameDict:
        spot = checkPos(batterName, batPos, gameID)
        spot = str(spot)
        gameDict[gameID][spot+"gameSO"] = SOpred
        gameDict[gameID][spot+"gameSingles"] = Singlespred
        gameDict[gameID][spot+"gameDoub"] = Doubpred
        gameDict[gameID][spot+"gameTrip"] = Trippred
        gameDict[gameID][spot+"gameHR"] = HRpred
        gameDict[gameID][spot+"gameAB"] = ABpred
        gameDict[gameID][spot+"gameHits"] = Hitspred
        gameDict[gameID][spot+"gameWalk"] = Walkpred
    else:
        gameID = gameID[:-1]
        if gameID in gameDict:
            spot = checkPos(batterName, batPos, gameID)
            spot = str(spot)
            gameDict[gameID][spot+"gameSO"] = SOpred
            gameDict[gameID][spot+"gameSingles"] = Singlespred
            gameDict[gameID][spot+"gameDoub"] = Doubpred
            gameDict[gameID][spot+"gameTrip"] = Trippred
            gameDict[gameID][spot+"gameHR"] = HRpred
            gameDict[gameID][spot+"gameAB"] = ABpred
            gameDict[gameID][spot+"gameHits"] = Hitspred
            gameDict[gameID][spot+"gameWalk"] = Walkpred
            

# Get rid of data not for training
#line for phase 3
# comment out for phase 2 and 1
retTrainScore, retTrainScoreY, retValScore, retValScoreY, retTestScore, retTestScoreY, retTrainWinner, retTrainWinnerY, retValWinner, retValWinnerY, retTestWinner, retTestWinnerY = fe.finalGamePrep(gameDict,79,99,0,14,15,16)
#line for phase two
#comment out for phase 3
#retTrainScore, retTrainScoreY, retValScore, retValScoreY, retTestScore, retTestScoreY, retTrainWinner, retTrainWinnerY, retValWinner, retValWinnerY, retTestWinner, retTestWinnerY = fe.featureSnWPrep(gameDict,79,99,0,14,15,16)


# turn data into floating point vector
trainingScoreX = vecTrainScore.fit_transform(retTrainScore).toarray()
valScoreX = vecValScore.fit_transform(retValScore).toarray()
testScoreX = vecTestScore.fit_transform(retTestScore).toarray()
trainingWinnerX = vecTrainWinner.fit_transform(retTrainWinner).toarray()
valWinnerX = vecValWinner.fit_transform(retValWinner).toarray()
testWinnerX = vecTestWinner.fit_transform(retTestWinner).toarray()

#train NN to predict the score a team will achieve
scoreNN = MLPClassifier(hidden_layer_sizes=(20,10,20))
scoreNN.fit(trainingScoreX, retTrainScoreY)
predictedScoreArray = scoreNN.predict(valScoreX)
predictedTrainingScoreY = predictedScoreArray.tolist()
#compare predicted validation Y to actual validation Y
print("score val mean absolute error = " + str(mean_absolute_error(retValScoreY, predictedTrainingScoreY)))
#combine validation and training set 
tsX = numpy.concatenate((trainingScoreX, valScoreX))
tsY = retTrainScoreY + retValScoreY
#retrain
scoreNN.fit(tsX, tsY)
#predict test
predArray = scoreNN.predict(testScoreX)
predScoreY = predArray.tolist()
#compare predicted test Y to actual test Y
print("Score test mean absolute error = "+ str(mean_absolute_error(retTestScoreY, predScoreY)))

#predict winner
winnerNN = MLPClassifier(hidden_layer_sizes=(24))
winnerNN.fit(trainingWinnerX, retTrainWinnerY)
#predicted validation set
predictedWinnerArray = winnerNN.predict(valWinnerX)
predictedWinnerY = predictedWinnerArray.tolist()
#compare predicted validation Y to actual validation Y
winnerAcc(retValWinnerY, predictedWinnerY)
#add validation to training
twX = numpy.concatenate((trainingWinnerX, valWinnerX))
twY = retTrainWinnerY + retValWinnerY
#retrain
winnerNN.fit(twX, twY)
#predict test
predArray = winnerNN.predict(testWinnerX)
predWinnerY = predArray.tolist()
#compare predicted test Y to actual test Y
winnerAcc(retTestWinnerY, predWinnerY)


#calculate score predictive accuracy    
perfect = 0
withinOne = 0
withinTwo = 0
more = 0
count = 0
for idx, val in enumerate(predScoreY):
    dif = abs(val - retTestScoreY[idx])
    if dif == 0:
        perfect +=1
    elif dif == 1:
        withinOne +=1
    elif dif == 2:
        withinTwo +=1
    else:
        more+=1
    count+=1

            
print(str((float(perfect)/(count))*100)+"% perfect prediction")
print(str((float(withinOne)/(count))*100)+"% within one prediction")
print(str((float(withinTwo)/(count))*100)+"% within two prediction")
print(str((float(more)/(count))*100)+"% more than two prediction")

        










