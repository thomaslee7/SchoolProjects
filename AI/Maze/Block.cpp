#include "block.h"
#include <iostream>




///Block Class
///A block is the data type used to represent each grid square in the maze
//Functions include basic getters and setters, a function to return a pointer
///to itself, and a print function that is called by the maze class to 
///print the entire grid
Block::Block(){
	row = column = cost = 0;
	status = 'w';
}

Block::Block(int x, int y, char stat, int cos){
	row = y;
	column = x;
	status = stat;
	cost = cos;
}
int Block::getX(){
	return column;
}

int Block::getY(){
	return row;
}

char Block::getStatus(){
	return status;
}

int Block::getCost(){
	return cost;
}
void Block::setStatus(char x){
	status = x;
}
void Block::setCost(int x){
	cost = x;
}
Block* Block::getBlock(){
	Block* p = this;
	return p;
}
void Block::printBlock(){
	std::cout << "|";
	switch (status){
	case('s') :
		std::cout << " " << 'S' << " ";
		break;
	case('g') :
		std::cout << " " << 'G' << " ";
		break;
	case('w') :
		std::cout << "   ";
		break;
	case('x') :
		std::cout << "|||";
		break;
	case('y') :
		std::cout << "~~~";
		break;
	default:
		std::cout << "   ";
	}
	std::cout << "|";

}