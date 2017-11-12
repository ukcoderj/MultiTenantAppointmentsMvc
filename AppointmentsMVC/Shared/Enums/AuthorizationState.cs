using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
    public enum AuthorizationState
    {
        NotAllowed = 0,
        ReadOnly = 5,
        CreateReadUpdate = 10,
    }
}
