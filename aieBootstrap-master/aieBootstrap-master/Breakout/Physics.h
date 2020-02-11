#pragma once
#include <glm\ext.hpp>
#include <vector>
#include <iostream>
enum ShapeType { PLANE = 0, SPHERE, BOX , SHAPE_COUNT};
class PhysicsObject {
protected:
	PhysicsObject() {};
	PhysicsObject(ShapeType a_shapeID) : m_shapeID(a_shapeID) {}

public:
	virtual void fixedUpdate(glm::vec2 gravity, float timeStep) = 0;
	virtual void debug() = 0;
	virtual void makeGizmo() = 0;
	virtual void resetPosition() {};
	ShapeType getShapeID() { return m_shapeID; };
	ShapeType m_shapeID;
};

typedef bool(*fn)(PhysicsObject*, PhysicsObject*);

class PhysicsScene
{
public:
	PhysicsScene::PhysicsScene(float timestep, glm::vec2 gravity);
	~PhysicsScene();
	void addActor(PhysicsObject* actor);
	void removeActor(PhysicsObject* actor);
	void update(float dt);
	void updateGizmos();
	//void debugScene();
	void setGravity(const glm::vec2 gravity) { m_gravity = gravity; }
	glm::vec2 getGravity() const { return m_gravity; }
	void setTimeStep(const float timeStep) { m_timeStep = timeStep; }
	float getTimeStep() const { return m_timeStep; }
	void checkForCollision();
	static bool plane2Plane(PhysicsObject*, PhysicsObject*);
	static bool plane2Sphere(PhysicsObject*, PhysicsObject*);
	static bool sphere2Plane(PhysicsObject*, PhysicsObject*);
	static bool sphere2Sphere(PhysicsObject*, PhysicsObject*);
	static bool plane2Box(PhysicsObject*, PhysicsObject*);
	static bool sphere2Box(PhysicsObject*, PhysicsObject*);
	static bool box2Box(PhysicsObject*, PhysicsObject*);
	static bool box2Sphere(PhysicsObject*, PhysicsObject*);
	static bool box2Plane(PhysicsObject*, PhysicsObject*);
protected:
	glm::vec2 m_gravity;
	float m_timeStep;
	std::vector<PhysicsObject*> m_actors;
};

static fn collisionFunctionArray[] =
{
PhysicsScene::plane2Plane, PhysicsScene::plane2Sphere, PhysicsScene::plane2Box,
PhysicsScene::sphere2Plane, PhysicsScene::sphere2Sphere, PhysicsScene::sphere2Box,
PhysicsScene::box2Plane, PhysicsScene::box2Sphere, PhysicsScene::box2Box,
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

	glm::vec2 getPosition() { return m_position; }
	float getRotation() { return m_rotation; }
	glm::vec2 getVelocity() { return m_velocity; }
	float getMass() { return m_mass; }

public:  glm::vec2 m_position;  glm::vec2 m_velocity;  float m_mass;  float m_rotation;
};

class Sphere : public Rigidbody {
public:  Sphere(glm::vec2 position, glm::vec2 velocity, float mass, float radius, glm::vec4 colour);  ~Sphere();

	  virtual void makeGizmo();

	  void debug() {};

	  float getRadius() { return m_radius; }  glm::vec4 getColour() { return m_colour; }

public:  float m_radius;  glm::vec4 m_colour;
};

class Plane : public PhysicsObject
{
public:
	Plane();
	Plane(glm::vec2 normal, float distance) : PhysicsObject(PLANE) { m_normal = normal;  m_distanceToOrigin = distance; }
	~Plane() {};
	virtual void fixedUpdate(glm::vec2 gravity, float timeStep) {};
	virtual void debug() {}
	virtual void makeGizmo();
	virtual void resetPosition() {};
	glm::vec2 getNormal() { return m_normal; }
	float getDistance() { return m_distanceToOrigin; }
protected:
	glm::vec2 m_normal;
	float m_distanceToOrigin;
};
