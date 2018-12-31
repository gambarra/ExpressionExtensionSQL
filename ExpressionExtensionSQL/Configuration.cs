using System;
using System.Collections.Generic;
using System.Linq;
namespace ExpressionExtensionSQL
{

    public class Configuration
    {

        private static Configuration instance=null;
        public static Configuration GetInstance() {
            if (instance == null)
                instance = new Configuration();
            return instance;
        }

        private  List<IEntityMap> entitiesMaps;
        private List<IPropertyMap> propertiesMaps;

        internal List<IEntityMap> Entities() {
            return entitiesMaps;
        }
        internal List<IPropertyMap> Properties() {
            return propertiesMaps;
        }
        internal void AddEntity(IEntityMap entity) {

            if (entitiesMaps == null)
                this.entitiesMaps = new List<IEntityMap>();
            this.entitiesMaps.Add(entity);
        }
        internal void AddProperty(IPropertyMap propertyMap) {
            if (propertiesMaps == null)
                this.propertiesMaps = new List<IPropertyMap>();
            this.propertiesMaps.Add(propertyMap);
        }


    }
}
