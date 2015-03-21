using UnityEngine;
using System.Collections;
using System;

public class SplinePipe : MonoBehaviour {
	private LineRenderer lineRend;
	private LineRenderer lineIntRend;
	public Transform toe;
	private Transform beginToe;
	public bool isBlowable = true;
	public float powerBlow = 2;
	
	[System.Serializable]
	public class LineOptions
	{
		public float pipeRadius = 0.6f;
		public float pipeInteriorRadius = 0.5f;
		public Material borderMaterial;
		public Material interiorMaterial;
	}
	
	//Line options
	public LineOptions lineOptions = new LineOptions();
	public Blower blower = new Blower();
	
	public float blowIntensity = 0f;
	
	public BezierSpline spline;
	
	private GameObject ball;
	private bool ballIsIn = false;
	
	private void Start () {
		//Pour les tests 
		//blower.intensity = 1;
		//blower.isAtBeginning = true;
		
		//Général
		float lineRadius = (lineOptions.pipeInteriorRadius + lineOptions.pipeRadius )/2;
		GameObject borders = new GameObject();
		borders.transform.parent = this.transform;
		
		PolygonCollider2D borderCol = borders.AddComponent<PolygonCollider2D>();
		borderCol.pathCount = 2;
		float borderWidth = lineOptions.pipeRadius - lineOptions.pipeInteriorRadius;
		
		ball = GameObject.FindGameObjectWithTag ("Ball");
		
		//Embout :
		beginToe = Instantiate(toe) as Transform;
		Vector3 beginPosition = spline.GetPoint(0);
		beginPosition.z = 1.1f;
		beginToe.transform.position = beginPosition;
		beginToe.transform.LookAt(beginPosition + spline.GetDirection(0) );
		beginToe.transform.Rotate(new Vector3(0,90,0));
		beginToe.gameObject.GetComponent<Embout>().isBeginning = true;
		
		Vector3 endPosition = spline.GetPoint(1);
		endPosition.z = 1.1f;
		toe.transform.position = endPosition;
		toe.transform.LookAt(endPosition + spline.GetDirection(1) );
		toe.transform.Rotate(new Vector3(0,90,180));
		toe.gameObject.GetComponent<Embout>().isBeginning = false;
		
	//Partie intérieure
		//Affichage intérieur
		GameObject pipeInt = new GameObject();
		pipeInt.transform.parent = this.transform;
		
		lineRend = pipeInt.AddComponent<LineRenderer>();
		lineRend.useWorldSpace = true;
		
		
		lineRend.SetWidth(lineOptions.pipeInteriorRadius*2, lineOptions.pipeInteriorRadius*2);
		lineRend.material = lineOptions.interiorMaterial;
		
		lineRend.SetVertexCount(1000);
		
		for(int i = 0; i < 1000; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/1000);
			lineRend.SetPosition(i, position);
		}
		
