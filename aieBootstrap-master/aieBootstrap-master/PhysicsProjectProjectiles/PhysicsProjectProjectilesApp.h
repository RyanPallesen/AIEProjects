#pragma once

#include "Application.h"
#include "Renderer2D.h"
#include "../Breakout/PhysicsObject.h"
#include "../Breakout/PhysicsScene.h"
#include "../Breakout/Sphere.h"
#include "../Breakout/Plane.h"

class PhysicsProjectProjectilesApp : public aie::Application {
public:

	PhysicsProjectProjectilesApp();
	virtual ~PhysicsProjectProjectilesApp();

	virtual bool startup();
	virtual void shutdown();

	virtual void update(float deltaTime);
	virtual void draw();
	void setupContinuousDemo(glm::vec2 startPos, glm::vec2 velocity, glm::vec2 gravity, float mass);

protected:

	aie::Renderer2D* m_2dRenderer;
	aie::Font* m_font;
	PhysicsScene* m_physicsScene;
};