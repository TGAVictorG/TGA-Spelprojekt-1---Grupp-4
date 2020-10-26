using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class CurveController : MonoBehaviour
{
    [SerializeField]
    private AudioClip myUpSound;
    [SerializeField]
    private AudioClip myDownSound;

    private AudioSource myAudioSource;

    public float mySpeed = 1;

    public float mySwimBackTime = 2f;

    public GameObject myCurveRoot;

    public bool myAutoStart = true;

    public bool myAnimation = true;

    internal bool nextCurve = false;

    protected float animationTime = float.MaxValue;

    protected CurveFly gizmo;

    protected CurveFly curveFly;

    private void OnDrawGizmos()
    {
        if(gizmo == null)
        {
            gizmo = new CurveFly(myCurveRoot.transform);
        }

        gizmo.RefreshTransforms(1f);

        if((gizmo.myPoints.Length - 1)%2 != 0)
        {
            return;
        }

        int accur = 50;
        Vector3 prevPos = gizmo.myPoints[0].position;
        for (int c = 0; c < accur; c++)
        {
            float currTime = c * gizmo.GetDuration() / accur;
            Vector3 currPos = gizmo.GetPositionAtTime(currTime);
            float mag = (currPos - prevPos).magnitude * 2;
            Gizmos.color = new Color(mag, 0, 0, 1);
            Gizmos.DrawLine(prevPos, currPos);
            Gizmos.DrawSphere(currPos, 0.01f);
            prevPos = currPos;
        }
    }

    public void PlayUpSound()
    {
        if (myAudioSource != null)
        {
            myAudioSource.PlayOneShot(myUpSound);
        }
    }

    public void PlayDownSound()
    {
        if (myAudioSource != null)
        {
            myAudioSource.PlayOneShot(myDownSound);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();

        curveFly = new CurveFly(myCurveRoot.transform);
        
        if(myAutoStart)
        {
            RefreshTransforms(mySpeed);
            FollowCurve();
            
            PlayUpSound();
            
        }
    }


    // Update is called once per frame
    void Update()
    {
        nextCurve = false;

        if(myAnimation && curveFly != null && animationTime < curveFly.GetDuration())
        {
            int curveIndexBefore;
            int curveIndexAfter;

            curveFly.GetCurveIndexAtTime(animationTime, out curveIndexBefore);
            animationTime += Time.deltaTime;
            curveFly.GetCurveIndexAtTime(animationTime, out curveIndexAfter);

            transform.position = curveFly.GetPositionAtTime(animationTime);
            transform.LookAt(curveFly.GetPositionAtTime(animationTime));

            if(curveIndexBefore != curveIndexAfter)
            {
                nextCurve = true;
            }



        }

        else if(myAnimation && curveFly != null && animationTime > curveFly.GetDuration())
        {
            animationTime = float.MaxValue;
            myAnimation = false;
        }
    }

    public void FollowCurve()
    {
        RefreshTransforms(mySpeed);
        animationTime = 0f;
        transform.position = curveFly.myPoints[0].position;
        myAnimation = true;
    }

    public Vector3 GetHighestPoint(int aCurveIndex)
    {
        return curveFly.GetHighestPoint(aCurveIndex);
    }

    public Transform[] GetPoints()
    {
        return curveFly.myPoints;
    }

    public Vector3 GetPositionAtTime(float aTime)
    {
        return curveFly.GetPositionAtTime(aTime);
    }

    public float GetDuration()
    {
        return curveFly.GetDuration();
    }

    public void StopFollow()
    {
        animationTime = float.MaxValue;
       // FollowCurve();
    }

    public void RefreshTransforms(float aSpeed)
    {
        curveFly.RefreshTransforms(aSpeed);
    }

    public static float DistanceToLine(Ray aRay, Vector3 aPoint)
    {
        return Vector3.Cross(aRay.direction, aPoint - aRay.origin).magnitude;
    }

    public static Vector3 ClosestPointInLine(Ray aRay, Vector3 aPoint)
    {
        return aRay.origin + aRay.direction * Vector3.Dot(aRay.direction, aPoint - aRay.origin);
    }

    public class CurveFly
    {
        public Transform[] myPoints;
        protected Curve3D[] myCurves;
        protected float[] myPartDuration;
        protected float myCompleteDuration;

        public CurveFly(Transform aCurveRoot)
        {
            List<Component> components = new List<Component>(aCurveRoot.GetComponentsInChildren(typeof(Transform)));
            List<Transform> transforms = components.ConvertAll(c => (Transform)c);

            transforms.Remove(aCurveRoot.transform);
            transforms.Sort(delegate (Transform a, Transform b)
            {
                return a.name.CompareTo(b.name);
            });

            myPoints = transforms.ToArray();

            if((myPoints.Length -1)% 2 != 0)
            {
                //throw new UnityException("Curveroot needs an odd number of points");
            }

            if(myCurves == null || myCurves.Length < (myPoints.Length -1) /2)
            {
                myCurves = new Curve3D[(myPoints.Length - 1) / 2];
                myPartDuration = new float[myCurves.Length];
            }
        }

        public Vector3 GetPositionAtTime(float time)
        {
            int myCurveIndex;
            float myTimeinCurve;
            GetCurveIndexAtTime(time, out myCurveIndex, out myTimeinCurve);

            var myPercent = myTimeinCurve / myPartDuration[myCurveIndex];
            return myCurves[myCurveIndex].GetPositionAtLength(myPercent * myCurves[myCurveIndex].myLenght);
        }

        public void GetCurveIndexAtTime(float aTime, out int aMyCurveIndex)
        {
            float myTimeInCurve;
            GetCurveIndexAtTime(aTime, out aMyCurveIndex, out myTimeInCurve);
        }

        public void GetCurveIndexAtTime(float aTime, out int aMyCurveIndex, out float aMyTimeinCurve)
        {
            aMyTimeinCurve = aTime;
            aMyCurveIndex = 0;

            while (aMyCurveIndex < myCurves.Length - 1 && myPartDuration[aMyCurveIndex] < aMyTimeinCurve)
            {
                aMyTimeinCurve -= myPartDuration[aMyCurveIndex];
                aMyCurveIndex++;
            }

        }

        public float GetDuration()
        {
            return myCompleteDuration;
        }

        public Vector3 GetHighestPoint(int aCurveIndex)
        {
            return myCurves[aCurveIndex].GetHighestPoint();
        }

        public void RefreshTransforms(float aSpeed)
        {
            if (aSpeed <= 0)
            {
                aSpeed = 1;
            }

            if(myPoints != null)
            {
                myCompleteDuration = 0;

                for (int i = 0; i < myCurves.Length; i++)
                {
                    if (myCurves[i] == null)
                    {
                        myCurves[i] = new Curve3D();
                    }

                    myCurves[i].Set(myPoints[i * 2].position, myPoints[i * 2 + 1].position, myPoints[i * 2 + 2].position);
                    myPartDuration[i] = myCurves[i].myLenght / aSpeed;
                    myCompleteDuration += myPartDuration[i];

                }
            }

        }
    }
  
    public class Curve3D
    {
        public float myLenght { get; private set; }

        public Vector3 myA;
        public Vector3 myB;
        public Vector3 myC;

        protected Curve2D myCurve2D;
        protected Vector3 myH;
        protected bool myToClose;


        public Curve3D()
        {

        }

        public Curve3D(Vector3 aA, Vector3 aB, Vector3 aC)
        {
            Set(aA, aB, aC);
        }

        public void Set(Vector3 aA, Vector3 aB, Vector3 aC)
        {
            myA = aA;
            myB = aB;
            myC = aC;

            RefreshCurve();
        }

        public Vector3 GetHighestPoint()
        {
            var d = (myC.y - myA.y) / myCurve2D.myLength;
            var e = myA.y - myC.y;

            var curveCompl = new Curve2D(myCurve2D.myA, myCurve2D.myB + d, myCurve2D.myC + e, myCurve2D.myLength);

            Vector3 E = new Vector3();
            E.y = curveCompl.myE.y;
            E.x = myA.x + (myC.x - myA.x) * (curveCompl.myE.x / curveCompl.myLength);
            E.z = myA.z + (myC.z - myA.z) * (curveCompl.myE.x / curveCompl.myLength);

            return E;
        }

        public Vector3 GetPositionAtLength(float aLenght)
        {
            var myPercent = aLenght / myLenght;

            var x = myPercent * (myC - myA).magnitude;

            if (myToClose)
            {
                x = myPercent * 2f;
            }

            Vector3 pos;

            pos = myA * (1f - myPercent) + myC * myPercent + myH.normalized * myCurve2D.F(x);

            if (myToClose)
            {
                pos.Set(myA.x, pos.y, myA.z);
            }

            return pos;
        }

        private void RefreshCurve()
        {
            if (Vector2.Distance(new Vector2(myA.x, myA.z), new Vector2(myB.x, myB.z)) < 0.1f &&
                    Vector2.Distance(new Vector2(myB.x, myB.z), new Vector2(myC.x, myC.z)) < 0.1f)
                myToClose = true;
            else
                myToClose = false;

            myLenght = Vector3.Distance(myA, myB) + Vector3.Distance(myB, myC);

            if (!myToClose)
            {
                RefreshCurveNormal();
            }
            else
            {
                RefreshCurveClose();
            }
        }


        private void RefreshCurveNormal()
        {
            Ray r1 = new Ray(myA, myC - myA);
            var v1 = ClosestPointInLine(r1, myB);

            Vector2 A2d, B2d, C2d;

            A2d.x = 0f;
            A2d.y = 0f;
            B2d.x = Vector3.Distance(myA, v1);
            B2d.y = Vector3.Distance(myB, v1);
            C2d.x = Vector3.Distance(myA, myC);
            C2d.y = 0f;

            myCurve2D = new Curve2D(A2d, B2d, C2d);

            myH = (myB - v1) / Vector3.Distance(v1, myB) * myCurve2D.myE.y;
        }

        private void RefreshCurveClose()
        {
            var fac01 = (myA.y <= myB.y) ? 1f : -1f;
            var fac02 = (myA.y <= myC.y) ? 1f : -1f;

            Vector2 A2d, B2d, C2d;

            A2d.x = 0f;
            A2d.y = 0f;

            B2d.x = 1f;
            B2d.y = Vector3.Distance((myA + myC) / 2f, myB) * fac01;

            C2d.x = 2f;
            C2d.y = Vector3.Distance(myA, myC) * fac02;

            myCurve2D = new Curve2D(A2d, B2d, C2d);
            myH = Vector3.up;
        }
    }

    public class Curve2D
    {
        public float myA { get; private set; }
        public float myB { get; private set; }
        public float myC { get; private set; }

        public Vector2 myE { get; private set; }
        public float myLength { get; private set; }


        public Curve2D(float aA, float aB, float aC, float aLenght)
        {
            myA = aA;
            myB = aB;
            myC = aC;

            SetMetaData();
            myLength = aLenght;
        }

        public Curve2D(Vector2 aA, Vector2 aB, Vector2 aC)
        {
            var divisor = ((aA.x - aB.x) * (aA.x - aC.x) * (aC.x - aB.x));
            if (divisor == 0f)
            {
                aA.x += 0.00001f;
                aB.x += 0.00002f;
                aC.x += 0.00003f;
                divisor = ((aA.x - aB.x) * (aA.x - aC.x) * (aC.x - aB.x));
            }
            myA = (aA.x * (aB.y - aC.y) + aB.x * (aC.y - aA.y) + aC.x * (aA.y - aB.y)) / divisor;
            myB = (aA.x * aA.x * (aB.y - aC.y) + aB.x * aB.x * (aC.y - aA.y) + aC.x * aC.x * (aA.y - aB.y)) / divisor;
            myC = (aA.x * aA.x * (aB.x * aC.y - aC.x * aB.y) + aA.x * (aC.x * aC.x * aB.y - aB.x * aB.x * aC.y) + aB.x * aC.x * aA.y * (aB.x - aC.x)) / divisor;

            myB = myB * -1f;//hack

            SetMetaData();
            myLength = Vector2.Distance(aA, aC);
        }

        public float F(float aX)
        {
            return myA * aX * aX + myB * aX + myC;
        }

        private void SetMetaData()
        {
            var x = -myB / (2 * myA);
            myE = new Vector2(x, F(x));
        }
    }




}
