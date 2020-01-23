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
        private bool isBlockInput=false;
        public const float TIME_WALK = 0.4f;
        public const float EAT_TIME = 0.4f;
        public const float MOVE_SPEED = 33;
        private Animator animator;
        public Position nextPosition;
        public Action EndMove;
        public Action EndEat;
        public IEnumerator SetHeroPositionWithEatAnimation(Position position, int layer)
        {
            isBlockInput = true;       
            HeroPosition = position;           
            var vector = startTilemap.GetCellCenterWorld(new Vector3Int(HeroPosition.x, HeroPosition.y, 0));
            var lastHeroPosition = gameObject.transform.position;
            animator.CrossFade("walk", 0.05f);
            //animator.Play("walk");
           
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
            
            animator.CrossFade("eat", 0.05f);
            yield return new WaitForSeconds(EAT_TIME);
            levelManager.TryEatFruit(position, layer);
            animator.CrossFade("walk", 0.1f);
           /* var newVector2 = Vector3.Lerp(lastHeroPosition,
                   new Vector3(vector.x, vector.y, vector.z + 0.23f), 1/TIME_WALK);*/
            gameObject.transform.DOMove(vector, 0.6f);
            yield return new WaitForSeconds(0.6f);
            isBlockInput = false;
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
            yield return null;
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
        public Tilemap startTilemap;

      //  public IEnumerator MovingHero()
        public void Awake()
        {
            animator = GetComponent<Animator>();

          //  BaseHexType.OnDestroyHexEvent += CheckHex;
            levelManager = FindObjectOfType<LevelManager>();
        }

        public void CheckHex(Position position, int layer,Func<IEnumerator> method)
        {
            if (nextPosition.x == position.x&& nextPosition.y == position.y && layer == this.layer)
            {
                WinLoseManager.instance.OnLose();
            }
        }

        public Action fallEndedAction;

        public void FallHero()
        {
            StartCoroutine(FallHeroCoroutine());
        }
        private IEnumerator FallHeroCoroutine()
        {
            gameObject.transform.DOMoveZ(gameObject.transform.position.z+2, 1f);
            yield return new WaitForSeconds(1f);
            fallEndedAction.Invoke();
        }


        public void Update()
        {
            if (isBlockInput)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                Debug.LogError("CLICK");
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

                isBlockInput = true;           
                StartCoroutine(LeaveHexCoroutine(hex));
            }
        }
        public void SetNextPosition()
        {            
            HeroPosition = nextPosition;
        }
        public void Move(Position backPosition, Position nextPosition, float percentMoveToPosition, bool isWalkAnimation = true)
        {
            StartCoroutine(MoveCoroutine(backPosition,nextPosition, percentMoveToPosition, isWalkAnimation));
        }

        public void Idle()
        {
            animator.CrossFade("idle", 0.5f);
        }
        public void EatWithMove(Position backPosition, Position nextPosition, float percentMoveBefore,float percentMoveAfter)
        {
            StartCoroutine(EatWithMoveCoroutine(backPosition,this.nextPosition, percentMoveBefore, percentMoveAfter));
        }
        private IEnumerator EatWithMoveCoroutine(Position beforePosition, Position nextPosition, float percentMoveBefore, float percentMoveAfter)
        {
            Debug.LogError(isBlockInput +"BLOCK");
            StartCoroutine(MoveM(beforePosition,nextPosition, percentMoveBefore));
            yield return new WaitForSeconds(TIME_WALK*percentMoveBefore);
            animator.CrossFade("eat", 0.05f);
            yield return new WaitForSeconds(EAT_TIME);
            levelManager.TryEatFruit(nextPosition, layer);
            StartCoroutine(MoveM(beforePosition,nextPosition, percentMoveAfter));
            yield return new WaitForSeconds(TIME_WALK * percentMoveAfter);
            EndMove?.Invoke();
            Debug.LogError(isBlockInput + "BLOCK FALSE");
        }

        public void LockInput()
        {
            isBlockInput = true;
        }

        public void UnlockInput()
        {
            isBlockInput = false;
        }
        private IEnumerator MoveCoroutine(Position backPosition, Position nextPosition, float percentMoveToPosition,bool isWalkAnimation=true)
        {
          //  Debug.LogError(isBlockInput + "BLOCK");
            // HeroPosition = position;
            var vector = startTilemap.GetCellCenterWorld(new Vector3Int(nextPosition.x, nextPosition.y, 0));
            var startVector = startTilemap.GetCellCenterWorld(new Vector3Int(HeroPosition.x, HeroPosition.y, 0));
            vector = Vector3.Lerp(startVector, vector, percentMoveToPosition);
            var lastHeroPosition = gameObject.transform.position;
            if (isWalkAnimation)
            {
                animator.CrossFade("walk", 0.5f);
            }
            

            float angle = PositionCalculator.GetAngleAroundNearHexes(backPosition, nextPosition);
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-Mathf.Sin(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK,
                Mathf.Cos(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK, 0));

            yield return new WaitForSeconds(TIME_WALK * percentMoveToPosition);
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Sin(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK,
                -Mathf.Cos(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK, 0));
            EndMove?.Invoke();
           // Debug.LogError(isBlockInput + "BLOCK FALSE");
            // animator.CrossFade("idle", 0.5f);
        }
        private IEnumerator MoveM(Position beforePosition, Position nextPosition, float percentMoveToPosition, bool isWalkAnimation = true)
        {
            var vector = startTilemap.GetCellCenterWorld(new Vector3Int(nextPosition.x, nextPosition.y, 0));
            var startVector = startTilemap.GetCellCenterWorld(new Vector3Int(HeroPosition.x, HeroPosition.y, 0));
            vector = Vector3.Lerp(startVector, vector, percentMoveToPosition);
            var lastHeroPosition = gameObject.transform.position;
            if (isWalkAnimation)
            {
                animator.CrossFade("walk", 0.5f);
            }
            
            float angle = PositionCalculator.GetAngleAroundNearHexes(beforePosition, nextPosition);
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-Mathf.Sin(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK,
                Mathf.Cos(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK, 0));

            yield return new WaitForSeconds(TIME_WALK * percentMoveToPosition);
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Sin(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK,
                -Mathf.Cos(angle * Mathf.PI / 180) * MOVE_SPEED / TIME_WALK, 0));
        }
        public IEnumerator LeaveHexCoroutine(IHexType hex)
        {
            var vector =
                levelManager.levelTilemap.CellToWorld(new Vector3Int(hex.Position.x, hex.Position.y, hex.Layer));

            float angle=PositionCalculator.GetAngleAroundNearHexes(HeroPosition, hex.Position);
            gameObject.transform.DOLocalRotate(
                new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y ,
                    gameObject.transform.rotation.z+angle), 0.5f);
            yield return new WaitForSeconds(0.2f);
            
            var lastPosition = HeroPosition;

            levelManager.TryGetHex(HeroPosition, 0, out var leavedHex);
            levelManager.TryGetHex(hex.Position, 0, out var enteredHex);
            leavedHex.OnLeaveHex(enteredHex.Position);
            leavedHex.LeaveHexEvent += OnLeaveHexEvent;            
            yield return null;
        }
        public void OnLeaveHexEvent(Position nextHex,Position currentHex)
        {
            levelManager.TryGetHex(nextHex, 0, out var enteredHex);
            enteredHex.OnEnterHex(currentHex);
        }
        public void SetIdleAnimation()
        {
            animator.CrossFade("idle", 0.5f);
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
