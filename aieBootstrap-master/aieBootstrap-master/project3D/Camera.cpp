#include "Camera.h"



Camera::Camera() : Camera(mat4(1))
{
}

Camera::Camera(mat4 transform) : m_mWorldTransform(transform)
{
    UpdateProjectionViewTransform();
}


Camera::~Camera()
{
}

void Camera::SetPerspective(float fov, float aspectRatio, float near, float far) {
    m_mProjectionTransform = glm::perspective(fov, aspectRatio, near, far);
    UpdateProjectionViewTransform();
}

void Camera::SetLookAt(vec3 from, vec3 to, vec3 up)
{
    m_mWorldTransform = glm::inverse(glm::lookAt(from, to, up));
    UpdateProjectionViewTransform();
}

void Camera::SetPosition(vec3 a_position) {

    m_mWorldTransform[3] = glm::vec4(a_position, 1);
    UpdateProjectionViewTransform();
}

void Camera::SetTransform(mat4 a_transform)
{
    m_mWorldTransform = a_transform;
    UpdateProjectionViewTransform();
}

void Camera::UpdateProjectionViewTransform() {
    m_mViewTransform = glm::inverse(m_mWorldTransform);
    m_mProjectionViewTransform = m_mProjectionTransform * m_mViewTransform;
    UpdateFrustrumPlanes();
}

//bool Camera::isBoundsInFrustrum(const SphereBoundingVolume& bounds)
//{
//    for (int i = 0; i < 6; i++)
//    {
//        const glm::vec4& plane = m_frustrumPlanes[i];
//
//        float d = glm::dot(vec3(plane), bounds.GetCentre()) + plane.w;
//
//        if (d < -bounds.GetRadius())
//        {
//            std::cout << ("Behind, Don't render \n");
//            return false;
//        }
//    }
//    std::cout << "Inside Frustrum, need to render \n";
//    return true;
//}

void Camera::UpdateFrustrumPlanes()
{
    glm::mat4 projView = GetProjectionView();

    //right side
    m_frustrumPlanes[0] = glm::vec4(projView[0][3] - projView[0][0],
        projView[1][3] - projView[1][0],
        projView[2][3] - projView[2][0],
        projView[3][3] - projView[3][0]);

    //left side
    m_frustrumPlanes[1] = glm::vec4(projView[0][3] + projView[0][0],
        projView[1][3] + projView[1][0],
        projView[2][3] + projView[2][0],
        projView[3][3] + projView[3][0]);
    //top
    m_frustrumPlanes[2] = glm::vec4(projView[0][3] - projView[0][1],
        projView[1][3] - projView[1][1],
        projView[2][3] - projView[2][1],
        projView[3][3] - projView[3][1]);

    //bottom
    m_frustrumPlanes[3] = glm::vec4(projView[0][3] + projView[0][1],
        projView[1][3] + projView[1][1],
        projView[2][3] + projView[2][1],
        projView[3][3] + projView[3][1]);

    //Far
    m_frustrumPlanes[4] = glm::vec4(projView[0][3] - projView[0][2],
        projView[1][3] - projView[1][2],
        projView[2][3] - projView[2][2],
        projView[3][3] - projView[3][2]);

    //near
    m_frustrumPlanes[5] = glm::vec4(projView[0][3] + projView[0][2],
        projView[1][3] + projView[1][2],
        projView[2][3] + projView[2][2],
        projView[3][3] + projView[3][2]);

    //plane normalisation, based on length of normal
    for (int i = 0; i < 6; i++)
    {
        float d = glm::length(glm::vec3(m_frustrumPlanes[i]));
        m_frustrumPlanes[i] /= d;
    }
}