using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public int[] TestVariable;
    private void Start()
    {
        var result = JsonConvert.SerializeObject(TestVariable);//Convert to json
        Debug.Log(result);
    }

    //public int i;
    //public float f;
    //public bool b;
    //public string str;
    //public int[] iArray;
    //public List<int> iList = new List<int>();
    //public Dictionary<string, float> fDictionary = new Dictionary<string, float>();
    //public JsonTest() { }
    //public JsonTest(bool isSet)
    //{

    //    if (isSet)

    //    {
    //        i = 10;
    //        f = 99.9f;
    //        b = true;
    //        str = "JSON Test String";
    //        iArray = new int[] { 1, 1, 3, 5, 8, 13, 21, 34, 55 };

    //        for (int idx = 0; idx < 5; idx++)
    //        {
    //            iList.Add(2 * idx);
    //        }

    //        fDictionary.Add("PIE", Mathf.PI);
    //        fDictionary.Add("Epsilon", Mathf.Epsilon);
    //        fDictionary.Add("Sqrt(2)", Mathf.Sqrt(2));

    //    }
    //}
    //void Start()
    //{
    //    JsonTest jtc = new JsonTest(true);
    //    string jsonData = ObjectToJson(jtc);
    //    CreateJsonFile(Application.dataPath, "JTestClass", jsonData);
    //}

    //string ObjectToJson(object obj)
    //{
    //    return JsonConvert.SerializeObject(obj);
    //}

    //T JsonToOject<T>(string jsonData)
    //{
    //    return JsonConvert.DeserializeObject<T>(jsonData);
    //}

    //void CreateJsonFile(string createPath, string fileName, string jsonData)
    //{
    //    FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
    //    byte[] data = Encoding.UTF8.GetBytes(jsonData);
    //    fileStream.Write(data, 0, data.Length);
    //    fileStream.Close();
    //}
}
