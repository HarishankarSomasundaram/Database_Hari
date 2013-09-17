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
    class BusinessLogic
    {
        DataLogic DL = new DataLogic();


        /// <summary>
        /// function that identifies querry and process to datalogic
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public string BusinessLogicMemberBL(string qry)
        {
            string status = null;
            List<string>[] QuerryList = MatchBL(qry);

            List<string> QuerryList1 = QuerryList[0];
            List<string> TableName = QuerryList[1];
            List<string> DatatypeName = QuerryList[2];
            List<string> DatatypeValue = QuerryList[3];
            List<string> QuerryList2 = QuerryList[4];
            bool create = false;
            bool table = false;
            bool Insert = false;
            bool Into = false;
            bool Values = false;
            bool Select = false;
            bool Star = false;
            bool From = false;
            bool StarValues=false;

            if (QuerryList1.Count == 3)
            {
                create =StringMatch( QuerryList1[0],"create");
                table = StringMatch(QuerryList1[1],  "table");
             
                if (create == true && table == true && TableName.Count == DatatypeName.Count && DatatypeName.Count == DatatypeValue.Count)
                {
                    status = DL.CreateXML(QuerryList1[2], TableName, DatatypeName, DatatypeValue);
                   
                }
                else
                {
                    status = "Please check your syntax";
                }

            }
            else if (QuerryList1.Count == 4)
            {
                Insert =StringMatch( QuerryList1[0], "insert" );
                Into = StringMatch( QuerryList1[1], "into" );
                Values = StringMatch( QuerryList1[3],  "values" );
               
                Select =StringMatch( QuerryList1[0], "select" );
                if (QuerryList1[1] == "*")
                {
                    Star = true;
                }
                else
                {
                    if (Regex.IsMatch(QuerryList1[1], @"\w+"))
                    {
                        StarValues = true;
                    }
                    else
                    {
                    Star = false;
                    }
                }
                From=StringMatch( QuerryList1[2], "from" );

                //if(true)
                //{
                if (Insert == true && Into == true && Values == true)
                {
                    var AttributeTuple = MatchInsertBL(QuerryList1[2], QuerryList2);
                    if (AttributeTuple != null)
                    {
                        status = DL.InsertIntoXML(QuerryList1[2], AttributeTuple, QuerryList2);
                        status = "Record inserted successfully";
                    }
                    else
                    {
                        status = "Please check your syntax";
                    }
                    // status = DL.CreateXML(QuerryList1[2], QuerryList2);

                }
               else if (Select == true && Star == true && From == true)
                {
                    
                    
                        DL.SelectAllXML(QuerryList1[3]);
                        status = "Command executed successfully";
                
                }
                else if (Select == true && StarValues == true && From == true)
                {
                    if (QuerryList1[1].Contains(','))
                    {
                        var ColumnList = QuerryList1[1].Split(',');
                        DL.SelectXML(QuerryList1[3],ColumnList);
                    }
                    else
                    {
                        //DL.SelectXML(QuerryList1[3]);
                    }
                    DL.SelectAllXML(QuerryList1[3]);
                    status = "Command executed successfully";

                }
                else
                {
                    status = "Please check your syntax";
                }
            }
            else if (QuerryList1.Count == 4)
            {
            }
            else
            {
                status = "Please check your syntax";
            }


            return status;
        }
        /// <summary>
        /// string matching fuction to match user input string
        /// </summary>
        /// <param name="DynamicValue"></param>
        /// <param name="StaticValue"></param>
        /// <returns></returns>
        public bool StringMatch(string DynamicValue, string StaticValue)
        {
            return Regex.IsMatch(DynamicValue, string.Format(@"\b{0}\b", Regex.Escape(StaticValue)), RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// returns tuples that contains list of columns and values from the user
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Columns"></param>
        /// <returns></returns>
        public Tuple<List<string>, List<string>, List<string>> MatchInsertBL(string filename, List<string> Columns)
        {//List<string> ColumnValues
            if (File.Exists(filename + ".xml"))
            {
                //XElement xele = XElement.Load(filename + ".xml");
                ////xele.Add(new XElement(filename, "Hari"));
                ////xele.Save(filename + ".xml");

                //XElement child1 = xele.Element(filename);
                //var nodes = child1.Descendants();
                ////  Console.WriteLine(xele + "\n\n");
                bool Match = true;

                //var pattern = @"\<(.*?)\>";
                //bool count = true;

                var AttributeTuple = DL.MatchInsertDL(filename, Columns);

                ////     child1.Descendants().Attributes().ToList()[1].ToString().Split('"')[1]
                //foreach (var node in nodes)
                //{
                //    var matches = Regex.Matches(node.ToString(), pattern);
                //    string[] attr = null;

                //    foreach (Match m in matches)
                //    {
                //        if(count)
                //        {
                //            attr = m.Groups[1].Value.Split(' ');
                //        }
                //        count = false;
                //    }


                //    AttributeTuple.Item1.Add(attr[0]);
                //    AttributeTuple.Item2.Add(attr[1].Split('=')[0]);
                //    AttributeTuple.Item3.Add(Regex.Match(attr[1].Split('=')[1], @"\d+").Value);

                //    count = true;
                //    //  child1.Descendants().Attributes().ToList()[1].ToString().Split('"')[0].Split('=')[0]
                //}

                if (Columns.Count == AttributeTuple.Item1.Count)
                {
                    for (int i = 0; i < AttributeTuple.Item1.Count; i++)
                    {
                        //if (Columns[i] != AttributeTuple.Item1[i])
                        //{
                        //    Match = false;
                        //}
                        if (Columns[i].Length > Convert.ToInt32(AttributeTuple.Item3[i]))
                        {
                            Match = false;
                        }
                        int value;
                        if (AttributeTuple.Item2[i] == "int")
                        {
                            if (!int.TryParse(Columns[i], out value))
                            {
                                Match = false;
                            }

                        }
                    }

                }
                else
                {
                    Match = false;
                }
                if (Match)
                {
                    return AttributeTuple;
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }






        public List<string>[] MatchBL(string Input)
        {

            string InputQuerry = null;

            List<string> QuerryList1 = new List<string>();
            List<string> QuerryList2 = new List<string>();

            List<string> TableName = new List<string>();
            List<string> DatatypeName = new List<string>();
            List<string> DatatypeValue = new List<string>();




            var pattern = @"\{(.*?)\}";
            var matches = Regex.Matches(Input, pattern);
            if (Input.Contains("{") && Input.Contains("}"))
            {
                InputQuerry = Input.Substring(0, Input.IndexOf("{"));
                InputQuerry = InputQuerry.Trim();




                var InputSplit = InputQuerry.Split(' ');

                foreach (var element in InputSplit)
                {
                    if (element != "")
                    {
                        QuerryList1.Add(element);
                    }
                }
                if (QuerryList1.Count == 4)
                {

                    foreach (Match m in matches)
                    {
                        if (m.Groups[1].Value.Contains(','))
                        {
                            var bracket = m.Groups[1].Value.Split(',');
                            //Console.WriteLine(m.Groups[1]);
                            foreach (var element in bracket)
                            {
                                if (element != "")
                                {
                                    QuerryList2.Add(element.Trim());
                                }
                            }
                        }
                    }



                }
                else if (QuerryList1.Count == 3)
                {
                    foreach (Match m in matches)
                    {
                        if (m.Groups[1].Value.Contains(','))
                        {
                            var bracket = m.Groups[1].Value.Split(',');
                            //Console.WriteLine(m.Groups[1]);
                            foreach (var element in bracket)
                            {
                                if (element != "")
                                {
                                    QuerryList2.Add(element);
                                }
                            }


                            foreach (var b in QuerryList2)
                            {
                                var QuerryList2SplitArray = b.Split(' ');
                                string value = null;

                                string DataName = null;

                                foreach (var item in QuerryList2SplitArray)
                                {

                                    if (item != "")
                                    {

                                        var SubPattern = @"\((.*?)\)";

                                        var SubMatches = Regex.Matches(item, SubPattern);
                                        if (SubMatches.Count > 0)
                                        {
                                            if (item.ToLower() != "int")
                                            {
                                                DataName = item.Substring(0, item.IndexOf("(")).Trim();

                                                foreach (Match Sm in SubMatches)
                                                {
                                                    value = Sm.Groups[1].Value;
                                                }

                                            }
                                        }
                                        else if (item.ToLower() == "int")
                                        {
                                            DataName = item;
                                            value = "255";
                                        }

                                        else
                                        {
                                            TableName.Add(item);
                                        }


                                    }

                                }
                                if (DataName != null || value != null)
                                {
                                    DatatypeName.Add(DataName);
                                    DatatypeValue.Add(value);
                                }

                            }
                        }

                        else
                        {
                            QuerryList2.Add(m.Groups[1].Value);
                            foreach (var b in QuerryList2)
                            {
                                var QuerryList2SplitArray = b.Split(' ');
                                string value = null;

                                string DataName = null;

                                foreach (var item in QuerryList2SplitArray)
                                {

                                    if (item != "")
                                    {

                                        var SubPattern = @"\((.*?)\)";

                                        var SubMatches = Regex.Matches(item, SubPattern);
                                        if (SubMatches.Count > 0)
                                        {
                                            DataName = item.Substring(0, item.IndexOf("(")).Trim();

                                            foreach (Match Sm in SubMatches)
                                            {
                                                value = Sm.Groups[1].Value;
                                            }

                                        }
                                        else
                                        {
                                            TableName.Add(item);
                                        }


                                    }

                                }
                                DatatypeName.Add(DataName);
                                DatatypeValue.Add(value);


                            }
                        }

                    }
                }
            }
            else
            {
                var InputSplit = Input.Split(' ');

                foreach (var element in InputSplit)
                {
                    if (element != "")
                    {
                        QuerryList1.Add(element);
                    }
                }
            }
            List<string>[] QuerryList = { QuerryList1, TableName, DatatypeName, DatatypeValue, QuerryList2 };

            ////var matches = Regex.Matches(input, pattern);
            ////foreach (Match m in matches)
            ////{
            ////    Console.WriteLine(m.Groups[1]);
            ////}
            //var v = Regex.Match(input, @"(^|\s)create table table(\s|$)").Success;
            //var v = Regex.IsMatch(Input, string.Format(@"\b{0}\b", Regex.Escape("create table")), RegexOptions.IgnoreCase);
            //  Console.WriteLine(v.ToString());
            return QuerryList;
        }



        //public string DataBaseBL(string qry)
        // {

        //     string status=null;
        //         if (qry != "")
        //         {
        //             string s = qry.Trim();
        //               Match qry_create = Regex.Match(s, @"^([\'create']+) ([\'table']+) ([\w]+)$");


        //           //  Match qry_create = Regex.Match(qry, @"^([\'create']+) ([\'table']+) ([\w]+)$");
        //             Match qry_delete = Regex.Match(qry, @"^([\'drop']+) ([\'table']+) ([\w]+)$");
        //             Match qry_insert = Regex.Match(qry, @"^([\'insert']+) ([\'table']+) ([\w]+)$");
        //             Match qry_select = Regex.Match(qry, @"^([\'select \*']+) ([\'from']+) ([\w]+) ([\'where']+) ([\w \=+-]+) ([\w]+)$");
        //             Match qry_selectall = Regex.Match(qry, @"^([\'select \*']+) ([\'from']+) ([\w]+)$");
        //             Match qry_selectColumn = Regex.Match(qry, @"^([\'select']+) (([\w]+),([\w]+))+ ([\'from']+) ([\w]+)$");




        //             if (qry_create.Success)
        //             {
        //                 try
        //                 {
        //                     string filename = qry.Split(' ').Last();
        //                     status = DL.CreateXML(filename);
        //                 }
        //                 catch (Exception e)
        //                 {
        //                     throw e;
        //                 }
        //             }

        //             else if (qry_selectColumn.Success)
        //             {

        //                 var oVar = qry.Split(' ');

        //                 // var match = Regex.Match(qry, @"select (.+?)from").Groups[1].Value;

        //                 string filename = oVar[3].ToString();
        //                 string[] Columns = oVar[1].Split(',');
        //                 //   string node = oVar[5].ToString();
        //                 //   string symbol = oVar[6].ToString();
        //                 //   string value = oVar[7].ToString();
        //                 try
        //                 {
        //                     // status= DL.SelectXML(filename, Columns);
        //                 }
        //                 catch (Exception e)
        //                 {
        //                     throw e;
        //                 }
        //                 status = "Server Problem";

        //             }
        //             else if (qry_delete.Success)
        //             {


        //                 string filename = qry.Split(' ').Last();
        //                 try
        //                 {
        //                     status = DL.DeleteXML(filename);
        //                 }
        //                 catch (Exception e)
        //                 {
        //                     throw e;
        //                 }

        //             }

        //             else if (qry_select.Success)
        //             {
        //                 //  DL.SelectXML(qry);
        //                 status = "Server Problem";
        //             }

        //             else if (qry_selectall.Success)
        //             {
        //                 try
        //                 {
        //                     status = DL.SelectAllXML(qry);
        //                 }
        //                 catch (Exception e)
        //                 {
        //                     throw e;
        //                 }

        //             }


        //             else if (qry_insert.Success)
        //             {
        //                 string filename = qry.Split(' ').Last();
        //                 try
        //                 {
        //                     status = DL.InsertIntoXML(qry);
        //                 }
        //                 catch (Exception e)
        //                 {
        //                     throw e;
        //                 }

        //             }
        //             else
        //             {
        //                 status = "\tPlease check your syntax";

        //             }
        //         }
        //         return status;


        // }

        private static void SymbolIdentifier(object[] oVar)
        {
            var filename = oVar[3];
            string node = oVar[5].ToString();
            string symbol = oVar[6].ToString();
            var value = oVar[7];

            XElement element = XElement.Load(filename + ".xml");
            if (symbol == "=")
            {
                var sam = from item in element.Elements(filename.ToString())
                          where item.Element(node) == value
                          select item;
                XElement xe = (XElement)sam;


                XElement tempElement = XElement.Load(filename + ".xml");
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
            //else if (symbol == ">")
            //{
            //    var sam = from item in element.Elements(filename.ToString())
            //              where item.Element(node) > value
            //              select item;
            //    return sam;
            //}

        }



    }
}

