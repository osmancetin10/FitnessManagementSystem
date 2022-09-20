using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FitnessManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace FitnessManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class MovementController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _sqlDataSource;
        private readonly ILogger<MovementController> _logger;

        public MovementController(IConfiguration configuration, ILogger<MovementController> logger)
        {
            _configuration = configuration;
            _sqlDataSource = _configuration.GetConnectionString("FitnessManagementSystemAppCon");
            _logger = logger;
        }

        #region Lists Movements With Given Parameters
        [HttpGet]
        public JsonResult Get([FromBody]Movement movement)
        {
            try
            {
                DataTable movements = new DataTable();
                MySqlDataReader dataReader;
                using (MySqlConnection conn = new MySqlConnection(_sqlDataSource))
                {
                    conn.Open();
                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "get_movements_by_exercise_id";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParExerciseId", movement.ExerciseId);

                        dataReader = comm.ExecuteReader();
                        movements.Load(dataReader);

                        dataReader.Close();
                    }
                    conn.Close();
                }

                return new JsonResult(movements);
            }
            catch(Exception E)
            {
                _logger.LogError(E.Message);
                return null;
            }
            
        }
        #endregion

        #region Gets Movement Information
        [HttpGet("{id}")]
        public string Get(int id)
        {
            try
            {
                MySqlDataReader dataReader;
                Movement movement = new Movement();
                using (MySqlConnection conn = new MySqlConnection(_sqlDataSource))
                {

                    conn.Open();
                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "get_movement_by_id";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParId", id);

                        dataReader = comm.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                movement.Id = dataReader.GetInt32(0);
                                movement.MovementName = dataReader.GetString(1);
                                movement.ExerciseId = dataReader.GetInt32(2);
                                movement.CreatedBy = dataReader.GetInt32(3);
                                movement.CreatedAt = dataReader.GetDateTime(4);
                                if (!dataReader.IsDBNull(5))
                                    movement.ModifiedBy = dataReader.GetInt32(5);
                                else
                                    movement.ModifiedBy = 0;
                                movement.ModifiedAt = dataReader.GetDateTime(6);
                            }
                        }
                        else
                        {
                            string json = "{\"RetMessage\": \" No Records Found\"}";
                            return JsonConvert.SerializeObject(json);
                        }

                        dataReader.Close();
                    }
                    conn.Close();
                }

                return JsonConvert.SerializeObject(movement);
            }
            catch (Exception E)
            {
                _logger.LogError(E.Message);
                return null;
            }
        }
        #endregion

        #region Inserts Given Movement Object To DB
        [HttpPost]
        public ContentResult Post([FromBody] Movement movement)
        {
            try
            {
                int affectedRows;
                using (MySqlConnection conn = new MySqlConnection(_sqlDataSource))
                {
                    conn.Open();
                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "insert_movement";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParMovementName", movement.MovementName);
                        comm.Parameters.AddWithValue("ParExerciseId", movement.ExerciseId);

                        affectedRows = comm.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                string json = "{\"RetMessage\": \" " + affectedRows + " Rows Inserted\"}";
                return new ContentResult { Content = json, ContentType = "application/json" };
            }
            catch (Exception E)
            {
                _logger.LogError(E.Message);
                return null;
            }
            
        }
        #endregion

        #region Updates Movement With Given Object
        [HttpPut]
        public ContentResult Put([FromBody] Movement movement)
        {
            try
            {
                int affectedRows;
                using (MySqlConnection conn = new MySqlConnection(_sqlDataSource))
                {
                    conn.Open();
                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "update_movement";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParId", movement.Id);
                        comm.Parameters.AddWithValue("ParMovementName", movement.MovementName);
                        comm.Parameters.AddWithValue("ParExerciseId", movement.ExerciseId);

                        affectedRows = comm.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                string json = "{\"RetMessage\": \" " + affectedRows + " Rows Updated\"}";
                return new ContentResult { Content = json, ContentType = "application/json" };
            }
            catch (Exception E)
            {
                _logger.LogError(E.Message);
                return null;
            }
        }
        #endregion

        #region Deletes Movement By Given Id
        [HttpDelete]
        public ContentResult Delete([FromBody] Movement movement)
        {
            try
            {
                int affectedRows;
                using (MySqlConnection conn = new MySqlConnection(_sqlDataSource))
                {
                    conn.Open();
                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "delete_movement";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParId", movement.Id);

                        affectedRows = comm.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                string json = "{\"RetMessage\": \" " + affectedRows + " Rows Deleted\"}";
                return new ContentResult { Content = json, ContentType = "application/json" };
            }
            catch (Exception E)
            {
                _logger.LogError(E.Message);
                return null;
            }
        }
        #endregion
    }
}

