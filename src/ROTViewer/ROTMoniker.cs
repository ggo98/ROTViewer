using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ROTViewer
{
    /// <summary>
    /// représente un élément de la ROT. Permet de récupérer l'objet attaché.
    /// </summary>
    public class ROTMoniker
    {
        private IMoniker Moniker { get; set; }
        private IMoniker ParentMoniker { get; set; }


        /// <summary>
        /// moniker est l'objet natif de la ROT.
        /// Le parent ne sert que pour les monikers composés. Sinon c'est NULL.
        /// </summary>
        /// <param name="moniker"></param>
        /// <param name="parent"></param>
        public ROTMoniker(IMoniker moniker, IMoniker parent)
        {
            Moniker = moniker;
            ParentMoniker = parent;
        }
        /// <summary>
        /// returns the moniker string visible in the ROT table.
        /// </summary>
        public string Name
        {
            get
            {
                string name;
                Ole32.CreateBindCtx(0, out IBindCtx bc);
                try
                {
                    Moniker.GetDisplayName(bc, ParentMoniker, out name);
                }
                finally
                {
                    Marshal.ReleaseComObject(bc);
                }
                return name;
            }
        }
        /// <summary>
        /// returns the GUID of the moniker type. This is not related to the actual object stored.
        /// Usually it is file moniker or item moniker, see MSDN doc.
        /// </summary>
        public Guid GUID
        {
            get
            {
                Moniker.GetClassID(out Guid guid);
                return guid;
            }
        }

        /// <summary>
        /// Enumerates children monikers in a composed moniker.
        /// </summary>
        public IEnumerable<ROTMoniker> Children
        {
            get
            {
                Moniker.Enum(true, out IEnumMoniker enumsousmink);
                if (enumsousmink != null)
                {
                    try
                    {
                        enumsousmink.Reset();
                        int celt = 1;
                        IMoniker[] monikers = new IMoniker[1];
                        IntPtr pfetched = new IntPtr();
                        while (enumsousmink.Next(celt, monikers, pfetched) == 0)
                        {
                            yield return new ROTMoniker(monikers[0], Moniker);
                        }
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(enumsousmink);
                    }
                }
            }
        }
        /// <summary>
        /// returns the object stored in the moniker.
        /// </summary>
        /// <returns></returns>
        public object GetObject()
        {
            object result;
            IRunningObjectTable rot;
            Ole32.CreateBindCtx(0, out IBindCtx bc);
            try
            {
                bc.GetRunningObjectTable(out rot);
            }
            finally
            {
                Marshal.ReleaseComObject(bc);
            }

            try
            {
                rot.GetObject(Moniker, out result);
            }
            finally
            {
                Marshal.ReleaseComObject(rot);
            }

            return result;
        }
    }
}
