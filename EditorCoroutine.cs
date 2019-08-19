using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace FuxiEditor.Coroutine
{
    public class EditorCoroutine
	{
		private static Queue<IEnumerator> coroutines;
		static EditorCoroutine()
		{
			EditorApplication.update += Update;
			coroutines = new Queue<IEnumerator>();
		}

		private static void Update()
		{
			int size = coroutines.Count;
			for (int i = 0; i < size; ++i)
			{
				IEnumerator coroutine = coroutines.Dequeue();
				if (!coroutine.MoveNext())
					continue;
				coroutines.Enqueue(coroutine);
			}
		}

		private IEnumerator coroutine;

		protected EditorCoroutine(IEnumerator coroutine)
		{
			this.coroutine = coroutine;
		}

		public static EditorCoroutine StartCoroutine(IEnumerator coroutine)
		{
			coroutines.Enqueue(coroutine);
			return new EditorCoroutine(coroutine);
		}

		public static void StopCoroutine(EditorCoroutine coroutine)
		{
			int size = coroutines.Count;
			for (int i = 0; i < size; ++i)
			{
				IEnumerator buffer = coroutines.Dequeue();
				if (buffer == coroutine.coroutine)
				{
					continue;
				}
				coroutines.Enqueue(buffer);
			}
		}
	}
}