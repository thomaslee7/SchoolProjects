#include "grid.h"
#include <vector>
#include <iostream>
#include <iomanip>
#include <fstream>
#include <algorithm>
#include <string>
#include <random>

using namespace std;
ofstream output;

Grid::Grid(){
	//Grid constructor sets up multidimensional array as grid, sets border walls
	for (int i = 0; i < 9; i++){
		vector<Block*> row;
		for (int j = 0; j < 15; j++){
			Block* p = new Block();
			p->x = i + 1;
			p->y = j + 1;
			if (i == 0 || i == 8){
				p->type = 'g';
				p->reward = -1;
			}
			else if (j == 0 || j == 14){
				p->type = 'g';
				p->reward = -1;
			}

			row.push_back(p);
			
		}
		gridfield.push_back(row);
	}
}
Grid::~Grid(){
	for (int i = 0; i < 9; i++){
		for (int j = 0; j < 15; j++){
			delete gridfield[i][j];
		}
	}
}
void Grid::setGrayAndStartGoal(){
	//sets start, goal, and internal walls
	gridfield[3][4]->type = 'g';
	gridfield[3][10]->type = 'g';
	gridfield[4][1]->type = 'g';
	gridfield[4][2]->type = 'g';
	gridfield[4][3]->type = 'g';
	gridfield[4][4]->type = 'g';
	gridfield[4][10]->type = 'g';
	gridfield[4][11]->type = 'g';
	gridfield[4][12]->type = 'g';
	gridfield[4][13]->type = 'g';
	gridfield[5][10]->type = 'g';

	//goal
	gridfield[6][2]->type = 'G';
	//start
	gridfield[2][12]->type = 'S';

	gridfield[3][4]->reward = -1;
	gridfield[3][10]->reward = -1;
	gridfield[4][1]->reward = -1;
	gridfield[4][2]->reward = -1;
	gridfield[4][3]->reward = -1;
	gridfield[4][4]->reward = -1;
	gridfield[4][10]->reward = -1;
	gridfield[4][11]->reward = -1;
	gridfield[4][12]->reward = -1;
	gridfield[4][13]->reward = -1;
	gridfield[5][10]->reward = -1;

	//goal
	gridfield[6][2]->reward = 3;
	//start
	gridfield[2][12]->reward = 0;
}



void Grid::search(float alpha, float gamma){
	//undergrad training, passes learning rate and discount
	
	int count = 0;
	//sets row and column to Start node position
	int row = 2;
	int column = 12;
	Block* currentState;	
	Block* toState;
	
	//find the goal state 1000 times
	while (count < 1000){
		row = 2;
		column = 12;
		currentState = gridfield[row][column];//start
		while (currentState != gridfield[6][2]){//while you're not in the goal state.
			int randomAction = rand() % 4 + 1;
			//randome action, LEFT RIGHT UP DOWN
			if (randomAction % 4 == 0){
				//up
				toState = gridfield[row - 1][column];//state you are going to move to
				//update the Q you just chose from the state you just left
				gridfield[row][column]->up = updateQ(currentState, toState, "up", alpha, gamma);
				//if its not a wall, actually move to the toState.
				if (!isWall(toState)){
					currentState = toState;
					row--;
				}
			}
			else if (randomAction % 4 == 1){
				//down
				toState = gridfield[row + 1][column];
				gridfield[row][column]->down = updateQ(currentState, toState, "down", alpha, gamma);
				if (!isWall(toState)){
					currentState = toState;
					row++;
				}
			}
			else if (randomAction % 4 == 2){
				//left
				toState = gridfield[row][column - 1];
				gridfield[row][column]->left = updateQ(currentState, toState, "left", alpha, gamma);
				if (!isWall(toState)){
					currentState = toState;
					column--;
				}
			}
			else if (randomAction % 4 == 3){
				//right
				toState = gridfield[row][column + 1];
				gridfield[row][column]->right = updateQ(currentState, toState, "right", alpha, gamma);
				if (!isWall(toState)){
					currentState = toState;
					column++;
				}
			}
			
		}
		count++;
	}
}

