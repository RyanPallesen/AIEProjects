#pragma once
#include "Camera.h"
#include <glm\glm.hpp>
#include <gl_core_4_4.h>
#include <imgui_glfw3.h>

using glm::vec3;


class FlyCamera : public Camera
{
public:
    FlyCamera(GLFWwindow& window);
    ~FlyCamera();

    void Update(float deltaTime);

    void SetMoveSpeed(float a_MoveSpeed);
    float GetMoveSpeed() { return m_fMoveSpeed; }

    void SetRotationSpeed(float a_RotationSpeed);
    float GetRotationSpeed() { return m_fRotationSpeed; };

    void CameraMovement(float deltaTime);
    void CameraLook(float deltaTime);
    void CalculateRotation(double dt, double xOffset, double yOffset);


protected:
    float          m_fMoveSpeed = 6.0f;
    float          m_fRotationSpeed = 0.5f;

    bool           m_bMouse2Clicked;

    double         m_dCursorX, m_dCursorY;

    GLFWwindow* m_pWindow;

};