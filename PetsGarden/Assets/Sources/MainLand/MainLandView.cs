using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using PModule;

public class MainLandView : MonoBehaviour
{
    private Camera m_camera;

    private Transform m_grab;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = UnityEngineExt.GetNodeFromRoot("Camera_MainLand").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseButtonDetect();
    }

    private void MouseButtonDetect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseBtnDownFn();
        } else if (Input.GetMouseButton(0))
        {
            MouseBtnFn();
        } else if (Input.GetMouseButtonUp(0))
        {
            MouseBtnUpFn();
        }
    }

    private void MouseBtnDownFn()
    {
        Debug.Log("mouse button down");
        Vector3 pos = Input.mousePosition;
        Ray ray = m_camera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("点击到物体：" + hit.collider.gameObject.name);
            Debug.Log("点击坐标为：" + hit.point);
            m_grab = hit.collider.gameObject.transform;
        }
        else
        {
            Debug.Log("未点击到物体");
        }

    }

    private void MouseBtnFn()
    {
        Debug.Log("mouse button");

        Vector3 pos = Input.mousePosition;
        Ray ray = m_camera.ScreenPointToRay(pos);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("点击坐标为：" + hit.point);
            m_grab.position = new Vector3(hit.point.x, m_grab.position.y, hit.point.z);
        }
        else
        {
            Debug.Log("未接收到射线");
        }

    }

    private void MouseBtnUpFn()
    {
        Debug.Log("mouse button up");
        // Vector3 pos = Input.mousePosition;
        m_grab = null;
    }

}

