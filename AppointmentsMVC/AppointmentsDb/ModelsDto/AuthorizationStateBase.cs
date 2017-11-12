using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.ModelsDto
{
    public abstract class AuthorizationStateBase
    {
        /// <summary>
        /// What is the user allowed to do with the info in this class
        /// Only ever trust what the db states. Don't use this flag for 
        /// anything coming back from the client.
        /// </summary>
        public AuthorizationState AuthState_OnlyTrustOnGeneration { get; set; }
    }
}
