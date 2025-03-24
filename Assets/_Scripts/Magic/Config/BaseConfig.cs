using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

public class BaseConfig : ScriptableObject
{
    /// <summary>
    /// Searches for a field with the given type and name that contains the given field name.
    /// </summary>
    /// <typeparam name="T">The type of the field</typeparam>
    /// <param name="fieldName">The field name to search for</param>
    /// <returns>The value of the field if found, or default(T) if not.</returns>
    private protected T GetValue<T>(string fieldName)
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(T) && field.Name.Contains(fieldName))
            {
                return (T)field.GetValue(this);
            }
        }

        Debug.LogError($"No field of type {typeof(T)} found containing name: {fieldName}");
        return default;
    }

    public Texture2D GetIcon(string glyphName) => GetValue<Texture2D>(glyphName);
    public string GetDescription(string glyphName) => GetValue<string>(glyphName);

    public Texture2D[] GetAllIcons()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(Texture2D))
            {
                return (Texture2D[])field.GetValue(this);
            }
        }
        return null;
    }
}