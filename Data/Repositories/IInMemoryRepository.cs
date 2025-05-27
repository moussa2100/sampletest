using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BusinessEntities;

namespace Data.Repositories
{
    /// <summary>
    /// Interface for in-memory repository operations
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from IdObject</typeparam>
    public interface IInMemoryRepository<T> where T : IdObject
    {
        /// <summary>
        /// Saves an entity to the repository
        /// </summary>
        /// <param name="entity">The entity to save</param>
        void Save(T entity);

        /// <summary>
        /// Deletes an entity from the repository
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        /// <returns>True if deleted, false if not found</returns>
        bool Delete(Guid id);

        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, null otherwise</returns>
        T Get(Guid id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>All entities in the repository</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Finds entities based on a predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns>Matching entities</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the total count of entities
        /// </summary>
        /// <returns>Total entity count</returns>
        int Count();

        /// <summary>
        /// Gets the count of entities matching a predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns>Count of matching entities</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Checks if any entity exists matching the predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets entities with pagination
        /// </summary>
        /// <param name="skip">Number of entities to skip</param>
        /// <param name="take">Number of entities to take</param>
        /// <returns>Paginated entities</returns>
        IEnumerable<T> GetPaged(int skip, int take);

        /// <summary>
        /// Gets entities with pagination and filtering
        /// </summary>
        /// <param name="predicate">The filter predicate</param>
        /// <param name="skip">Number of entities to skip</param>
        /// <param name="take">Number of entities to take</param>
        /// <returns>Filtered and paginated entities</returns>
        IEnumerable<T> GetPaged(Expression<Func<T, bool>> predicate, int skip, int take);

        /// <summary>
        /// Clears all entities from the repository
        /// </summary>
        void Clear();
    }
}
