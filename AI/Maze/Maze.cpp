#include "block.h"
#include "maze.h"
///The maze class creates and controlls the maze grid set
///arguments are x and y values for a start node and two goal nodes
///provided constructor creates a 12 x 10 grid with the three passed nodes,
///all other nodes are set to a blank block with a cost of one

Maze::Maze(int startX, int startY, int goalOneX, int goalOneY, int goalTwoX, int goalTwoY){
	mazeGrid = new Block*[10];
	for (int i = 0; i < 10; i++){
		mazeGrid[i] = new Block[12];
	}
	for (int i = 0; i < 10;i++){
		for (int j = 0; j < 12; j++){
			mazeGrid[i][j] = Block::Block(0, 0, 'x',0);
		}
	}
	for (int i = 0; i < 10; i++){
		for (int j = 0; j < 12; j++){
			if (i == startY && j == startX)
				mazeGrid[i][j] = Block::Block(startX, startY, 's', 0);
			else if (i == goalOneY && j == goalOneX)
				mazeGrid[i][j] = Block::Block(goalOneX, goalOneY, 'g', 1);
			else if (i == goalTwoY && j == goalTwoX)
				mazeGrid[i][j] = Block::Block(goalTwoX, goalTwoY, 'g', 1);
			else
				mazeGrid[i][j] = Block::Block(j, i, 'w', 1);
		}
	}

}
///SetCost() must be called after the constructor
///it iterates through the grid and sets all the costs of water blocks 
///to two and wall blocks to 1000. The 1000 is aribitrary, is should never matter
void Maze::setCost(){
	for (int i = 0; i < 10; i++){
		for (int j = 0; j < 12; j++){
			if (mazeGrid[i][j].getStatus() == 'y')
				mazeGrid[i][j].setCost(2);
			else if (mazeGrid[i][j].getStatus() == 'x')
				mazeGrid[i][j].setCost(1000);
		}
	}
}
///the setWalls function explicitly sets walls in the maze
///any custom wall layout will need to be explicitly entered
void Maze::setWalls(){
	mazeGrid[0][3].setStatus('x');
	mazeGrid[2][1].setStatus('x');
	mazeGrid[2][4].setStatus('x');
	mazeGrid[2][5].setStatus('x');
	mazeGrid[2][7].setStatus('x');
	mazeGrid[2][8].setStatus('x');
	mazeGrid[3][1].setStatus('x');
	mazeGrid[3][5].setStatus('x');
	mazeGrid[3][6].setStatus('x');
	mazeGrid[4][1].setStatus('x');
	mazeGrid[4][7].setStatus('x');
	mazeGrid[5][1].setStatus('x');
	mazeGrid[5][8].setStatus('x');
	mazeGrid[5][9].setStatus('x');
	mazeGrid[6][9].setStatus('x');
}
///setWater explicitly sets all appropriate water blocks to the
///appropriate status. any custom water layout will need
///to be explicitly entered
void Maze::setWater(){
	mazeGrid[0][5].setStatus('y');
	mazeGrid[0][6].setStatus('y');
	mazeGrid[0][7].setStatus('y');
	mazeGrid[0][8].setStatus('y');
	mazeGrid[0][9].setStatus('y');
	mazeGrid[0][10].setStatus('y');
	mazeGrid[0][11].setStatus('y');
	mazeGrid[1][5].setStatus('y');
	mazeGrid[1][6].setStatus('y');
	mazeGrid[1][7].setStatus('y');
	mazeGrid[1][8].setStatus('y');
	mazeGrid[1][9].setStatus('y');
	mazeGrid[5][2].setStatus('y');
	mazeGrid[5][3].setStatus('y');
	mazeGrid[6][2].setStatus('y');
	mazeGrid[6][3].setStatus('y');
	mazeGrid[6][4].setStatus('y');
	mazeGrid[7][0].setStatus('y');
	mazeGrid[7][1].setStatus('y');
	mazeGrid[7][2].setStatus('y');
	mazeGrid[7][3].setStatus('y');
	mazeGrid[7][4].setStatus('y');
	mazeGrid[8][0].setStatus('y');
	mazeGrid[8][1].setStatus('y');
	mazeGrid[8][2].setStatus('y');
	mazeGrid[8][3].setStatus('y');
	mazeGrid[8][4].setStatus('y');
	mazeGrid[9][2].setStatus('y');
	mazeGrid[9][3].setStatus('y');
	mazeGrid[9][4].setStatus('y');
}
///isBorder returns true if the block is on the outside edge of the maze
bool Maze::isBorder(Block block){
	if (block.getX() == 0 || block.getX() == 11 || block.getY() == 0 || block.getY() == 9)
		return true;
	else
		return false;		
}
///isWall returns true if the block's status is set to 'x'
///meaning the block is a wall
bool Maze::isWall(Block block){
	if (block.getStatus() == 'x')
		return true;
	else
		return false;
		
}