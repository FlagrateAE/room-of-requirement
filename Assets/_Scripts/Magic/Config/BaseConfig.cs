using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

public abstract class BaseConfig : ScriptableObject
{
    /// <summary>
    /// Searches for a field with the given type and name that contains the given field name.
    /// </summary>
    /// <typeparam name="T">The type of the field</typeparam>
    /// <param name="fieldName">The field name to search for</param>
    /// <returns>The value of the field if found, or default(T) if not.</returns>
    protected T GetValue<T>(string fieldName)
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (typeof(T).IsAssignableFrom(field.FieldType) && field.Name.Contains(fieldName))
            {
                return (T)field.GetValue(this);
            }
        }

        return default;
    }

    public bool GlyphExists(string glyphName) => GetValue<string>($"{glyphName}") != null;

    public Sprite GetIcon(string glyphName) => GetValue<Sprite>(glyphName);
    public string GetDescription(string glyphName) => GetValue<string>(glyphName);

    public List<Sprite> GetAllIcons()
    {
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        List<Sprite> result = new();

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(Sprite))
            {
                result.Add((Sprite)field.GetValue(this));
            }
        }
        return result;
    }
}