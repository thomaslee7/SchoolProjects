#include "block.h"
#include "graph.h"
#include "maze.h"
#include <iostream>
#include <fstream>
#include <iomanip>


///This program creates a maze with a specified start position and 
///two goal nodes. For this program the start and goal positions are
///explicitly entered, but the program can be modified to take user input
///for the start and goal positions.
///After creating the maze the program creates a graph to represent all the 
///various connections between the nodes.

///The program asks for users input for what kind of search they want to perform
///B for Breadth First Search
///U for Uniform Cost Search - uses total cost from the start node to current node
///G for Greedy search - Heuristic: the average of the Euclidean distance between
/// the node and the two goal nodes
///A for A* search - Heuristic: the sum of the total cost from Uniform Cost Search
///and the average from the Greedy Search
///the user must input E to exit the program

///After the search is complete the program outputs the original maze
///the algorithm used
///the heuristic used in the algorithm
///the number of nodes explored
///the solution length
///the total cost of the solution path
///and finally the solution path

int main(){
	Maze maze(0, 0, 8, 6, 9, 3);
	maze.setWalls();
	maze.setWater();
	maze.setCost();	
	
	Graph graph(120);
	for (int row = 0; row < 10; row++){
		for (int column = 0; column < 12; column++){
			int avgGreedyCost = ((int)sqrt((column - 8)*(column - 8) + (row - 6)*(row - 6)) + (int)sqrt((column - 9)*(column - 9) + (row - 3)*(row - 3)))/2;
			graph.addVertex((column + 1) + (12 * row), maze.mazeGrid[row][column].getBlock(), avgGreedyCost);
		}
	}
	for (int column = 0; column < 12; column++){
		for (int row = 0; row < 10; row++){
			if (maze.mazeGrid[row][column].getStatus() != 'x'){
				if (!maze.isBorder(maze.mazeGrid[row][column])){
					//north
					if(!maze.isWall(maze.mazeGrid[row - 1][column]))
						graph.addEdge((column + 1)+(12 * row), ((column + 1)+(12 * row) - 12));
					if (!maze.isWall(maze.mazeGrid[row + 1][column]))
						//south
						graph.addEdge((column + 1) + (12 * row), ((column + 1)+(12 * row) + 12));
					if (!maze.isWall(maze.mazeGrid[row][column + 1]))
						//east
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) + 1));
					if (!maze.isWall(maze.mazeGrid[row][column - 1]))
						//west
						graph.addEdge((column + 1)+(12 * row), (column + 1)+(12 * row) - 1);
				}
				else if (column == 0){
					if (!maze.isWall(maze.mazeGrid[row][column + 1])){
						//east
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) + 1));
					}
					if (row != 9 && !maze.isWall(maze.mazeGrid[row + 1][column])){
						//south
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) + 12));
					}
					if (row != 0 && !maze.isWall(maze.mazeGrid[row -1][column])){
						//north
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) - 12));
					}
				}
				else if (column == 11){
					if (row != 9 && !maze.isWall(maze.mazeGrid[row + 1][column])){
						//south
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) + 12));
					}
					if (!maze.isWall(maze.mazeGrid[row][column - 1]))
						//west
						graph.addEdge((column + 1) + (12 * row), (column + 1) + (12 * row) - 1);
					if (row != 0 && !maze.isWall(maze.mazeGrid[row - 1][column])){
						//north
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) - 12));
					}
				}
				else if (row == 0){
					if (column != 11 && !maze.isWall(maze.mazeGrid[row][column + 1])){
						//east
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) + 1));
					}
					if (!maze.isWall(maze.mazeGrid[row + 1][column]))
						//south
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) + 12));
					if (column != 0 && !maze.isWall(maze.mazeGrid[row][column - 1])){
						//west
						graph.addEdge((column + 1) + (12 * row), (column + 1) + (12 * row) - 1);
					}
				}
				else if (row == 9){
					if (column != 11 && !maze.isWall(maze.mazeGrid[row][column + 1])){
						//east
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) + 1));
					}
					if (!maze.isWall(maze.mazeGrid[row - 1][column]))
						//north
						graph.addEdge((column + 1) + (12 * row), ((column + 1) + (12 * row) - 12));
					if (column != 0 && !maze.isWall(maze.mazeGrid[row][column - 1])){
						//west
						graph.addEdge((column + 1) + (12 * row), (column + 1) + (12 * row) - 1);
					}
				}
			}
		}
	}

	char userInput = NULL;
	while (userInput != 'E'){
		std::cout << "Please select Search Method: (B)readth, (U)niform, (G)reedy, or (A)*" << std::endl;
		std::cout << "or select (E)xit" << std::endl;
		std::cin >> userInput;
		std::cout << "    1    2    3    4    5    6    7    8    9   10   11    12 " << std::endl;
		std::cout << "  ------------------------------------------------------------" << std::endl;
		for (int i = 0; i < 10; i++){
			if (i != 9)
				std::cout << i + 1 << " ";
			else
				std::cout << i + 1;
			for (int j = 0; j < 12; j++){
				maze.mazeGrid[i][j].printBlock();
			}
			std::cout << '\n';
			std::cout << "  ------------------------------------------------------------" << '\n';
		}
		graph.searchTree(1, userInput);
	}
	
	return 0;
}