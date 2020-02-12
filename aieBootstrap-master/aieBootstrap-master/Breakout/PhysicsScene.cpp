#include "PhysicsScene.h"
#include "Sphere.h"
#include "Plane.h"
PhysicsScene::PhysicsScene(float timestep, glm::vec2 gravity) : m_timeStep(timestep), m_gravity(gravity) { }
PhysicsScene::~PhysicsScene() { for (auto pActor : m_actors) { delete pActor; } }

void PhysicsScene::addActor(PhysicsObject* actor)
{
	m_actors.push_back(actor);
}
void PhysicsScene::removeActor(PhysicsObject* actor)
{
	for (int i = 0; i < m_actors.size(); i++)
	{
		if (m_actors[i] == actor)
			m_actors.erase(m_actors.begin() + i);
	}
}
void PhysicsScene::updateGizmos() { for (auto pActor : m_actors) { pActor->makeGizmo(); } }

void PhysicsScene::checkForCollision()
{

	int actorCount = m_actors.size();
	//need to check for collisions against all objects except this one.
	for (int outer = 0; outer < actorCount - 1; outer++)
	{
		for (int inner = outer + 1; inner < actorCount; inner++)
		{
			PhysicsObject* object1 = m_actors[outer];
			PhysicsObject* object2 = m_actors[inner];
			int shapeId1 = object1->getShapeID();
			int shapeId2 = object2->getShapeID();
			// using function pointers
			int functionIdx = (shapeId1 * SHAPE_COUNT) + shapeId2;

			if (object1 == object2)
				continue;

			fn collisionFunctionPtr = collisionFunctionArray[functionIdx];
			if (collisionFunctionPtr != nullptr)
			{
				// did a collision occur?
				if (collisionFunctionPtr(object1, object2))
				{

				}
			}
		}
	}

}


bool PhysicsScene::sphere2Sphere(PhysicsObject* obj1, PhysicsObject* obj2)
{
	//try to cast objects to sphere and sphere
	Sphere* sphere1 = dynamic_cast<Sphere*>(obj1);
	Sphere* sphere2 = dynamic_cast<Sphere*>(obj2);
	//if we are successful then test for collision
	if (sphere1 != nullptr && sphere2 != nullptr)
	{
		Sphere* workingSphere = sphere1;
		glm::vec2 result = (workingSphere->m_position - sphere2->m_position);
		float distance = sqrt((result.x * result.x) + (result.y * result.y));

		if (distance < workingSphere->m_radius + sphere2->m_radius)
		{
			sphere1->resolveCollision(sphere2);
			return true;
		}
	}
	return false;
}

bool PhysicsScene::sphere2Plane(PhysicsObject* obj1, PhysicsObject* obj2)
{
	Sphere* sphere = dynamic_cast<Sphere*>(obj1);
	Plane* plane = dynamic_cast<Plane*>(obj2);
	//if we are successful then test for collision
	if (sphere != nullptr && plane != nullptr)
	{
		glm::vec2 collisionNormal = plane->getNormal();
		float sphereToPlane = glm::dot(
			sphere->getPosition(),
			plane->getNormal()) - plane->getDistance();
		// if we are behind plane then we flip the normal
		if (sphereToPlane < 0) {
			collisionNormal *= -1;
			sphereToPlane *= -1;
		}
		float intersection = sphere->getRadius() - sphereToPlane;
		if (intersection > 0) {
			//set sphere velocity to zero here
			plane->resolveCollision(sphere);

			return true;
		}
	}
	return false;
}


bool PhysicsScene::plane2Plane(PhysicsObject*, PhysicsObject*) { return false; };
bool PhysicsScene::plane2Sphere(PhysicsObject*, PhysicsObject*) { return false; };
bool PhysicsScene::plane2Box(PhysicsObject*, PhysicsObject*) { return false; };
bool PhysicsScene::sphere2Box(PhysicsObject*, PhysicsObject*) { return false; };
bool PhysicsScene::box2Box(PhysicsObject*, PhysicsObject*) { return false; };
bool PhysicsScene::box2Sphere(PhysicsObject*, PhysicsObject*) { return false; };
bool PhysicsScene::box2Plane(PhysicsObject*, PhysicsObject*) { return false; };

void PhysicsScene::update(float dt) {


	// update physics at a fixed time step 
	static float accumulatedTime = 0.0f;
	accumulatedTime += dt;
	while (accumulatedTime >= m_timeStep)
	{
		for (auto pActor : m_actors) { pActor->fixedUpdate(m_gravity, m_timeStep); }   accumulatedTime -= m_timeStep;

		checkForCollision();

	}
}