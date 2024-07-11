using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace NewEOSR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            string query = "SELECT * FROM dbo.tblUsers";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var users = await connection.QueryAsync<dynamic>(query);
                return Ok(users);
            }
        }

        [HttpPost]
        [Route("AddUsers")]
        public async Task<IActionResult> AddUsers([FromForm] string newUsers)
        {
            string query = "INSERT INTO dbo.tblUsers VALUES (@newUsers)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(query, new { newUsers });
                return new JsonResult($"Added Successfully. Rows affected: {affectedRows}");
            }
        }

        [HttpDelete]
        [Route("DeleteUsers")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            string query = "DELETE FROM dbo.tblUsers WHERE id = @id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(query, new { id });
                return new JsonResult($"Deleted Successfully. Rows affected: {affectedRows}");
            }
        }

        [HttpPost]
        [Route("SubmitReport")]
        public async Task<IActionResult> SubmitReport([FromBody] ReportModel report)
        {
            string query = @"
INSERT INTO tblReports (userID, reportDate, reportContent) 
VALUES (@UserID, @ReportDate, @ReportContent)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(query, report);
                return new JsonResult($"Report Submitted Successfully. Rows affected: {affectedRows}");
            }
        }

        [HttpGet]
        [Route("GetReportsByUser")]
        public async Task<IActionResult> GetReportsByUser(int userId)
        {
            string query = "SELECT * FROM tblReports WHERE userId = @userId";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var reports = await connection.QueryAsync<dynamic>(query, new { userId });
                return Ok(reports);
            }
        }

        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            string query = "SELECT * FROM dbo.tblUsers WHERE userID = @userId";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var user = await connection.QueryAsync<dynamic>(query, new { userId });
                return Ok(user);
            }
        }

        [HttpGet]
        [Route("GetAllReports")]
        public async Task<IActionResult> GetAllReports()
        {
            string query = "SELECT * FROM tblReports";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var reports = await connection.QueryAsync<dynamic>(query);
                return Ok(reports);
            }
        }

        public class ReportModel
        {
            public int UserID { get; set; }
            public DateTime ReportDate { get; set; }
            public string ReportContent { get; set; }
        }
    }
}
