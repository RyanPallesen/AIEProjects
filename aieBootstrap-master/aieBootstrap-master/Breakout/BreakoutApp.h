#pragma once

#include "Application.h"
#include "Renderer2D.h"
#include "PhysicsScene.h"
#include "PhysicsObject.h"
class BreakoutApp : public aie::Application {
public:

	BreakoutApp();
	virtual ~BreakoutApp();

	virtual bool startup();
	virtual void shutdown();

	virtual void update(float deltaTime);
	virtual void draw();
	void setupContinuousDemo(glm::vec2 startPos, glm::vec2 velocity, float gravity, float mass);

protected:

	aie::Renderer2D*	m_2dRenderer;
	aie::Font*			m_font;
	PhysicsScene*        m_physicsScene;
};