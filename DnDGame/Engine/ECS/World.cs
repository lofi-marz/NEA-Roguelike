using DnDGame.Engine.ECS.Systems.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{
    public sealed class World
    {
        private static Lazy<World> lazy = new Lazy<World>(() => new World());
        
        public static World Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        
        public List<Entity> Entities;
        //public Dictionary<int, List<Component>> EntityComponents;
        public Dictionary<Type, Dictionary<int, Component>> EntityComponents;
        public SpatialHash Sprites;
        private int entityI;
        public World()
        {
            Entities = new List<Entity>();
            EntityComponents = new Dictionary<Type, Dictionary<int, Component>>();
            entityI = 0;
            Sprites = new SpatialHash();
        }



        public int CreateEntity(params Component[] components)
        {
            Entities.Add(new Entity(entityI));
            foreach (var component in components)
            {
                AddComponent(entityI, component);
            }
            
            return entityI++;
        }

        /// <summary>
        /// Retrieve all entities with the given types.
        /// </summary>
        /// <param name="ComponentTypes">A list of components. The returned entities will have all of these./param>
        public List<int> GetEntitiesByType(params Type[] ComponentTypes)
        {
            List<int> Entities = new List<int>();
            foreach (var type in ComponentTypes)
            {
               Entities = Entities.Concat(EntityComponents[type].Select(x => x.Key)).ToList();
            }
            var ValidEntities = Entities.GroupBy(r => r)
                .Select(grp => new
                {
                    Value = grp.Key,
                    Count = grp.Count()
                })
                .Where(x => x.Count >= ComponentTypes.Length)
                .Select(x => x.Value).ToList();
            return ValidEntities;
        }

        public List<int> GetByTypeAndRegion(Rectangle region, params Type[] types)
        {
            List<int> entitiesInRegion = Instance.Sprites.GetItems(region);
            List<int> typeEntities = Instance.GetEntitiesByType(types);
            return  entitiesInRegion.Intersect(typeEntities).ToList();
        }

        public void AddComponent(int entityid, Component component)
        {

            var componentType = component.GetType();
            if (!EntityComponents.ContainsKey(componentType))
            {
                EntityComponents.Add(componentType, new Dictionary<int, Component>());
            }
            if (!EntityComponents[componentType].ContainsKey(entityid))
            {
                EntityComponents[componentType].Add(entityid, component);
            }
            Type type = component.GetType();
            
            var i = (int)(ComponentUtils.GetEnum(type));

            //Entities[entityid].ComponentFlags[i] = true;
        }

        /// <summary>
        /// If the entity has a component of the given type, return it.
        /// </summary>
        /// <typeparam name="T">The type of component to look for.</typeparam>
        /// <param name="entityid">The id of the entity.</param>
        /// <returns>Returns the entity's component if found, otherwise null.</returns>
        public T GetComponent<T>(int entityid)
        {
            var components = EntityComponents.ContainsKey(typeof(T)) ? EntityComponents[typeof(T)] : null;
            return components == null ? (T)Convert.ChangeType(null, typeof(T)) : (T)Convert.ChangeType(components[entityid], typeof(T));
        }

        public bool HasComponent<T>(int entityid)
        {
            return EntityComponents.ContainsKey(typeof(T)) && EntityComponents[typeof(T)].ContainsKey(entityid);
        }

        public void SetComponent(int entityid, Component newComponent)
        {
            Type componentType = newComponent.GetType();
            EntityComponents[componentType][entityid] = newComponent;
            
        }

        /*public List<int> FilterEntities(Type type)
        {
            return EntityComponents[ComponentUtils.GetEnum(type)]
        }*/
    }
}
