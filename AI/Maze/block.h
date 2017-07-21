#ifndef _BLOCK_H
#define _BLOCK_H
#include <fstream>


class Block{
private:
	int row;
	int column;
	char status;
	int cost;
public:
	Block();
	Block(int row, int column, char status, int cost);
	int getX();
	int getY();
	char getStatus();
	int getCost();
	void setStatus(char x);
	void setCost(int x);
	Block* getBlock();
	void printBlock();
};

#endif