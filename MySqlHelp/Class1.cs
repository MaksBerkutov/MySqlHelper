using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.IO;

namespace CreateClass
{
    public static class Creators
    {
        public static object CreateNewObject(List<(string FieldName,Type FieldType)> inputDate,string name)
        {
            var myType = CompileResultType(inputDate,name);
            return Activator.CreateInstance(myType);
        }
        public static Type CompileResultType(List<(string FieldName, Type FieldType)> Fields,string name)
        {
            TypeBuilder tb = GetTypeBuilder(name);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            foreach (var field in Fields)
                CreateProperty(tb, field.FieldName, field.FieldType);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string name)
        {
            var typeSignature = name;
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    (typeof(MySqlHelp.MySqlObject)));
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
namespace MySqlHelp
{
   
    public class Converter
    {
        public static string ToSQLmessageType(object Type)
        {
            if (Type.GetType().Name.ToLower() == "string") return $"'{Type.ToString()}'";
            else return Type.ToString();
        }

    }
    public class Connector
    {
        private MySqlConnector.MySqlConnection conn;
        public Connector(string host, int port, string database, string username, string password)
        {
            MySqlConnector.MySqlConnectionStringBuilder ConString = new MySqlConnector.MySqlConnectionStringBuilder
            {
                Server = host,
                Database = database,
                UserID = username,
                Password = password,
                ConnectionTimeout = 60,
                Port = 3306,
                AllowZeroDateTime = true
            };
            conn = new MySqlConnector.MySqlConnection(ConString.ConnectionString);
        }
        public MySqlConnector.MySqlConnection GetConnection() => conn;
        public MySqlConnector.MySqlConnection Connection => conn;
    }
    public class MySqlDate
    {
        private string nameVariable;
        public string NameVariable => nameVariable;
        private string typeVariable;
        public string TypeeVariable => typeVariable;
        private object dateVariable;
        public object DateVariable => dateVariable;
        public MySqlDate(string name, string type, object date)
        {
            nameVariable = name; typeVariable = type; dateVariable = date;
        }

    }
    public class MySqlObject : IMySqlObject
    {

        public T Init<T>(List<MySqlDate> Date) where T : IMySqlObject, new()
        {

            T myTable = new T();
            PropertyInfo[] myPropertyInfo = myTable.GetType().GetProperties();
            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                foreach (var item in Date)
                {
                    if (item.NameVariable == myPropertyInfo[i].Name)
                    {
                        PropertyInfo Variable_Selected = myTable.GetType().GetProperty(item.NameVariable);
                        if (item.TypeeVariable == "VARCHAR")
                        {
                            Variable_Selected.SetValue(myTable, item.DateVariable.ToString());
                            continue;
                        }
                        if (Variable_Selected.PropertyType == item.DateVariable.GetType())
                            Variable_Selected.SetValue(myTable, item.DateVariable);
                    }

                }
            }
            return myTable;
        }
    }
    public interface IMySqlObject
    {

        T Init<T>(List<MySqlDate> Date) where T : IMySqlObject, new();
    }
    public class MySqlUploadDate
    {
        string Variable; string Valuel; bool IsString;

        public MySqlUploadDate(string variable, string valuel, bool isString)
        {
            Variable = variable;
            Valuel = valuel;
            IsString = isString;
        }
        public MySqlUploadDate()
        {

        }
        public override string ToString()
        {
            if (IsString)
                return $"{Variable} = '{Valuel}',";
            else
                return $"{Variable} = {Valuel},";
        }
        public static string Convert(string Variable, string Valuel, bool IsString)
        {
            if (IsString)
                return $"{Variable} = '{Valuel}'";
            else
                return $"{Variable} = {Valuel}";
        }
    }
    public class MySqlWork
    {
        private Connector conn;
        public MySqlWork(Connector conector) => conn = conector;
        public List<T> LoadDate<T>(string sql, bool thisCMD = false) where T : IMySqlObject, new()
        {

            List<T> result = new List<T>();
            T Obj_Creaters = new T();
            MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
            conn.Connection.Open();
            cmd.Connection = conn.Connection;
            if (thisCMD)
                cmd.CommandText = sql;
            else
                cmd.CommandText = $"SELECT * FROM {sql}";
            using (MySqlConnector.MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<MySqlDate> dates = new List<MySqlDate>();
                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            (string type, object date, string name) tmp;
                            tmp.name = reader.GetName(i);
                            tmp.type = reader.GetDataTypeName(i);

                            tmp.date = reader.GetValue(i);

                            dates.Add(new MySqlDate(tmp.name, tmp.type, tmp.date));
                        }
                        result.Add(Obj_Creaters.Init<T>(dates));


                    }
                }
            }

