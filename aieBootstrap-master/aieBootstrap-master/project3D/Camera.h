#pragma once
#include <glm\glm.hpp>
#include <glm\ext.hpp>

#include <iostream>

using glm::mat4;
using glm::vec3;
using glm::vec4;

class Camera
{
public:
    Camera();
    Camera(mat4 transform);
    ~Camera();

    virtual void Update(float deltaTime) = 0;

    void SetPerspective(float fov, float aspectRatio, float near, float far);

    void SetLookAt(const vec3 from, const vec3 to, const vec3 up);

    void SetPosition(vec3 position);
    vec3 GetPosition() const { return vec3(m_mWorldTransform[3].x, m_mWorldTransform[3].y, m_mWorldTransform[3].z); }

    void SetTransform(mat4 a_transform);
    const mat4 GetTransform() { return m_mWorldTransform; }


    const mat4 GetWorldTransform() const { return m_mWorldTransform; }
    const mat4 GetView() const { return m_mViewTransform; }
    const mat4 GetProjection() const { return m_mProjectionTransform; }
    const mat4 GetProjectionView() const { return m_mProjectionViewTransform; }

    //will return true if object bounds interesects this camera
    //rendering frustrum
    //bool isBoundsInFrustrum(const SphereBoundingVolume& bounds);


protected:

    void UpdateProjectionViewTransform();

    mat4     m_mViewTransform;
    mat4     m_mProjectionTransform;
    mat4     m_mProjectionViewTransform;

private:
    mat4     m_mWorldTransform;
    void UpdateViewFromWorld();
    void UpdateWorldFromView();

    //Will be called to recalculate Frustrum Planes.
    void UpdateFrustrumPlanes();

    //array to hold our calculated frustrum planes
    glm::vec4 m_frustrumPlanes[6];

};