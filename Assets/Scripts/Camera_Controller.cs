using UnityEngine;
using System.Collections;

public class Camera_Controller : MonoBehaviour {

    private float mouseX;
    // Per la rotazione verticale: possibile eliminazione
    //    private float mouseY;

  //  private float VerticalRotationMin = 0f;
   // private float VerticalRotationMax = 65f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate(){
        CameraMouseRotation();

        mouseX = Input.mousePosition.x;
  //      mouseY = Input.mousePosition.y;
    }

    // per la rotazione del mouse
   public void CameraMouseRotation()
    {
        float easeFactor = 10f;

        if(Input.GetMouseButton(1))
        {
            //orizzontalmente
            if(Input.mousePosition.x!= mouseX)
            {
                float CameraRotationY = (Input.mousePosition.x - mouseX)*easeFactor*Time.deltaTime;
                this.transform.Rotate(0, CameraRotationY, 0);
            }
            /*
            if(Input.mousePosition.y!= mouseY)
            {
                GameObject MainCamera = this.transform.FindChild("Main Camera").gameObject;

                float camerarotationX = (mouseY - Input.mousePosition.y) * easeFactor * Time.deltaTime;
                float desiredRotationX = MainCamera.transform.eulerAngles.x + camerarotationX;

                if (desiredRotationX>= VerticalRotationMin&& desiredRotationX <= VerticalRotationMax)
                {
                    this.transform.Rotate(camerarotationX, 0, 0);
                }
            }
            */
        }
    }

}