            conn.Connection.Close();
            return result;
        }
        public bool UploadDate<T>(List<T> Date, string nameIdVariable, string nameTable, out string error) where T : IMySqlObject, new()
        {
            MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
            try
            {
                foreach (T t in Date)
                {
                    PropertyInfo[] myPropertyInfo = t.GetType().GetProperties();
                    List<MySqlUploadDate> tmp = new List<MySqlUploadDate>();
                    string id = String.Empty; bool ISstring = false;
                    for (int i = 0; i < myPropertyInfo.Length; i++)
                    {
                        PropertyInfo Variable_Selected = t.GetType().GetProperty(myPropertyInfo[i].Name);
                        if (nameIdVariable == myPropertyInfo[i].Name)
                        {
                            id = myPropertyInfo[i].Name;
                            ISstring = "string" == Variable_Selected.PropertyType.Name.ToLower();
                        }
                        else
                        {
                            tmp.Add(new MySqlUploadDate(myPropertyInfo[i].Name,
                                myPropertyInfo[i].GetValue(Variable_Selected).ToString(),
                                "string" == Variable_Selected.PropertyType.Name.ToLower()));
                        }

                    }
                    //sql message
                    string sqlMess = $"UPDATE {nameTable} SET ";
                    foreach (var tmpI in tmp)
                        sqlMess += $"{tmpI}";
                    sqlMess = sqlMess.Remove(sqlMess.Length - 1); sqlMess = $"WHERE {MySqlUploadDate.Convert(nameIdVariable, id, ISstring)} ;";
                    conn.Connection.Open();
                    cmd.CommandText = sqlMess;
                    cmd.Connection = conn.Connection;
                    cmd.Connection.Clone();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                cmd.Connection.Clone();
                return false;
            }
            error = "None";
            cmd.Connection.Clone();
            return true;
        }
        public bool LoadVariable(string tableName, (object key, string keyName) key, string TableLoadName, out object result)
        {
            try
            {

                MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
                conn.Connection.Open();
                cmd.Connection = conn.Connection;
                cmd.CommandText = $"SELECT * FROM {tableName} WHERE {key.keyName} = {Converter.ToSQLmessageType(key.key)}";
                using (MySqlConnector.MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            for (int i = 0; i < reader.VisibleFieldCount; i++)
                                if (reader.GetName(i).Equals(TableLoadName))
                                {
                                    result = reader.GetValue(i); return true;

                                }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
            result = null;
            return false;
        }
        public bool UpdateVariable(string tableName, (object key, string keyName) key, string TableLoadName, object result)
        {
            try
            {

                MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
                conn.Connection.Open();
                cmd.Connection = conn.Connection;
                cmd.CommandText = $"UPDATE {tableName} SET {TableLoadName} = {Converter.ToSQLmessageType(result)} WHERE {key.keyName} = {Converter.ToSQLmessageType(key.key)}";
               
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //EXPEREMINTAL
        
        
    }
    public class MySQLWorks
    {
        private MySqlWork work;

        public MySQLWorks(MySqlWork work)
        {
            this.work = work ?? throw new ArgumentNullException(nameof(work));
        }
        public void AllDateWork<T>(string TableName,string NameIDVariable,T TableClass,Action<List<T>>Funcs) where T : IMySqlObject, new()
        {
            var res = work.LoadDate<T>(TableName);
            Funcs.Invoke(res);
            work.UploadDate<T>(res, NameIDVariable, TableName, out string error);

        }
    }
    public class ExperementalWork<T> where T : IMySqlObject, new()
    {
        public Connector conn { get; set; }
        public List<T> LoadDate(string sql) 
        {

            List<T> result = new List<T>();
            T Obj_Creaters = new T();
            MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
            conn.Connection.Open();
            cmd.Connection = conn.Connection;
            cmd.CommandText = $"SELECT * FROM {sql}";
            using (MySqlConnector.MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<MySqlDate> dates = new List<MySqlDate>();
                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            (string type, object date, string name) tmp;
                            tmp.name = reader.GetName(i);
                            tmp.type = reader.GetDataTypeName(i);

                            tmp.date = reader.GetValue(i);

                            dates.Add(new MySqlDate(tmp.name, tmp.type, tmp.date));
                        }
                        result.Add(Obj_Creaters.Init<T>(dates));


                    }
                }
            }

            conn.Connection.Close();
            return result;
        }
    }
    public class Experemental
    {
        private Connector conn;

        public Experemental(Connector conn)
        {
            this.conn = conn;
        }

        public object CteClass(string TableName)
        {
            MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
            conn.Connection.Open();
            cmd.Connection = conn.Connection;
            cmd.CommandText = $"SELECT * FROM {TableName}";
            List<(string, Type)> dates = new List<(string, Type)>();
            using (MySqlConnector.MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                            dates.Add((reader.GetName(i), reader.GetValue(i).GetType()));

                        break;

                    }
                }
            }
            conn.Connection.Close();
            var _class = CreateClass.Creators.CreateNewObject(dates,TableName);
            Type general = typeof(ExperementalWork<>);
            general = general.MakeGenericType(_class.GetType());
            var _MainClass = Activator.CreateInstance(general);
            PropertyInfo myPropertyInfo = general.GetProperty("conn");
            myPropertyInfo.SetValue(_MainClass, conn);
           
            var Method = general.GetMethod("LoadDate", BindingFlags.Public | BindingFlags.Instance,
        null,
        CallingConventions.Any,
        new Type[] { typeof(string) },
        null);
            var result = Method.Invoke(_MainClass, new object[] { TableName });
            return result;
        }
        public void CteClassFileCode(string TableName)
        {
            MySqlConnector.MySqlCommand cmd = new MySqlConnector.MySqlCommand();
            conn.Connection.Open();
            cmd.Connection = conn.Connection;
            cmd.CommandText = $"SELECT * FROM {TableName}";
            List<(string, Type)> dates = new List<(string, Type)>();
            using (MySqlConnector.MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                            dates.Add((reader.GetName(i), reader.GetValue(i).GetType()));

                        break;

                    }
                }
            }
            conn.Connection.Close();
            var _class = CreateClass.Creators.CreateNewObject(dates,TableName);
            using (StreamWriter sw = new StreamWriter("Output.cs"))
            {
                sw.WriteLine("using System;\nnamespace Output\n{");
                sw.WriteLine($"    public class {_class.GetType().Name} : MySqlObject\n    {{ ");
                foreach (var i in _class.GetType().GetProperties())
                    sw.WriteLine($"        public {i.PropertyType.Name} {i.Name} {{get; set; }}");
                sw.WriteLine("    }\n}");
            }
        }

    }
    public class ExperementalHelper
    {

        List<object> MainVar;
        public object this[(string nameId,object ID) key]
        {
            get
            {
                foreach (var item in MainVar)
                {
                    if(item != null)
                    {
                        var VariableID = item.GetType().GetProperty(key.nameId);
                        if(VariableID.GetValue(item).Equals(key.ID))return item;

                        
                    }
                }
                

                return null;
                   
            }
        }
        public object this[(object str,string nameVar) info]
        {
            get
            {
                foreach (var item in info.str.GetType().GetProperties())
                    if (item.Name.Equals(info.nameVar))
                        return item.GetValue(info.str);
                


                return null;

            }
        }
        public ExperementalHelper(object mainVar)
        {
            MainVar = ((IEnumerable<object>)mainVar).ToList();
        }
        

     
    }
    

}
   


