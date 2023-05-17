﻿using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Department : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public IList<Student> Students { get; set; }
        public IList<Topic> Topics { get; set; }
        public IList<Lecturer> Lecturers { get; set; }
    }
}
