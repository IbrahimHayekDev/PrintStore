
using System.ComponentModel.DataAnnotations;

namespace PrintStoreApi.Core.Entities.Base;

    public abstract class EntityBase<TId>: IEntityBase<TId>
    {
	[Key]
	public virtual TId Id { get; protected set; }
    }

