using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public static class ReflectionHelper
    {
        public static ScriptableObject GetMainToolbar()
        {
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.Toolbar");
            var get = type.GetField("get", BindingFlags.Public | BindingFlags.Static);
            return get.GetValue(null) as ScriptableObject;
        }
        
        public static VisualElement GetToolbarRoot()
        {
            var obj = GetMainToolbar();
            var rootField = obj.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            var root = rootField.GetValue(obj);
            var result = root as VisualElement;
            return result;
        }
    }
    public static class WindowLayoutHelper
    {
        public static void LoadLayout(string path) => EditorUtility.LoadWindowLayout(path);

        public static void SaveLayout(string path)
        {
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.WindowLayout");
            var method = type.GetMethod("SaveWindowLayout", BindingFlags.Public | BindingFlags.Static);
            method.Invoke(null, new object[] { path });
        }
    }
}