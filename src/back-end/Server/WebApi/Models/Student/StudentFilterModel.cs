﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace WebApi.Models.Student
{
    public class StudentFilterModel : PagingModel
    {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }

        [DisplayName("Khoa")]
        public int? DepartmentId { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
    }
}