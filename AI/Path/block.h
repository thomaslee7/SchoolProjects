#ifndef BLOCK_H
#define BLOCK_H

class Block{
public:
	Block();
	int x;
	int y;
	float up;
	float upLeft;
	float upRight;
	float down;
	float left;
	float right;
	char type;
	int reward;
};

#endif