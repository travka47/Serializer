using System.Collections;
using System.Text.RegularExpressions;
namespace Serializer;

public class Serializer
{
  
  /// <summary>
  /// Method that returns serialized string
  /// </summary>
  /// <param name="obj"></param>
  /// <exception cref="InvalidOperationException"></exception>
  public string Serialize(object obj)
  {
    return GetSerializedData(obj).ToString() ?? throw new InvalidOperationException();
  }

  /// <summary>
  /// Method for encode object's properties or plain objects
  /// </summary>
  /// <param name="obj"></param>
  private object GetSerializedData(object obj)
  {
    var type = obj.GetType();
    if (obj is IDictionary dictionary) return SerializeDict(dictionary);
    if (type.IsArray || (obj is IEnumerable && type.IsGenericType)) return SerializeArray(obj as IEnumerable ?? Array.Empty<string>());
    if (type.IsClass && obj is not string && !type.IsGenericType) return SerializeClassStruct(obj);
    if (type.IsValueType && !type.IsPrimitive) return SerializeClassStruct(obj);  // structure
    return obj;
  }

  /// <summary>
  /// Method for serialize arrays/lists
  /// </summary>
  /// <param name="arr"></param>
  private string SerializeArray(IEnumerable arr)
  {
    List<string> formattedArr = new();
    foreach (var v in arr) formattedArr.Add($"{JsonValue(GetSerializedData(v))}");
    return $"[{string.Join(",", formattedArr)}]";
  }

  /// <summary>
  /// Method for serialize dictionaries
  /// </summary>
  /// <param name="dictionary"></param>
  private string SerializeDict(IDictionary dictionary)
  {
    List<string> formattedProps = new();
    foreach (DictionaryEntry v in dictionary) 
      formattedProps.Add($"{{\"{ToSnakeCase(v.Key)}\": {JsonValue(GetSerializedData(v.Value ?? "undefined"))}}}");
    return $"{{{string.Join(",", formattedProps)}}}";
  }

  /// <summary>
  /// Method for serialize classes/structures
  /// </summary>
  /// <param name="obj"></param>
  private string SerializeClassStruct(object obj)
  {
    var properties = obj.GetType().GetProperties();
    List<string> formattedProps = new();
    foreach (var property in properties)
      formattedProps.Add($"\"{ToSnakeCase(property.Name)}\":" +
                         $"{JsonValue(GetSerializedData(property.GetValue(obj) ?? "undefined"))}");
    return $"{{\"type\":\"{obj.GetType().FullName}\", {string.Join(", ", formattedProps)}}}";
  }
  
  /// <summary>
  /// Method for wrapping the string values
  /// </summary>
  /// <param name="obj"></param>
  private static object JsonValue(object obj) 
    => obj is string s && !s.StartsWith("{") ? $"\"{obj}\"" : obj;
  
  /// <summary>
  /// Method for converting to snake_case
  /// </summary>
  /// <param name="obj"></param>
  private static string ToSnakeCase(object obj) 
    => string.Join("_", Regex
      .Split(obj as string ?? string.Empty, @"(?<!^)(?=[A-Z])")
      .Select(v => v.ToLower()));
}