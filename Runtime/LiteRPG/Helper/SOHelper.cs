using UnityEngine;

namespace LiteRPG.Helper
{
    public class SOHelper
    {
        public static T LoadAndInstantiateSOFromResources<T>(string path) where T : ScriptableObject
        {
            T scriptableObject = Resources.Load(path, typeof(T)) as T;
            if(scriptableObject == null)
            {
                Debug.LogError("Could not load scriptable object from path: " + path);
                return null;
            }

            T instance = Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name;
            return instance;
        }
    }
}