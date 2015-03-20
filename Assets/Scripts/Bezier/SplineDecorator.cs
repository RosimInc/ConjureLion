using UnityEngine;
using System.Collections;

public class SplineDecorator : MonoBehaviour {
	private LineRenderer lineRend;
	public Transform toe;
	
	[System.Serializable]
	public class LineOptions
	{
		public float lineWidth = 1f;
		public Material lineMaterial;
	}

	//Line options
	public LineOptions lineOptions = new LineOptions();
	
	public BezierSpline spline;
	
	private void Awake () {
		CreateLineRenderer();
		
		lineRend.SetVertexCount(10000);
		
		for(int i = 0; i < 10000; i++ )
		{
	        Vector3 position = spline.GetPoint((float)i/10000);
	        //Debug.Log("position "+  position.x);
			lineRend.SetPosition(i, position);
		}
		
		//Butées :
		Transform beginToe = Instantiate(toe) as Transform;
		beginToe.transform.localPosition = spline.GetPoint(0);
		
		toe.transform.localPosition = spline.GetPoint(1);
	}
	
	/*
	 * Creates the line renderer.
	 */
	private void CreateLineRenderer()
	{
		lineRend = this.gameObject.AddComponent("LineRenderer")
			as LineRenderer;
		lineRend.SetWidth(lineOptions.lineWidth, lineOptions.lineWidth);
		lineRend.material = lineOptions.lineMaterial;
	}
	
}
