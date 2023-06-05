﻿using Core.Contracts;
using Core.DTO.Student;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Apps.Students
{
    public interface IStudentRepository
    {
        Task<IList<StudentItem>> GetStudentsAsync(CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedStudentAsync<T>(StudentQuery query,
            IPagingParams pagingParams,
            Func<IQueryable<Student>, IQueryable<T>> mapper,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteStudentByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Student> GetStudentBySlugAsync(string slug, bool includeDetails = false, CancellationToken cancellationToken = default);
        Task<bool> UpdateStudentAsync(Student student, CancellationToken cancellationToken = default);
        Task<Student> GetStudentByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<bool> IsStudentEmailExitedAsync(int id, string email, CancellationToken cancellationToken = default);

        Task<bool> CreateStudentAccountAsync(Student student, CancellationToken cancellationToken = default);
        Task<bool> GetStudentPasswordBySlugAsync(string slug, string password, CancellationToken cancellationToken = default);
    }
}
