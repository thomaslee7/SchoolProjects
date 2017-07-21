///graph.h
class Graph{
public:
	int vertexCount;
	Graph(int size);
	void addVertex(int vertName, Block* block, int greedyCost);
	void addEdge(int fromVert, int toVert);
	void searchTree(int startVert, char strategy);
};