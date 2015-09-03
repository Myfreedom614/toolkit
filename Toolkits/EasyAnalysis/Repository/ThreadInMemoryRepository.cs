﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyAnalysis.Models;

namespace EasyAnalysis.Repository
{
    public class ThreadInMemoryRepository : IThreadRepository
    {
        private readonly static IList<ThreadModel> ThreadsStore = new List<ThreadModel>
        {
            new ThreadModel
            {
                Id = "18273ef6-92f8-4f11-818d-7e3b8470307a",
                Title = "[UWP] Is there a way to set ForegroundText for Live Tiles in UWP apps?",
                AuthorId = "413B7E76-1CDF-41D5-8758-AB0E3C73C463",
                CreateOn = DateTime.Now,
                ForumId = "618962AB-67E0-4443-ACEF-CFF16A15C19E"
            }
        };

        public string Create(ThreadModel model)
        {
            ThreadsStore.Add(model);

            return model.Id;
        }

        public bool Exists(string id)
        {
            return ThreadsStore.FirstOrDefault(m => m.Id.Equals(id)) != null;
        }

        public ThreadModel Get(string id)
        {
            return ThreadsStore.FirstOrDefault(m => m.Id.Equals(id));
        }
    }
}