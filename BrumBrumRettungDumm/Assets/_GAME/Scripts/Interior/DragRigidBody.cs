using UnityEngine; 
using System.Collections; 
 
public class DragRigidbody : MonoBehaviour 
{ 
    public float maxDistance = 100.0f; 
    
    public float spring = 50.0f; 
    public float damper = 5.0f; 
    public float drag = 10.0f; 
    public float angularDrag = 5.0f; 
    public float distance = 0.2f; 
    public bool attachToCenterOfMass = false;
    [HideInInspector]
    public Camera paramedicCamera;
    
    private SpringJoint springJoint;

    private void Start()
    {
        paramedicCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
    }

    void Update() 
    { 
        if(!Input.GetMouseButtonDown(0)) 
            return; 
        
        RaycastHit hit; 
        Ray ray = paramedicCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        if(!Physics.Raycast(ray, out hit, maxDistance, LayerMask.GetMask("Ragdoll"))) 
            return;
        if(!hit.rigidbody || hit.rigidbody.isKinematic)
            return;
        
        if(!springJoint) 
        { 
            GameObject go = new GameObject("Rigidbody dragger"); 
            Rigidbody body = go.AddComponent<Rigidbody>(); 
            body.isKinematic = true; 
            springJoint = go.AddComponent<SpringJoint>(); 
        } 
        
        springJoint.transform.position = hit.point; 
        if(attachToCenterOfMass) 
        { 
            Vector3 anchor = transform.TransformDirection(hit.rigidbody.centerOfMass) + hit.rigidbody.transform.position; 
            anchor = springJoint.transform.InverseTransformPoint(anchor); 
            springJoint.anchor = anchor; 
        } 
        else 
        { 
            springJoint.anchor = Vector3.zero; 
        } 
        
        springJoint.spring = spring; 
        springJoint.damper = damper; 
        springJoint.maxDistance = distance; 
        springJoint.connectedBody = hit.rigidbody; 
        
        StartCoroutine(DragObject()); 
    } 
    
    IEnumerator DragObject()
    {
        float distance = 2f;
        float oldDrag             = springJoint.connectedBody.drag; 
        float oldAngularDrag     = springJoint.connectedBody.angularDrag; 
        springJoint.connectedBody.drag             = this.drag; 
        springJoint.connectedBody.angularDrag     = this.angularDrag; 
        
        while(Input.GetMouseButton(0)) 
        { 
            Ray ray = paramedicCamera.ScreenPointToRay(Input.mousePosition); 
            springJoint.transform.position = ray.GetPoint(distance); 
            yield return null; 
        } 
        
        if(springJoint.connectedBody) 
        { 
            springJoint.connectedBody.drag             = oldDrag; 
            springJoint.connectedBody.angularDrag     = oldAngularDrag; 
            springJoint.connectedBody                 = null; 
        } 
    } 
}