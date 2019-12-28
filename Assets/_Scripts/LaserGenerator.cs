using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using Assets.Scripts;

public class LaserGenerator :Singleton<LaserGenerator>
{
    public event Action<Vector2Int> LaserInteractionEvent;
	private LevelManager levelManager;
	public GameObject laser90;
    public GameObject laser75;
    public GameObject laser150;
    public void Start()
	{
		levelManager = FindObjectOfType<LevelManager>();
	}
	public void DrawLaser(Position startPosition, Position endPosition, int rangeInAir, int range)
	{
		StartCoroutine(LaserDrawer(startPosition, endPosition,rangeInAir,range));
	}

	public IEnumerator LaserDrawer(Position startPosition, Position endPosition,int rangeInAir,int range )
	{
		Vector3 start = levelManager.levelTilemap.CellToWorld(new Vector3Int(startPosition.x, startPosition.y, 0));
		Vector3 end = levelManager.levelTilemap.CellToWorld(new Vector3Int(endPosition.x, endPosition.y, 0));
		float distance=Vector2.Distance(start, end);
            //laser.transform.localScale = new Vector3(10f,0);
	
        float c = Vector3.Distance(start, end);
        float a =Mathf.Abs(end.y-start.y);
        if ((start.x < end.x&&start.y<end.y)|| (start.x > end.x && start.y > end.y))
        {
            a = -a;
        }
        float angle =90-Mathf.Asin(a / c)*180/Mathf.PI;
        //  float scale = 4.9f;
        GameObject go=null;
        if (angle > -5 && angle <5)
        {
            go = Instantiate(laser90);
                if (startPosition.x -1== endPosition.x && startPosition.y == endPosition.y)
                {
                    var particle = go.GetComponent<ParticleSystem>();
                    var shape = particle.shape;
                    shape.rotation = new Vector3(particle.shape.rotation.x, particle.shape.rotation.y + 180, particle.shape.rotation.z);
                }

        }
        if (angle > 115&& angle < 125)
        {
            go = Instantiate(laser75);
            if (startPosition.y % 2 == 0)
            {
                if (startPosition.x - 1 == endPosition.x && startPosition.y -1 == endPosition.y)
                {
                    var particle = go.GetComponent<ParticleSystem>();
                    var shape = particle.shape;
                    shape.rotation = new Vector3(particle.shape.rotation.x, particle.shape.rotation.y + 180, particle.shape.rotation.z);
                }
            }
            else
            {
                if (startPosition.x == endPosition.x && startPosition.y - 1 == endPosition.y)
                {
                    var particle = go.GetComponent<ParticleSystem>();
                    var shape = particle.shape;
                    shape.rotation = new Vector3(particle.shape.rotation.x, particle.shape.rotation.y + 180, particle.shape.rotation.z);
                }
            }
        }
        if (angle > 55 && angle < 65)
        {
            go = Instantiate(laser150);
            if (startPosition.y % 2 == 0)
            {
                if (startPosition.x - 1 == endPosition.x && startPosition.y + 1 == endPosition.y)
                {
                    var particle = go.GetComponent<ParticleSystem>();
                   var shape= particle.shape;
                       shape.rotation=new Vector3(particle.shape.rotation.x, particle.shape.rotation.y+180, particle.shape.rotation.z);
                }               
            }
            else
            {
                if (startPosition.x == endPosition.x && startPosition.y + 1 == endPosition.y)
                {
                    var particle = go.GetComponent<ParticleSystem>();
                    var shape = particle.shape;
                    shape.rotation = new Vector3(particle.shape.rotation.x, particle.shape.rotation.y + 180, particle.shape.rotation.z);
                }
            }
        }
        go.transform.position = new Vector3(start.x, start.y, 1);
        ParticleSystem ps = go.transform.GetComponent<ParticleSystem>();
            ps.Play(true);

            //go.transform.eulerAngles=new Vector3(go.transform.eulerAngles.x, go.transform.eulerAngles.y, angle);
        /*  float xDistance = Mathf.Abs(Mathf.Sin(angle * Mathf.PI / 180) * distance)/4f;
         float yDistance =Mathf.Abs( Mathf.Cos(angle * Mathf.PI / 180) * distance) / 4f;
         if (start.x > end.x)
         {
             xDistance = -xDistance;
         }
         if (start.y > end.y)
         {
             yDistance = -yDistance;
         }
         for (int i=0;i<distance*30;i++)
         {
             go.transform.localPosition=new Vector3(go.transform.localPosition.x+xDistance/30, go.transform.localPosition.y + yDistance / 30, go.transform.localPosition.z);
             go.transform.localScale =new Vector3(laser.transform.localScale.x,
                 go.transform.localScale.y + (float)i/(scale), laser.transform.localScale.z);			
             yield return new WaitForSeconds(0.03f);
         }*/
        yield return new WaitForSeconds(0.25f);
         if (levelManager.TryGetHex(endPosition,0,out var hex))
         {
             rangeInAir = 0;
             if (range < 30)
             {
                 hex.OnLaserHit(startPosition,0,range+1);
             }

             //LaserInteractionEvent?.Invoke(endPosition);
         }
         else
         {
             if (rangeInAir < 10)
             {
                 Position newposition = PositionCalculator.GetOppositeSidePosition(startPosition, endPosition);
                 StartCoroutine(LaserDrawer(endPosition, newposition, rangeInAir + 1,range+1));
             }
         }
        yield return new WaitForSeconds(1f);
        Destroy(go);
       /* for (int i = 0; i < distance * 30; i++)
        {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x + xDistance / 30, go.transform.localPosition.y + yDistance / 30, go.transform.localPosition.z);
            go.transform.localScale = new Vector3(laser.transform.localScale.x,
                go.transform.localScale.y - (float)i / (scale), laser.transform.localScale.z);
            yield return new WaitForSeconds(0.03f);
        }
        Destroy(go);*/
        yield return new WaitForSeconds(1f);
	}
}