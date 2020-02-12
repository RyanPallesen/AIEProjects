#include "PhysicsProjectBreakoutApp.h"

int main() {
	
	// allocation
	auto app = new PhysicsProjectBreakoutApp();

	// initialise and loop
	app->run("AIE", 1280, 720, false);

	// deallocation
	delete app;

	return 0;
}