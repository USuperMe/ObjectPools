using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    public Bullet bulletPrefab;
    public Transform gun;

    void Update()
    {
        var plane = new Plane(Vector3.up, transform.position);
        Debug.DrawLine(transform.position, Vector3.up*10, new Color(1, 0, 0, 1));
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hit;
        if (plane.Raycast(ray, out hit))
        {
            var aimDirection = Vector3.Normalize(ray.GetPoint(hit) - transform.position);
            Debug.DrawLine(transform.position,aimDirection*10,new Color(1,0,0,1));
            var targetRotation = Quaternion.LookRotation(aimDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime);

            if (Input.GetMouseButtonDown(0)) ;
            // bulletPrefab.Spawn(gun.position, gun.rotation);
            //   bulletPrefab.Spawn(GameObject.Find("P").GetComponent<Transform>(),this.transform.localPosition,gun.rotation);
        }
    }
}
