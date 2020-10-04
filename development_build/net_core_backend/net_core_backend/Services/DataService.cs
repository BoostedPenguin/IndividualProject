﻿using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    /// <summary>
    /// Handles default CRUD operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataService<T> : IDataService<T> where T : DefaultModel
    {
        private readonly ContextFactory _contextFactory;


        public DataService(ContextFactory _contextFactory)
        {
            this._contextFactory = _contextFactory;
        }

        public virtual async Task<T> Create(T entity)
        {
            using(var _context = _contextFactory.CreateDbContext())
            {
                var created = await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return created.Entity;
            }
        }

        public virtual async Task<T> Delete(int id)
        {
            using(var _context = _contextFactory.CreateDbContext())
            {
                var entity = await _context.Set<T>().FirstOrDefaultAsync(o => o.Id == id);
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
        }

        public virtual async Task<T> Get(int id)
        {
            using(var _context = _contextFactory.CreateDbContext())
            {
                var entity = await _context.Set<T>().FirstOrDefaultAsync((o) => o.Id == id);
                return entity;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            using(var _context = _contextFactory.CreateDbContext())
            {
                var entity = await _context.Set<T>().ToListAsync();
                return entity;
            }
        }

        public virtual async Task<T> Update(int id, T entity)
        {
            using(var _context = _contextFactory.CreateDbContext())
            {
                var found = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

                var result = InjectNonNull(found, entity);
                _context.Set<T>().Update(result);

                await _context.SaveChangesAsync();

                return result;
            }
        }

        /// <summary>
        /// Sets only non null values
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public T InjectNonNull(T dest, T src)
        {
            foreach (var propertyPair in dest.GetType().GetProperties())
            {
                if (propertyPair.GetType() == typeof(DateTime?) || propertyPair.GetType() == typeof(DateTime)) continue;
                var fromValue = propertyPair.GetValue(src, null);
                if (fromValue != null && propertyPair.CanWrite)
                {
                    propertyPair.SetValue(dest, fromValue, null);
                }
            }
            return dest;
        }

        public async Task<int> GetUserId(string auth_id)
        {
            using(var _context = _contextFactory.CreateDbContext())
            {
                return await _context.Users.Where(x => x.Auth == auth_id).Select(x => x.Id).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> ValidateUser(int id, string auth_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                if (await _context.Users.Where(x => x.Auth == auth_id && x.Id == id).FirstOrDefaultAsync() == null) return false;
                return true;
            }
        }
    }
}
