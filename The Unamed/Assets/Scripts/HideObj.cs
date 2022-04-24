using System;
using System.Collections;
using UnityEngine;
using zheavy.PlayerControl;

public class HideObj : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Transform camera;

    private void Awake()
    {
        camera = this.transform;
    }

    void Update()
    {
        GetAllObjectsInTheWay();
    }

    private void GetAllObjectsInTheWay()
    {
        float camToPlayerDistance = Vector3.Magnitude(camera.position - player.position);
        Ray hitForward = new Ray(camera.position, player.position - camera.position);
        Ray hitBackwards= new Ray(player.position, camera.position - player.position);

        RaycastHit[] hitsForward = Physics.RaycastAll(hitForward, camToPlayerDistance);
        RaycastHit[] hitsBackwards = Physics.RaycastAll(hitBackwards, camToPlayerDistance);

        foreach (var hit in hitsForward)
        {
            if (hit.collider.GetComponent<ObjectToHide>())
            {
                hit.collider.GetComponent<ObjectToHide>().FadeObject();
            }
        }

        foreach (var hit in hitsBackwards)
        {
            if (hit.collider.GetComponent<ObjectToHide>())
            {
                hit.collider.GetComponent<ObjectToHide>().FadeObject();
            }
        }
    }
}
