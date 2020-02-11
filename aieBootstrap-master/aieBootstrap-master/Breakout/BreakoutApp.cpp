#include "BreakoutApp.h"
#include "Texture.h"
#include "Font.h"
#include "Input.h"
#include <Gizmos.h>
#include <glm\ext.hpp>
#include <math.h>

using namespace glm;
using namespace aie;
BreakoutApp::BreakoutApp() {

}

BreakoutApp::~BreakoutApp() {

}

void BreakoutApp::setupContinuousDemo(glm::vec2 startPos, glm::vec2 velocity, float gravity, float mass)
{
	float t = 0;
	float tStep = 0.5f;
	float radius = 1.0f;
	int segments = 12;
	glm::vec4 colour = glm::vec4(1, 1, 0, 1);

	while (t <= 5)
	{
		// calculate the x, y position of the projectile at time t

		float x = startPos.x + (velocity.x * t);
		float y = startPos.y + (velocity.y * t) + ((gravity) * t * t * 0.5f);//can't factor in mass

		aie::Gizmos::add2DCircle(vec2(x, y), radius, segments, colour);
		t += tStep;
	}
}

bool BreakoutApp::startup() {
	
	Gizmos::create(255U, 255U, 65535U, 65535U);

	m_2dRenderer = new Renderer2D();
	m_physicsScene = new PhysicsScene(0.01f,vec2(0,-25));

	//Sphere* ball1 = new Sphere(vec2(10, 1), vec2(-20, 0), 4.0f, 2, vec4(1, 0, 0, 1));
	Sphere* ball2 = new Sphere(vec2(-40, 0), vec2(20, 45), 10.0f, 2, vec4(0, 1, 0, 1));
	Plane* plane = new Plane(vec2(0, 5), 5);

	setupContinuousDemo(vec2(-40, 0), vec2(20, 45), -25, 10.0f);

	//m_physicsScene->addActor(ball1);
	m_physicsScene->addActor(ball2);
	m_physicsScene->addActor(plane);
	// TODO: remember to change this when redistributing a build!
	// the following path would be used instead: "./font/consolas.ttf"
	m_font = new Font("../bin/font/consolas.ttf", 32);

	return true;
}

void BreakoutApp::shutdown() {

	delete m_font;
	delete m_2dRenderer;
}

void BreakoutApp::update(float deltaTime) {

	// input example
	Input* input = Input::getInstance();


	//Gizmos::clear();
	m_physicsScene->update(deltaTime);
	m_physicsScene->updateGizmos();

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
			Gizmos::add2DAABBFilled(pos, boxExtents, colours[y]);
			
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

void BreakoutApp::draw() {

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