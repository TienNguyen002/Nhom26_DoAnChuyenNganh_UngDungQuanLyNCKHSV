﻿using Core.Contracts;
using Core.DTO.Lecturer;
using Core.Entities;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Apps.Lecturers
{
    public class LecturerRepository : ILecturerRepository
    {
        private readonly WebDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public LecturerRepository(WebDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<IList<LecturerItem>> GetLecturersAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<Lecturer> lecturers = _context.Set<Lecturer>();
            return await lecturers
                .OrderBy(l => l.FullName)
                .Select(l => new LecturerItem()
                {
                    Id = l.Id,
                    FullName = l.FullName,
                    Email = l.Email,
                    UrlSlug = l.UrlSlug,
                    Qualification = l.Qualification,
                    DoB = l.DoB,
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<Lecturer> GetLecturerByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Lecturer>().FindAsync(id, cancellationToken);
        }

        public async Task<Lecturer> GetLecturerBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Lecturer>()
                .Where(l => l.UrlSlug.Contains(slug))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsLecturerEmailExitedAsync(int id, string email, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Lecturer>()
                .AnyAsync(l => l.Id != id && l.Email == email, cancellationToken);
        }

        private IQueryable<Lecturer> GetLecturerByQueryable(LecturerQuery query)
        {
            IQueryable<Lecturer> lecturerQuery = _context.Set<Lecturer>()
                .Include(l => l.Department);
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                lecturerQuery = lecturerQuery.Where(l => l.FullName.Contains(query.Keyword)
                || l.Email.Contains(query.Keyword)
                || l.UrlSlug.Contains(query.Keyword)
                || l.Qualification.Contains(query.Keyword));
            }
            if(query.DepartmentId > 0)
            {
                lecturerQuery = lecturerQuery.Where(l => l.DepartmentId == query.DepartmentId);
            }
            return lecturerQuery;
        }
        public async Task<IPagedList<T>> GetPagedLecturesAsync<T>(LecturerQuery query,
            IPagingParams pagingParams, 
            Func<IQueryable<Lecturer>, IQueryable<T>> mapper,  
            CancellationToken cancellationToken = default)
        {
            IQueryable<Lecturer> lecturerFindResultsQuery = GetLecturerByQueryable(query);
            IQueryable<T> result = mapper(lecturerFindResultsQuery);
            return await result.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<bool> CreateLecturerAccountAsync(Lecturer lecturer, CancellationToken cancellationToken = default)
        {
            _context.Add(lecturer); 
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> ChangeInformationAsync(Lecturer lecturer, CancellationToken cancellationToken = default)
        {
            _context.Update(lecturer);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
