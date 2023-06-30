using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public static class Logger
{
    [Conditional("EnableLogger")]
    public static void Debug(object logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }

    [Conditional("EnableLogger")]
    public static void Debug(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    [Conditional("EnableLogger")]
    public static void Error(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [Conditional("EnableLogger")]
    public static void Error(object message, Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    [Conditional("EnableLogger")]
    public static void CheckNullObject(object obj)
    {
        // obj의 타입에 대한 정보를 얻습니다.
        var type = obj.GetType();

        // 해당 타입의 모든 필드를 가져옵니다.
        var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            // 참조 타입만 검사
            if (!field.FieldType.IsValueType)
            {
                // 필드에 SerializeField 어트리뷰트가 있는지 확인합니다.
                if (System.Attribute.IsDefined(field, typeof(SerializeField)))
                {
                    // 필드의 값을 가져옵니다.
                    var fieldValue = field.GetValue(obj);

                    if (fieldValue as UnityEngine.Object == null)
                    {
                        UnityEngine.Debug.LogError("Field name: " + field.Name + " is null!");
                    }
                }

            }
        }
    }
}