void Grid::gradSearch(float alpha, float gamma){

		//grad training
		int count = 0;
		int row = 2;
		int column = 12;
		gridfield[7][6]->type = 'P';//pizza place!!!!!
		gridfield[7][6]->reward = 2;
		//various reward changes depending on learning rate and discount.
		if (gamma <= .9 && gamma > .3 && alpha > .2)
			gridfield[6][2]->reward = 20;//change goal
		else if (gamma < .9 && gamma < .3)
			gridfield[6][2]->reward = 100;
		else if (alpha < .2){
			gridfield[6][2]->reward = 20;
		}
		else
			gridfield[6][2]->reward = 20;

		Block* currentState;
		Block* toState;

		//find the goal state 100 times
		while (count < 100){
			row = 2;
			column = 12;
			currentState = gridfield[row][column];//start
			while (currentState != gridfield[6][2]){
				int randomAction = rand() % 6 + 1;			
				//random action, LEFT RIGHT UP DOWN UPLEFT UPRIGHT
				if (randomAction % 6 == 0){
					//up
					toState = gridfield[row - 1][column];
					gridfield[row][column]->up = updateQGrad(currentState, toState, "up", alpha, gamma);

					if (!isWall(toState)){
						currentState = toState;
						row--;
					}
				}
				else if (randomAction % 6 == 1){
					//upright
					toState = gridfield[row - 1][column + 1];
					gridfield[row][column]->upRight = updateQGrad(currentState, toState, "upRight", alpha, gamma);
					if (!isWall(toState)){
						currentState = toState;
						row--;
						column++;
					}
				}
				else if (randomAction % 6 == 2){
					//down
					toState = gridfield[row + 1][column];
					gridfield[row][column]->down = updateQGrad(currentState, toState, "down", alpha, gamma);
					if (!isWall(toState)){
						currentState = toState;
						row++;
					}
				}
				else if (randomAction % 6 == 3){
					//upLeft
					toState = gridfield[row - 1][column - 1];
					gridfield[row][column]->upLeft = updateQGrad(currentState, toState, "upLeft", alpha, gamma);
					if (!isWall(toState)){
						currentState = toState;
						row--;
						column--;
					}
				}
				else if (randomAction % 6 == 4){
					//left
					toState = gridfield[row][column - 1];
					gridfield[row][column]->left = updateQGrad(currentState, toState, "left", alpha, gamma);
					if (!isWall(toState)){
						currentState = toState;
						column--;
					}
				}
				else if (randomAction % 6 == 5){
					//right
					toState = gridfield[row][column + 1];
					gridfield[row][column]->right = updateQGrad(currentState, toState, "right", alpha, gamma);
					if (!isWall(toState)){
						currentState = toState;
						column++;
					}
				}

			}
			count++;
		}
	}


float Grid::maxQ(Block* x){
	//Returns highest Q value of state
	//precedence for equal values: UP > DOWN > LEFT > RIGHT
	float maxQ = x->up;
	if (x->down > maxQ)
		maxQ = x->down;
	if (x->left > maxQ)
		maxQ = x->left;
	if (x->right > maxQ)
		maxQ = x->right;

	return maxQ;	
}
float Grid::maxQGrad(Block* x){
	//adds upleft and upright for grad training
	float maxQ = x->up;
	if (x->upLeft > maxQ)
		maxQ = x->upLeft;
	if (x->upRight > maxQ)
		maxQ = x->upRight;
	if (x->down > maxQ)
		maxQ = x->down;
	if (x->left > maxQ)
		maxQ = x->left;
	if (x->right > maxQ)
		maxQ = x->right;

	return maxQ;
}
string Grid::maxDirection(Block* x){
	//for outputing final path
	//returns direction of highest Q
	float max = x->up;
	string direction = "up";
	if (x->upLeft > max){
		max = x->upLeft;
		direction = "upLeft";
	}
	if (x->upRight > max){
		max = x->upRight;
		direction = "upRight";
	}
	if (x->down > max){
		max = x->down;
		direction = "down";
	}
	if (x->left > max){
		max = x->left;
		direction = "left";
	}
	if (x->right > max){
		max = x->right;
		direction = "right";
	}

	return direction;
}

