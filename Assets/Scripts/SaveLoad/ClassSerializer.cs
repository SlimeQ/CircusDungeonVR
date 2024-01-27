using UnityEngine;

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Specialized; 

[System.Serializable]
public class ClassSerializer
{
    public static void Save(string path, string name, object obj)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path + "/" + name,
                                        FileMode.Create,
                                        FileAccess.Write,
                                        FileShare.None);
        formatter.Serialize(stream, obj);
        stream.Close();
    }
   
    /*
    public static void SaveBytesArray(string path, string name, byte[] data)
    {
        try
        {
            using (FileStream stream = new FileStream(path + "/" + name, FileMode.Create, FileAccess.Write))
            {
                stream.Write(data, 0, data.Length);
            }
        }
        catch (IOException ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public static object LoadBytesArray(string path, string name)
    {
        byte[] bytes = null;
        try
        {
            using (FileStream stream = new FileStream(path + "/" + name, FileMode.Open, FileAccess.Read))
            {
                stream.Read(bytes, 0, bytes.Length);
            }
        }
        catch (IOException ex)
        {
            Debug.Log(ex.ToString());
        }

        return bytes;
    }
    */

    public static object Load(string path, string name)
    {
        object obj;
        IFormatter formatter = new BinaryFormatter();
        try
        {
            Stream stream = new FileStream(path + "/" + name,
                                            FileMode.Open,
                                            FileAccess.Read,
                                            FileShare.Read);
            obj = formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }

    ////https://stackoverflow.com/a/4736185
    //public static object LoadX(string name)
    //{
    //    if (GameManager.SMReady == true)
    //    {
    //        object obj;
    //        IFormatter formatter = new BinaryFormatter();
    //        try
    //        {
    //            byte[] b = null;
    //            GameManager.TS.DownloadFileAsync(name, b, null);
    //            Stream str = b.ToStream();
    //            obj = formatter.Deserialize(str);
    //            str.Close();
    //            return obj;
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.Log(e.Message);
    //            return null;
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("SM not ready!");
    //        return null;
    //    }
    //}

    //// https://stackoverflow.com/a/1080445
    //public static void SaveX(string name, object obj)
    //{
    //    if (GameManager.SMReady == true)
    //    {
    //        IFormatter formatter = new BinaryFormatter();
    //        Stream stream = new System.IO.MemoryStream();   //Create a dummy stream
    //        formatter.Serialize(stream, obj);
    //        byte[] b = stream.ReadToEnd();
    //        Debug.Log("Converted stream to bytes");
    //        stream.Close();
    //        Debug.Log("Uploading the file");
    //        GameManager.TS.UploadFileAsync(name, b, null);  //Upload the file
    //        Debug.Log("Upload function called");
    //    }
    //    else
    //    {
    //        Debug.LogError("SM not ready!");
    //    }
    //}

    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static System.Object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        System.Object obj = (System.Object)binForm.Deserialize(memStream);

        return obj;
    }
}