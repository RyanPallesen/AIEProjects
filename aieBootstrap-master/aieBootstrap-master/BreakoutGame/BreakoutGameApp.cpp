#include "BreakoutGameApp.h"
#include "Texture.h"
#include "Font.h"
#include "Input.h"
#include <Gizmos.h>
#include <math.h>

#include <Application.h>

using namespace glm;
using namespace aie;

BreakoutGameApp::BreakoutGameApp()
{
	gameInstance = this;
}
BreakoutGameApp::~BreakoutGameApp()
{
}

void BreakoutGameApp::BallHitBrick(PhysicsObject* self, PhysicsObject* brick)
{
	gameScore += 1;
	m_physicsScene->removeActor(brick);
}


Box* paddle = new Box(vec2(0, 0), vec2(0, 0), 256, vec2(5, 2), vec4(0.25, 0.25, 1, 1), 0, 1.1f, 0.0f, 0.0f);
Sphere* ball = new Sphere(vec2(0, -30), vec2(25, 25), 1, 1, vec4(0.2f,0, 0.2f, 1), 1.0f, 0.0f, 0.0f);

bool BreakoutGameApp::startup() {
	srand(time(NULL));

	Gizmos::create(255U, 255U, 65535U, 65535U);

	m_2dRenderer = new Renderer2D();
	m_physicsScene = new PhysicsScene(0.01f, vec2(0, 0));

	m_physicsScene->addActor(paddle);
	m_physicsScene->addActor(ball);


	paddle->setKinematic(true);
	paddle->tags.push_back("Paddle");
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
	static const int hSpace = 6;
	static const int vSpace = 3;

	static const vec2 scrExtents(100, 50);
	static const vec2 boxExtents(5, 3);
	static const vec2 startPos(-(columns >> 1)* ((boxExtents.x * 2) + vSpace) + boxExtents.x + (vSpace / 2.0f), scrExtents.y - ((boxExtents.y * 2) + hSpace));

	

	//draw the grid of blocks
	vec2 pos;
	for (int y = 0; y < rows; y++)
	{
		pos = vec2(startPos.x, startPos.y - (y * ((boxExtents.y * 2) + hSpace)));
		for (int x = 0; x < columns; x++)
		{
			Box* box = new Box(pos, vec2(0, 0), 256, boxExtents, colours[y], 15, 1.0f, 2.0f, 2.0f);
			box->tags.push_back("Breakable");
			box->setKinematic(true);
			m_physicsScene->addActor(box);
			pos.x += (boxExtents.x * 2) + vSpace;
		}
	}

	m_physicsScene->addActor(new Plane(vec2(0, 1), 64));
	m_physicsScene->addActor(new Plane(vec2(1, 0), 72));
	m_physicsScene->addActor(new Plane(vec2(1, 0), -72));


	// TODO: remember to change this when redistributing a build!
	// the following path would be used instead: "./font/consolas.ttf"
	m_font = new Font("../bin/font/consolas.ttf", 32);

	return true;
}

void BreakoutGameApp::shutdown() {

	delete m_font;
	delete m_2dRenderer;
}

void BreakoutGameApp::update(float deltaTime) {

	// input example
	Input* input = Input::getInstance();



	Gizmos::clear();

	m_physicsScene->update(deltaTime);
	m_physicsScene->updateGizmos();

	if (ball->getPosition().y < -50)
	{
		if (gameLives > 0)
		{
			gameLives -= 1;
			ball->m_velocity.y *= -1;
		}
	}

	static vec2 paddlePosition = vec2(0, -40);

	if (input->isKeyDown(INPUT_KEY_A))
	{
		paddlePosition.x--;
	}

	if (input->isKeyDown(INPUT_KEY_D))
	{
		paddlePosition.x++;
	}

	//Draw paddle
	paddle->setPosition(paddlePosition);

	// exit the application
	if (input->isKeyDown(INPUT_KEY_ESCAPE))
		quit();
}



void BreakoutGameApp::draw() {

	// wipe the screen to the background colour
	clearScreen();

	// begin drawing sprites
	m_2dRenderer->begin();

	// draw your stuff here!
	static float aspectRatio = 16 / 9.f;
	Gizmos::draw2D(ortho<float>(-100, 100, -100 / aspectRatio, 100 / aspectRatio, -1.0f, 1.0f));
	// output some text, uses the last used colour
	char score[32];
	sprintf_s(score, 32, "Score: %i", gameScore);
	m_2dRenderer->drawText(m_font, score, 0, 720 - 32);

	char lives[32];
	sprintf_s(lives, 32, "Lives: %i", gameLives);
	m_2dRenderer->drawText(m_font, lives, 0, 720 - 64);

	// done drawing sprites
	m_2dRenderer->end();
}