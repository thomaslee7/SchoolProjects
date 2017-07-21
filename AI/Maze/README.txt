My program was written in C++ and compiled using visual studio 2013.
The .sln file may be opened in visual studio to run in debugging mode, 
but just running the program only requires you to run the .exe file.

Heuristics:
For Uniform Cost I simply assigned a path value to a node equal to
its own cost plus the path cost of its parent node. Recursively, this 
tracks back all the way to the start node and give a total path cost.

For Greedy, I obtained the position of the two goal nodes, then for each
node, calcualted the Euclidean distance to each goal. I then took the 
average of these two values and assigned it to the node.

For A*, I just added the Uniform Cost and Greedy Heuristic values.

