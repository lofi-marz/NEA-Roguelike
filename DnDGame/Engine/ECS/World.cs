using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{
    public class World
    {
        public List<Entity> Entities;
        public Dictionary<int, List<Component>> EntityComponents;
        public List<ECSSystem> Systems;

        public World()
        {
            Entities = new List<Entity>();
            EntityComponents = new Dictionary<int, List<Component>>();
            Systems = new List<ECSSystem>();
        }

        public void Update()
        {

        }

        public void CreateEntity(int entityid, params Component[] components)
        {
            Entities.Add(new Entity(entityid));
            EntityComponents.Add(entityid, components.ToList() ?? new List<Component>());

        }

       

        public void CreateComponent(int entityid, Component component)
        {
            if (!EntityComponents.ContainsKey(entityid)) return;
            EntityComponents[entityid].Add(component);
        }

        public Component GetComponent(int entityid, Type componentType)
        {
            return EntityComponents[entityid].Where(x => x.GetType() == componentType).First();
        }

        public void SetComponent(int entityid, Component newComponent)
        {
            var currentComponent = EntityComponents[entityid].Find(x => x.GetType() == newComponent.GetType());
            var i = EntityComponents[entityid].IndexOf(currentComponent);
            EntityComponents[entityid][i] = newComponent;
        }

        public List<int> FilterEntities(Type type)
        {
            return EntityComponents.Where(x => x.Value.Where(y => y.GetType() == type).Count() > 0).Select(x => x.Key).ToList();
        }
    }
}
