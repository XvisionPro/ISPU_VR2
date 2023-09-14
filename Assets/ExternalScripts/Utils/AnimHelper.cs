using CSharpDeferred;
using RSG;
using UnityEngine;
using UnityEngine.UI;

namespace ExternalScripts
{
	public class AnimHelper
	{
		
		/** Не работает **/
		public static IPromise<object> showAlphaArray(GameObject[] array, float time = 0.5f)
		{
			return Deferred.Resolve(null);
			
/*			var deferred = new Deferred();
			var promises = new IPromise<object>[] { };

			foreach (var go in array)
			{
				promises.Push(hide(go));
			}

			Promise<object>.All(promises)
			       .Then(data => deferred.resolve(null));

			return deferred.promise;

			IPromise<object> hide(GameObject go)
			{
				var _deferred = new Deferred();
				
				go.GetComponent<MaskableGraphic>().DOFade(1, time)
				  .OnComplete(()=> _deferred.resolve(null));
				
				return _deferred.promise;
			}*/
		}
		
		/** Не работает **/
		public static IPromise<object> hideAlphaArray(GameObject[] array, float time = 0.5f)
		{
			return Deferred.Resolve(null);
/*			var deferred = new Deferred();
			var promises = new IPromise<object>[] { };

			foreach (var go in array)
			{
				var canvasRenderers = go.GetComponentsInChildren<CanvasRenderer>();
				foreach (var canvasRenderer in canvasRenderers)
				{
					promises.Push(hide(canvasRenderer));
				}
				
			}

			Promise<object>.All(promises)
			               .Then(data => deferred.resolve(null));

			return deferred.promise;

			IPromise<object> hide(CanvasRenderer canvasRenderer)
			{
				var _deferred = new Deferred();

				var a = canvasRenderer.GetMaterial();
				canvasRenderer.GetMaterial().DOFade(0, time)
				  .OnComplete(()=> _deferred.resolve(null));
				
				return _deferred.promise;
			}*/
		}
	}
}