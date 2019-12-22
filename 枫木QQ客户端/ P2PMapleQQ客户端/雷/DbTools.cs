using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Login
{
    class DbTools
    {
        //通用的sql语句连接，增删改方法
        public int zsg(string sql)
        {
            SqlConnection con = new SqlConnection("server=.;uid=sa;pwd=sasa;database=MapleQQ");
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            int h = (int)cmd.ExecuteNonQuery();
            return h;
        }
        public SqlDataAdapter jiazai;
        public DataSet QuerByAdapter(string sql, string tableName)//断开式查询的方法
        {
            SqlConnection con = new SqlConnection("server=.;uid=sa;pwd=sasa;database=MapleQQ");
            DataSet movie = new DataSet();//创建临时数据库供全局使用
            jiazai = new SqlDataAdapter(sql, con);
            jiazai.Fill(movie, tableName);
            return movie;
        }
        public int query(string sql) 
        {
            SqlConnection con = new SqlConnection("server=.;uid=sa;pwd=sasa;database=MapleQQ");
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
             int r= int.Parse(cmd.ExecuteScalar().ToString());
             return r;
        }
        //连接方法
        public SqlDataReader queny(string sql)
        {
            SqlConnection con = new SqlConnection("server=.;uid=sa;pwd=sasa;database=MapleQQ");
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader red = cmd.ExecuteReader();
            return red;
        }

       
    }
}
