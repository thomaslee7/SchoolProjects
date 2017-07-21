#include "block.h"


class Maze{
public:
	Maze(int startX, int startY, int goalOneX, int goalOneY, int goalTwoX, int goalTwoY);
	
	Block** mazeGrid;
	void setWalls();
	void setWater();
	void setCost();
	bool isBorder(Block block);
	bool isWall(Block block);
};