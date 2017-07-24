using System.Data.Entity.ModelConfiguration;

namespace Repository.EFRealize.Mapping
{
    public abstract class MyEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected MyEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}