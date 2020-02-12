#include "Plane.h"
#include <Gizmos.h>
Plane::Plane() : PhysicsObject(ShapeType::PLANE) {
	m_distanceToOrigin = 0;
	m_normal = glm::vec2(0, 1);
	m_colour = glm::vec4(1, 1, 1, 1);
}
void Plane::resolveCollision(Rigidbody* actor2)
{
	glm::vec2 relativeVelocity = actor2->getVelocity();
	float elasticity = (actor2->m_elasticity);
	float j = glm::dot(-(1 + elasticity) * (relativeVelocity), getNormal()) / (1 / actor2->getMass());
	glm::vec2 force = getNormal() * j;
	actor2->applyForce(force);

}

void Plane::makeGizmo()
{
	float lineSegmentLength = 300;
	glm::vec2 centerPoint = m_normal * m_distanceToOrigin;
	// easy to rotate normal through 90 degrees around z
	glm::vec2 parallel(m_normal.y, -m_normal.x);
	glm::vec4 colour(1, 1, 1, 1);

	glm::vec2 start = centerPoint + (parallel * lineSegmentLength);
	glm::vec2 end = centerPoint - (parallel * lineSegmentLength);
	aie::Gizmos::add2DLine(start, end, colour);
	aie::Gizmos::add2DLine(start, end, getColour());
}