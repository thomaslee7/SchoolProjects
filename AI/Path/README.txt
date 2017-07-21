I learned that when I decreased my discount, I had to increase my goal reward by 500% just to get my agent to leave the pizza place, but who can blame him. Pizza is awesome. 

When changing the learning rate, it seemed like no matter what I did, the agent wouldn't perform correctly. If I increased the reward for Goal too much, it would skip the pizza place. If the reward for Pizza was too high relative to Goal, it would never leave. What fixed the problem was finding the right amount of training. Over training cause an extreme (either straight to Goal, or never leave Pizza). Decreasing my training to 100 allowed the optimal path to reveal itself.

The optimum path should always be found so I'm sure that the amount of training shouldn't effect that, but with the actual implementation I found a sweet spot that got the agent to do what I want. The same sweet spot could probably be found with reward manipulation, but I was using integers for all my rewards so incremental manipulation was difficult. 

*********************************************************************************
code compiles on cse machine with:

g++ -std=c++0x Main.cpp Block.cpp Grid.cpp

normal running: ./a.out

*********************************************************************************

I'm including all my output files with my submission as the "CLEARLY LABELLED" examples..... but, running the program will APPEND another output to the end of those files... so you may want to delete them, or move them to a new directory if you want a clear picture of how the program runs..

The program is not interactive so it should run and finish on its own.

included program files:
Main.cpp
Block.cpp
block.h
Grid.cpp
Grid.h