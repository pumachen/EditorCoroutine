using System;
using UnityEngine;

namespace FuxiEditor.Coroutine
{
    public class CustomAsyncOperation : CustomYieldInstruction
	{
		protected float m_progress = 0.0f;
        protected string m_message = "Loading...";
		public float progress { get { return m_progress; } }
        public string message { get { return m_message; } }

		public bool isDone { get { return progress >= 1.0f; } }

		public override bool keepWaiting { get { return !isDone; } }

		protected event Action<CustomAsyncOperation> m_completed;
		public event Action<CustomAsyncOperation> completed
		{
			add
			{
				if (value == null)
					return;
				if (isDone)
				{
					value.Invoke(this);
				}
				m_completed += value;
			}

			remove
			{
				if (value == null)
					return;
				m_completed -= value;
			}
		}

		protected void SetProgress(float progress)
		{
			lock (this)
			{
				m_progress = progress;
				if (progress >= 1.0f && m_completed != null)
				{
					m_completed.Invoke(this);
				}
			}
		}

        protected void SetMessage(string message)
        {
            m_message = message;
        }

		public CustomAsyncOperation(out Action<float> setProgress)
		{
			setProgress = SetProgress;
		}

        public CustomAsyncOperation(out Action<float> setProgress, out Action<string> setMessage)
        {
            setProgress = SetProgress;
            setMessage  = SetMessage;
        }
	}

	public class CustomAsyncOperation<T> : CustomAsyncOperation
	{
		protected T m_value;
		public T Value { get { return m_value; } }

		protected void SetValue(T value)
		{
			lock (this)
			{
				m_value = value;
			}
		}

		public CustomAsyncOperation(out Action<float> setProgress, out Action<T> setValue) : base(out setProgress)
		{
			setValue = SetValue;
		}

        public CustomAsyncOperation(
            out Action<float> setProgress, 
            out Action<string> setMessage, 
            out Action<T> setValue) : base(out setProgress, out setMessage)
        {
            setValue = SetValue;
        }

		public CustomAsyncOperation(
            T value, 
            out Action<float> setProgress, 
            out Action<T> setValue) : base(out setProgress)
		{
			m_value = value;
			setValue = SetValue;
		}

        public CustomAsyncOperation(
            T value, 
            out Action<float> setProgress,
            out Action<string> setMessage, 
            out Action<T> setValue) : base(out setProgress, out setMessage)
        {
            m_value = value;
            setMessage = SetMessage;
            setValue = SetValue;
        }
	}
}