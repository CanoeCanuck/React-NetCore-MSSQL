using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotionAlertController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public MotionAlertController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT ma.AlertID, ma.Camera,ma.TimeStamp,ma.Message,ma.ImageURL,COUNT(mad.MotionAlertDetailID) as 'TotalObjects'
                            FROM MotionAlert ma
                            LEFT JOIN MotionAlertDetail mad ON ma.AlertID = mad.MotionAlertID
                            GROUP BY ma.AlertID, ma.Camera,ma.TimeStamp,ma.Message,ma.ImageURL
                            ORDER BY 1 DESC
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
        [Route("{id}")]
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            string query = @"
                            SELECT [MotionAlertDetailID]
                              ,[MotionAlertID]
                              ,[Label]
                              ,[Confidence]
                              ,[MinX]
                              ,[MinY]
                              ,[MaxX]
                              ,[MaxY]
                              ,[SizeX]
                              ,[SizeY]
                          FROM [dbo].[MotionAlertDetail]
                            WHERE MotionAlertID = @AlertId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@AlertId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Root alert)
        {
            string query = @"
                           insert into dbo.MotionAlert
                           (Camera,TimeStamp,Message,ImageURL)
                    values (@Camera,GETDATE(),@Message,@ImageURL)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Camera", alert.camera ?? (object)DBNull.Value);
                    myCommand.Parameters.AddWithValue("@Message", alert.message ?? (object)DBNull.Value);
                    myCommand.Parameters.AddWithValue("@ImageURL", alert.imageUrl ?? (object)DBNull.Value);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            var detail = JsonConvert.SerializeObject(alert);
            dynamic predict = JsonConvert.DeserializeObject<dynamic>(detail);

            foreach (var det in predict.predictions)
            {
                string Label = det.Label;
                string Confidence = det.Confidence;
                string MinX = det.MinX;
                string MinY = det.MinY;
                string MaxX = det.MaxX;
                string MaxY = det.MaxY;
                string SizeX = det.SizeX;
                string SizeY = det.SizeY;
                string insertStatement = $"INSERT INTO dbo.MotionAlertDetail (MotionAlertID, Label, Confidence, MinX, MinY, MaxX, MaxY, SizeX, SizeY ) SELECT (SELECT MAX(AlertID) FROM MotionAlert), '{Label}', {Confidence}, {MinX}, {MinY}, {MaxX}, {MaxY}, {SizeX}, {SizeY};";

                DataTable tabledet = new DataTable();
                string sqlDataSourcedet = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReaderdet;
                using (SqlConnection myCondet = new SqlConnection(sqlDataSourcedet))
                {
                    myCondet.Open();
                    using (SqlCommand myCommanddet = new SqlCommand(insertStatement, myCondet))
                    {
                        myReaderdet = myCommanddet.ExecuteReader();
                        tabledet.Load(myReaderdet);
                        myReaderdet.Close();
                        myCondet.Close();
                    }
                }
            }

            return new JsonResult("Added Successfully");
        }

    }
}
