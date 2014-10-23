using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain
{
    public interface IEntityExtendedProperty
    {
        string DefaultOrderColumnName { get; }
        bool HasColumnIsDeleted { get; }
        bool HasColumnDeletedOn { get; }
        bool HasColumnCreatedOn { get; }
        bool HasColumnCreatedBy { get; }
        bool HasColumnLastModifiedOn { get; }
        bool HasColumnLastModifiedBy { get; }
    }
}
