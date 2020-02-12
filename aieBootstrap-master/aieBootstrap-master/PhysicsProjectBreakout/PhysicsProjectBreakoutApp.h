#pragma once

#include "Application.h"
#include "Renderer2D.h"
#include <glm\ext.hpp>
#include "../Breakout/PhysicsScene.h"
#include "../Breakout/Sphere.h"
#include "../Breakout/Plane.h"
#include "../Breakout/PhysicsObject.h"
#include "../Breakout/Rigidbody.h"

class PhysicsProjectilesApp : public aie::Application {
public:

	PhysicsProjectilesApp();
	virtual ~PhysicsProjectilesApp();

	virtual bool startup();
	virtual void shutdown();

	virtual void update(float deltaTime);
	virtual void draw();
	void setupContinuousDemo(glm::vec2 startPos, glm::vec2 velocity, float gravity, float mass);

protected:

	aie::Renderer2D* m_2dRenderer;
	aie::Font* m_font;
	PhysicsScene* m_physicsScene;
};