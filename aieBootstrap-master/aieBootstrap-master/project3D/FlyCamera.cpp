#include "FlyCamera.h"
#include "../dependencies/glfw/include/GLFW/glfw3.h"



FlyCamera::FlyCamera(GLFWwindow& window)
{
    m_pWindow = &window;
    m_bMouse2Clicked = false;
}


FlyCamera::~FlyCamera()
{
}

void FlyCamera::Update(float deltaTime)
{
    CameraMovement(deltaTime);
    CameraLook(deltaTime);


    UpdateProjectionViewTransform();
}

void FlyCamera::SetMoveSpeed(float a_MoveSpeed)
{
    m_fMoveSpeed = a_MoveSpeed;
}

void FlyCamera::SetRotationSpeed(float a_RotationSpeed)
{
    m_fRotationSpeed = a_RotationSpeed;
}

void FlyCamera::CameraMovement(float deltaTime)
{
    //Get the cameras forward/up/right
    mat4 mTransfrom = GetTransform();

    vec3 vRight = vec3(mTransfrom[0].x, mTransfrom[0].y, mTransfrom[0].z);
    vec3 vUp = vec3(mTransfrom[1].x, mTransfrom[1].y, mTransfrom[1].z);
    vec3 vForward = vec3(mTransfrom[2].x, mTransfrom[2].y, mTransfrom[2].z);
    vec3 moveDir(0.0f);

    if (glfwGetKey(m_pWindow, GLFW_KEY_W)) {
        moveDir -= vForward * deltaTime;
    }

    if (glfwGetKey(m_pWindow, GLFW_KEY_S)) {
        moveDir += vForward * deltaTime;
    }

    if (glfwGetKey(m_pWindow, GLFW_KEY_A)) {
        moveDir -= vRight * deltaTime;
    }

    if (glfwGetKey(m_pWindow, GLFW_KEY_D)) {
        moveDir += vRight * deltaTime;
    }

    float fLength = glm::length(moveDir);
    if (fLength > 0.01f)
    {
        //moveDir = ((float)deltaTime * m_fMoveSpeed) * glm::normalize(moveDir);
        SetPosition(GetPosition() + moveDir);
    }

}

void FlyCamera::CameraLook(float deltaTime)
{
    if (glfwGetMouseButton(m_pWindow, GLFW_MOUSE_BUTTON_2) == GLFW_PRESS)
    {
        glfwSetInputMode(m_pWindow, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);

        if (m_bMouse2Clicked == false)
        {
            int width, height;
            glfwGetFramebufferSize(m_pWindow, &width, &height);

            m_dCursorX = width / 2.0;
            m_dCursorY = height / 2.0;

            glfwSetCursorPos(m_pWindow, width / 2, height / 2);

            m_bMouse2Clicked = true;
        }
        else
        {
            double mouseX, mouseY;
            glfwGetCursorPos(m_pWindow, &mouseX, &mouseY);

            double xOffset = mouseX - m_dCursorX;
            double yOffset = mouseY - m_dCursorY;

            CalculateRotation(deltaTime, xOffset, yOffset);

        }

        int width, height;
        glfwGetFramebufferSize(m_pWindow, &width, &height);
        glfwSetCursorPos(m_pWindow, width / 2, height / 2);
    }
    else
    {
        m_bMouse2Clicked = false;
        glfwSetInputMode(m_pWindow, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
    }
}

void FlyCamera::CalculateRotation(double deltaTime, double xOffset, double yOffset)
{
    if (yOffset != 0.0) {

        glm::mat4 rot = glm::mat4(1.0f);
        rot = glm::rotate(rot, (float)(m_fRotationSpeed * deltaTime * -yOffset), glm::vec3(1,0,0));

        SetTransform(GetTransform() * rot);
    }

    if (xOffset == 0 && yOffset == 0) return;
    if (xOffset != 0.0) {
        glm::mat4 rot = glm::rotate(rot, (float)(m_fRotationSpeed * deltaTime * -xOffset), glm::vec3(0, 1, 0));
        SetTransform(GetTransform() * rot);
    }
    glm::mat4 oldTransform = GetTransform();
    glm::mat4 transform;
    glm::vec3 worldUp = glm::vec3(0, 1, 0);

    glm::vec3 oldForward = glm::vec3(oldTransform[2].x, oldTransform[2].y, oldTransform[2].z);

    transform[0] = glm::normalize(glm::vec4(glm::cross(worldUp, oldForward), 0));
    glm::vec3 newRight = glm::vec3(transform[0].x, transform[0].y, transform[0].z);

    transform[1] = glm::normalize(glm::vec4(glm::cross(oldForward, newRight), 0));
    transform[2] = glm::normalize(oldTransform[2]);
    transform[3] = oldTransform[3];

    SetTransform(transform);
}