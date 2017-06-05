using UnityEngine;


public class CameraController : MonoBehaviour {

    private void Awake()
    {
        SetObliqueness(0, 0);
    }

    void FixedUpdate() {

		float xAxisValue = Input.GetAxis ("Horizontal");
		float zAxisValue = Input.GetAxis ("Vertical");

		if (Camera.current != null) {
			Camera.current.transform.Translate (new Vector3 (xAxisValue * 0.6f, zAxisValue * 0.6f, 0));
		}

        ObliqueProjection op = gameObject.GetComponent<ObliqueProjection>();
        float rAxisValue = Input.GetAxis("RotateCamera");
        //Debug.Log(rAxisValue);
        if (op != null)
        {
            if (rAxisValue != 0)
            {
                op.angle += rAxisValue;
                op.Apply();
            }
        } else
        {
            if (Camera.current != null)
            {
                Transform target = Camera.current.transform;
                target.eulerAngles = new Vector3(target.localRotation.eulerAngles.x, target.localRotation.eulerAngles.y + rAxisValue, target.localRotation.eulerAngles.z);
            }
        }
	}

    void SetObliqueness(float horizObl, float vertObl)
    {
        Matrix4x4 mat = Camera.main.projectionMatrix;
        mat[0, 2] = horizObl;
        mat[1, 2] = vertObl;
        Camera.main.projectionMatrix = mat;
    }
}
