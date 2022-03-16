using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    Transform camTrans;
    GameObject cam;
    Camera camMain;

    private float startYmouse;
    private Vector2 oldt1, oldt2, oldtmax;

    bool locked = true;
    public bool landed;      // should the model be placed to a horizontal surface
    public bool noGravity;   // should not only a vertical axis rotation of the model be applied, otherwise an azimuth angle is applied and only

    bool wasDoubleTouch, firstTouch, oneTouch;
    float timer;
    bool tapped, upped;
     
    ModelManager modelManager;
    GameObject myGO;
    [HideInInspector]
    public string modelName;
    public string objectId;
    PlaneManager pm;


    void Start()
    {
        cam = Camera.main.gameObject;
        camTrans = Camera.main.transform;
        camMain = Camera.main;
        timer = 0;
        myGO = this.gameObject;

        
        GameObject man = GameObject.FindGameObjectWithTag("Manager");
        modelManager = man.GetComponent<ModelManager>();

        pm = man.GetComponent<PlaneManager>();
        
    }

    void Update()
    {
      /*if (Input.GetMouseButton(0))
        {
            //  if (modelManager.pl != null) modelManager.pl.SetActive(true);
            Vector3 pp = new Vector3();
            int layerMask = 1 << 9;
            //layerMask = ~layerMask;
            Ray ray = camMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                pp = hit.point;
                Debug.Log("hit pos = " + pp + "    GO = " + hit.collider.gameObject + "magnitude = " + (transform.position - pp).magnitude);

              //if ((transform.position - pp).magnitude < 0.6f)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(pp.x, transform.position.y, pp.z), 0.04f);
                if (modelManager.shadowObj != null)
                    modelManager.shadowObj.transform.position = new Vector3(
                        transform.position.x, modelManager.shadowObj.transform.position.y, transform.position.z);
            }
        }*/
        if (modelManager.GetEditMode())
        {
            if (!locked)
            {
                GetComponent<Collider>().enabled = true;
                if (Input.touchCount == 2)
                {
                    float razn = Input.mousePosition.y - startYmouse;
                    wasDoubleTouch = true;
                    Vector2 t1 = Input.GetTouch(0).position;
                    Vector2 t2 = Input.GetTouch(1).position;
                    int sign = 0;
                    if ((t2 - t1).magnitude < 500)
                    {
                        float rast = Vector3.Magnitude(transform.position - camTrans.position);
                        if (((rast > 1) || (razn > 0))) //|| ((rast < 20)&& (razn < 0)))
                        {
                            //   Vector3 transV = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
                            if (!firstTouch)
                            {
                                transform.position = new Vector3(transform.position.x,
                                                                 transform.position.y + (razn) * 0.0015f,
                                                                 transform.position.z);
                                //modelManager.pl.transform.position = new Vector3(modelManager.pl.transform.position.x, modelManager.pl.transform.position.y + (razn) * 0.003f, modelManager.pl.transform.position.z);
                            }   //transform.Translate(transV * (razn) * 0.003f, Space.World);
                            firstTouch = false;
                        }
                        startYmouse = Input.mousePosition.y;
                    }
                    else
                    {
                        float scaleKoef = ((t1 - t2).magnitude - (oldt1 - oldt2).magnitude) * 0.0004f;
                        if (Mathf.Abs(scaleKoef) > 0.04f) { scaleKoef = 0; }

                        transform.localScale = new Vector3(
                            transform.localScale.x + (scaleKoef * transform.localScale.x),
                            transform.localScale.y + (scaleKoef * transform.localScale.y),
                            transform.localScale.z + (scaleKoef * transform.localScale.z));

                        if (modelManager.shadowObj != null) {
                            modelManager.shadowObj.transform.localScale = new Vector3(
                                modelManager.shadowObj.transform.localScale.x + (scaleKoef * modelManager.shadowObj.transform.localScale.x),
                                modelManager.shadowObj.transform.localScale.y,
                                modelManager.shadowObj.transform.localScale.z + (scaleKoef * modelManager.shadowObj.transform.localScale.z));
                        }
                        //Debug.Log("transform.localScale = " + transform.localScale + ", scaleKoef = " + scaleKoef);
                        if (oldt1.x < -1f)
                        {
                            oldt1 = t1; oldt2 = t2;
                        }

                        Vector2 tmax = t2;
                        if (t1.y > t2.y) tmax = t1;
                        if (tmax.x < oldtmax.x) { sign = -1; }
                        if (tmax.x > oldtmax.x) { sign = 1; }
                        float delta = (oldt1 - t1).magnitude + (oldt2 - t2).magnitude;
                        oldt1 = t1;
                        oldt2 = t2;
                        oldtmax = tmax;
                        transform.Rotate(transform.up, -sign * delta * 0.04f);
                    }
                }
                else if (Input.touchCount == 1)
                {
                    oneTouch = true;
                    //----
                    if ((Input.touchCount < 2) && (!wasDoubleTouch) && (oneTouch))
                    {
                        //  if (modelManager.pl != null) modelManager.pl.SetActive(true);
                        Vector3 pp = new Vector3();
                        int layerMask = 1 << 9;
                        //layerMask = ~layerMask;
                        Ray ray = camMain.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
                        {
                            pp = hit.point;
                          //  Debug.Log("hit pos = " + pp + "    GO = " + hit.collider.gameObject + "magnitude = " + (transform.position - pp).magnitude);

                            transform.position = Vector3.MoveTowards(transform.position,
                                new Vector3(pp.x, transform.position.y, pp.z), 0.08f);

                            if (modelManager.shadowObj != null)
                                modelManager.shadowObj.transform.position = new Vector3(
                                    transform.position.x,
                                    modelManager.shadowObj.transform.position.y,
                                    transform.position.z);
                        }
                    }
                    //----
                }
                else
                {
                    oldt1 = new Vector2(-2, -2);
                    oldt2 = new Vector2(-2, -2);
                    startYmouse = Input.mousePosition.y;
                    firstTouch = true;
                    wasDoubleTouch = false;
                    oneTouch = false;
                }

                if (modelManager.pl != null)
                {
                    float newPosY = transform.position.y + 0.3f;    //FixMe: ?
                    if (newPosY >= camTrans.position.y) {
                        newPosY = transform.position.y + 0.7f;      //FixMe: ?
                    }
                    modelManager.pl.transform.position = new Vector3(
                        modelManager.pl.transform.position.x,
                        newPosY,
                        modelManager.pl.transform.position.z);
                }
            }
            else  // if (!locked)
            {
                GetComponent<Collider>().enabled = false;
            }
        }
        else  // if (modelManager.GetEditMode())
        {
            if (tapped && locked)
            {
                timer = timer + Time.deltaTime;
                if (timer > 3)
                    Debug.Log(timer + "        " + modelManager.timeForLongTap);
            }

            if ((upped) || (timer > 2f))
            {
                if (timer > modelManager.timeForLongTap)
                {
                    Debug.Log(name + " LONG COLIDER DOWN!");
                    modelManager.DeleteMenu(this.gameObject, objectId);
                    // longTap.SetActive(true);
                    //  setLocked(false);
                }
                upped = false;
                tapped = false;
                timer = 0;
            }
        }

// #if !UNITY_EDITOR
        if (!noGravity) {
            myGO.transform.eulerAngles = new Vector3(0, myGO.transform.eulerAngles.y, 0);
        }
// #endif
        if (landed && locked) {
            myGO.transform.position = new Vector3(myGO.transform.position.x, pm.getPlaneY(), myGO.transform.position.z);
        }
    }

    void OnMouseDown()
    {
        if (!locked)
        {
            // if (modelManager.pl != null) modelManager.pl.SetActive(true);

            mZCoord = camMain.WorldToScreenPoint(gameObject.transform.position).z;

            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

            startYmouse = Input.mousePosition.y;
        }
        else
        {
            tapped = true;
            Debug.Log(name + "COLLIDER ABEKT Click in Progress");
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if (!locked)
        {
            /*if ((Input.touchCount < 2) && (!wasDoubleTouch) && (oneTouch))
            {
                // if (modelManager.pl != null) modelManager.pl.SetActive(true);
                Vector3 pp = new Vector3();
                int layerMask = 1 << 9;
                //layerMask = ~layerMask;
                Ray ray = camMain.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, layerMask))
                {
                    pp = hit.point;
                    Debug.Log("hit pos = " + pp + "    GO = " + hit.collider.gameObject);
                    transform.position = new Vector3 (pp.x, transform.position.y, pp.z);
                    if (modelManager.shadowObj != null)
                        modelManager.shadowObj.transform.position = new Vector3(transform.position.x,
                                                                                modelManager.shadowObj.transform.position.y,
                                                                                transform.position.z);
                }
                // if (modelManager.pl != null) modelManager.pl.SetActive(false);

                // transform.position = GetMouseAsWorldPoint() + mOffset;
                //    transform.position = new Vector3(GetMouseAsWorldPoint().x + mOffset.x, GetMouseAsWorldPoint().y + mOffset.y, transform.position.z);// GetMouseAsWorldPoint().z + mOffset.z);
            }*/
        }
    }

    void OnMouseUp()
    {
        // Debug.Log("cam.transform.forward = " + cam.transform.forward);
        if (tapped) upped = true;
        Debug.Log(name + " UPPED!!!");
        // if (modelManager.pl != null) modelManager.pl.SetActive(false);
    }


    public void setLocked(bool loc)
    {
        GameObject man = GameObject.FindGameObjectWithTag("Manager");
        modelManager = man.GetComponent<ModelManager>();

        locked = loc;
        modelManager.SetEditMode(!loc);
    }

}
