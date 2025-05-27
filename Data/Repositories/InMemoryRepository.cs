using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BusinessEntities;
using Common;

namespace Data.Repositories
{
    /// <summary>
    /// Thread-safe in-memory repository implementation using ConcurrentDictionary
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from IdObject</typeparam>
    [AutoRegister]
    public class InMemoryRepository<T> : IInMemoryRepository<T> where T : IdObject
    {
        private readonly ConcurrentDictionary<Guid, T> _entities;

        /// <summary>
        /// Initializes a new instance of the InMemoryRepository class
        /// </summary>
        public InMemoryRepository()
        {
            _entities = new ConcurrentDictionary<Guid, T>();
        }

        /// <summary>
        /// Saves an entity to the repository
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
        public void Save(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.AddOrUpdate(entity.Id, entity, (key, oldValue) => entity);
        }

        /// <summary>
        /// Deletes an entity from the repository
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.TryRemove(entity.Id, out T removedEntity);
        }

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        /// <returns>True if deleted, false if not found</returns>
        public bool Delete(Guid id)
        {
            return _entities.TryRemove(id, out T removedEntity);
        }

        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, null otherwise</returns>
        public T Get(Guid id)
        {
            _entities.TryGetValue(id, out T entity);
            return entity;
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>All entities in the repository</returns>
        public IEnumerable<T> GetAll()
        {
            return _entities.Values.ToList();
        }

        /// <summary>
        /// Finds entities based on a predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns>Matching entities</returns>
        /// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var compiledPredicate = predicate.Compile();
            return _entities.Values.Where(compiledPredicate).ToList();
        }

        /// <summary>
        /// Gets the total count of entities
        /// </summary>
        /// <returns>Total entity count</returns>
        public int Count()
        {
            return _entities.Count;
        }

        /// <summary>
        /// Gets the count of entities matching a predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns>Count of matching entities</returns>
        /// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var compiledPredicate = predicate.Compile();
            return _entities.Values.Count(compiledPredicate);
        }

        /// <summary>
        /// Checks if any entity exists matching the predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var compiledPredicate = predicate.Compile();
            return _entities.Values.Any(compiledPredicate);
        }

        /// <summary>
        /// Gets entities with pagination
        /// </summary>
        /// <param name="skip">Number of entities to skip</param>
        /// <param name="take">Number of entities to take</param>
        /// <returns>Paginated entities</returns>
        /// <exception cref="ArgumentException">Thrown when skip or take are negative</exception>
        public IEnumerable<T> GetPaged(int skip, int take)
        {
            if (skip < 0)
            {
                throw new ArgumentException("Skip cannot be negative.", nameof(skip));
            }

            if (take < 0)
            {
                throw new ArgumentException("Take cannot be negative.", nameof(take));
            }

            return _entities.Values.Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Gets entities with pagination and filtering
        /// </summary>
        /// <param name="predicate">The filter predicate</param>
        /// <param name="skip">Number of entities to skip</param>
        /// <param name="take">Number of entities to take</param>
        /// <returns>Filtered and paginated entities</returns>
        /// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
        /// <exception cref="ArgumentException">Thrown when skip or take are negative</exception>
        public IEnumerable<T> GetPaged(Expression<Func<T, bool>> predicate, int skip, int take)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (skip < 0)
            {
                throw new ArgumentException("Skip cannot be negative.", nameof(skip));
            }

            if (take < 0)
            {
                throw new ArgumentException("Take cannot be negative.", nameof(take));
            }

            var compiledPredicate = predicate.Compile();
            return _entities.Values.Where(compiledPredicate).Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Clears all entities from the repository
        /// </summary>
        public void Clear()
        {
            _entities.Clear();
        }
    }
}
