#include "RigidBody.h"

void Rigidbody::fixedUpdate(glm::vec2 gravity, float timeStep)
{
	applyForce(gravity * m_mass * timeStep);
	m_position += m_velocity * timeStep;
	m_velocity -= m_velocity * m_linearDrag * timeStep;
	m_angularVelocity -= m_angularVelocity * m_angularDrag * timeStep;

	if (length(m_velocity) < 0) {
		m_velocity = glm::vec2(0, 0);
	}
	if (abs(m_angularVelocity) < 0) {
		m_angularVelocity = 0;
	}

}

void Rigidbody::resolveCollision(Rigidbody* actor2)
{
	glm::vec2 normal = glm::normalize(actor2->getPosition() - m_position);
	glm::vec2 relativeVelocity = actor2->getVelocity() - m_velocity;
	float elasticity = (m_elasticity + actor2->m_elasticity)/2.0f;
	float j = glm::dot(-(1 + elasticity) * (relativeVelocity), normal) / glm::dot(normal, normal * ((1 / m_mass) + (1 / actor2->getMass())));
	glm::vec2 force = normal * j;
	applyForceToActor(actor2, force);
}