#include "grid.h"
#include <iostream>


using namespace std;

int main(){
	//create various grids
	Grid gDiscount9;//Discount .9
	Grid gDiscount2;//Discount .2
	Grid gLearn9;//Learning rate .9
	Grid gLearn1;//Learning rate .1
	Grid gradLevelDiscount9;//Grad level Discount .9
	Grid gradLevelDiscount2;//Grad level Discount .2
	Grid gradLevelLearn9;//Grad level learning rate .9
	Grid gradLevelLearn1;//Grad level learning rate .1

	//set grid values
	gDiscount2.setGrayAndStartGoal();
	gDiscount9.setGrayAndStartGoal();
	gLearn1.setGrayAndStartGoal();
	gLearn9.setGrayAndStartGoal();
	
	//run training
	gDiscount9.search(.5, .9);
	gDiscount9.learnedPath();
	gDiscount9.printBlocks("AI_Discount_9.txt");
	
	gDiscount2.search(.5, .2);
	gDiscount2.learnedPath();
	gDiscount2.printBlocks("AI_Discount_2.txt");

	gLearn9.search(.9, .5);
	gLearn9.learnedPath();
	gLearn9.printBlocks("AI_Learn_9.txt");

	gLearn1.search(.1, .5);
	gLearn1.learnedPath();
	gLearn1.printBlocks("AI_Learn1.txt");

	//graduate level
	//set grid values
	gradLevelDiscount2.setGrayAndStartGoal();
	gradLevelDiscount9.setGrayAndStartGoal();
	gradLevelLearn1.setGrayAndStartGoal();
	gradLevelLearn9.setGrayAndStartGoal();

	//run training
	gradLevelLearn1.gradSearch(.1, .5);
	gradLevelLearn1.learnedPath();
	gradLevelLearn1.printBlocks("AI_Learn1_Grad.txt");

	gradLevelDiscount9.gradSearch(.5, .9);
	gradLevelDiscount9.learnedPath();
	gradLevelDiscount9.printBlocks("AI_Discount_9_Grad.txt");
	

	gradLevelDiscount2.gradSearch(.5, .2);
	gradLevelDiscount2.learnedPath();
	gradLevelDiscount2.printBlocks("AI_Discount_2_Grad.txt");

	gradLevelLearn9.gradSearch(.9, .5);
	gradLevelLearn9.learnedPath();
	gradLevelLearn9.printBlocks("AI_Learn_9_Grad.txt");

	return 0;

}