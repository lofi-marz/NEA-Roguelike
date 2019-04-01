using DnDGame.Engine.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine
{
	public sealed class World
	{
		private static Lazy<World> lazy = new Lazy<World>(() => new World()); //If there is no instance of the world, create one, otherwise return the instance

		/// <summary>
		/// The current instance of the world object.
		/// </summary>
		public static World Instance { get => lazy.Value; }

		/// <summary>
		/// The list of entities in the world.
		/// </summary>
		public List<Entity> Entities;

		/// <summary>
		/// All of the component instances, organised into dictionaries by type.
		/// </summary>
        public Dictionary<Type, Dictionary<int, IComponent>> EntityComponents;

		/// <summary>
		/// The Spatial Hash system used to organise the world's objects.
		/// </summary>
        public SpatialHash SpriteHash;

		/// <summary>
		/// The id to give the next entity created.
		/// </summary>
        private int entityI;

        public World()
        {
            Entities = new List<Entity>();
            EntityComponents = new Dictionary<Type, Dictionary<int, IComponent>>();
            entityI = 0;
            SpriteHash = new SpatialHash();
        }


		/// <summary>
		/// Create an entity with the given components, and the id of the value in entityI
		/// </summary>
		/// <param name="components">THe components to assign to the entity.</param>
		/// <returns>Returns the id of the entity created.</returns>
        public int CreateEntity(params IComponent[] components)
        {
            Entities.Add(new Entity(entityI));
            foreach (var component in components)
            {
                AddComponent(entityI, component);
            }
            
            return entityI++;
        }

		/// <summary>
		/// Create an entity with the given components, and the id of the value in entityI, and assign it to a group.
		/// </summary>
		/// <param name="group">The group to assign to the entity.</param>
		/// <param name="components">The components to assign to the entity.</param>
		/// <returns>Returns the id of the entity created.</returns>
		public int CreateEntity(string group, params IComponent[] components)
		{
			Entities.Add(new Entity(entityI, group));
			foreach (var component in components)
			{
				AddComponent(entityI, component);
			}

			return entityI++;
		}

		/// <summary>
		/// Removes the entity from the Entities list, and removes all of the components assigned to it.
		/// </summary>
		/// <param name="entity">The id of the entity to destroy.</param>
		public void DestroyEntity(int entity)
		{
			foreach (var e in Entities)
			{
				if (e.Id == entity)
				{
					Entities.Remove(e);
					break;
				}
			}
			foreach (var componentList in EntityComponents)
			{
				if (componentList.Value.ContainsKey(entity))
				{
					EntityComponents[componentList.Key].Remove(entity);
				}
			}
		}

		/// <summary>
		/// Retrieve all entities with the given types.
		/// </summary>
		/// <param name="ComponentTypes">A list of components. The returned entities will have all of these./param>
		/// 
		public IEnumerable<int> GetEntitiesByType(params Type[] ComponentTypes)
        {

            List<int> Entities = new List<int>();
            List<int> ValidEntities = new List<int>();
			if (!EntityComponents.ContainsKey(ComponentTypes[0]))
			{
				return new List<int>();
			}
            foreach (var entity in EntityComponents[ComponentTypes[0]]) {
                int count = 1;
                for (int i = 1; i < ComponentTypes.Length; i++)
                {
                    if (EntityComponents[ComponentTypes[i]].ContainsKey(entity.Key))
                    {

                        count++;
                    }
 
                }
                if (count == ComponentTypes.Length)
                {
                    ValidEntities.Add(entity.Key);
                }
            }
            return ValidEntities;
        }

		
		/// <summary>
		/// Get
		/// </summary>
		/// <param name="region">The rectangular region to retrieve entitiesfrom.</param>
		/// <param name="pad">Whether or not to check a certain region outside of the given one.</param>
		/// <param name="types">The components the entities must have to be retrieved.</param>
		/// <returns>Returns a list of entities that are in the given region, with the given components.</returns>
        public IEnumerable<int> GetByTypeAndRegion(Rectangle region, bool pad, params Type[] types)
        {
            IEnumerable<int> entitiesInRegion = Instance.SpriteHash.GetItems(region, pad);
            IEnumerable<int> typeEntities = Instance.GetEntitiesByType(types);

			return entitiesInRegion.Intersect(typeEntities);
        }

		/// <summary>
		/// Add a component to the world and assign it to the given entity.
		/// </summary>
		/// <param name="entityid">The entity to assign the component to.</param>
		/// <param name="component">The component to assign to the entity.</param>
        public void AddComponent(int entityid, IComponent component)
        {
            var componentType = component.GetType();
            if (!EntityComponents.ContainsKey(componentType))
            {
                EntityComponents.Add(componentType, new Dictionary<int, IComponent>());
            }
			if (!EntityComponents[componentType].ContainsKey(entityid))
			{
				EntityComponents[componentType].Add(entityid, component);
			}
        }

        /// <summary>
        /// If the entity has a component of the given type, return it.
        /// </summary>
        /// <typeparam name="T">The type of component to look for.</typeparam>
        /// <param name="entityid">The id of the entity.</param>
        /// <returns>Returns the entity's component if found, otherwise null.</returns>
        public T GetComponent<T>(int entityid) where T : IComponent
        {
            var componentsOfTypeT = EntityComponents.ContainsKey(typeof(T)) ? EntityComponents[typeof(T)] : null;
			if (componentsOfTypeT == null)
			{
				return (T)Convert.ChangeType(null, typeof(T));
			}
			else if (!componentsOfTypeT.ContainsKey(entityid))
			{
				return (T)Convert.ChangeType(null, typeof(T));
			}
			else
			{
				return (T)Convert.ChangeType(componentsOfTypeT[entityid], typeof(T));
			}
        }
		

		/// <summary>
		/// Update the component belonging to the given entity, and replace it with the given component.
		/// </summary>
		/// <param name="entityid">The entity to update.</param>
		/// <param name="newComponent">The component to update it with.</param>
        public void SetComponent(int entityid, IComponent newComponent)
        {
            Type componentType = newComponent.GetType();
            EntityComponents[componentType][entityid] = newComponent;
            
        }

    }
}
