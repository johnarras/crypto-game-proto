using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
/// <summary>
/// This is a list of some reflection utility functions.
/// </summary>
public class EntityUtils
{

	/// <summary>
	/// Returns an id from an object regardless of whether it has an int Id or a string Id.
	/// </summary>
	/// <param name="obj">Object with Id</param>
	/// <param name="IdName">The name of the Id field (if not "Id")</param>
	/// <returns>The Id as a string if it exists.</returns>
	public static string GetObjId(Object obj, string IdName = "Id")
	{

		if (obj == null || string.IsNullOrEmpty(IdName))
		{
			return null;
		}

		Type type = obj.GetType();
		PropertyInfo prop = type.GetProperty(IdName);
		if (prop != null)
		{
			object propVal = prop.GetValue(obj, null);
			if (propVal != null)
			{
				return propVal.ToString();
			}
		}
		FieldInfo field = type.GetField(IdName);
		if (field != null)
		{
			object fieldVal = field.GetValue(obj);
			if (fieldVal != null)
			{
				return fieldVal.ToString();
			}
		}
		return null;

	}

	/// <summary>
	/// Clone properties from an object to another
	/// </summary>
	/// <param name="from">Start object</param>
	/// <param name="to">End object</param>
	/// <returns></returns>
	public static bool CloneProperties(object from, object to)
	{
		if (from == null || to == null)
		{
			return false;
		}

		Type fromType = from.GetType();
		Type toType = to.GetType();

		PropertyInfo[] props = fromType.GetProperties();

		for (int p = 0; p < props.Length; p++)
		{
			SetObjectValue(to, props[p].Name, GetObjectValue(from, props[p]));
		}
		return true;

	}

	public static int GetObjectInt(object obj, string name)
	{
		object valObj = GetObjectValue(obj, name);
		if (valObj == null)
		{
			return 0;
		}

		int val = 0;
		Int32.TryParse(valObj.ToString(), out val);
		return val;
	}

	public static float GetObjectFloat(object obj, string name)
	{
		object valObj = GetObjectValue(obj, name);
		if (valObj == null)
		{
			return 0.0f;
		}

		float val = 0;
		Single.TryParse(valObj.ToString(), out val);
		return val;
	}

	public static double GetObjectDouble(object obj, string name)
	{
		object valObj = GetObjectValue(obj, name);
		if (valObj == null)
		{
			return 0.0;
		}

		double val = 0;
		Double.TryParse(valObj.ToString(), out val);
		return val;
	}

	/// <summary>
	/// Get an object value from the name of a member
	/// </summary>
	/// <param name="obj">Object with member</param>
	/// <param name="name">Name of the member</param>
	/// <returns>member value </returns>
	public static Object GetObjectValue(object obj, string name)
	{
		MemberInfo mem = GetMemberInfo(obj, name);

		if (mem == null)
		{
			return null;
		}

		return GetObjectValue(obj, mem);

	}

	/// <summary>
	/// Get object value from a MemberInfo
	/// </summary>
	/// <param name="obj">Object with member</param>
	/// <param name="mem">MemberInfo object</param>
	/// <returns>the value of that MemberInfo on that object</returns>
	public static Object GetObjectValue(Object obj, MemberInfo mem)
	{
		if (obj == null || mem == null)
		{
			return null;
		}

		PropertyInfo prop = null;
		try
		{
			// Do this recheck in case the member isn't a part of the type of the 
			// object that was passed in.
			Type otype = obj.GetType();
			MemberInfo mem2 = GetMemberInfo(obj, mem.Name);
			if (mem2 == null)
			{
				return null;
			}

			prop = mem2 as PropertyInfo;
			if (prop != null)
			{
				return prop.GetValue(obj, new object[] { });
			}

			FieldInfo field = mem2 as FieldInfo;
			return field.GetValue(obj);
		}
		// This exception can happen when we show the edit stack and skip from an element to
		// a totally different location because of dropdown clicks.
		catch (Exception e)
		{
			Console.WriteLine("Exception: " + e.Message);
		}
		return null;
	}

