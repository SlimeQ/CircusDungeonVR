using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Allows the creation of a custom Unity Inspector element (1D array of something with defined names for each element)
/// See: https://forum.unity.com/threads/how-to-change-the-name-of-list-elements-in-the-inspector.448910/#post-3730240
/// </summary>
public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;         //Name for each array element 

    /// <summary>
    /// Define a new NamedArrayAttribute by literal string[] array
    /// </summary>
    /// <param name="names">String array</param>
    public NamedArrayAttribute(string[] names)
    {
        this.names = names;
    }

    /// <summary>
    /// Define a new NamedArrayAttribute by Enum type (creates string array based on elements' names)
    /// </summary>
    /// <param name="_enum">typeof(Enum)</param>
    public NamedArrayAttribute(Type _enum)
    {
        this.names = System.Enum.GetNames(_enum);
    }

    /// <summary>
    /// Defines a new NamedArrayAttribute by a const string[] variable held in a particular namespace/class.
    /// Both params should be stringNames of class/variable
    /// </summary>
    /// <param name="byClassName">strName of class</param>
    /// <param name="byVarName">strName of string[] var</param>
    public NamedArrayAttribute(string byClassName, string byVarName)
    {
        string[] Names = new string[2] { "Err", "or" }; //Array of names to apply

        try
        {
            //Get the type of class byName
            Type T = Type.GetType(byClassName); 
            if (T != null)
            {                
                if (T.IsClass)
                {
                    //If type was found and as class type

                    //Get a field from class (variable byName)
                    FieldInfo FI = T.GetField(byVarName);
                    if (FI != null)
                    {
                        //If field found

                        //Get the value of the field as object
                        System.Object obj = FI.GetValue(T);
                        if (obj != null)
                        {
                            //If object was found, convert object to string[] array, and set to Names
                            Names = (string[])(obj);
                        }
                    }
                }
            }
        }
        catch { }

        //Set the names
        this.names = Names;
    }
}