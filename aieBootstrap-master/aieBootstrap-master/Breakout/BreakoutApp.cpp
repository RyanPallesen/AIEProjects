#include "BreakoutApp.h"
#include "Texture.h"
#include "Font.h"
#include "Input.h"
#include <Gizmos.h>
#include <glm\ext.hpp>
#include <math.h>
#include "Sphere.h"
#include "Plane.h"
#include "Box.h"
#include <string>
using namespace glm;
using namespace aie;

//Tutorial for rotation

BreakoutApp::BreakoutApp() {

}

BreakoutApp::~BreakoutApp() {

}

// Convert screen space coords to world space coords
glm::vec2 BreakoutApp::ToWorldSpace(int screenX, int screenY)
{
	// Get the window dimensions
	glm::vec2 windowDimensions = glm::vec2(getWindowWidth() * 0.5f, getWindowHeight() * 0.5f);
	static float aspectRatio = windowDimensions.x / windowDimensions.y;

	// Convert screen position to numbers in range -1 to 1
	glm::vec2 normalisedScreenPos = (glm::vec2((float)screenX, (float)screenY) / windowDimensions) - glm::vec2(1.0f, 1.0f);

	// Convert screen position to world space
	glm::vec2 worldPos = glm::vec2(normalisedScreenPos.x * 100.0f, normalisedScreenPos.y * 100.0f / aspectRatio);

	return worldPos;
}

bool BreakoutApp::startup() {

	//seed random geneation with time
	srand(time(NULL));

	//Set gizmo drawing variables
	Gizmos::create(255U, 255U, 65535U, 65535U);

	//Create new renderer and physics scene
	m_2dRenderer = new Renderer2D();
	m_physicsScene = new PhysicsScene(0.002f, vec2(0, -25));

	//planes that make up the world
	Plane* plane1 = new Plane(vec2(2, -2), 80);
	Plane* plane2 = new Plane(vec2(2, 2), -80);
	Plane* plane3 = new Plane(vec2(2, -3), 70);
	Plane* plane4 = new Plane(vec2(2, 3), -70);
	Plane* plane5 = new Plane(vec2(1, -4), 60);
	Plane* plane6 = new Plane(vec2(1, 4), -60);
	Plane* plane7 = new Plane(vec2(0, 90), -55);

	m_physicsScene->addActor(plane1);
	m_physicsScene->addActor(plane2);
	m_physicsScene->addActor(plane3);
	m_physicsScene->addActor(plane4);
	m_physicsScene->addActor(plane5);
	m_physicsScene->addActor(plane6);
	m_physicsScene->addActor(plane7);


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

	Input* input = Input::getInstance();


	//if left mouse clicked, generate a single random object somewhere
	if (input->wasMouseButtonPressed(0))
	{
		float size = 0.5f + ((float)rand() / RAND_MAX) * 4;
		float sizey = 0.5f + ((float)rand() / RAND_MAX) * 4;
		float x = (rand() % 512) - 256;
		float y = (rand() % 1024);

		//choose between sphere and box
		if (rand() % 100 < 60)
		{
			m_physicsScene->addActor(new Sphere(vec2(x, y), vec2(1, 1), size, size, vec4((float)rand() / RAND_MAX, (float)rand() / RAND_MAX, (float)rand() / RAND_MAX, 1),0.9f,0.01f,0.01f));
		}
		else
		{
			m_physicsScene->addActor(new Box(vec2(x, y), vec2(-1, -1), size * sizey, vec2(size, sizey), vec4((float)rand() / RAND_MAX, (float)rand() / RAND_MAX, (float)rand() / RAND_MAX, 1), rand() % 360, 0.1f, 0.01f, 0.01f));
		}
	}

	//while right mouse clicked, delete any objects within cursor
	if (input->isMouseButtonDown(1))
	{
		for each (PhysicsObject * var in m_physicsScene->getActors())
		{
			if (dynamic_cast<Rigidbody*>(var) != nullptr)
			{
				if (dynamic_cast<Rigidbody*>(var)->ContainsPoint(ToWorldSpace(input->getMouseX(), input->getMouseY())))
				{
					m_physicsScene->removeActor(var);
				}
			}
		}
	}
	{
		Gizmos::clear();
		m_physicsScene->update(deltaTime);
		m_physicsScene->updateGizmos();
	}

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
	std::string myString = "Current Actors: " + std::to_string(m_physicsScene->getActorCount());
	const char* cstr = myString.c_str();

	m_2dRenderer->drawText(m_font, cstr, 0, 0);
	// done drawing sprites
	m_2dRenderer->end();
}