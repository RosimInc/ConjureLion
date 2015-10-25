using UnityEngine;
using System.Collections;

public class SplineDecorator : MonoBehaviour {
	private LineRenderer lineRend;
	private LineRenderer lineIntRend;
	public Transform toe;
	
	[System.Serializable]
	public class LineOptions
	{
		public float lineWidth = 1f;
		public Material lineMaterial;
		public float lineIntWidth = 2f;
		public Material lineIntMaterial;
	}

	//Line options
	public LineOptions lineOptions = new LineOptions();
	
	public BezierSpline spline;
	
	private void Start () {
		//Line Ext
		GameObject lineExt = new GameObject();
		lineExt.transform.parent = this.transform;
		Vector3 lineExtPos = lineExt.transform.position;
		lineExtPos.z = 2.5f;
		lineExt.transform.position = lineExt.transform.position + new Vector3(0,0,-0.3f);
		lineExt.layer = 13;
		lineRend = lineExt.AddComponent<LineRenderer>();
		lineRend.useWorldSpace = true;
		lineRend.SetWidth(lineOptions.lineWidth, lineOptions.lineWidth);
		lineRend.material = lineOptions.lineMaterial;
		
		lineRend.SetVertexCount(10000);
		
		for(int i = 0; i < 10000; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/10000);
			lineRend.SetPosition(i, position);
		}
		
		//Line Int
		lineIntRend = this.gameObject.AddComponent<LineRenderer>() as LineRenderer;
		lineIntRend.useWorldSpace = true;
		lineIntRend.SetWidth(lineOptions.lineIntWidth, lineOptions.lineIntWidth);
		lineIntRend.material = lineOptions.lineIntMaterial;
		
		lineIntRend.SetVertexCount(10000);
		
		for(int i = 0; i < 10000; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/10000);
			lineIntRend.SetPosition(i, position);
		}
		
		//Butées :
		Transform beginToe = Instantiate(toe) as Transform;
		Vector3 beginPosition = spline.GetPoint(0);
		beginPosition.z = 1.1f;
		beginToe.transform.position = beginPosition;
		
		Vector3 endPosition = spline.GetPoint(1);
		endPosition.z = 1.1f;
		toe.transform.position = endPosition;
		
	}
	
}
