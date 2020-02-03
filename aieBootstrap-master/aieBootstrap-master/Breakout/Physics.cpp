#include "Physics.h"
PhysicsScene::PhysicsScene(float timestep, glm::vec2 gravity) : m_timeStep(0.01f), m_gravity(glm::vec2(0, 0)) { }

void PhysicsScene::update(float dt) {  // update physics at a fixed time step 

    static float accumulatedTime = 0.0f;  accumulatedTime += dt;

    while (accumulatedTime >= m_timeStep) { for (auto pActor : m_actors) { pActor->fixedUpdate(m_gravity, m_timeStep); }      accumulatedTime -= m_timeStep; }
}

void PhysicsScene::updateGizmos() { for (auto pActor : m_actors) { pActor->makeGizmo(); } }