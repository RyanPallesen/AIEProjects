#include "PhysicsProjectProjectilesApp.h"
#include "Texture.h"
#include "Font.h"
#include "Input.h"
#include <Gizmos.h>
#include <math.h>

#include <Application.h>

using namespace glm;
using namespace aie;
PhysicsProjectProjectilesApp::PhysicsProjectProjectilesApp() {

}

PhysicsProjectProjectilesApp::~PhysicsProjectProjectilesApp() {

}

void PhysicsProjectProjectilesApp::setupContinuousDemo(glm::vec2 startPos, glm::vec2 velocity, glm::vec2 gravity, float mass)
{
	int maxT = 16;
	float t = 0;
	float tStep = 0.1f;
	float radius = 0.5f;
	int segments = 12;
	glm::vec4 colour = glm::vec4(1, 1, 0, 1);

	while (t <= maxT)
	{
		radius -= (radius / (maxT / tStep));
		// calculate the x, y position of the projectile at time t

		float x = startPos.x + (velocity.x * t) + ((gravity.x) * t * t * 0.5f);;
		float y = startPos.y + (velocity.y * t) + ((gravity.y)*t * t * 0.5f);

		aie::Gizmos::add2DCircle(vec2(x, y), radius, segments, colour);
		t += tStep;
	}
}

Sphere* ball1 = new Sphere(vec2(40, 0), vec2(-50, 8), 2.0f, 2, vec4((float)rand() / RAND_MAX, (float)rand() / RAND_MAX, (float)rand() / RAND_MAX, 1));
Sphere* ball2 = new Sphere(vec2(-40, 0), vec2(35, 4), 5.0f, 5, vec4((float)rand() / RAND_MAX, (float)rand() / RAND_MAX, (float)rand() / RAND_MAX, 1));
Sphere* ball3 = new Sphere(vec2(0, 0), vec2(5, 10), 0.1f, 1, vec4((float)rand() / RAND_MAX, (float)rand() / RAND_MAX, (float)rand() / RAND_MAX, 1));

bool PhysicsProjectProjectilesApp::startup() {
	srand(time(NULL));

	Gizmos::create(255U, 255U, 65535U, 65535U);

	m_2dRenderer = new Renderer2D();
	m_physicsScene = new PhysicsScene(0.01f, vec2(0, -25));

	m_physicsScene->addActor(ball1);
	m_physicsScene->addActor(ball2);
	m_physicsScene->addActor(ball3);
	// TODO: remember to change this when redistributing a build!
	// the following path would be used instead: "./font/consolas.ttf"
	m_font = new Font("../bin/font/consolas.ttf", 32);

	return true;
}

void PhysicsProjectProjectilesApp::shutdown() {

	delete m_font;
	delete m_2dRenderer;
}

void PhysicsProjectProjectilesApp::update(float deltaTime) {

	// input example
	Input* input = Input::getInstance();


	if (input->isMouseButtonDown(0))
	{
		Gizmos::clear();

		setupContinuousDemo(ball1->getPosition(), ball1->getVelocity(), m_physicsScene->getGravity(), ball1->getMass());
		setupContinuousDemo(ball2->getPosition(), ball2->getVelocity(), m_physicsScene->getGravity(), ball2->getMass());
		setupContinuousDemo(ball3->getPosition(), ball3->getVelocity(), m_physicsScene->getGravity(), ball3->getMass());

		m_physicsScene->update(deltaTime);
		m_physicsScene->updateGizmos();
	}

	static const vec4 colours[] =
	{

	vec4(1,0,0,1),
	vec4(0,1,0,1),
	vec4(0,0,1,1),
	vec4(0.8f,0,0.5f,1),
	vec4(0,1,1,1),

	};

	static const int rows = 5;
	static const int columns = 10;
	static const int hSpace = 1;
	static const int vSpace = 1;

	static const vec2 scrExtents(100, 50);
	static const vec2 boxExtents(7, 3);
	static const vec2 startPos(-(columns >> 1)* ((boxExtents.x * 2) + vSpace) + boxExtents.x + (vSpace / 2.0f), scrExtents.y - ((boxExtents.y * 2) + hSpace));


	//draw the grid of blocks
	vec2 pos;
	for (int y = 0; y < rows; y++)
	{

		pos = vec2(startPos.x, startPos.y - (y * ((boxExtents.y * 2) + hSpace)));
		for (int x = 0; x < columns; x++)
		{
			//Gizmos::add2DAABBFilled(pos, boxExtents, colours[y]);

			pos.x += (boxExtents.x * 2) + vSpace;
		}
	}

	//Draw ball

	static int paddleX = -40;

	if (input->isKeyDown(INPUT_KEY_A))
	{
		paddleX--;
	}

	if (input->isKeyDown(INPUT_KEY_D))
	{
		paddleX++;
	}

	//Draw paddle
	Gizmos::add2DAABBFilled(vec2(paddleX, -40), vec2(12, 2), vec4(1, 0, 1, 1));
	// exit the application
	if (input->isKeyDown(INPUT_KEY_ESCAPE))
		quit();
}

void PhysicsProjectProjectilesApp::draw() {

	// wipe the screen to the background colour
	clearScreen();

	// begin drawing sprites
	m_2dRenderer->begin();

	// draw your stuff here!
	static float aspectRatio = 16 / 9.f;
	Gizmos::draw2D(ortho<float>(-100, 100, -100 / aspectRatio, 100 / aspectRatio, -1.0f, 1.0f));
	// output some text, uses the last used colour
	m_2dRenderer->drawText(m_font, "Press ESC to quit", 0, 0);

	// done drawing sprites
	m_2dRenderer->end();
}