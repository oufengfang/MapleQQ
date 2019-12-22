using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace 音频播放
{
    public class Like
    {
       
        public int DIDUpdata(string sql)//将需要的sql语句交给DIDUpdata
        {
            int rs = 0;
            SqlConnection con = new SqlConnection("server=.;uid=sa;pwd=sasa;database=MapleQQ;");
            SqlCommand com = new SqlCommand(sql, con);
            con.Open();
            rs = com.ExecuteNonQuery();
            con.Close();
            return rs;
        }
        public int DB(string sql) 
        {
            int rs = 0;
            SqlConnection con = new SqlConnection("server=.;uid=sa;pwd=sasa;database=MapleQQ;");
            SqlCommand com = new SqlCommand(sql, con);
            con.Open();
            rs = int.Parse(com.ExecuteScalar().ToString ());
            con.Close();
            return rs;
        }
        public DataSet QueryByAdapter(string sql, string tableName)
        {
            SqlConnection con = new SqlConnection("server=.;uid=sa;pwd=sasa;database=MapleQQ;");
            SqlDataAdapter sda = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            sda.Fill(ds, tableName);
            return ds;
        }
    }
}
