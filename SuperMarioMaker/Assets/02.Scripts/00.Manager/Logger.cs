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
        // obj�� Ÿ�Կ� ���� ������ ����ϴ�.
        var type = obj.GetType();

        // �ش� Ÿ���� ��� �ʵ带 �����ɴϴ�.
        var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            // ���� Ÿ�Ը� �˻�
            if (!field.FieldType.IsValueType)
            {
                // �ʵ忡 SerializeField ��Ʈ����Ʈ�� �ִ��� Ȯ���մϴ�.
                if (System.Attribute.IsDefined(field, typeof(SerializeField)))
                {
                    // �ʵ��� ���� �����ɴϴ�.
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