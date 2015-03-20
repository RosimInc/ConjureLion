using UnityEngine;
using System.Collections;
using System;

public class SplinePipe : MonoBehaviour {
	private LineRenderer lineRend;
	private LineRenderer lineIntRend;
	public Transform toe;
	
	[System.Serializable]
	public class LineOptions
	{
		public float pipeRadius = 0.6f;
		public float pipeInteriorRadius = 0.5f;
		public Material borderMaterial;
	}
	
	//Line options
	public LineOptions lineOptions = new LineOptions();
	
	public BezierSpline spline;
	
	private void Start () {
		float lineRadius = (lineOptions.pipeInteriorRadius + lineOptions.pipeRadius )/2;
		
		//Butées :
		Transform beginToe = Instantiate(toe) as Transform;
		Vector3 beginPosition = spline.GetPoint(0);
		beginPosition.z = 1.1f;
		beginToe.transform.position = beginPosition;
		
		Vector3 endPosition = spline.GetPoint(1);
		endPosition.z = 1.1f;
		toe.transform.position = endPosition;
		
	//Partie supérieure
		//Partie supérieure affichage
		GameObject borderSup = new GameObject();
		borderSup.transform.parent = this.transform;
		
		Vector3 lineExtPos = borderSup.transform.position;
		lineRend = borderSup.AddComponent<LineRenderer>();
		lineRend.useWorldSpace = true;
		
		float borderWidth = lineOptions.pipeRadius - lineOptions.pipeInteriorRadius;
		lineRend.SetWidth(borderWidth, borderWidth);
		lineRend.material = lineOptions.borderMaterial;
		
		lineRend.SetVertexCount(1000);
		
		for(int i = 0; i < 1000; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/1000);
			Vector3 direction = spline.GetDirection((float)i/1000);
			Vector3 translation = new Vector3 (-direction.y * lineRadius, direction.x * lineRadius, 0);
			lineRend.SetPosition(i, position+translation);
		}
		
		PolygonCollider2D borderCol = this.gameObject.AddComponent<PolygonCollider2D>();
		borderCol.pathCount = 2;
		
		//Partie supérieure collider
		Vector2[] borderSupPath = new Vector2[200];
		for(int i = 0; i < 100; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				-direction.y * lineOptions.pipeInteriorRadius, 
				direction.x * lineOptions.pipeInteriorRadius, 
				0);
			borderSupPath[i] = Vector3to2(position+translation);
		}
		for(int i = 99; i >= 0; i-- )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				-direction.y * lineOptions.pipeRadius, 
				direction.x * lineOptions.pipeRadius, 
				0);
			borderSupPath[199 - i] = Vector3to2 (position+translation);
		}
		borderCol.SetPath(0, borderSupPath);
		
		//Partie inférieure
		GameObject lineInf = new GameObject();
		lineInf.transform.parent = this.transform;
		
		LineRenderer lineInfRend = lineInf.AddComponent<LineRenderer>();
		lineInfRend.useWorldSpace = true;
		
		lineInfRend.SetWidth(borderWidth, borderWidth);
		lineInfRend.material = lineOptions.borderMaterial;
		
		lineInfRend.SetVertexCount(1000);
		
		for(int i = 0; i < 1000; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/1000);
			Vector3 direction = spline.GetDirection((float)i/1000);
			Vector3 translation = new Vector3 (direction.y * lineRadius, -direction.x* lineRadius, 0);
			lineInfRend.SetPosition(i, position+translation);
		}
		
		//Partie inférieure collider
		Vector2[] borderInfPath = new Vector2[200];
		for(int i = 0; i < 100; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				direction.y * lineOptions.pipeInteriorRadius, 
				-direction.x * lineOptions.pipeInteriorRadius, 
				0);
			borderInfPath[i] = Vector3to2(position+translation);
		}
		for(int i = 99; i >= 0; i-- )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				direction.y * lineOptions.pipeRadius, 
				-direction.x * lineOptions.pipeRadius, 
				0);
			borderInfPath[199 - i] = Vector3to2 (position+translation);
		}
		borderCol.SetPath(1, borderInfPath);
	}
	
	private Vector2 Vector3to2 (Vector3 u) {
		return new Vector2(u.x, u.y);	
	}
}
