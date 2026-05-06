using UnityEditor;
using UnityEngine;

namespace SnowmanCount.Editor
{
    [InitializeOnLoad]
    public static class ProjectTagSetup
    {
        static ProjectTagSetup()
        {
            EnsureTagsExist();
        }

        [MenuItem("Tools/Setup Project Tags")]
        public static void EnsureTagsExist()
        {
            string[] requiredTags = { "Follower", "Player", "Enemy", "Gate", "Obstacle", "Hole" };
            
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            foreach (string tag in requiredTags)
            {
                if (!HasTag(tagsProp, tag))
                {
                    tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                    tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
                    Debug.Log($"[TagSetup] Added missing tag: {tag}");
                }
            }

            tagManager.ApplyModifiedProperties();
        }

        private static bool HasTag(SerializedProperty tagsProp, string tag)
        {
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                if (tagsProp.GetArrayElementAtIndex(i).stringValue == tag) return true;
            }
            return false;
        }
    }
}