	//Partie intérieure collider
		Vector2[] interiorPath = new Vector2[200];
		for(int i = 0; i < 100; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				-direction.y * lineOptions.pipeInteriorRadius, 
				direction.x * lineOptions.pipeInteriorRadius, 
				0);
			interiorPath[i] = Vector3to2(position + translation - this.transform.parent.transform.position);
		}
		for(int i = 99; i >= 0; i-- )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				direction.y * lineOptions.pipeInteriorRadius, 
				-direction.x * lineOptions.pipeInteriorRadius, 
				0);
			
			interiorPath[199 - i] = Vector3to2 (position + translation - this.transform.parent.transform.position );
		}
		PolygonCollider2D interiorCol = this.gameObject.AddComponent<PolygonCollider2D>();
		interiorCol.pathCount = 1;
		interiorCol.SetPath(0, interiorPath);
		interiorCol.isTrigger = true;
		
	//Partie supérieure
		//Partie supérieure affichage
		GameObject borderSup = new GameObject();
		borderSup.transform.parent = this.transform;
		
		LineRenderer lineSupRend = borderSup.AddComponent<LineRenderer>();
		lineSupRend.useWorldSpace = true;
		
		lineSupRend.SetWidth(borderWidth, borderWidth);
		lineSupRend.material = lineOptions.borderMaterial;
		
		lineSupRend.SetVertexCount(1000);
		
		for(int i = 0; i < 1000; i++ )
		{
			Vector3 position = spline.GetPoint((float)i/1000);
			Vector3 direction = spline.GetDirection((float)i/1000);
			Vector3 translation = new Vector3 (-direction.y * lineRadius, direction.x * lineRadius, 0);
			lineSupRend.SetPosition(i, position+translation);
		}
		
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
			borderSupPath[i] = Vector3to2(position + translation );
		}
		for(int i = 99; i >= 0; i-- )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				-direction.y * lineOptions.pipeRadius, 
				direction.x * lineOptions.pipeRadius, 
				0);
			
			borderSupPath[199 - i] = Vector3to2 (position + translation );
		}
		borderCol.SetPath(0, borderSupPath);
		
		//Partie inférieure
		GameObject borderInf = new GameObject();
		borderInf.transform.parent = this.transform;
		
		LineRenderer lineInfRend = borderInf.AddComponent<LineRenderer>();
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
			borderInfPath[i] = Vector3to2(position+translation );
		}
		for(int i = 99; i >= 0; i-- )
		{
			Vector3 position = spline.GetPoint((float)i/100);
			Vector3 direction = spline.GetDirection((float)i/100);
			Vector3 translation = new Vector3 (
				direction.y * lineOptions.pipeRadius, 
				-direction.x * lineOptions.pipeRadius, 
				0);
			borderInfPath[199 - i] = Vector3to2 (position+translation );
		}
		borderCol.SetPath(1, borderInfPath);
	}
	
	private Vector2 Vector3to2 (Vector3 u) {
		return new Vector2(u.x, u.y);	
	}
	
	private void Update() {
		//Checking player is blowing in the player
		if(blower == null)
			return;
		if(blower.intensity == 0) 
			return;
		
		if(blower.isAtBeginning)
			blowIntensity = blower.intensity;
		else
			blowIntensity = - (blower.intensity);
			
		//Afficher des particules d'air
		if(blowIntensity != 0) 
		{
			drawParticules();
		}
		
		Debug.Log ("blow intensity "+blowIntensity);
		
		//Déplacer la balle
		if(ballIsIn && blowIntensity != 0) {
			//Récupérer la progression de la balle dans le tuyau
			float progression = GetProgression(ball.transform.position);
			Vector2 direction = spline.GetDirection(progression);
			/*if(blower.isAtBeginning) {
				direction.x = -direction.x; 
				direction.y = -direction.y;
			}*/
			int coef = 50;
			if(blower.isAtBeginning && toe.GetComponent<Embout>().GetBallIsIn()) {
				coef = 150;
			} else if (!blower.isAtBeginning && beginToe.GetComponent<Embout>().GetBallIsIn()) {
				coef = 150;
			}
			Vector2 force = direction * blowIntensity * coef* powerBlow* Time.deltaTime; //150 pour aspirer
			ball.rigidbody2D.AddForce( force );
			Debug.Log("Apply blow itnensity "+blowIntensity + " force "+force.x+" y "+force.y);
			
		}
	}
	
	private void OnTriggerEnter2D (Collider2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		else 
			ballIsIn = true;
		
		Debug.Log("ballIsIn "+ballIsIn);
	}
	
	private void OnTriggerStay2D (Collider2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		else 
			ballIsIn = true;
			
		Debug.Log("ballIsIn "+ballIsIn);
	}
	
	private void OnTriggerExit2D (Collider2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		else 
			ballIsIn = false;
			
		Debug.Log("Leave ballIsIn "+ballIsIn);
	}
	
	[System.Serializable]
	public class Blower
	{
		public float intensity; //-1 to 1
		public bool isAtBeginning = false;
	}
	
	public float GetProgression (Vector2 pos) {
		float minDistance = 99;
		float progression = 0;
		
		for(float i = 0.01f; i < 1f; i = i+0.01f) 
		{
			float distance = Vector2.Distance(pos, spline.GetPoint(i));
			if(distance < minDistance )
			{
				minDistance = distance;
				progression = i;
			}			
		}
		return progression;
	}
	
	public void setBallIsIn (bool value)
	{
		ballIsIn = value;
	}
	
	private void drawParticules () {
		int nbPart = 5;
		
		for(int i = 0; i < nbPart; i ++) {
			float progress = UnityEngine.Random.Range(0f, 1f);
			//Générer part
		}
		
	}
	
}