	/// <summary>
	/// Get the MemberInfo associated with a given name
	/// </summary>
	/// <param name="obj">Object with member info</param>
	/// <param name="name">Name of the member info</param>
	/// <returns>The MemberInfo object if it exists</returns>
	public static MemberInfo GetMemberInfo(object obj, string name)
	{
		if (obj == null || string.IsNullOrEmpty(name))
		{
			return null;
		}

		Type type = obj.GetType();

		PropertyInfo prop = type.GetProperty(name);
		if (prop != null)
		{
			return prop;
		}

		FieldInfo field = type.GetField(name);
		if (field != null)
		{
			return field;
		}

		return null;

	}


	/// <summary>
	/// This returns all static strings in a class in a list.
	/// </summary>
	/// <param name="t"><The type with the strings we want to extract/param>
	/// <returns>A list of static strings from that class</returns>
	public static List<string> GetStaticStrings(Type t)
	{
		List<String> list = new List<String>();
		if (t == null)
		{
			return list;
		}

		FieldInfo[] fields = t.GetFields();

		foreach (FieldInfo field in fields)
		{

			if (!field.IsStatic ||
				field.FieldType.FullName != "System.String")
			{
				continue;
			}

			string strval = field.GetValue(null) as String;

			if (string.IsNullOrEmpty(strval))
			{
				continue;
			}

			list.Add(strval);

		}

		return list;
	}

	/// <summary>
	/// This returns the underlying type for a generic Array or List.
	/// </summary>
	/// <param name="obj">The List or Array we want to investigate</param>
	/// <returns>The type for the generic type parameter if it exists.</returns>
	public static Type GetUnderlyingType(Object obj)
	{
		if (obj == null)
		{
			return null;
		}

		Type type = obj as Type;

		if (type == null)
		{
			type = obj.GetType();
		}

		if (type.IsArray)
		{
			return type.GetElementType();
		}

		if (type.IsGenericType)
		{
			Type[] pars = type.GetGenericArguments();
			if (pars != null && pars.Length > 0)
			{
				return pars[0];
			}
		}
		return null;

	}

	/// <summary>
	/// Return the type of the thing the memberinfo represents.
	/// </summary>
	/// <param name="mem">The memberinfo we are investigating</param>
	/// <returns>The type of this member</returns>
	public static Type GetMemberType(MemberInfo mem)
	{
		PropertyInfo prop = mem as PropertyInfo;
		if (prop != null)
		{
			return prop.PropertyType;
		}

		FieldInfo field = mem as FieldInfo;
		if (field != null)
		{
			return field.FieldType;
		}

		return null;
	}


	/// <summary>
	/// Set an object value based on a member name.
	/// </summary>
	/// <param name="obj">The object that will be updated</param>
	/// <param name="name">The name of the member</param>
	/// <param name="val">The value we will set</param>
	public static void SetObjectValue(Object obj, string name, object val)
	{
		if (obj == null || string.IsNullOrEmpty(name))
		{
			return;
		}

		MemberInfo mem = GetMemberInfo(obj, name);
		if (mem == null)
		{
			return;
		}

		SetObjectValue(obj, mem, val);
	}

	public static bool IsValueType(string name)
	{
		if (String.IsNullOrEmpty(name))
		{
			return false;
		}
		for (int b = 0; b < BaseValueTypes.Length; b++)
		{
			if (BaseValueTypes[b] == name)
			{
				return true;
			}
		}
		return false;
	}

	public static string[] BaseValueTypes =
	{

		"Int32",
		"Single",
		"SByte",
		"Int16",
 		"Int64",
		"Byte",
		"UInt16",
		"UInt32",
		"UInt64",
		"Boolean",
		"Double",
		"DateTime",
	};


