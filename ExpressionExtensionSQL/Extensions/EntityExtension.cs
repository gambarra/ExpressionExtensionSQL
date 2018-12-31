using System.Linq;

namespace ExpressionExtensionSQL.Extensions {
    public static class EntityExtension {

        public static Entity<TEntity> Entity<TEntity>(this Configuration configuration) where TEntity : class {

            var entity = Configuration.GetInstance().Entities()?.FirstOrDefault(p => p.Type() == typeof(TEntity));
            if (Configuration.GetInstance().Entities() == null || entity == null) {
                entity = new Entity<TEntity>();
                Configuration.GetInstance().AddEntity(entity);
                
            }
            return (Entity<TEntity>)entity;
        }

        public static void ToTable<TEntity>(this Entity<TEntity> entity, string tableName) {
            entity.SetTableName(tableName);
        }

        public static void ToColumn<TEntity, TProperty>(this PropertyEntry<TEntity, TProperty> propertyEntry, string columnName) {
            Configuration.GetInstance().AddProperty(propertyEntry);
            propertyEntry.SetColumnName(columnName);
        }
    }
}
