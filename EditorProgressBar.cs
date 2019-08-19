using System.Collections;
using UnityEditor;

namespace FuxiEditor.Coroutine
{
    public static class EditorProgressBar
    {
        public static void DisplayProgressBar(CustomAsyncOperation asyncOperation, string title = "Loading...")
        {
            EditorCoroutine.StartCoroutine(UpdateRoutine(asyncOperation, title));
        }

        static IEnumerator UpdateRoutine(CustomAsyncOperation asyncOperation, string title)
        {
            while (!asyncOperation.isDone)
            {
                EditorUtility.DisplayProgressBar(title, asyncOperation.message, asyncOperation.progress);
                yield return null;
            }
            EditorUtility.ClearProgressBar();
        }
    }
}