﻿using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Student : IEntity
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public string UrlSlug { get; set; }
        public DateTime DoB { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public string Class { get; set; }
        public string Year { get; set; }
        public string Address { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public IList<Account> Accounts { get; set; }
        public IList<Group> Groups { get; set; }
    }
}