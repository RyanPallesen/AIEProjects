#pragma once
#include <glm\ext.hpp>
#include <vector>

class PhysicsObject {
protected:
	PhysicsObject() {}

public:
	virtual void fixedUpdate(glm::vec2 gravity, float timeStep) = 0;
	virtual void debug() = 0;
	virtual void makeGizmo() = 0;
	virtual void resetPosition() {};
};

class PhysicsScene {
public:  PhysicsScene(float timeStep, glm::vec2 gravity);  ~PhysicsScene();

	  void addActor(PhysicsObject* actor);  void removeActor(PhysicsObject* actor); 
	  
	  void update(float dt);  void updateGizmos();

	  void setGravity(const glm::vec2 gravity) { m_gravity = gravity; }
	  
	  glm::vec2 getGravity() const { return m_gravity; }

	  void setTimeStep(const float timeStep) { m_timeStep = timeStep; } 
	  
	  float getTimeStep() const { return m_timeStep; }

protected:  glm::vec2 m_gravity;  float m_timeStep;  std::vector<PhysicsObject*> m_actors;
};