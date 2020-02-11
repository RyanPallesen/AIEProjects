#pragma once

#include "Application.h"
#include "Renderer2D.h"
#include "Physics.h"
class BreakoutApp : public aie::Application {
public:

	BreakoutApp();
	virtual ~BreakoutApp();

	virtual bool startup();
	virtual void shutdown();

	virtual void update(float deltaTime);
	virtual void draw();
	void setupContinuousDemo(glm::vec2 startPos, float inclination, float speed, float gravity);

protected:

	aie::Renderer2D*	m_2dRenderer;
	aie::Font*			m_font;
	PhysicsScene*        m_physicsScene;
};