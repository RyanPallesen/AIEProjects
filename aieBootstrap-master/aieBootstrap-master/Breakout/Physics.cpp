#include "Physics.h"
#include <list>
#include <Gizmos.h>
PhysicsScene::PhysicsScene(float timestep, glm::vec2 gravity) : m_timeStep(0.01f), m_gravity(glm::vec2(0, 0)) { }
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

void PhysicsScene::update(float dt) {
	static std::list<PhysicsObject*> dirty;

	// update physics at a fixed time step 
	static float accumulatedTime = 0.0f;
	accumulatedTime += dt;
	while (accumulatedTime >= m_timeStep)
	{
		for (auto pActor : m_actors) { pActor->fixedUpdate(m_gravity, m_timeStep); }   accumulatedTime -= m_timeStep;

		// check for collisions (ideally you'd want to have some sort of
		// scene management in place)
		for (auto pActor : m_actors)
		{
			for (auto pOther : m_actors)
			{
				if (pActor == pOther)
					continue;
				if (std::find(dirty.begin(), dirty.end(), pActor) != dirty.end() && std::find(dirty.begin(), dirty.end(), pOther) != dirty.end())
					continue;

				Rigidbody* pRigid = dynamic_cast<Rigidbody*>(pActor);
				if (pRigid->checkCollision(pOther) == true)
				{
					if (!(pRigid->getVelocity() == glm::vec2(0,0)))
					{
						pRigid->applyForceToActor(dynamic_cast<Rigidbody*>(pOther), pRigid->getVelocity() * (pRigid->getMass() + dynamic_cast<Rigidbody*>(pOther)->getMass()));
						dirty.push_back(pRigid);
						dirty.push_back(pOther);
					}


				}
			}
		}



		dirty.clear();
	}
}
void Rigidbody::fixedUpdate(glm::vec2 gravity, float timeStep)
{
	applyForce(gravity * m_mass * timeStep);
	m_position += m_velocity * timeStep;
}


void Sphere::makeGizmo()
{
	aie::Gizmos::add2DCircle(glm::vec2(m_position), m_radius, 12, m_colour);

}

bool Sphere::checkCollision(PhysicsObject* pOther)
{
	switch (pOther->m_shapeID)
	{
	case SPHERE:
	{
		Sphere* workingSphere = dynamic_cast<Sphere*>(pOther);
		glm::vec2 result = (workingSphere->m_position - m_position);
		float distance = sqrt((result.x * result.x) + (result.y * result.y));

		if (distance < workingSphere->m_radius + m_radius)
		{
			return true;
		}
		break;
	}
	}
}

Sphere::Sphere(glm::vec2 position, glm::vec2 velocity, float mass, float radius, glm::vec4 colour) : Rigidbody(SPHERE, position, velocity, 0, mass) { m_radius = radius;  m_colour = colour; }