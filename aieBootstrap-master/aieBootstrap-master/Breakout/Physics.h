#pragma once
#include <glm\ext.hpp>
#include <vector>
#include <iostream>
enum ShapeType { PLANE = 0, SPHERE, BOX };

class PhysicsObject {
protected:
	PhysicsObject() {};
	PhysicsObject(ShapeType a_shapeID) : m_shapeID(a_shapeID) {}

public:
	virtual void fixedUpdate(glm::vec2 gravity, float timeStep) = 0;
	virtual void debug() = 0;
	virtual void makeGizmo() = 0;
	virtual void resetPosition() {};

	ShapeType m_shapeID;
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

	virtual bool checkCollision(PhysicsObject* pOther) = 0;

	glm::vec2 getPosition() { return m_position; }
	float getRotation() { return m_rotation; }
	glm::vec2 getVelocity() { return m_velocity; }
	float getMass() { return m_mass; }

protected:  glm::vec2 m_position;  glm::vec2 m_velocity;  float m_mass;  float m_rotation;
};

class Sphere : public Rigidbody {
public:  Sphere(glm::vec2 position, glm::vec2 velocity, float mass, float radius, glm::vec4 colour);  ~Sphere();

	  virtual void makeGizmo();  virtual bool checkCollision(PhysicsObject* pOther);

	  void debug() {};

	  float getRadius() { return m_radius; }  glm::vec4 getColour() { return m_colour; }

protected:  float m_radius;  glm::vec4 m_colour;
};
