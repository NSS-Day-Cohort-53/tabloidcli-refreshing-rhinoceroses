﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    public class NoteRepository : DatabaseConnector, IRepository<Note>
    {
        private int _postId;
        public NoteRepository(int postId, string connectionString) : base(connectionString)
        {
            _postId = postId;
        }

        public List<Note> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Title,
                                               Content,
                                               CreateDateTime
                                          FROM Note
                                         WHERE PostId = @postId";
                    cmd.Parameters.AddWithValue("@postId", _postId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Note> notes = new List<Note>();

                    while (reader.Read())
                    {
                        Note note = new Note()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                        notes.Add(note);
                    }
                    reader.Close();

                    return notes;
                }
            }
        }

        public Note Get(int id)
        {
            return null;
        }

        public void Insert(Note note)
        {

        }

        public void Update(Note note)
        {

        }

        public void Delete(int id)
        {

        }
    }
}