float Grid::updateQ(Block* from, Block* to, string direction, float alpha, float gamma){
	//undergrad updateQ
	//depending on direction
	float newQ = 0;
	if (direction == "up"){
		//newQ = (1-alpha) * the current Q + alpha * (reward of toState + (discount)*(highest Q of toState))
		newQ = ((1 - alpha)*from->up) + alpha*(to->reward + (gamma * maxQ(to)));
		return newQ;
	}
	else if (direction == "down"){
		newQ = ((1 - alpha)*from->down) + alpha*(to->reward + (gamma * maxQ(to)));
		return newQ;
	}
	else if (direction == "left"){
		newQ = ((1 - alpha)*from->left) + alpha*(to->reward + (gamma * maxQ(to)));
		return newQ;
	}
	else if (direction == "right"){
		newQ = ((1 - alpha)*from->right) + alpha*(to->reward + (gamma * maxQ(to)));
		return newQ;
	}

}
float Grid::updateQGrad(Block* from, Block* to, string direction, float alpha, float gamma){
	//same as undergrad but adds upleft and upright directions
	float newQ = 0;
	if (direction == "up"){
		newQ = ((1 - alpha)*from->up) + alpha*(to->reward + (gamma * maxQGrad(to)));
		return newQ;
	}
	else if (direction == "upRight"){
		newQ = ((1 - alpha)*from->upRight) + alpha*(to->reward + (gamma * maxQGrad(to)));
		return newQ;
	}
	else if (direction == "down"){
		newQ = ((1 - alpha)*from->down) + alpha*(to->reward + (gamma * maxQGrad(to)));
		return newQ;
	}
	else if (direction == "upLeft"){
		newQ = ((1 - alpha)*from->upLeft) + alpha*(to->reward + (gamma * maxQGrad(to)));
		return newQ;
	}
	else if (direction == "left"){
		newQ = ((1 - alpha)*from->left) + alpha*(to->reward + (gamma * maxQGrad(to)));
		return newQ;
	}
	else if (direction == "right"){
		newQ = ((1 - alpha)*from->right) + alpha*(to->reward + (gamma * maxQGrad(to)));
		return newQ;
	}
}

bool Grid::isWall(Block* state){
	//returns true if state is a gray block 
	if (state->type == 'g')
		return true;
	else
		return false;
}

void Grid::learnedPath(){
	int row = 2;
	int column = 12;
	//start state
	Block* p = gridfield[row][column];
	//while not in the goal state
	//find the highest Q, move that direction
	//then add that state to the path
	while (p != gridfield[6][2]){
		if (maxDirection(p) == "up"){
			path.push_back(p);
			p = gridfield[row - 1][column];
			row--;
		}
		else if (maxDirection(p) == "upLeft"){
			path.push_back(p);
			p = gridfield[row - 1][column - 1];
			row--;
			column--;
		}
		else if (maxDirection(p) == "upRight"){
			path.push_back(p);
			p = gridfield[row - 1][column + 1];
			row--;
			column++;
		}
		else if (maxDirection(p) == "down"){
			path.push_back(p);
			p = gridfield[row + 1][column];
			row++;
		}
		else if (maxDirection(p) == "left"){
			path.push_back(p);
			p = gridfield[row][column - 1];
			column--;
		}
		else if (maxDirection(p) == "right"){
			path.push_back(p);
			p = gridfield[row][column + 1];
			column++;
		}
	}
	//then add goal state to the path
	p = gridfield[6][2];
	path.push_back(p);

}



void Grid::printBlocks(string fileName){
	//output final Q values and learned path to the terminal and to an output file 'fileName'
	output.open(fileName, ios_base::app);
	cout.setf(ios::fixed, ios::floatfield);
	output.setf(ios::fixed, ios::floatfield);
	//output precision to 10 because some of the values are super tiny
	cout.precision(10);
	output.precision(10);
	for (int i = 0; i < 9; i++){
		cout << "Row: " << i + 1 << endl;
		output << "Row: " << i + 1 << endl;
		for (int j = 0; j < 15; j++){
			cout << "        " << gridfield[i][j]->up << endl;
			cout << gridfield[i][j]->left << "          " << gridfield[i][j]->right << " column: " << j + 1 << endl;
			cout << "        " << gridfield[i][j]->down << endl;
			cout << "---------------------------" << endl;

			output << "        " << gridfield[i][j]->up << endl;
			output << gridfield[i][j]->left << "          " << gridfield[i][j]->right << " column: " << j + 1 << endl;
			output << "        " << gridfield[i][j]->down << endl;
			output << "---------------------------" << endl;
		}
	}
	cout << "Learned Path:" << endl;
	output << "Learned Path:" << endl;
	for (auto i = path.begin(); i != path.end() - 1; i++){
		cout << "<" << (*i)->x << "," << (*i)->y << ">  -->  ";
		output << "<" << (*i)->x << "," << (*i)->y << ">  -->  ";		
	}
	cout << "<" << 7 << "," << 3 << ">";
	output << "<" << 7 << "," << 3 << ">";
	output.close();
}