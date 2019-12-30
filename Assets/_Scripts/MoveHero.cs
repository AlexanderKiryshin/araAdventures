using System;
using System.Collections;
using Assets.Scripts.Cells;
using Assets._Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class MoveHero:MoveHeroBase
    {
        public bool isBlockInput=false;
        public const float TIME_WALK = 0.5f;
        public const float EAT_TIME = 1f;
        private Animator animator;

        public IEnumerator SetHeroPositionWithEatAnimation(Position position, int layer)
        {
            HeroPosition = position;           
            var vector = startTilemap.GetCellCenterWorld(new Vector3Int(HeroPosition.x, HeroPosition.y, 0));
            var lastHeroPosition = gameObject.transform.position;
            animator.CrossFade("walk", 0.05f);
            //animator.Play("walk");
            isBlockInput = true;
            var newVector = Vector3.Lerp(lastHeroPosition,
                    new Vector3(vector.x, vector.y, vector.z + 0.23f), 0.6f);
            gameObject.transform.DOMove(newVector, 0.9f);
            /*for (float i = 0; i < TIME_WALK * 60; i++)
            {            
                var newVector = Vector3.Lerp(lastHeroPosition,
                    new Vector3(vector.x, vector.y, vector.z + 0.23f), i / (TIME_WALK * 100));
                gameObject.transform.position = newVector;
                yield return new WaitForSeconds(0.01f);
            }*/
            isBlockInput = false;
            animator.CrossFade("eat", 0.05f);
            yield return new WaitForSeconds(EAT_TIME);
            levelManager.TryRemoveFruit(position, layer);
            animator.CrossFade("walk", 0.1f);
           /* var newVector2 = Vector3.Lerp(lastHeroPosition,
                   new Vector3(vector.x, vector.y, vector.z + 0.23f), 1/TIME_WALK);*/
            gameObject.transform.DOMove(vector, 0.6f);
            /*for (float i = TIME_WALK * 60; i < TIME_WALK * 100; i++)
            {
                var newVector = Vector3.Lerp(lastHeroPosition,
                    new Vector3(vector.x, vector.y, vector.z + 0.23f), i / (TIME_WALK * 100));
                gameObject.transform.position = newVector;
                yield return new WaitForSeconds(0.01f);
            }*/
            animator.CrossFade("idle", 0.5f);

        }

        public IEnumerator SetHeroPositionCoroutine(Position position, bool isInstanceMove)
        {
            
            HeroPosition = position;
            var vector = startTilemap.GetCellCenterWorld(new Vector3Int(HeroPosition.x, HeroPosition.y, 0));
            if (isInstanceMove)
            {

                gameObject.transform.position = new Vector3(vector.x, vector.y, vector.z + 0.23f);
            }
            else
            {
            /*   var anim=animator.GetCurrentAnimatorStateInfo(0);

               var anim1 = animator.GetCurrentAnimatorClipInfo(0);*/

        var lastHeroPosition = gameObject.transform.position;
        //animation.CrossFade("walk");
                animator.CrossFade("walk",0.5f);
               // animator.Play("walk");
                isBlockInput = true;
                var newVector = Vector3.Lerp(lastHeroPosition,
                   new Vector3(vector.x, vector.y, vector.z + 0.23f), 1 / TIME_WALK);
                gameObject.transform.DOMove(newVector, 1.5f);
               /* for (float i = 0; i < TIME_WALK*100; i++)
                {
                    var newVector= Vector3.Lerp(lastHeroPosition,
                        new Vector3(vector.x, vector.y, vector.z + 0.23f), i / (TIME_WALK * 100));
                    gameObject.transform.position = newVector;
                    yield return new WaitForSeconds(0.01f);
                }*/
                isBlockInput = false;
                animator.CrossFade("idle", 0.5f);
               // animator.Play("idle");
                // gameObject.transform.DOMove(, TIME_WALK);

            }
            yield return new WaitForSeconds(TIME_WALK);
            levelManager.FindFruitAndRemove(HeroPosition, layer, out var fruit);
        }

        public void SetHeroPosition(Position position, bool isInstanceMove)
        {
            StartCoroutine(SetHeroPositionCoroutine(position, isInstanceMove));
        }


        public void SetHeroPositionWithEat(Position position,int layer)
        {
            StartCoroutine(SetHeroPositionWithEatAnimation(position,layer));
        }

        private LevelManager levelManager;
        [SerializeField]
        private Tilemap startTilemap;

      //  public IEnumerator MovingHero()
        public void Awake()
        {
            animator = GetComponent<Animator>();

          //  BaseHexType.OnDestroyHexEvent += CheckHex;
            levelManager = FindObjectOfType<LevelManager>();
        }

        public void CheckHex(Position position, int layer,Func<IEnumerator> method)
        {
            if (HeroPosition.x == position.x&& HeroPosition.y == position.y && layer == this.layer)
            {
                WinLoseManager.instance.OnLose();
            }
        }
        public void Update()
        {
            if (isBlockInput)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {

           // animator.Play((AnimationClip.);)
                Ray ray = FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition);
                var hits=Physics.RaycastAll(ray);
                IHexType hex=null;
                foreach (var hit in hits)
                {
                    if (hit.collider != null)
                    {
                        if (levelManager.FindHexOnGameObject(hit.transform.gameObject) != null)
                        {
                            hex = levelManager.FindHexOnGameObject(hit.transform.gameObject);
                        }
                    }
                }
               

                if (hex == null)
                {
                    return;
                }
                if (hex.Position.x == HeroPosition.x && hex.Position.y == HeroPosition.y)
                {
                    return;
                }

                var aroundCoordinates= PositionCalculator.GetAroundSidePositions(HeroPosition);
                bool coordinateIsFound = false;
                foreach (var coordinate in aroundCoordinates)
                {
                    if (hex.Position.x == coordinate.x&& hex.Position.y == coordinate.y)
                    {
                        coordinateIsFound = true;
                        break;
                    }                   
                }

                if (!coordinateIsFound)
                {
                    return;
                }
           
				if (Math.Abs(hex.Position.x - HeroPosition.x)>1 || Math.Abs(hex.Position.y - HeroPosition.y)> 1)
				{
					return;
				}
                           
                StartCoroutine(EnterHexCoroutine(hex));
                //  transform.position = startTilemap.CellToLocal(new Vector3Int(hex.Position.x, hex.Position.y, 0));
                //
                /*  bool isFoundHex = levelManager.TryGetHex(new Vector2Int(cursorPosition.x, cursorPosition.y), 0, out var hex);
                  if (isFoundHex)
                  {
                      levelManager.TryGetHex(HeroPosition, 0, out var leavedHex);
                      var lastPosition = HeroPosition;
                      HeroPosition = new Vector2Int(cursorPosition.x, cursorPosition.y);
                      leavedHex.OnLeaveHex();
                      //OnLeaveHexEvent(leavedHex);
                    
                      levelManager.TryGetHex(HeroPosition, 0, out var enteredHex);
                      enteredHex.OnEnterHex(lastPosition);
                    //  transform.position = startTilemap.CellToLocal(new Vector3Int(hex.Position.x, hex.Position.y, 0));
                      LevelManager.CheckWinCondition();
                  }*/

            }
        }

        public IEnumerator EnterHexCoroutine(IHexType hex)
        {
            var vector =
                levelManager.levelTilemap.CellToWorld(new Vector3Int(hex.Position.x, hex.Position.y, hex.Layer));
            // vector.z = vector.z;
            //  gameObject.transform.DOLookAt(vector ,0.5f);
            float angle=PositionCalculator.GetAngleAroundNearHexes(HeroPosition, hex.Position);
            gameObject.transform.DOLocalRotate(
                new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y ,
                    gameObject.transform.rotation.z+angle), 0.5f);
            yield return new WaitForSeconds(0.2f);
           // levelManager.FindFruitAndRemove(HeroPosition, layer, out var fruit);
            levelManager.TryGetHex(HeroPosition, 0, out var leavedHex);
            var lastPosition = HeroPosition;
            bool fruitIsFound=levelManager.TryFindFruit(hex.Position, layer, out var fruit);
            if (fruitIsFound)
            {
                SetHeroPositionWithEat(new Position(hex.Position.x, hex.Position.y),layer);
                yield return new WaitForSeconds(TIME_WALK);
            }
            else
            {
                SetHeroPosition(new Position(hex.Position.x, hex.Position.y), false);
                yield return new WaitForSeconds(TIME_WALK);
            }                     
            leavedHex.OnLeaveHex();

            levelManager.TryGetHex(HeroPosition, 0, out var enteredHex);
            enteredHex.OnEnterHex(lastPosition);
        }

        public IEnumerator WinMove()
        {
            while (isBlockInput)
            {
                yield return new WaitForSeconds(0.05f);
            }
            gameObject.transform.DOLocalRotate(new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, -180), 1f);
            yield return new WaitForSeconds(1f);
            animator.Play("win");
            yield return new WaitForSeconds(2f);
            LevelManager.WinEvent?.Invoke();
        }
    }
}
