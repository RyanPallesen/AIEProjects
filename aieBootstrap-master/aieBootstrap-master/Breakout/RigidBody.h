#pragma once
#include "PhysicsObject.h"
class Rigidbody : public PhysicsObject {
public:
	Rigidbody(ShapeType shapeID, glm::vec2 position, glm::vec2 velocity, float rotation, float mass) { m_shapeID = shapeID, m_position = position, m_velocity = velocity, m_mass = mass, m_rotation = rotation; };
	~Rigidbody() {};

	virtual void fixedUpdate(glm::vec2 gravity, float timeStep);
	virtual void debug() {};
	void applyForce(glm::vec2 force) {
		m_velocity += force / m_mass;
	};
	void applyForceToActor(Rigidbody* actor2, glm::vec2 force)
	{
		applyForce(-force);

		actor2->applyForce(force);
	};
	void resolveCollision(Rigidbody* actor2);

	glm::vec2 getPosition() { return m_position; }
	float getRotation() { return m_rotation; }
	glm::vec2 getVelocity() { return m_velocity; }
	float getMass() { return m_mass; }

public:  
	  glm::vec2 m_position;
	  glm::vec2 m_velocity;
	  float m_mass;
	  float m_rotation;
	  float m_linearDrag = 2;
	  float m_angularDrag = 1;
	  float m_angularVelocity = 1;
	  float m_elasticity = 0.85f;
};