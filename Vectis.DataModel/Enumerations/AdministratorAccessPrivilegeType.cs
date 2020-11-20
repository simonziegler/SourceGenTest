using System;
using System.Collections.Generic;
using System.Text;

namespace Vectis.DataModel
{
    /// <summary>
    /// Access privilege granted by a lender to a Dioptra administrator.
    /// </summary>
    public enum AdministratorAccessPrivilegeType 
    { 
        /// <summary>
        /// Read only privilege.
        /// </summary>
        ReadOnly, 

        /// <summary>
        /// Read and write privileges.
        /// </summary>
        ReadWrite 
    }
}
