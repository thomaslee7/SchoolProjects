#ifndef GRID_H
#define GRID_H

#include <vector>
#include <string>
#include "block.h"

enum action { UP, DOWN, LEFT, RIGHT };

class Grid{
public:
	Grid();
	~Grid();
	std::vector<std::vector<Block*> > gridfield;
	std::vector<std::vector<action> > Q;
	std::vector<Block*> path;
	bool isWall(Block* state);
	void printGrid();
	void setGrayAndStartGoal();
	void search(float alpha, float gamma);
	void gradSearch(float alpha, float gamma);
	float maxQ(Block* x);
	std::string maxDirection(Block* x);
	float updateQ(Block* from, Block* to, std::string direction, float alpha, float gamma);
	float updateQGrad(Block* from, Block* to, std::string direction, float alpha, float gamma);
	float maxQGrad(Block* x);
	void learnedPath();
	void printBlocks(std::string fileName);

	

};

#endif