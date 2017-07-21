#include "block.h"
#include "graph.h"
#include "stdlib.h"
#include <iostream>
#include <vector>
#include <queue>
#include <concurrent_priority_queue.h>
#include <stack>


///declaration of the struct vertexNode
struct vertexNode;
///declaration of a vector used through the graph.cpp file
std::vector<vertexNode*> verticies;
///definition of vertexNode
///vertName is the value given to a grid block based on its position range: 1 -120
///ptr is a pointer the the Block in that grid position
//visited is a bool check to keep from adding parent nodes to the list of their child's adjacencies when constructing seach trees
//uniCheck is a bool 
///adjacencies is a vector of adjacent vertexNodes associated with *this vertexNode
//parentName is the vertexNode's parent range: 1-120;
//pathCost, greedyCost, and aStarCost are heuristic values used to determine selection during the search tree phase.
struct vertexNode{
	int vertName;
	Block* ptr;
	bool visited;
	bool uniCheck;
	std::vector<vertexNode*> adjacencies;
	int parentName = 0;
	int pathCost = 0;
	int greedyCost = 0;
	int aStarCost = 0;
};
///Comparator, GreedyComparator, AStarComparator 
///are comparing functions used by stl priority queues to sort the queue
struct Comparator{
	bool operator()(const vertexNode* lhs, const vertexNode* rhs){
		return lhs->pathCost > rhs->pathCost;
	}
};
struct GreedyComparator{
	bool operator()(const vertexNode* lhs, const vertexNode* rhs){
		return lhs->greedyCost > rhs->greedyCost;
	}
};
struct AStarComparator{
	bool operator()(const vertexNode* lhs, const vertexNode* rhs){
		return lhs->aStarCost > rhs->aStarCost;
	}
};
///The Graph class is where the bulk of the work in this program happens
///it has addVertex and addEdge functions to populate the graph
///and a searchTree function that performs either Breadth First,
///Uniform Cost, Greedy, or A* searches throught the graph from 
///vertex 1 to a goal vertex

