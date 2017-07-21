#include "block.h"
#include <iostream>

using namespace std;

Block::Block(){
	//block constructor
	x = 0;
	y = 0;
	up = 0.0f;
	upLeft = 0.0f;
	upRight = 0.0f;
	down = 0.0f;
	left = 0.0f;
	right = 0.0f;
	type = 'w';
	reward = 0;
}