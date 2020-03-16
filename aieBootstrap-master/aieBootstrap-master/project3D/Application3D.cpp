#include "Application3D.h"
#include "Gizmos.h"
#include "Input.h"
#include <glm/glm.hpp>
#include <glm/ext.hpp>

using glm::vec3;
using glm::vec4;
using glm::mat4;
using aie::Gizmos;

Application3D::Application3D() {

}

Application3D::~Application3D() {

}

bool Application3D::startup() {
	

	m_camera = new FlyCamera(*Application::m_window);

	setBackgroundColour(0.25f, 0.25f, 0.25f);

	// initialise gizmo primitive counts
	Gizmos::create(10000, 10000, 10000, 10000);

	// create simple camera transforms
	m_viewMatrix = glm::lookAt(vec3(10), vec3(0), vec3(0, 1, 0));
	m_projectionMatrix = glm::perspective(glm::pi<float>() * 0.25f,
										  getWindowWidth() / (float)getWindowHeight(),
										  0.1f, 1000.f);

	return true;
}

void Application3D::shutdown() {

	Gizmos::destroy();
}

void Application3D::update(float deltaTime) {


	m_camera->Update(deltaTime);
	
	// query time since application started
	float time = getTime();

	// rotate camera
	//m_viewMatrix = glm::lookAt(vec3(glm::sin(time) * 10, 10, glm::cos(time) * 10), vec3(0), vec3(0, 1, 0));

	// wipe the gizmos clean for this frame
	Gizmos::clear();

	// draw a simple grid with gizmos
	vec4 white(1);
	vec4 black(0, 0, 0, 1);
	for (int i = 0; i < 101; ++i) {
		Gizmos::addLine(vec3(-50 + i, 0, 10),
			vec3(-50 + i, 0, -50),
			i == 50 ? white : black);
		Gizmos::addLine(vec3(50, 0, -50 + i),
			vec3(-50, 0, -50 + i),
			i == 50 ? white : black);
	}

	// add a transform so that we can see the axis
	Gizmos::addTransform(mat4(1));



	struct planet
	{
		planet(vec4 _colour, vec3 _position, float _selfRotationSpeed, float _orbitSpeed, float _radius)
		{
			colour = _colour;
			position = _position;
			selfRotationSpeed = _selfRotationSpeed;
			orbitSpeed = _orbitSpeed;
			radius = _radius;
		}

		vec4 colour;
		vec3 position;
		float radius;
		float selfRotationSpeed;
		float orbitSpeed;
	};

	planet solarSystem[4]
	{
		planet(vec4(0.921, 0.619, 0.058,1.0),vec3(0,0,0),1,0,1),//Sun
		planet(vec4(0.349, 0.309, 0.227,1.0),vec3(3,0,0),1,0.5, 0.3),//Mercury
		planet(vec4(0.705, 0.396, 0.294,1.0),vec3(5,0,0),1,1.2, 0.45),//Mars
		planet(vec4(0.066, 0.192, 0.313,1.0),vec3(8,0,0),1, 1, 0.7),//Earth
	};

	for (int i = 0; i < 4; i++)
	{
		planet currentPlanet = solarSystem[i];

		glm::mat4 trans = glm::mat4(1.0f);
		trans = glm::rotate(trans, time * currentPlanet.orbitSpeed, glm::vec3(0.0f, 1.0f, 0.0f));
		glm::vec4 result = trans * glm::vec4(currentPlanet.position, 2.0f);



		Gizmos::addSphere(result, currentPlanet.radius, 8, 8, currentPlanet.colour);
	}


	// demonstrate 2D gizmos
	//Gizmos::add2DAABB(glm::vec2(getWindowWidth() / 2, 100),
	//				  glm::vec2(getWindowWidth() / 2 * (fmod(getTime(), 3.f) / 3), 20),
	//				  vec4(0, 1, 1, 1));

	// quit if we press escape
	aie::Input* input = aie::Input::getInstance();

	if (input->isKeyDown(aie::INPUT_KEY_ESCAPE))
		quit();
}

void Application3D::draw() {

	// wipe the screen to the background colour
	clearScreen();

	// update perspective in case window resized
	m_projectionMatrix = glm::perspective(glm::pi<float>() * 0.25f,
										  getWindowWidth() / (float)getWindowHeight(),
										  0.1f, 1000.f);



	// draw 3D gizmos
	Gizmos::draw(m_projectionMatrix * m_camera->GetView());

	// draw 2D gizmos using an orthogonal projection matrix (or screen dimensions)
	Gizmos::draw2D((float)getWindowWidth(), (float)getWindowHeight());
}