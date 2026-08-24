using System;
using Caliburn.Micro;

namespace PhxStudio.Utils.Results
{
	public abstract class ResultBase
		: IResult
	{
		public abstract void Execute(CoroutineExecutionContext context);

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		protected virtual void OnCompleted()
		{
			OnCompleted(new ResultCompletionEventArgs());
		}

		protected virtual void OnError(Exception exception)
		{
			OnCompleted(new ResultCompletionEventArgs
			{
				Error = exception
			});
		}

		protected virtual void OnCancelled()
		{
			OnCompleted(new ResultCompletionEventArgs
			{
				WasCancelled = true
			});
		}

		protected virtual void OnCompleted(ResultCompletionEventArgs e)
		{
			Caliburn.Micro.Execute.OnUIThread(() => Completed(this, e));
		}
	}
}
