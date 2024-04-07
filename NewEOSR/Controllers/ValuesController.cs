using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.AccessControl;

namespace NewEOSR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _configuration;
        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetUsers")]
        public JsonResult GetUsers()
        {
            string query = "select * from dbo.tblUserss";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using(SqlConnection myCon=new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        
        [HttpPost]
        [Route("AddUsers")]
        public JsonResult AddUsers([FromForm] string newUserss)
        {
            string query = "insert into dbo.tblUserss values(@newUserss)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@newUserss", newUserss);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpDelete]
        [Route("DeleteUsers")]
        public JsonResult DeleteUsers(int id)
        {
            string query = "delete from dbo.tblUserss where id=@id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", "id");
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [HttpPost]
        [Route("SubmitReport")]
        public JsonResult SubmitReport([FromBody] ReportModel report)
        {
            string query = @"
            INSERT INTO Reports (userId, reportDate, reportContent) 
            VALUES (@userId, @reportDate, @reportContent)";
        
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@userId", report.UserId);
                    myCommand.Parameters.AddWithValue("@reportDate", report.ReportDate);
                    myCommand.Parameters.AddWithValue("@reportContent", report.ReportContent);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Report Submitted Successfully");
        }

        [HttpGet]
        [Route("GetReportsByUser")]
        public JsonResult GetReportsByUser(int userId)
        {
            string query = @"
                SELECT * FROM Reports 
                WHERE userId = @userId";
                
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@userId", userId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); 
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetUserById")]
        public JsonResult GetUserById(int userId)
        {
            string query = "SELECT * FROM dbo.tblUserss WHERE id = @userId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using(SqlConnection myCon=new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@userId", userId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }



public class ReportModel
{
    public int UserId { get; set; }
    public DateTime ReportDate { get; set; }
    public string ReportContent { get; set; }
}


        
    }
}
