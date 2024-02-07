using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebAccess
{
    /// <summary>
    /// Summary description for CRUDService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CRUDService : System.Web.Services.WebService
    {
        SqlConnection conObj = new SqlConnection();
        [WebMethod]
        public DataTable GetPersonByName(string name)
        {
            DataTable dtPerson = new DataTable();
            if (conObj.State == ConnectionState.Closed)
            {
                conObj.ConnectionString = ConfigurationManager.AppSettings["connectionString"].ToString();
                conObj.Open();
            }
            SqlCommand objCommand = new SqlCommand();
            objCommand.Connection = conObj;
            objCommand.CommandText = "SELECT * FROM Person WHERE Name LIKE @Name";
            objCommand.Parameters.AddWithValue("@Name", "%" + name + "%");
            DataSet dsPer = new DataSet();
            SqlDataAdapter objAdapter = new SqlDataAdapter(objCommand);
            objAdapter.Fill(dsPer);
            return dsPer.Tables[0];
        }



        [WebMethod]
        public DataTable GetPersons()
        {
            DataTable dtPerson = new DataTable();
            if (conObj.State == ConnectionState.Closed)
            {
                conObj.ConnectionString = ConfigurationManager.AppSettings["connectionString"].ToString();
                conObj.Open();
            }
            SqlCommand objCommand = new SqlCommand();
            objCommand.Connection = conObj;
            objCommand.CommandText = "Select * from Person";
            DataSet dsPer = new DataSet();
            SqlDataAdapter objAdapter = new SqlDataAdapter(objCommand);
            objAdapter.Fill(dsPer);
            return dsPer.Tables[0];

        }

        [WebMethod]
        public string SavePerson(Person pers)
        {
            DataTable dtPerson = new DataTable();
            if (conObj.State == ConnectionState.Closed)
            {
                conObj.ConnectionString = ConfigurationManager.AppSettings["connectionString"].ToString();
                conObj.Open();
            }
            SqlCommand objCommand = new SqlCommand();
            objCommand.Connection = conObj;
            //objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "INSERT INTO Person (Name, Surname, Age) VALUES (@Name, @Surname, @Age)";
            objCommand.Parameters.AddWithValue("@Id", pers.Id);
            objCommand.Parameters.AddWithValue("@Name", pers.Name);
            objCommand.Parameters.AddWithValue("@Surname", pers.Surname);
            objCommand.Parameters.AddWithValue("@Age", pers.Age);

            objCommand.ExecuteNonQuery();
            return "Record has been saved.";
        }

        


        [WebMethod]
        public void DeletePerson(int personId)
        {
           using (SqlConnection conObj = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]))
            {
                conObj.Open();

                SqlCommand objCommand = new SqlCommand();
                objCommand.Connection = conObj;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "DeletePerson";
                objCommand.Parameters.AddWithValue("@PersonId", personId);
                objCommand.ExecuteNonQuery();

            }
        }

        public class Person
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Surname { get; set; }

            public int Age { get; set; }
        }
    }
}
