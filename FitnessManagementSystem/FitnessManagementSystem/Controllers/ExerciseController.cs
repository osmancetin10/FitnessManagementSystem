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
    public class ExerciseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _sqlDataSource;
        private readonly ILogger<MovementController> _logger;

        public ExerciseController(IConfiguration configuration, ILogger<MovementController> logger)
        {
            _configuration = configuration;
            _sqlDataSource = _configuration.GetConnectionString("FitnessManagementSystemAppCon");
            _logger = logger;
        }

        #region Lists Exercises With Given Parameters
        [HttpGet]
        public JsonResult Get([FromBody]Exercise exercise)
        {
            try
            {
                DataTable exercises = new DataTable();
                MySqlDataReader dataReader;
                using (MySqlConnection conn = new MySqlConnection(_sqlDataSource))
                {
                    conn.Open();
                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "get_exercises";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ExerciseDuration", exercise.ExerciseDuration);
                        comm.Parameters.AddWithValue("ExerciseDifficulty", exercise.ExerciseDifficulty);
                        comm.Parameters.AddWithValue("ExerciseArea", exercise.ExerciseArea);

                        dataReader = comm.ExecuteReader();
                        exercises.Load(dataReader);

                        dataReader.Close();
                    }
                    conn.Close();
                }

                return new JsonResult(exercises);
            }
            catch (Exception E)
            {
                _logger.LogError(E.Message);
                return null;
            }
            
        }
        #endregion

        #region Gets Exercise And Exercises Movements Information
        [HttpGet("{id}")]
        public string Get(int id)
        {
            try
            {
                MySqlDataReader dataReader;
                Exercise exercise = new Exercise();
                using (MySqlConnection conn = new MySqlConnection(_sqlDataSource))
                {

                    conn.Open();
                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "get_exercise_by_id";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParId", id);

                        dataReader = comm.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                exercise.Id = dataReader.GetInt32(0);
                                exercise.ExerciseName = dataReader.GetString(1);
                                exercise.ExerciseDuration = dataReader.GetInt32(2);
                                exercise.ExerciseDifficulty = dataReader.GetInt32(3);
                                exercise.ExerciseArea = dataReader.GetInt32(4);
                                exercise.CreatedBy = dataReader.GetInt32(5);
                                exercise.CreatedAt = dataReader.GetDateTime(6);
                                if (!dataReader.IsDBNull(7))
                                    exercise.ModifiedBy = dataReader.GetInt32(7);
                                else
                                    exercise.ModifiedBy = 0;
                                exercise.ModifiedAt = dataReader.GetDateTime(8);
                            }
                        }
                        else
                        {
                            string json = "{\"RetMessage\": \" No Records Found\"}";
                            return JsonConvert.SerializeObject(json);
                        }

                        dataReader.Close();
                    }

                    using (MySqlCommand comm = new MySqlCommand())
                    {
                        DataTable movements = new DataTable();
                        comm.Connection = conn;
                        comm.CommandText = "get_movements_by_exercise_id";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParExerciseId", id);

                        dataReader = comm.ExecuteReader();
                        movements.Load(dataReader);

                        exercise.Movements = MovementTableToObjList(movements);
                        dataReader.Close();
                    }
                    conn.Close();
                }

                return JsonConvert.SerializeObject(exercise);
            }
            catch (Exception E)
            {
                _logger.LogError(E.Message);
                return null;
            }
            
        }
        #endregion

        #region Inserts Given Exercise Object To DB
        [HttpPost]
        public ContentResult Post([FromBody] Exercise exercise)
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
                        comm.CommandText = "insert_exercise";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ExerciseName", exercise.ExerciseName);
                        comm.Parameters.AddWithValue("ExerciseDuration", exercise.ExerciseDuration);
                        comm.Parameters.AddWithValue("ExerciseDifficulty", exercise.ExerciseDifficulty);
                        comm.Parameters.AddWithValue("ExerciseArea", exercise.ExerciseArea);

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

        #region Updates Exercise With Given Object
        [HttpPut]
        public ContentResult Put([FromBody] Exercise exercise)
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
                        comm.CommandText = "update_exercise";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParId", exercise.Id);
                        comm.Parameters.AddWithValue("ParExerciseName", exercise.ExerciseName);
                        comm.Parameters.AddWithValue("ParExerciseDuration", exercise.ExerciseDuration);
                        comm.Parameters.AddWithValue("ParExerciseDifficulty", exercise.ExerciseDifficulty);
                        comm.Parameters.AddWithValue("ParExerciseArea", exercise.ExerciseArea);

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

        #region Deletes Exercise By Given Id
        [HttpDelete]
        public ContentResult Delete([FromBody] Exercise exercise)
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
                        comm.CommandText = "delete_exercise";
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("ParId", exercise.Id);

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

        #region Helper Method For Convert Movement DataTable To Movement Object List
        private List<Movement> MovementTableToObjList(DataTable dt)
        {
            try
            {
                var convertedList = (from rw in dt.AsEnumerable()
                                     select new Movement()
                                     {
                                         Id = Convert.ToInt32(rw["id"]),
                                         MovementName = Convert.ToString(rw["movement_name"]),
                                         ExerciseId = Convert.ToInt32(rw["exercise_id"]),
                                         CreatedBy = Convert.ToInt32(rw["created_by"]),
                                         CreatedAt = Convert.ToDateTime(rw["created_at"]),
                                         ModifiedBy = (rw["updated_by"] == DBNull.Value) ? 0 : Convert.ToInt32(rw["updated_by"]),
                                         ModifiedAt = Convert.ToDateTime(rw["updated_at"])
                                     }).ToList();

                return convertedList;
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

