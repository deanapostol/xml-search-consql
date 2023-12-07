using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using static ConsoleApp2.Program;
using System.Data;

namespace ConsoleApp2
{
    internal class Program
    {
        public partial class program : Program
        {






            static void Main(string[] args)

            {
                Class1 contest = new Class1();
                SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-ROU6NJ8\SQLEXPRESS;Initial Catalog=working;Integrated Security=True;");
                SqlCommand cmd = new SqlCommand("checkandinsert", sqlcon);
                

                
                DateTime today = DateTime.Today;
                
                XmlReader reader = XmlReader.Create("https://mpr.datamart.ams.usda.gov/ws/report/v1/hogs/LM_HG234?filter=%7B%22filters%22:%5B%7B%22fieldName%22:%22Report%20date%22,%22operatorType%22:%22EQUAL%22,%22values%22:%5B%2212/5/2023%22%5D%7D%5D%7D.xml");
                do
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "record"))
                    {
                        if (reader.HasAttributes &&
                            reader.GetAttribute("weight_range") == "300-399 pounds" &&
                            reader.GetAttribute("purchase_type") == "Negotiated Sows (Live basis)")
                        {

                            //sql query

                            sqlcon.Open();

                            cmd.CommandType = CommandType.StoredProcedure;
                            
                            cmd.Parameters.Clear();


                            cmd.Parameters.AddWithValue("@weight", reader.GetAttribute("weight_range"));
                            cmd.Parameters.AddWithValue("@purchase", reader.GetAttribute("purchase_type"));
                            cmd.Parameters.AddWithValue("@price", reader.GetAttribute("wtd_avg"));

                            cmd.Parameters.AddWithValue("@date", today);

                            cmd.ExecuteNonQuery();

                            sqlcon.Close();




                            //print on console
                            Console.WriteLine("weight_range:" + reader.GetAttribute("weight_range") + '\t' + "purchase_type:" + reader.GetAttribute("purchase_type") + '\t' + "price-avg" + reader.GetAttribute("wtd_avg") + '\t' + today);




                        }

                    }
                } while (reader.Read());
                    Console.ReadLine();

                }

            }
        }
    }