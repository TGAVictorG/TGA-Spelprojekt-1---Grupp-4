using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	//-------------------------------------------------------------------------
	public enum FadeType
	{
		FadeIn,
		FadeOut
	}
	
    //-------------------------------------------------------------------------
    public class Fader : MonoBehaviour
    {
		#region Private Serializable Fields

		[Header("Fade Settings")]
		[SerializeField] private Image myFadeImage = null;
		[SerializeField] private FadeType myFadeType = FadeType.FadeIn;
		[SerializeField] private float myFadeDuration = 2.0f;
		
		#endregion
        
        
        #region Private Fields
        
        private float myTargetAlpha = 0.0f;
		
		#endregion
        

        #region Unity Methods
        
        //-------------------------------------------------
        private void Start()
        {
	        InitializeFade();
	        StartCoroutine(FadeRoutine());
        }
        
        #endregion


		#region Private Methods
		
		//-------------------------------------------------
		private void InitializeFade()
		{
			switch (myFadeType)
			{
				case FadeType.FadeIn:
					myTargetAlpha = 0.0f;
					break;
				case FadeType.FadeOut:
					myTargetAlpha = 1.0f;
					break;
			}
		}

		//-------------------------------------------------
		private IEnumerator FadeRoutine()
		{
			while (Math.Abs(myFadeImage.color.a - myTargetAlpha) > 0.01f)
			{
				Fade();
				yield return new WaitForSeconds(Time.deltaTime);
			}

			myFadeImage.gameObject.SetActive(false);
		}

		//-------------------------------------------------
		private void Fade()
		{
			Color color = myFadeImage.color;
			myFadeImage.color = new Color(
				r: color.r,
				g: color.g,
				b: color.b,
				a: Mathf.MoveTowards(
					current: color.a,
					target: myTargetAlpha,
					maxDelta: myFadeDuration * Time.deltaTime));
		}

		#endregion
    }
}