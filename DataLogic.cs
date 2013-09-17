using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace DataBase
{
    class DataLogic
    {
        string status;

        /// <summary>
        /// add data to xml file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="TableName"></param>
        /// <param name="DatatypeName"></param>
        /// <param name="DatatypeValue"></param>
        public static void AddXML(string filename, List<string> TableName, List<string> DatatypeName, List<string> DatatypeValue)
        {
            try
            {
                XElement customer = XElement.Load(filename + ".xml");
                //  XElement customer = new XElement(filename+"root");
                XElement items = new XElement(filename);
                customer.AddFirst(items);


                for (int i = 0; i < TableName.Count; i++)
                {
                    //foreach (var o in TableName)
                    //{   customer.Element(filename).Add((new XElement(element[0])));
                    //var element = o.ToString().Split(' ');
                    // customer.Add( new XAttribute(DatatypeName[0], DatatypeValue[0]));
                    customer.Element(filename).Add((new XElement(TableName[i], new XAttribute(DatatypeName[i], DatatypeValue[i]))));
                    //}
                }
                customer.Save(filename + ".xml");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Columns"></param>
        /// <returns></returns>
        public Tuple<List<string>, List<string>, List<string>> MatchInsertDL(string filename, List<string> Columns)
        {
            XElement xele = XElement.Load(filename + ".xml");
            //xele.Add(new XElement(filename, "Hari"));
            //xele.Save(filename + ".xml");

            XElement child1 = xele.Element(filename);
            var nodes = child1.Descendants();
            //  Console.WriteLine(xele + "\n\n");
            bool Match = true;

            var pattern = @"\<(.*?)\>";
            bool count = true;

            var AttributeTuple = new Tuple<List<string>, List<string>, List<string>>(new List<string>(), new List<string>(), new List<string>());

            //     child1.Descendants().Attributes().ToList()[1].ToString().Split('"')[1]
            foreach (var node in nodes)
            {
                var matches = Regex.Matches(node.ToString(), pattern);
                string[] attr = null;

                foreach (Match m in matches)
                {
                    if (count)
                    {
                        attr = m.Groups[1].Value.Split(' ');
                    }
                    count = false;
                }


                AttributeTuple.Item1.Add(attr[0]);
                AttributeTuple.Item2.Add(attr[1].Split('=')[0]);
                AttributeTuple.Item3.Add(Regex.Match(attr[1].Split('=')[1], @"\d+").Value);

                count = true;
                //  child1.Descendants().Attributes().ToList()[1].ToString().Split('"')[0].Split('=')[0]
            }
            return AttributeTuple;
        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="TableName"></param>
        /// <param name="DatatypeName"></param>
        /// <param name="DatatypeValue"></param>
        /// <returns></returns>
        public string CreateXML(string filename, List<string> TableName, List<string> DatatypeName, List<string> DatatypeValue)
        {
            bool DataTypeMissMatch = false;
           
            if (TableName.Count != 0 || DatatypeName.Count != 0 || DatatypeValue.Count != 0)
            {
                foreach (var v in DatatypeName)
                {
                    if (!(v.ToLower().Contains("int") || v.ToLower().Contains("varchar")))
                    {
                        DataTypeMissMatch = true;

                    }
                    
                }
                if (!DataTypeMissMatch)
                {
                    if (File.Exists(filename + ".xml"))
                    {
                        status = "\ttable already exists.";

                    }
                    else
                    {


                        try
                        {
                            using (XmlWriter writer = XmlWriter.Create(filename + ".xml"))
                            {
                                writer.WriteStartDocument();
                                writer.WriteStartElement(filename);
                                writer.WriteEndElement();
                                writer.WriteEndDocument();
                            }
                            //string str = Console.ReadLine();

                            //  var o = str.Split(' ');
                            //  Creation(filename, o);


                            AddXML(filename, TableName, DatatypeName, DatatypeValue);
                            status = "\tTable Created Successfully";
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                    }

                }
                else
                {
                    status = "only 'int' and 'varchar' datatype is allowed";
                }
            }
            else
            {
                status = "please define columns for \'" + filename + "\' table";
            }
            return status;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string DeleteXML(string filename)
        {

            try
            {

                if (File.Exists(filename + ".xml"))
                {
                    File.Delete(filename + ".xml");
                    status = "\ttable deleted succesfully";

                }
                else
                {
                    status = "\ttable does not exist.";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return status;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="list"></param>
        public static void Creation(string filename, params object[] list)
        {
            try
            {
                XElement customer = XElement.Load(filename + ".xml");
                //  XElement customer = new XElement(filename+"root");
                XElement items = new XElement(filename);
                customer.AddFirst(items);
                foreach (object o in list)
                {
                    var element = o.ToString().Split(' ');
                    customer.Element(filename).Add((new XElement(element[0])));
                }
                customer.Save(filename + ".xml");
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //public string GetInsertedColumnDataDL(string TableName, List<string> ColumnValues)
        //{


   /// <summary>
   /// 
   /// </summary>
   /// <param name="filename"></param>
   /// <param name="AttributeTuple"></param>
   /// <param name="Values"></param>
   /// <returns></returns>
        public string InsertIntoXML(string filename, Tuple<List<string>, List<string>, List<string>> AttributeTuple, List<string> Values)
        {


            string status;
            try
            {
                if (File.Exists(filename + ".xml"))
                {
                    XElement customer = XElement.Load(filename + ".xml");


                    XElement items = new XElement(filename);
                    customer.AddFirst(items);


                    for (int i = 0; i < AttributeTuple.Item1.Count; i++)
                    {

                        customer.Element(filename).Add((new XElement(AttributeTuple.Item1[i], Values[i], new XAttribute(AttributeTuple.Item2[i], AttributeTuple.Item3[i]))));
                    }

                    customer.Save(filename + ".xml");

                    status = "\ttable Inserted succesfully";

                }
                else
                {
                    status = "\ttable does not exist.";

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return status;
        }









        //public void SelectXML(string filename, string node, string symbol, string value)
        //{


        //    XElement oElement = XElement.Load(filename + ".xml");


        //    var sam = from item in oElement.Elements(filename.ToString())
        //              where (string)item.Element(node) == value
        //              select item;

        //    if (sam != null)
        //    {
        //        XElement tempElement = new XElement(filename);
        //        foreach (XElement name in sam)
        //        {
        //            tempElement.Add(name);
        //        }
        //        if (tempElement != null)
        //        {
        //            Console.WriteLine(tempElement);
        //        }

        //        else
        //        {
        //            Console.WriteLine("no record found");
        //        }
        //    }

        //    else
        //    {
        //        Console.WriteLine("\ttable does not exist.");
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        public string SelectXML(string filename, string[] Column)
        {
            List<string> s = new List<string>(Column.Length);

            XElement oElement = XElement.Load(filename + ".xml");

            List<string> Key = new List<string>();
            List<string> Value = new List<string>();
            string[] keyvalue = new string[10];
            keyvalue[1] = "as";
            foreach (XElement e in oElement.Descendants())
            {

                for (int i = 0; i < Column.Count(); i++)
                {

                    if (e.Name == Column[i])
                    {
                        Key.Add(Column[i]);
                        Value.Add(e.Value.ToString());
                        //Console.Write(e.Name + "=" + e.Value);
                    }

                }

            }
            var KeyDistinctArray = Key.Distinct().ToArray();


            foreach (var v in KeyDistinctArray)
            {
                var result = Key.GroupBy(a => a.Contains(v));

                foreach (var group in result)
                {
                    // Display key for group.
                    Console.WriteLine("IsEven = {0}:", group.Key);

                    // Display values in group.

                    foreach (var value in group)
                    {

                        Console.WriteLine("{0} ", value);
                    }

                    // End line.
                    Console.WriteLine();
                }
            }
            //for (int i = 0; i < Key.Count; i++)
            //{

            //        if (v == Key[i])
            //        {

            //        }
            //    }
            //    Console.WriteLine(Key[i] + " " + Value[i]);
            //}
            //var sam = from item in oElement.Elements(filename.ToString())
            //          where item.Element(Column[0])
            //          select item;
            var sam = from item in oElement.Elements(filename)
                      where item.Elements(Column[0]).Any()
                      select item;
            if (sam != null)
            {
                XElement tempElement = new XElement(filename);
                foreach (XElement name in sam)
                {
                    tempElement.Add(name);
                }
                if (tempElement != null)
                {
                    Console.WriteLine(tempElement);
                }

                else
                {
                    Console.WriteLine("no record found");
                }
            }

            else
            {
                Console.WriteLine("\ttable does not exist.");
            }
            return "s";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void SelectAllXML(string filename)
        {


            try
            {
                if (File.Exists(filename + ".xml"))
                {
                    //  XElement oElement = XElement.Load(filename + ".xml");

                    XElement element = XElement.Load(filename + ".xml");
                    var sam = from item in element.Elements(filename)
                              select item;
                    bool token = true;

                    foreach (XElement name in sam)
                    {
                        foreach (XElement n in name.Elements())
                        {
                            if (token)
                            {
                                Console.Write(n.Name + "\t");

                            }
                        }
                        token = false;
                    }
                    Console.WriteLine();
                    token = true;
                    foreach (XElement name in sam)
                    {
                        foreach (XElement n in name.Elements())
                        {
                            if (token)
                            {
                                Console.Write("--------");
                            }
                        }
                        token = false;
                    }
                    Console.WriteLine();
                    foreach (XElement name in sam)
                    {


                        foreach (XElement n in name.Elements())
                        {

                            if (name.Value != "")
                            {

                                Console.Write(n.Value + "\t");
                            }
                        }

                        Console.WriteLine();
                    }

                }
                else
                {
                    status = "\ttable does not exist.";

                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        //public string InsertIntoXML(string qry)
        //{
        //    string filename = qry.Split(' ').Last();
        //    string status;
        //    try
        //    {
        //        if (File.Exists(filename + ".xml"))
        //        {
        //            //XElement xele = XElement.Load(filename + ".xml");
        //            //xele.Add(new XElement(filename, (new XElement("num", "12345"))));
        //            //xele.Save(filename + ".xml"); 


        //            XElement xele = XElement.Load(filename + ".xml");
        //            //xele.Add(new XElement(filename, "Hari"));
        //            //xele.Save(filename + ".xml");

        //            XElement child1 = xele.Element(filename);
        //            var nodes = child1.Descendants();
        //            //  Console.WriteLine(xele + "\n\n");
        //            List<string> i = new List<string>();
        //            object o = null;
        //            foreach (var node in nodes)
        //            {
        //                Console.Write(node.Name + " = ");
        //                string s = Console.ReadLine();
        //                i.Add(node.Name + "=" + s);
        //            }



        //            ////string str = Console.ReadLine();
        //            o = i;
        //            ////var o = str.Split(' ');
        //            Insertion(filename, i);

        //            status = "\ttable Inserted succesfully";

        //        }
        //        else
        //        {
        //            status = "\ttable does not exist.";

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return status;
        //}


        public static void Insertion(string filename, List<string> list)
        {
            try
            {
                XElement customer = XElement.Load(filename + ".xml");
                //  xele.Add(new XElement(filename, Insertion(filename,list)));

                XElement items = new XElement(filename);
                customer.AddFirst(items);

                foreach (string o in list)
                {
                    var element = o.ToString().Split('=');

                    customer.Element(filename).Add((new XElement(element[0], element[1])));
                }
                customer.Save(filename + ".xml");
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
