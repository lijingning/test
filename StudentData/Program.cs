﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace StudentData
{
    class Program
    {
        static void Main(string[] args)
        {
            int ignoreLine;
            string[] files;
            int filesCount;
            System.Data.DataTable dt = new System.Data.DataTable();
            Console.WriteLine("请输入TXT文件内容中需要忽略的行数(一般默认值为5)\n");
            ignoreLine = int.Parse(Console.ReadLine());
            files = Directory.GetFiles(System.Environment.CurrentDirectory, "*.txt", SearchOption.AllDirectories);
            filesCount = files.Count();
            for (int i=0;i<filesCount;i++)
            {
                dt = DataTableUnion(dt, txtToDataTable(files[i], ignoreLine));
            }
            if (datatableToCSV(dt,"output.csv")==true)
            {
                Console.WriteLine("Done!");
            }
            else
            {
                Console.WriteLine("Failed!");
            }
        }

        public static System.Data.DataTable txtToDataTable(string filepath,int ignoreLine)
        {
            try
            {
                System.Data.DataTable dt;
                FileStream fs;
                StreamReader sr;
                string[] inputrow;
                string filename;
                string classname;
                fs = new FileStream(filepath, FileMode.Open);
                sr = new StreamReader(fs, Encoding.GetEncoding("GBK"));
                filename = filepath.Split('\\').Last();
                classname = sr.ReadLine().Split(' ')[0];
                for (int i = 1; i < ignoreLine; i++)
                {
                    sr.ReadLine();
                }
                dt = new System.Data.DataTable();
                inputrow = sr.ReadLine().Split('\t');
                inputrow[17] = "Course";
                foreach (string columnsname in inputrow)
                {
                    dt.Columns.Add(columnsname);
                }
                for (; ; )
                {
                    inputrow = sr.ReadLine().Split('\t');
                    if (inputrow[0] == "")
                        break;
                    inputrow[17] = filename.Split('.').First();
                    dt.Rows.Add(inputrow);
                }
                dt.Columns.Add("Class");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Class"] = classname;
                }
                return dt;
            }
            catch (NoNullAllowedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static System.Data.DataTable DataTableUnion(System.Data.DataTable dataTable1, System.Data.DataTable dataTable2)
        {
            if (dataTable2==null)
            {
                Console.WriteLine("Input Data Error: DataTable2 Is Null!\n");
                return null;
            }
            if (dataTable1.Columns.Count==0)
            {
                dataTable1 = dataTable2.Clone();
            }
            object[] obj = new object[dataTable1.Columns.Count];
            for (int i=0;i<dataTable2.Rows.Count;i++)
            {
                dataTable2.Rows[i].ItemArray.CopyTo(obj, 0);
                dataTable1.Rows.Add(obj);
            }
            return dataTable1;
        }

        public static bool datatableToCSV(DataTable dt, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                string data = "";
                //写出列名称
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    data += dt.Columns[i].ColumnName.ToString();
                    if (i < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
                //写出各行数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        data += dt.Rows[i][j].ToString();
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}