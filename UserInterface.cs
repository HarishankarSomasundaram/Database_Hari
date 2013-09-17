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
    class UserInterface
    {
        /// <summary>
        /// database application is called by delegate
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public delegate string Database(string s);

        /// <summary>
        /// 
        /// </summary>
       public static void Main()
        {

          UserInterface ui = new UserInterface();
            ui.UI();
          
        }
        /// <summary>
        /// calls business logic
        /// </summary>
       public void UI()
       {
           BusinessLogic BL = new BusinessLogic();
           string Result;

          
           Console.WriteLine("\tWelcome to Hari DataBase");
           do
           {
               Console.Write(">> ");
               string qry = Console.ReadLine();
               if (qry == "bye")
               {
                   break;

               }
               else
               {


                   // Result = UserInterface.db(qry, BL.DataBaseBL);
                   Result = BL.BusinessLogicMemberBL(qry);
                   Console.WriteLine(Result);

                   // UserInterface.match(); @"^([\'create'])$"


               }
           } while (true);

       }
        
        /// <summary>
        /// delegate function
        /// </summary>
        /// <param name="qry"></param>
        /// <param name="oDB"></param>
        /// <returns></returns>
        public static string db(string qry, Database oDB)
        {
            string status;
            try
            { 
               status= oDB(qry);
            }
            catch (Exception e)
            {
            status = e.ToString();
            }
            return status;
        }
    }
}
