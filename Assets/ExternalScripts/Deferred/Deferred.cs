using System;
using System.Linq;
using RSG;

namespace CSharpDeferred
{
	public class Deferred
	{
		public Promise<object> promise { get;}

		public Deferred()
		{
			promise = new Promise<object>();
		}

		public void resolve(object val = null)
		{
			promise.Resolve(val);
		}
		
		public void reject(object val)
		{
			promise.Reject(new SystemException(val.ToString()));
		}
		
		public static IPromise<object> Resolve(object data = null)
		{
			var promise = new Promise<object>();

			promise.Resolve(data);
			return promise;
		}

        public static IPromise<object> Sequence(params Func<IPromise<object>>[] fns)
        {
			var deferred = new Deferred();

            int count = 0;

            fns.Aggregate(
                    Resolve(),
                    (prevPromise, fn) =>
                    {
                        int itemSequence = count;
                        ++count;

                        return prevPromise
                                .Then(data =>
                                {
                                    var sliceLength = 1f / count;
                                    deferred.promise.ReportProgress(sliceLength * itemSequence);
                                    return fn();
                                })
                                .Progress(v =>
                                {
                                    var sliceLength = 1f / count;
                                    deferred.promise.ReportProgress(sliceLength * (v + itemSequence));
                                })
                            ;
                    }
                )
                .Then(data => deferred.resolve(null))
                .Catch(deferred.reject);

            return deferred.promise;
        }
		
		public static IPromise<object> Reject(object data)
		{
			var promise = new Promise<object>();

			promise.Reject(new SystemException(""));
			
			return promise;
		}
	}
}