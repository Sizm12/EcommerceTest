using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class UnitofWork : IUnitofWork
    {
        private Hashtable _repository;

        private readonly EcommerceDbContext _context;

        public UnitofWork(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : ClaseBase
        {
            if (_repository == null)
            {
                _repository = new Hashtable();

            }
            var type = typeof(TEntity).Name;

            if (!_repository.Contains(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                //GenericRepository<Producto>
                _repository.Add(type,repositoryInstance);
            }
            return (IGenericRepository<TEntity>) _repository[type];
        }
    }
}