	/// <summary>
	/// Use this to get an object value from a string and a BaseSystemType
	/// </summary>
	/// <param name="typeName">Base System type to search</param>
	/// <param name="valStr">Value string</param>
	/// <returns></returns>
	public static object ValueFromString(string typeName, string valStr)
	{
		if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(valStr))
		{
			return null;
		}

		if (typeName == "System.Int32") { Int32 val = 0; Int32.TryParse(valStr, out val); return val; }
		if (typeName == "System.Single") { Single val = 0; Single.TryParse(valStr, out val); return val; }

		if (typeName == "System.Sbyte") { SByte val = 0; SByte.TryParse(valStr, out val); return val; }
		if (typeName == "System.Int16") { Int16 val = 0; Int16.TryParse(valStr, out val); return val; }
		if (typeName == "System.Int64") { Int64 val = 0; Int64.TryParse(valStr, out val); return val; }


		if (typeName == "System.Byte") { Byte val = 0; Byte.TryParse(valStr, out val); return val; }
		if (typeName == "System.UInt16") { UInt16 val = 0; UInt16.TryParse(valStr, out val); return val; }
		if (typeName == "System.UInt32") { UInt32 val = 0; UInt32.TryParse(valStr, out val); return val; }
		if (typeName == "System.UInt64") { UInt64 val = 0; UInt64.TryParse(valStr, out val); return val; }


		if (typeName == "System.Double") { Double val = 0; Double.TryParse(valStr, out val); return val; }
		if (typeName == "System.Boolean") { Boolean val = false; Boolean.TryParse(valStr, out val); return val; }


		if (typeName == "System.DateTime")
		{
			DateTime val = DateTime.UtcNow;
			DateTime.TryParse(valStr, out val);
			return val;
		}
		return null;
	}

	/// <summary>
	/// Set a member value on an object. There can be exceptions thrown here and caught here
	/// but this works well if the memberinfo 
	/// </summary>
	/// <param name="obj">The object to update</param>
	/// <param name="mem">The member to update</param>
	/// <param name="val">The value to set on the object's member There is aome extra work here with System. value types because the val may be a string and we have to convert it to the underlying type.</param>
	public static void SetObjectValue(Object obj, MemberInfo mem, Object val)
	{
		if (obj == null || mem == null)
		{
			return;
		}

		MemberInfo mem2 = GetMemberInfo(obj, mem.Name);
		if (mem2 == null)
		{
			return;
		}

		Type memtype = GetMemberType(mem2);

		Type valtype = null;
		string valtypeName = "";
		if (val != null)
		{
			valtype = val.GetType();
			valtypeName = valtype.Name;
		}


		object newval = null;
		if (val != null)
		{
			newval = ValueFromString(memtype.FullName, val.ToString());
			if (newval != null)
			{
				val = newval;
			}
		}

		if (newval == null && memtype.IsEnum)
		{
			bool setEnum = false;
			Array values = Enum.GetValues(memtype);
			for (int i = 0; i < values.Length; i++)
			{
				object val1 = values.GetValue(i);
				if (val1.ToString() == val.ToString())
				{
					setEnum = true;
					val = val1;
					break;
				}
			}
			if (!setEnum)
			{
				return;
			}
		}

		PropertyInfo prop = mem2 as PropertyInfo;
		if (prop != null)
		{
			prop.SetValue(obj, val, null);
		}
		if (mem2 != null && mem2.GetType().Name == "DBNull")
		{
			return;
		}
		FieldInfo field = mem2 as FieldInfo;
		if (field != null)
		{
			field.SetValue(obj, val);
		}
	}

	public static object DefaultConstructor(Type type)
	{
		if (type == null)
		{
			return null;
		}

		ConstructorInfo cinfo = type.GetConstructor(Type.EmptyTypes);
		if (cinfo == null)
		{
			return null;
		}
		return cinfo.Invoke(new object[0]);

	}

}
