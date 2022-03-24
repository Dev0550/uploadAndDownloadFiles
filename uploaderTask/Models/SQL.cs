using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace uploaderTask.Models
{
    public class SQL
    {
        private readonly string _dbConnection = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        public ObjFile GetInfo(string name)
        {
            ObjFile result = null;
            using (SqlConnection cn = new SqlConnection(_dbConnection))
            {
                try
                {
                    cn.Open();
                    result = cn.Query<ObjFile>("Select [Date] from [dbo].[Files] where Name = @name", new { name }).FirstOrDefault();
                    cn.Close();
                }
                catch
                {
                }
            }
            return result;
        }
        public void NewRecord(string name, long size, string type, DateTime date)
        {
            using (SqlConnection cn = new SqlConnection(_dbConnection))
            {
                try
                {
                    cn.Open();
                    cn.Execute("Insert into [dbo].[Files]([Name], [Size], [Date], [Type]) values (@name, @size, @date, @type)", new { name, size, date, type });
                    cn.Close();
                }
                catch
                {
                }
            }
        }
        public void DeleteRecord(string name)
        {
            using (SqlConnection cn = new SqlConnection(_dbConnection))
            {
                try
                {
                    cn.Open();
                    cn.Execute("Delete from [dbo].[Files] where Name = @name", new { name });
                    cn.Close();
                }
                catch
                {
                }
            }
        }
    }
}