Graph::Graph(int size){
	vertexCount = size;
	
}
void Graph::addVertex(int vertName, Block* block, int greedyCost){
	vertexNode *p = new vertexNode;
	p->vertName = vertName;
	p->ptr = block;
	p->visited = false;
	p->uniCheck = false;
	p->parentName = 0;
	p->pathCost = 0;
	p->greedyCost = greedyCost;
	p->aStarCost = greedyCost + p->pathCost;
	verticies.push_back(p);
}
void Graph::addEdge(int from, int to){
	verticies[from - 1]->adjacencies.push_back(verticies[to - 1]);
}
///searchTree goes through the graph's verticies and checks to see
///if any of the verticies are the goal, if not it pushes them into a 
///queue to be searched (based on the particualr type of search)
///Breadth adds all children to the queue
///Uniform Cost adds all the children to a priority queue sorted by the total cost from 
///the start vertex to them
///Greedy adds the child with closest to the goal nodes using and average of the 
///Euclidean distance to each goal, sorted by euclidean distance avg in the priority queue
///A* add the children to the priority queue and sorts them by total path from Uniform Cost
///added to the cost from the Greedy search
void Graph::searchTree(int startVert, char strategy){
	int nodesExplored = 0;
	std::queue<vertexNode*> toBeSearched;
	std::priority_queue<vertexNode*, std::vector<vertexNode*>, Comparator> unionToBeSearched;
	std::priority_queue<vertexNode*, std::vector<vertexNode*>, GreedyComparator> greedyToBeSearched;
	std::priority_queue<vertexNode*, std::vector<vertexNode*>, AStarComparator> aStarToBeSearched;
	std::stack<vertexNode*> solnPath;
	if (strategy == 'B'){
		int totalCost = 0;
		toBeSearched.push(verticies[startVert - 1]);
		while (!toBeSearched.empty()){
			vertexNode *searchingNode = toBeSearched.front();
			searchingNode->visited = true;
			for (auto i = verticies[searchingNode->vertName - 1]->adjacencies.begin(); i != verticies[searchingNode->vertName - 1]->adjacencies.end(); i++){
				if ((*i)->ptr->getStatus() != 'g'){
					if (!(*i)->visited){
						(*i)->parentName = searchingNode->vertName;
						(*i)->pathCost = ((*i)->ptr->getCost()) + searchingNode->pathCost;
						toBeSearched.push(*i);
					}
				}
				else{
					(*i)->parentName = searchingNode->vertName;
					(*i)->pathCost = ((*i)->ptr->getCost()) + searchingNode->pathCost;
					solnPath.push((*i));
					totalCost = (*i)->pathCost;
					vertexNode *temp = *i;
					while (temp->parentName != 0){
						solnPath.push(verticies[temp->parentName - 1]);
						temp = verticies[temp->parentName - 1];
					}
					while (!toBeSearched.empty()){
						toBeSearched.pop();
					}
				}
			}
			toBeSearched.pop();
			nodesExplored++;
		}
		std::cout << "Breadth First" << std::endl;
		std::cout << "Nodes Explored: " << nodesExplored << std::endl;
		std::cout << "Solution Length: " << solnPath.size() << std::endl;
		std::cout << "Total Cost: " << totalCost << std::endl;
	}
	if (strategy == 'U'){
		int totalCost = 0;
		unionToBeSearched.push(verticies[startVert - 1]);
		while (!unionToBeSearched.empty()){
			vertexNode* searchingNode = unionToBeSearched.top();
			searchingNode->visited = true;
			unionToBeSearched.pop();
			for (auto i = verticies[searchingNode->vertName - 1]->adjacencies.begin(); i != verticies[searchingNode->vertName - 1]->adjacencies.end(); i++){
				if ((*i)->ptr->getStatus() != 'g'){
					if (!(*i)->visited && !(*i)->uniCheck){
						(*i)->parentName = searchingNode->vertName;
						(*i)->pathCost = ((*i)->ptr->getCost()) + searchingNode->pathCost;
						(*i)->uniCheck = true;
						unionToBeSearched.push(*i);
					}

				}
				else{
					(*i)->parentName = searchingNode->vertName;
					(*i)->pathCost += ((*i)->ptr->getCost()) + searchingNode->pathCost;
					solnPath.push((*i));
					totalCost = (*i)->pathCost;
					vertexNode *temp = *i;
					while (temp->parentName != 0){
						solnPath.push(verticies[temp->parentName - 1]);
						temp = verticies[temp->parentName - 1];
					}
					while (!unionToBeSearched.empty()){
						unionToBeSearched.pop();
					}
				}
			}
			nodesExplored++;
		}
		std::cout << "Uniform Cost" << std::endl;
		std::cout << "Nodes Explored: " << nodesExplored << std::endl;
		std::cout << "Solution Length: " << solnPath.size() << std::endl;
		std::cout << "Total Cost: " << totalCost << std::endl;
	}
	if (strategy == 'G'){		
		int totalCost = 0;
		greedyToBeSearched.push(verticies[startVert - 1]);
		while (!greedyToBeSearched.empty()){
			vertexNode *searchingNode = greedyToBeSearched.top();
			searchingNode->visited = true;
			greedyToBeSearched.pop();
			for (auto i = verticies[searchingNode->vertName - 1]->adjacencies.begin(); i != verticies[searchingNode->vertName - 1]->adjacencies.end(); i++){
				if ((*i)->ptr->getStatus() != 'g'){
					if (!(*i)->visited){
						(*i)->parentName = searchingNode->vertName;
						(*i)->pathCost = ((*i)->ptr->getCost()) + searchingNode->pathCost;
						greedyToBeSearched.push(*i);
					}
				}
				else{

					(*i)->parentName = searchingNode->vertName;
					(*i)->pathCost += ((*i)->ptr->getCost()) + searchingNode->pathCost;
					solnPath.push((*i));
					totalCost = (*i)->pathCost;
					vertexNode *temp = *i;
					while (temp->parentName != 0){
						solnPath.push(verticies[temp->parentName - 1]);
						temp = verticies[temp->parentName - 1];
					}
					while (!greedyToBeSearched.empty()){
						greedyToBeSearched.pop();
					}
					break;
				}
			}
			nodesExplored++;
		}
		std::cout << "Greedy" << std::endl;
		std::cout << "Heuristic: The average of the Euclidean distance from the Node ";
		std::cout << "to the goal nodes" << std::endl;
		std::cout << "Nodes Explored: " << nodesExplored << std::endl;
		std::cout << "Solution Length: " << solnPath.size() << std::endl;
		std::cout << "Total Cost: " << totalCost << std::endl;
	}
	if (strategy == 'A'){
		int totalCost = 0;
		aStarToBeSearched.push(verticies[startVert - 1]);
		while (!aStarToBeSearched.empty()){
			vertexNode *searchingNode = aStarToBeSearched.top();
			searchingNode->visited = true;
			aStarToBeSearched.pop();
			for (auto i = verticies[searchingNode->vertName - 1]->adjacencies.begin(); i != verticies[searchingNode->vertName - 1]->adjacencies.end(); i++){
				if (searchingNode->ptr->getStatus() != 'g'){
					if (!(*i)->visited && !(*i)->uniCheck){
						(*i)->parentName = searchingNode->vertName;
						(*i)->pathCost = ((*i)->ptr->getCost()) + searchingNode->pathCost;
						(*i)->aStarCost = ((*i)->pathCost + (*i)->greedyCost);
						(*i)->uniCheck = true;
						aStarToBeSearched.push(*i);
					}
				}
				else{
					solnPath.push((searchingNode));
					totalCost = (searchingNode)->pathCost;
					vertexNode *temp = searchingNode;
					while (temp->parentName != 0){
						solnPath.push(verticies[temp->parentName - 1]);
						temp = verticies[temp->parentName - 1];
					}
					while (!aStarToBeSearched.empty())
						aStarToBeSearched.pop();
					break;
				}
			}
			nodesExplored++;
		}
		std::cout << "A*" << std::endl;
		std::cout << "Heuristic: Greedy Cost + Uniform Cost Search" << std::endl;
		std::cout << "Nodes Explored: " << nodesExplored << std::endl;
		std::cout << "Solution Length: " << solnPath.size() << std::endl;
		std::cout << "Total Cost: " << totalCost << std::endl;
	}
	while (!solnPath.empty()){
		if (solnPath.top()->ptr->getStatus() != 'g'){
			std::cout << "(" << solnPath.top()->ptr->getX() + 1 << "," << solnPath.top()->ptr->getY() + 1 << ")" << "-->";
		}
		else{
			std::cout << "(" << solnPath.top()->ptr->getX() + 1 << "," << solnPath.top()->ptr->getY() + 1 << ")" << std::endl;
		}

		solnPath.pop();
	}
	int resetVisited = 120;
	for (int vert = 1; vert < resetVisited; vert++){
		for (auto i = verticies[vert]->adjacencies.begin(); i != verticies[vert]->adjacencies.end(); i++){
			(*i)->visited = false;
			(*i)->uniCheck = false;
			(*i)->pathCost = 0;
		}
	}
}
