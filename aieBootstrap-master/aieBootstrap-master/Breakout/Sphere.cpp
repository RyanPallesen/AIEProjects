#include "Sphere.h"
#include <Gizmos.h>

void Sphere::makeGizmo()
{
	aie::Gizmos::add2DCircle(glm::vec2(m_position), m_radius, 12, m_colour);

}

Sphere::Sphere(glm::vec2 position, glm::vec2 velocity, float mass, float radius, glm::vec4 colour) : Rigidbody(SPHERE, position, velocity, 0, mass) { m_radius = radius;  m_colour = colour; }
